using System;
using System.Collections.Generic;
using System.Linq;
using Candid.Extv2Standard;
using Candid.Extv2Boom;
using Candid.IcpLedger;
using Candid.IcpLedger.Models;
using Candid.StakingHub;
using Candid.World;
using Candid.World.Models;
using Candid.WorldHub;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Candid.Models;
using Boom.Patterns.Broadcasts;
using Boom.Utility;
using Boom.Values;
using UnityEngine;
using WebSocketSharp;
using Boom.UI;
using Candid.Extv2Boom.Models;
using Candid.IcrcLedger;
using Unity.VisualScripting;
using PeterO.Numbers;
using Candid.UserNode;
using System.Security.Principal;
using EdjCase.ICP.BLS;

namespace Candid
{
    public class CandidApiManager : MonoBehaviour
    {
        // Instance
        public static CandidApiManager Instance;

        //Cache
        public InitValue<IAgent> cachedAnonAgent;
        public InitValue<string> cachedUserAddress;

        // Canister APIs
        public UserNodeApiClient UserNodeApiClient { get; private set; }
        public WorldApiClient WorldApiClient { get; private set; }
        public WorldHubApiClient WorldHub { get; private set; }
        public StakingHubApiClient StakingHubApiClient { get; private set; }

        [SerializeField, ShowOnly] string paymentHubIdentifier;
        [SerializeField, ShowOnly] string stakingHubIdentifier;
        public static string PaymentHubIdentifier { get { return Instance.paymentHubIdentifier; } }
        public static string StakingHubIdentifier { get { return Instance.stakingHubIdentifier; } }

        bool areDependenciesReady;
        bool configsRequested;

        private void Awake()
        {
            IAgent CreateAgentWithRandomIdentity(bool useLocalHost = false)
            {
                IAgent randomAgent = null;

                var httpClient = new UnityHttpClient();
#if UNITY_WEBGL && !UNITY_EDITOR
                var bls = new WebGlBlsCryptography();
#else
                var bls = new WasmBlsCryptography();
#endif

                try
                {
                    if (useLocalHost)
                        randomAgent = new HttpAgent(Ed25519Identity.Generate(), new Uri("http://localhost:4943"), bls);
                    else
                        randomAgent = new HttpAgent(httpClient, Ed25519Identity.Generate(), bls);
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                }

                return randomAgent;
            }

            Instance = this;

            Broadcast.Register<StartLogin>(StartLogin);
            Broadcast.Register<UserLogout>(UserLogoutHandler);

            UserUtil.RegisterToRequestData<DataTypes.Token>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.TokenConfig>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.Entity>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.Action>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.NftCollection>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.Stake>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.Listing>(FetchHandler);

            InitializeCandidApis(CreateAgentWithRandomIdentity(), true).Forget();
        }

        private void OnDestroy()
        {
            Broadcast.Unregister<StartLogin>(StartLogin);
            Broadcast.Unregister<UserLogout>(UserLogoutHandler);

            UserUtil.UnregisterToRequestData<DataTypes.Token>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.TokenConfig>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.Entity>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.Action>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.NftCollection>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.Stake>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.Listing>(FetchHandler);
        }
        private void StartLogin(StartLogin arg)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LoginManager.Instance.StartLoginFlowWebGl(OnLoginCompleted);
            return;
#endif

            LoginManager.Instance.StartLoginFlow(OnLoginCompleted);
        }
        void OnLoginCompleted(string json)
        {
            var getIsLoginResult = UserUtil.GetLogInType();

            if (getIsLoginResult.Tag == UResultTag.Err)
            {
                CreateAgentUsingIdentityJson(json, false).Forget();
                return;
            }

            if (getIsLoginResult.Tag == UResultTag.Ok && getIsLoginResult.AsOk() == UserUtil.LoginType.Anon)
            {
                CreateAgentUsingIdentityJson(json, false).Forget();
                return;
            }

            Debug.Log("You already have an Agent created");
        }
        public async UniTaskVoid CreateAgentUsingIdentityJson(string json, bool useLocalHost = false)
        {
            await UniTask.SwitchToMainThread();

            try
            {
                WindowManager.Instance.OpenWindow<BalanceWindow>(null, 1000);

                var identity = Identity.DeserializeJsonToIdentity(json);

                var httpClient = new UnityHttpClient();

#if UNITY_WEBGL && !UNITY_EDITOR
                var bls = new WebGlBlsCryptography();
#else
                var bls = new WasmBlsCryptography();
#endif
                if (useLocalHost) await InitializeCandidApis(new HttpAgent(identity, new Uri("http://localhost:4943"), bls));
                else await InitializeCandidApis(new HttpAgent(httpClient, identity, bls));

                Debug.Log("You have logged in");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void UserLogoutHandler(UserLogout obj)
        {
            InitializeCandidApis(cachedAnonAgent.Value, true).Forget();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actiontType">If the type is "Update" then must use the return value once at a time to record the update call</param>
        /// <returns></returns>
        private async UniTask InitializeCandidApis(IAgent agent, bool asAnon = false)
        {
            //Build Interfaces
            WorldHub = new WorldHubApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD_HUB));
            StakingHubApiClient = new StakingHubApiClient(agent, Principal.FromText(Env.CanisterIds.STAKING_HUB));
            WorldApiClient = new WorldApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD));

            var userPrincipal = agent.Identity.GetPublicKey().ToPrincipal().ToText();
            string userAccountIdentity;
            //Check if anon setup is required
            if (asAnon)
            {
                //If anon Agent is cached then set it up
                if (cachedAnonAgent.IsInit)
                {
                    userAccountIdentity = cachedUserAddress.Value;
                    agent = cachedAnonAgent.Value;

                }
                //Else fetch required dependencies and catch it
                else
                {
                    cachedAnonAgent.Value = agent;

                    userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);
                    cachedUserAddress.Value = userAccountIdentity;
                }
                //Set Login Data
                UserUtil.UpdateLoginData(agent, userPrincipal, userAccountIdentity, asAnon);

                //Clean up logged in user data
                UserUtil.Clean<DataTypes.Token>();
                UserUtil.Clean<DataTypes.Entity>();
                UserUtil.Clean<DataTypes.Action>();
                UserUtil.Clean<DataTypes.NftCollection>();
                UserUtil.Clean<DataTypes.Stake>();
            }
            else
            {
                userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);

                "Try Fetch User Data".Log(GetType().Name);


                //HERE: YOU CAN REQUEST FOR THE FIRST TIME ON THE GAME THE USER DATA

                //Set Login Data
                UserUtil.UpdateLoginData(agent, userPrincipal, userAccountIdentity, asAnon);

                //USER DATA
                UserUtil.RequestData<DataTypes.Entity>();
                UserUtil.RequestData<DataTypes.Action>();
                UserUtil.RequestData<DataTypes.Stake>();

                //ICP
                UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);

                //ICRC
                UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICRC_LEDGER);

                //NFTs
                UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch("er7d4-6iaaa-aaaaj-qac2q-cai", "The Moonwalkers", true));
                UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch("bzsui-sqaaa-aaaah-qce2a-cai", "Poked Bots", true));
                UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));
            }



            //INIT CONFIGS
            if (areDependenciesReady == false)
            {
                paymentHubIdentifier = await CandidApiManager.Instance.WorldHub.GetAccountIdentifier(Env.CanisterIds.PAYMENT_HUB);
                stakingHubIdentifier = await CandidApiManager.Instance.WorldHub.GetAccountIdentifier(Env.CanisterIds.STAKING_HUB);

                areDependenciesReady = true;
            }

            if(!configsRequested)
            {
                configsRequested = true;
                await FetchConfigs();
            }
        }

        private async UniTask FetchConfigs()
        {
            try
            {
                Debug.Log("Try fetch configs");
                var actionConfig = await WorldApiClient.GetActionConfigs();

                //Set Entity Configs
                var entityConfigsByWolrdResult =
                    await WorldUtil.ProcessWorldCall<IEnumerable<EntityConfig>>(
                        async e => await e.GetEntityConfigs(),
                        //HERE: You can specify all World's Ids you want to fetch entity configs from
                        Env.CanisterIds.WORLD
                    );

                if (entityConfigsByWolrdResult.IsOk)
                {
                    var entityConfigsByWolrd = entityConfigsByWolrdResult.AsOk();

                    var mergedEntities = entityConfigsByWolrd.Merge<string, EntityConfig>();

                    UserUtil.UpdateData(mergedEntities.Map(e =>
                    {
                        DataTypes.EntityConfig ec = new(e.key, e.value);
                        return ec;
                    }).ToArray());
                }
                else
                {
                    throw new(entityConfigsByWolrdResult.AsErr());
                }

                //Set Action Configs
                UserUtil.UpdateData(actionConfig.Map(e =>
                {
                    DataTypes.ActionConfig ac = new(e);
                    return ac;
                }).ToArray());

                //HERE: YOU CAN REQUEST TO LOAD OTHER TOKENS BALANCES
                UserUtil.RequestData<DataTypes.TokenConfig>(Env.CanisterIds.ICP_LEDGER);
                UserUtil.RequestData<DataTypes.TokenConfig>(Env.CanisterIds.ICRC_LEDGER);
            }
            catch (Exception e)
            {
                configsRequested = false;
            }
        }

        //
        #region Fetch
        private async UniTask FetchEntities()
        {
            await UniTask.SwitchToMainThread();

            var loginDataResult = UserUtil.GetLogInData();

            if (loginDataResult.IsErr)
            {
                throw new(loginDataResult.AsErr());
            }

            var loginData = loginDataResult.AsOk();

            var userNodeIdResult = await WorldHub.GetUserNodeCanisterId(loginData.principal);

            if (userNodeIdResult.Tag == Candid.WorldHub.Models.ResultTag.Err)
            {
                throw new(userNodeIdResult.AsErr());
            }

            var userNodeId = userNodeIdResult.AsOk();

            var userNodeInterface = new UserNodeApiClient(loginData.agent, Principal.FromText(userNodeId));

            var getUserGameDataResult = await userNodeInterface.GetAllUserWorldEntities(loginData.principal, Env.CanisterIds.WORLD);

            List<UserNode.Models.Entity> userGameEntities = null;

            if (getUserGameDataResult.Tag == UserNode.Models.Result_2Tag.Ok)
            {
                userGameEntities = getUserGameDataResult.AsOk();

                UserUtil.UpdateData(userGameEntities.ConvertToDataType());
            }

            //List<World.Models.Entity> userGameEntities = null;

            ////Fetch Game Data
            //var getUserGameDataResult = await WorldApiClient.GetAllUserWorldEntities();

            //if (getUserGameDataResult.Tag == World.Models.Result_4Tag.Ok)
            //{
            //    userGameEntities = getUserGameDataResult.AsOk();

            //    UserUtil.UpdateData(userGameEntities.ConvertToDataType());
            //}
            else
            {
                UserUtil.UpdateData(new DataTypes.Entity[0]);
                $"DATA of type{nameof(DataTypes.Token)} failed to load. Message: {getUserGameDataResult.AsErr()}".Warning(nameof(CandidApiManager));
            }
        }
        private async UniTask FetchActions()
        {
            await UniTask.SwitchToMainThread();

            var loginDataResult = UserUtil.GetLogInData();

            if (loginDataResult.IsErr)
            {
                throw new(loginDataResult.AsErr());
            }

            var loginData = loginDataResult.AsOk();

            var userNodeIdResult = await WorldHub.GetUserNodeCanisterId(loginData.principal);

            if (userNodeIdResult.Tag == Candid.WorldHub.Models.ResultTag.Err)
            {
                throw new(userNodeIdResult.AsErr());
            }

            var userNodeId = userNodeIdResult.AsOk();

            var userNodeInterface = new UserNodeApiClient(loginData.agent, Principal.FromText(userNodeId));

            var getUserGameDataResult = await userNodeInterface.GetAllUserWorldActions(loginData.principal, Env.CanisterIds.WORLD);

            List<UserNode.Models.Action> userGameActions = null;

            if (getUserGameDataResult.Tag == UserNode.Models.Result_3Tag.Ok)
            {
                userGameActions = getUserGameDataResult.AsOk();

                UserUtil.UpdateData(userGameActions.ConvertToDataType());
            }

            //List<World.Models.Action> userGameActions = null;

            //////Fetch Game Data
            //var getUserGameDataResult = await WorldApiClient.GetAllUserWorldActions();

            //if (getUserGameDataResult.Tag == World.Models.Result_5Tag.Ok)
            //{
            //    userGameActions = getUserGameDataResult.AsOk();

            //    UserUtil.UpdateData(userGameActions.ConvertToDataType());
            //}
            else
            {
                UserUtil.UpdateData(new DataTypes.Action[0]);
                $"DATA of type{nameof(DataTypes.Action)} failed to load. Message: {getUserGameDataResult.AsErr()}".Warning(nameof(CandidApiManager));
            }
        }
        //
        private async UniTask FetchToken(FetchDataReq<DataTypes.Token> req)
        {
            await UniTask.SwitchToMainThread();

            try
            {
                if (req.optional == null)
                {
                    throw new($"optional value of {nameof(FetchDataReq<DataTypes.Token>)} cannot be null, it must have a string Token Canister Id value");
                }

                var tokenCanisterId = req.optional.ToString();

                if (string.IsNullOrEmpty(tokenCanisterId)) tokenCanisterId = Env.CanisterIds.ICP_LEDGER;

                var getLoginDataResult = UserUtil.GetLogInData();

                if (getLoginDataResult.Tag == UResultTag.Err)
                {
                    throw new(getLoginDataResult.AsErr());
                }

                var loginData = getLoginDataResult.AsOk();

                ///If "tokenCanisterId" is empty then fetch all tokens
                if (tokenCanisterId == Env.CanisterIds.ICP_LEDGER)
                {
                    var tokenInterface = new IcpLedgerApiClient(loginData.agent, Principal.FromText(tokenCanisterId));
                    var baseUnitAmountIcp = await tokenInterface.AccountBalance(new AccountBalanceArgs(CandidUtil.HexStringToByteArray(loginData.accountIdentifier).ToList()));
                    UserUtil.UpdateData(new DataTypes.Token(tokenCanisterId, baseUnitAmountIcp.E8s));
                }
                ///If "tokenCanisterId" is ICP_LEDGER then ICP token
                else
                {
                    var tokenInterface = new IcrcLedgerApiClient(loginData.agent, Principal.FromText(Env.CanisterIds.ICRC_LEDGER));
                    var baseUnitAmount = await tokenInterface.Icrc1BalanceOf(new IcrcLedger.Models.Account__2(Principal.FromText(loginData.principal), new OptionalValue<List<byte>>()));
                    baseUnitAmount.TryToUInt64(out ulong _baseUnitAmount);
                    UserUtil.UpdateData(new DataTypes.Token(tokenCanisterId, _baseUnitAmount));
                }
            }
            catch (Exception e)
            {
                $"DATA of type{nameof(DataTypes.Token)} failed to load. Message: {e.Message}".Warning(nameof(CandidApiManager));
            }
        }
        private async UniTask FetchTokenConfig(FetchDataReq<DataTypes.TokenConfig> req)
        {
            await UniTask.SwitchToMainThread();

            try
            {
                if (req.optional == null)
                {
                    throw new($"optional value of {nameof(FetchDataReq<DataTypes.Token>)} cannot be null, it must have a string Token Canister Id value");
                }

                var tokenCanisterId = req.optional.ToString();

                if (string.IsNullOrEmpty(tokenCanisterId)) tokenCanisterId = Env.CanisterIds.ICP_LEDGER;

                if (tokenCanisterId == Env.CanisterIds.ICP_LEDGER)
                {
                    var tokenInterface = new IcpLedgerApiClient(cachedAnonAgent.Value, Principal.FromText(tokenCanisterId));
                    var decimals = await tokenInterface.Icrc1Decimals();
                    var name = await tokenInterface.Icrc1Name();
                    var symbol = await tokenInterface.Symbol();
                    var fee = await tokenInterface.Icrc1Fee();
                    fee.TryToUInt64(out ulong _fee);

                    UserUtil.UpdateData(new DataTypes.TokenConfig(tokenCanisterId, name, symbol.Symbol, decimals, _fee));
                }
                else
                {
                    var tokenInterface = new IcrcLedgerApiClient(cachedAnonAgent.Value, Principal.FromText(tokenCanisterId));
                    var decimals = await tokenInterface.Icrc1Decimals();
                    var name = await tokenInterface.Icrc1Name();
                    var symbol = await tokenInterface.Icrc1Symbol();
                    var fee = await tokenInterface.Icrc1Fee();
                    fee.TryToUInt64(out ulong _fee);

                    UserUtil.UpdateData(new DataTypes.TokenConfig(tokenCanisterId, name, symbol, decimals, _fee));
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        #region NFTs
        private async UniTask<DataTypes.NftCollection> GetNfts(string collectionId, string collectionName)
        {
            await UniTask.SwitchToMainThread();

            var getLoginDataResult = UserUtil.GetLogInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getLoginDataResult.AsErr());
                return null;
            }

            var loginData = getLoginDataResult.AsOk();

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return null;
            }

            var api = new Extv2StandardApiClient(getAgentResult.AsOk(), Principal.FromText(collectionId));
            var registry = await api.GetRegistry(); // In the returned ValueTuple, the UInt32 is the token index, and the String is the address that owns it
            var collection = new DataTypes.NftCollection() { canister = collectionId, collectionName = collectionName };
            foreach (var value in registry)
            {
                if (string.Equals(value.F1, loginData.accountIdentifier)) // Checks that the address that owns the NFT is same as your address
                {
                    var tokenIdentifier = await WorldHub.GetTokenIdentifier(collection.canister, value.F0);

                    collection.tokens.Add(
                        new(
                            collection.canister,
                            value.F0,
                            tokenIdentifier,
                            $"https://{collection.canister}.raw.icp0.io/?&tokenid={tokenIdentifier}&type=thumbnail",
                            ""));
                }
            }

            return collection;
        }

        private async UniTask FetchNfts(NftCollectionToFetch req)
        {
            await UniTask.SwitchToMainThread();

            try
            {
                string collectionId = req.collectionId;
                string collectionName = req.collectionName;

                if (string.IsNullOrEmpty(collectionId))
                {
                    throw new($"collectionId value of {nameof(FetchDataReq<DataTypes.NftCollection>)} cannot be an empty string, specify some CollectionId");
                }
                if (string.IsNullOrEmpty(collectionName))
                {
                    throw new($"collectionName value of {nameof(FetchDataReq<DataTypes.NftCollection>)} cannot be an empty string, specify some Collection Name");
                }

                var collection = await GetNfts(collectionId, collectionName) ?? throw new($"collection value of {nameof(FetchDataReq<DataTypes.NftCollection>)} is Null");

//#if UNITY_EDITOR
                $"Standard Collections\nCount: {collection.tokens.Count}\nElements:  {collection.tokens.Reduce(k => $" - {k.index}", "\n")}".Log(nameof(CandidApiManager));
//#endif
                UserUtil.UpdateData(collection);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        #endregion

        #region BoomDao NFTs

        private async UniTask<string> GetNftTokenIdentifier(DataTypes.NftCollection collection, uint index)
        {
            return await WorldHub.GetTokenIdentifier(collection.canister, index);
        }

        private async UniTask<OptionalValue<Extv2Boom.Models.Metadata>> GetNftMetadata(Extv2BoomApiClient api, uint index)
        {
            return await api.GetTokenMetadata(index);
        }

        private async UniTask GetPagedRegistry(Extv2BoomApiClient api, DataTypes.NftCollection collection, uint index)
        {
            await UniTask.SwitchToMainThread();

            try
            {
                Debug.Log("-- Try Fetch NFTs from collection of id: " + collection.canister);
                var getAccountIdentifierResult = UserUtil.GetAccountIdentifier();

                if (getAccountIdentifierResult.Tag == UResultTag.Err)
                {
                    Debug.LogError(getAccountIdentifierResult.AsErr());
                    return;
                }

                var accountIdentifier = getAccountIdentifierResult.AsOk();

                var pagedRegistry = await api.GetPagedRegistry(index); // We used paged registries for Boom NFTs
                List<uint> indexes = new();
                List<UniTask<string>> asyncTokenIdFunctions = new();
                List<UniTask<OptionalValue<Extv2Boom.Models.Metadata>>> asyncMetadataFunctions = new();
                foreach (var value in pagedRegistry)
                {
                    if (string.Equals(value.F1,
                        accountIdentifier.value)) // Checks that the address that owns the NFT is same as your address
                    {
                        Debug.Log($"-- Try fetch Token Metadata of index: {value.F0}");

                        indexes.Add(value.F0);
                        asyncMetadataFunctions.Add(GetNftMetadata(api, value.F0));
                        asyncTokenIdFunctions.Add(GetNftTokenIdentifier(collection, value.F0));
                    }
                }

                var metadataResults = await UniTask.WhenAll(asyncMetadataFunctions);
                var tokenIdResults = await UniTask.WhenAll(asyncTokenIdFunctions);

                if (metadataResults.Length != indexes.Count)
                {
                    Debug.LogError("Metadata array is not same length as NFT index array. Index length=" + indexes.Count + "  Metadata length=" + metadataResults.Length);
                    return;
                }

                if (tokenIdResults.Length != indexes.Count)
                {
                    Debug.LogError("Token id array is not same length as NFT index array. Index length=" + indexes.Count + "  Token id length=" + tokenIdResults.Length);
                    return;
                }

                for (int i = 0; i < indexes.Count; i++)
                {
                    string metadata = metadataResults[i].ValueOrDefault.AsNonfungible().Metadata.ValueOrDefault
                        .AsJson();
                    Debug.Log($"-- Nft metadata fetched of index: {indexes[i]}");


                    collection.tokens.Add(
                        new(
                            collection.canister,
                            indexes[i],
                            tokenIdResults[i],
                            $"https://{collection.canister}.raw.icp0.io/?&tokenid={tokenIdResults[i]}&type=thumbnail",
                            metadata));
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        private async UniTask<DataTypes.NftCollection> GetBoomDaoNfts(string collectionId, string collectionName)
        {
            await UniTask.SwitchToMainThread();

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getAgentResult.AsErr());
                return null;
            }

            var api = new Extv2BoomApiClient(getAgentResult.AsOk(), Principal.FromText(collectionId));
            Debug.Log("-- Try Fetch BoomDao Collection Supply");
            var result = await api.Supply(""); // Query the NFT supply so we can calculate the amount of pages in the NFT registry
            Debug.Log("-- BoomDao Collection Supply has been fetched");

            UnboundedUInt supply = (UnboundedUInt)result.Value;
            int pages = (int)supply / 10000;

            var collection = new DataTypes.NftCollection() { canister = collectionId, collectionName = collectionName };

            List<UniTask> asyncFunctions = new();
            for (uint i = 0; i <= pages; i++)
            {
                asyncFunctions.Add(GetPagedRegistry(api, collection, i));
            }

            await UniTask.WhenAll(asyncFunctions);

            return collection;
        }
        private async UniTask FetchBoomDaoNfts(NftCollectionToFetch req)
        {
            await UniTask.SwitchToMainThread();

            try
            {
                string collectionId = req.collectionId;
                string collectionName = req.collectionName;

                if (string.IsNullOrEmpty(collectionId))
                {
                    throw new($"collectionId value of {nameof(FetchDataReq<DataTypes.NftCollection>)} cannot be an empty string, specify some CollectionId");
                }
                if (string.IsNullOrEmpty(collectionName))
                {
                    throw new($"collectionName value of {nameof(FetchDataReq<DataTypes.NftCollection>)} cannot be an empty string, specify some Collection Name");
                }

                var collection = await GetBoomDaoNfts(collectionId, collectionName) ?? throw new($"collection value of {nameof(FetchDataReq<DataTypes.NftCollection>)} is Null");

//#if UNITY_EDITOR
                $"BoomDao Collections\nCount: {collection.tokens.Count}\nElements: {collection.tokens.Reduce(k => $" - {k.index}", "\n")}".Log(nameof(CandidApiManager));
//#endif
                UserUtil.UpdateData(collection);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        #endregion
        private async UniTask FetchStakes()
        {
            await UniTask.SwitchToMainThread();

            var getLoginDataResult = UserUtil.GetLogInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getLoginDataResult.AsErr());
                return;
            }

            var loginData = getLoginDataResult.AsOk();

            List<Candid.StakingHub.Models.Stake> candidStake = await StakingHubApiClient.GetUserStakes(loginData.principal);

            var stakes = candidStake.Map(k => new DataTypes.Stake((uint)k.Amount, k.CanisterId, k.BlockIndex.ValueOrDefault, k.TokenType));

            UserUtil.UpdateData(stakes.ToArray());
        }

        private async UniTask FetchListings()
        {
            await UniTask.SwitchToMainThread();

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(Env.Nfts.BOOM_COLLECTION_CANISTER_ID));

            var listingResult = await collectionInterface.Listings();

            List<DataTypes.Listing> listing = new();

            foreach (var item in listingResult)
            {
                var tokenIdentifier = await CandidApiManager.Instance.WorldHub.GetTokenIdentifier(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, item.F0);
                listing.Add(new(tokenIdentifier, item));
            }

            UserUtil.UpdateData(listing.ToArray());
        }

        #endregion


        #region FetchHandlers
        private void FetchHandler(FetchDataReq<DataTypes.Entity> req)
        {
            $"DATA of type {nameof(DataTypes.Entity)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchEntities().Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.Action> req)
        {
            $"DATA of type {nameof(DataTypes.Action)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchActions().Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.Token> req)
        {
            $"DATA of type {nameof(DataTypes.Token)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchToken(req).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.TokenConfig> req)
        {
            $"DATA of type {nameof(DataTypes.TokenConfig)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchTokenConfig(req).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.NftCollection> req)
        {
            NftCollectionToFetch arg = (NftCollectionToFetch)req.optional;
            if(arg == null)
            {
                $"req.optional must be of type {nameof(NftCollectionToFetch)}".Error(nameof(CandidApiManager));
                return;
            }

            if(arg.isStandard)
            {
                $"DATA of type {nameof(DataTypes.NftCollection)} Standard was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

                FetchNfts(arg).Forget();
            }
            else
            {
                $"DATA of type {nameof(DataTypes.NftCollection)} BoomDao was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

                FetchBoomDaoNfts(arg).Forget();
            }

        }
        private void FetchHandler(FetchDataReq<DataTypes.Stake> req)
        {
            $"DATA of type {nameof(DataTypes.Stake)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchStakes().Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.Listing> req)
        {
            $"DATA of type {nameof(DataTypes.Listing)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchListings().Forget();
        }
        #endregion
    }
}