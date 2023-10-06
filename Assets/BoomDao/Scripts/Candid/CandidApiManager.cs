namespace Candid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candid.Extv2Standard;
    using Candid.Extv2Boom;
    using Candid.IcpLedger;
    using Candid.IcpLedger.Models;
    using Candid.World;
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
    using Candid.IcrcLedger;
    using Unity.VisualScripting;
    using Candid.UserNode;
    using Boom;

    public class CandidApiManager : MonoBehaviour
    {
        // Instance
        public static CandidApiManager Instance { get; private set; }

        //Cache
        [ShowOnly, SerializeField] InitValue<IAgent> cachedAnonAgent;
        [ShowOnly, SerializeField] InitValue<string> cachedUserAddress;

        // Canister APIs
        public WorldApiClient WorldApiClient { get; private set; }
        public WorldHubApiClient WorldHub { get; private set; }

        [SerializeField, ShowOnly] string principal;
        [SerializeField, ShowOnly] string paymentHubIdentifier;
        public static string PaymentHubIdentifier { get { return Instance.paymentHubIdentifier; } }

        bool areDependenciesReady;
        bool configsRequested;
        public bool CanLogIn { 
            get {
                bool isLoadingDataValid = UserUtil.IsLoginDataValid();
                bool isAnonLoggedIn = UserUtil.IsAnonLoggedIn();
                bool isConfigReady = UserUtil.IsDataValid<DataTypes.Config>();
                bool areActionsReady = UserUtil.IsDataValid<DataTypes.Action>();

                return isConfigReady && areActionsReady && isLoadingDataValid && isAnonLoggedIn;
            } 
        }



        private void Awake()
        {
            IAgent CreateAgentWithRandomIdentity(bool useLocalHost = false)
            {
                IAgent randomAgent = null;

                var httpClient = new UnityHttpClient();

                try
                {
                    if (useLocalHost)
                        randomAgent = new HttpAgent(Ed25519Identity.Generate(), new Uri("http://localhost:4943"));
                    else
                        randomAgent = new HttpAgent(httpClient, Ed25519Identity.Generate());
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
            UserUtil.RegisterToRequestData<DataTypes.TokenMetadata>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.Entity>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.ActionState>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.NftCollection>(FetchHandler);
            UserUtil.RegisterToRequestData<DataTypes.Listing>(FetchHandler);

            UserUtil.RegisterToDataChange<DataTypes.Config>(CanLogingEventHandler);
            UserUtil.RegisterToDataChange<DataTypes.Action>(CanLogingEventHandler);
            UserUtil.RegisterToLoginDataChange(CanLogingEventHandler);

            InitializeCandidApis(CreateAgentWithRandomIdentity(), true).Forget();
        }

        private void OnDestroy()
        {
            Broadcast.Unregister<StartLogin>(StartLogin);
            Broadcast.Unregister<UserLogout>(UserLogoutHandler);

            UserUtil.UnregisterToRequestData<DataTypes.Token>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.TokenMetadata>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.Entity>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.ActionState>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.NftCollection>(FetchHandler);
            UserUtil.UnregisterToRequestData<DataTypes.Listing>(FetchHandler);

            UserUtil.UnregisterToDataChange<DataTypes.Config>(CanLogingEventHandler);
            UserUtil.UnregisterToDataChange<DataTypes.Action>(CanLogingEventHandler);
            UserUtil.UnregisterToLoginDataChange(CanLogingEventHandler);
        }
        //
        private void CanLogingEventHandler(DataState<LoginData> state)
        {
            BroadcastState.Invoke(new CanLogin(CanLogIn));
        }
        private void CanLogingEventHandler(DataState<Data<DataTypes.Config>> state)
        {
            BroadcastState.Invoke(new CanLogin(CanLogIn));
        }
        private void CanLogingEventHandler(DataState<Data<DataTypes.Action>> state)
        {
            BroadcastState.Invoke(new CanLogin(CanLogIn));
        }
        //
        private void StartLogin(StartLogin arg)
        {
            if (BroadcastState.TryRead<DataState<LoginData>>(out var dataState))
            {
                if (dataState.IsLoading()) return;
            }

            PlayerPrefs.SetString("walletType", "II");

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

                if (useLocalHost) await InitializeCandidApis(new HttpAgent(identity, new Uri("http://localhost:4943")));
                else await InitializeCandidApis(new HttpAgent(httpClient, identity));

                Debug.Log("You have logged in");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void UserLogoutHandler(UserLogout obj)
        {
            PlayerPrefs.SetString("authTokenId", string.Empty);
            InitializeCandidApis(cachedAnonAgent.Value, true).Forget();
            BroadcastState.Invoke(new CanLogin(CanLogIn));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actiontType">If the type is "Update" then must use the return value once at a time to record the update call</param>
        /// <returns></returns>
        private async UniTask InitializeCandidApis(IAgent agent, bool asAnon = false)
        {
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

                    //Build Interfaces
                    WorldHub = new WorldHubApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD_HUB));
                    WorldApiClient = new WorldApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD));
                }
                //Else fetch required dependencies and catch it
                else
                {
                    //Build Interfaces
                    WorldHub = new WorldHubApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD_HUB));
                    WorldApiClient = new WorldApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD));

                    cachedAnonAgent.Value = agent;

                    userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);
                    cachedUserAddress.Value = userAccountIdentity;
                }


                //Set Login Data
                UserUtil.UpdateLoginData(agent, userPrincipal, userAccountIdentity, asAnon);

                //Clean up logged in user data
                UserUtil.Clean<DataTypes.Token>();
                UserUtil.Clean<DataTypes.Entity>();
                UserUtil.Clean<DataTypes.ActionState>();
                UserUtil.Clean<DataTypes.NftCollection>();
            }
            else
            {
                //Build Interfaces
                WorldHub = new WorldHubApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD_HUB));
                WorldApiClient = new WorldApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD));

                userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);

                "Try Fetch User Data".Log(GetType().Name);

                //HERE: YOU CAN REQUEST FOR THE FIRST TIME ON THE GAME THE USER DATA

                //Set Login Data
                UserUtil.UpdateLoginData(agent, userPrincipal, userAccountIdentity, asAnon);

                // Create UserNode Interface
                var userNodeIdResult = await WorldHub.GetUserNodeCanisterId(userPrincipal);

                if (userNodeIdResult.Tag == Candid.WorldHub.Models.ResultTag.Err)
                {
                    $"{userNodeIdResult.AsErr()}. Therefore, a new user will be created!".Warning(nameof(CandidApiManager));
                    userNodeIdResult = await WorldHub.CreateNewUser(Principal.FromText(userPrincipal));

                    if (userNodeIdResult.Tag == Candid.WorldHub.Models.ResultTag.Err)
                    {
                        throw new(userNodeIdResult.AsErr());
                    }
                }

                var userNodeId = userNodeIdResult.AsOk();

                var userNodeInterface = new UserNodeApiClient(agent, Principal.FromText(userNodeId));

                //USER DATA
                UserUtil.RequestData<DataTypes.Entity>(userNodeInterface);
                UserUtil.RequestData<DataTypes.ActionState>(userNodeInterface);

                //HERE WE FETCH ALL USER TOKEN HOLDING SPECIFIED IN CONFIGS, WE QUERY THEM BY "tag", ITS TAG MUST BE EQUAL TO "token"
                if (EntityUtil.QueryConfigsByTag("token", out var tokenConfigs))
                {
                    tokenConfigs.Iterate(e =>
                    {
                        if (e.GetConfigFieldAs<string>("canister", out var canisterId))
                        {
                            UserUtil.RequestData<DataTypes.Token>(canisterId);
                        }
                        else
                        {
                            $"config of tag \"token\" doesn't have field \"canister\"".Error();
                        }
                    });
                }

                //HERE WE FETCH ALL USER NFT HOLDING SPECIFIED IN CONFIGS, WE QUERY THEM BY "tag", ITS TAG MUST BE EQUAL TO "nft"
                if (EntityUtil.QueryConfigsByTag("nft", out var nftConfigs))
                {
                    nftConfigs.Iterate(e =>
                    {
                        if (!e.GetConfigFieldAs<string>("name", out var collectionName))
                        {
                            $"config of tag \"nft\" doesn't have field \"collectionName\"".Warning();
                        }
                        if (!e.GetConfigFieldAs<string>("description", out var description))
                        {
                            $"config of tag \"nft\" doesn't have field \"description\"".Warning();
                        }
                        if (!e.GetConfigFieldAs<string>("urlLogo", out var urlLogo))
                        {
                            $"config of tag \"nft\" doesn't have field \"urlLogo\"".Warning();
                        }

                        if (!e.GetConfigFieldAs<bool>("isStandard", out var isStandard))
                        {
                            $"config of tag \"nft\" doesn't have field \"isStandard\"".Error();

                            return;
                        }

                        if (!e.GetConfigFieldAs<string>("canister", out var canisterId))
                        {
                            $"config of tag \"nft\" doesn't have field \"canister\"".Error();

                            return;
                        }

                        UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(canisterId, collectionName, description, urlLogo, isStandard));
                    });
                }
            }



            //INIT CONFIGS
            if (areDependenciesReady == false)
            {
                paymentHubIdentifier = await CandidApiManager.Instance.WorldHub.GetAccountIdentifier(Env.CanisterIds.PAYMENT_HUB);

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
                //Set Entity Configs
                var configsByWolrdResult =
                    await WorldUtil.ProcessWorldCall<IEnumerable<DataTypes.Config>>(
                        async (worldInterface, wid) =>
                        {
                            var stableConfigs = await worldInterface.GetAllConfigs();

                            return stableConfigs.Map(e => new DataTypes.Config(e, wid));
                        },
                        //HERE: You can specify all World's Ids you want to fetch entity configs from
                        Env.CanisterIds.WORLD
                    );

                if (configsByWolrdResult.IsOk)
                {
                    Debug.Log("Try fetch configs aa");

                    var entityConfigsByWolrd = configsByWolrdResult.AsOk();

                    var mergedEntities = entityConfigsByWolrd.Map(e => e.Value).Merge();

                    UserUtil.UpdateData(mergedEntities.ToArray());
                }
                else
                {
                    throw new(configsByWolrdResult.AsErr());
                }

                var actions = await WorldApiClient.GetAllActions();

                //Set Action Configs
                UserUtil.UpdateData(actions.Map(e =>
                {
                    DataTypes.Action a = new(e);
                    return a;
                }).ToArray());

                //HERE WE FETCH ALL TOKEN METADATA SPECIFIED IN CONFIGS, WE QUERY THEM BY "tag", ITS TAG MUST BE EQUAL TO "token"
                if (EntityUtil.QueryConfigsByTag("token", out var tokensMetadata))
                {
                    tokensMetadata.Iterate(e =>
                    {
                        if (e.GetConfigFieldAs<string>("canister", out var canisterId))
                        {
                            UserUtil.RequestData<DataTypes.TokenMetadata>(canisterId);
                        }
                        else
                        {
                            $"config of tag \"token\" doesn't have field \"canister\"".Warning();
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);

                configsRequested = false;
            }
        }

        //
        #region Fetch
        private async UniTask FetchEntities(UserNodeApiClient userNodeInterface, string principal)
        {
            await UniTask.SwitchToMainThread();

            var getUserGameDataResult = await userNodeInterface.GetAllUserEntities(principal, Env.CanisterIds.WORLD);

            if (getUserGameDataResult.Tag == UserNode.Models.ResultTag.Ok)
            {
                List<UserNode.Models.StableEntity> userGameEntities = getUserGameDataResult.AsOk();

                UserUtil.UpdateData(userGameEntities.Map(e=> new DataTypes.Entity(e)).ToArray());
            }
            else
            {
                UserUtil.UpdateData(new DataTypes.Entity[0]);
                $"DATA of type{nameof(DataTypes.Token)} failed to load. Message: {getUserGameDataResult.AsErr()}".Warning(nameof(CandidApiManager));
            }
        }
        private async UniTask FetchActionStates(UserNodeApiClient userNodeInterface, string principal)
        {
            await UniTask.SwitchToMainThread();

            var actionStateResult = await userNodeInterface.GetAllUserActionStates(principal, Env.CanisterIds.WORLD);

            List<UserNode.Models.ActionState> actionsStates = null;

            if (actionStateResult.Tag == UserNode.Models.Result_2Tag.Ok)
            {
                actionsStates = actionStateResult.AsOk();

                UserUtil.UpdateData(actionsStates.Map(e => new DataTypes.ActionState(e)).ToArray());
            }
            else
            {
                UserUtil.UpdateData(new DataTypes.ActionState[0]);
                $"DATA of type{nameof(DataTypes.ActionState)} failed to load. Message: {actionStateResult.AsErr()}".Warning(nameof(CandidApiManager));
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

                if (tokenCanisterId == Env.CanisterIds.ICP_LEDGER)
                {
                    var tokenInterface = new IcpLedgerApiClient(loginData.agent, Principal.FromText(tokenCanisterId));
                    var baseUnitAmountIcp = await tokenInterface.AccountBalance(new AccountBalanceArgs(CandidUtil.HexStringToByteArray(loginData.accountIdentifier).ToList()));
                    UserUtil.UpdateData(new DataTypes.Token(tokenCanisterId, baseUnitAmountIcp.E8s));
                }
                else
                {
                    var tokenInterface = new IcrcLedgerApiClient(loginData.agent, Principal.FromText(tokenCanisterId));
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
        private async UniTask FetchTokenConfig(FetchDataReq<DataTypes.TokenMetadata> req)
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

                Debug.Log("FETCH TOKEN OF ID " + tokenCanisterId);

                if (tokenCanisterId == Env.CanisterIds.ICP_LEDGER)
                {
                    var tokenInterface = new IcpLedgerApiClient(cachedAnonAgent.Value, Principal.FromText(tokenCanisterId));

                    var decimals = await tokenInterface.Icrc1Decimals();
                    var name = await tokenInterface.Icrc1Name();
                    var symbol = await tokenInterface.Symbol();
                    var fee = await tokenInterface.Icrc1Fee();
                    fee.TryToUInt64(out ulong _fee);


                    if (EntityUtil.TryGetConfig(e =>
                    {
                        e.GetConfigFieldAs<string>("canister", out var _canister, "");

                        return _canister == tokenCanisterId;
                    }, out var tokenConfig))
                    {
                        tokenConfig.GetConfigFieldAs("description", out var description, "");

                        tokenConfig.GetConfigFieldAs("urlLogo", out var urlLogo, "");

                        UserUtil.UpdateData(new DataTypes.TokenMetadata(tokenCanisterId, name, symbol.Symbol, decimals, _fee, description, urlLogo));
                    }
                }
                else
                {
                    var tokenInterface = new IcrcLedgerApiClient(cachedAnonAgent.Value, Principal.FromText(tokenCanisterId));

                    var decimals = await tokenInterface.Icrc1Decimals();
                    var name = await tokenInterface.Icrc1Name();
                    var symbol = await tokenInterface.Icrc1Symbol();
                    var fee = await tokenInterface.Icrc1Fee();

                    fee.TryToUInt64(out ulong _fee);

                    if(EntityUtil.TryGetConfig(e =>
                    {
                        e.GetConfigFieldAs<string>("canister", out var _canister, "");

                        return _canister == tokenCanisterId;
                    }, out var tokenConfig))
                    {
                        tokenConfig.GetConfigFieldAs("description", out var description, "");

                        tokenConfig.GetConfigFieldAs("urlLogo", out var urlLogo, "");

                        UserUtil.UpdateData(new DataTypes.TokenMetadata(tokenCanisterId, name, symbol, decimals, _fee, description, urlLogo));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        #region NFTs
        private async UniTask<DataTypes.NftCollection> GetNfts(string collectionId, string collectionName, string description, string urlLogo)
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
            var collection = new DataTypes.NftCollection(collectionId, collectionName, description, urlLogo);
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

                if (string.IsNullOrEmpty(collectionId))
                {
                    throw new($"collectionId value of {nameof(FetchDataReq<DataTypes.NftCollection>)} cannot be an empty string, specify some CollectionId");
                }

                var collection = await GetNfts(collectionId, req.name, req.description, req.urlLogo) ?? throw new($"collection value of {nameof(FetchDataReq<DataTypes.NftCollection>)} is Null");

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
                //Debug.Log("-- Try Fetch NFTs from collection of id: " + collection.canister);
                var getAccountIdentifierResult = UserUtil.GetAccountIdentifier();

                if (getAccountIdentifierResult.IsErr)
                {
                    Debug.LogError(getAccountIdentifierResult.AsErr().Value);
                    return;
                }

                var accountIdentifier = getAccountIdentifierResult.AsOk().Value;

                var pagedRegistry = await api.GetPagedRegistry(index); // We used paged registries for Boom NFTs
                List<uint> indexes = new();
                List<UniTask<string>> asyncTokenIdFunctions = new();
                List<UniTask<OptionalValue<Extv2Boom.Models.Metadata>>> asyncMetadataFunctions = new();
                foreach (var value in pagedRegistry)
                {
                    if (string.Equals(value.F1,
                        accountIdentifier)) // Checks that the address that owns the NFT is same as your address
                    {
                        //Debug.Log($"-- Try fetch Token Metadata of index: {value.F0}");

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
                    //Debug.Log($"-- Nft metadata fetched of index: {indexes[i]}");


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
        private async UniTask<DataTypes.NftCollection> GetBoomDaoNfts(string collectionId, string collectionName, string description, string urlLogo)
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

            var collection = new DataTypes.NftCollection(collectionId, collectionName, description, urlLogo);

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

                if (string.IsNullOrEmpty(collectionId))
                {
                    throw new($"collectionId value of {nameof(FetchDataReq<DataTypes.NftCollection>)} cannot be an empty string, specify some CollectionId");
                }

                var collection = await GetBoomDaoNfts(collectionId, req.name, req.description, req.urlLogo) ?? throw new($"collection value of {nameof(FetchDataReq<DataTypes.NftCollection>)} is Null");

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
            
            var userNodeInterface = (UserNodeApiClient) req.optional;

            var getLoginDataResult = UserUtil.GetLogInData();
            var loginData = getLoginDataResult.AsOk();


            FetchEntities(userNodeInterface, loginData.principal).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.ActionState> req)
        {
            $"DATA of type {nameof(DataTypes.ActionState)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            var userNodeInterface = (UserNodeApiClient)req.optional;

            var getLoginDataResult = UserUtil.GetLogInData();
            var loginData = getLoginDataResult.AsOk();

            FetchActionStates(userNodeInterface, loginData.principal).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.Token> req)
        {
            $"DATA of type {nameof(DataTypes.Token)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchToken(req).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypes.TokenMetadata> req)
        {
            $"DATA of type {nameof(DataTypes.TokenMetadata)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

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
        private void FetchHandler(FetchDataReq<DataTypes.Listing> req)
        {
            $"DATA of type {nameof(DataTypes.Listing)} was requested, args: {req.optional.ToSafeString()}".Log(nameof(CandidApiManager));

            FetchListings().Forget();
        }
        #endregion
    }
}