using System;
using System.Collections.Generic;
using System.Linq;
using Candid.ext_v2_standard;
using Candid.extv2_boom;
using Candid.IcpLedger;
using Candid.IcpLedger.Models;
using Candid.Icrc1Ledger;
using Candid.StakingHub;
using Candid.World;
using Candid.World.Models;
using Candid.WorldHub;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using Mono.CSharp;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;


namespace Candid
{
    public class CandidApiManager : MonoBehaviour
    {
        public enum CanisterActionType
        {
            Update,
            Query
        }

        // ICP.NET
        public InitValue<IAgent> cachedAnonAgent;

        // Canister APIs
        public WorldApiClient WorldApiClient { get; private set; }
        public WorldHubApiClient WorldHub { get; private set; }
        public StakingHubApiClient StakingHubApiClient { get; private set; }

        public IcpLedgerApiClient IcpLedgerApiClient { get; private set; }

        [SerializeField, ShowOnly] string paymentCanisterOfferIdentifier;
        [SerializeField, ShowOnly] string paymentCanisterStakeIdentifier;
        public static string PaymentCanisterOfferIdentifier { get { return Instance.paymentCanisterOfferIdentifier; } }
        public static string PaymentCanisterStakeIdentifier { get { return Instance.paymentCanisterStakeIdentifier; } }

        public InitValue<string> cachedUserAddress;

        bool areDependenciesReady;
        [SerializeField, ShowOnly] bool fetchingEntities;

        // Instance
        public static CandidApiManager Instance;


        private void Awake()
        {
            IAgent CreateAgentWithRandomIdentity(bool useLocalHost = false)
            {
                IAgent randomAgent = null;

                try
                {
                    if (useLocalHost)
                        randomAgent = new HttpAgent(Ed25519Identity.Generate(), new Uri("http://localhost:4943"));
                    else
                        randomAgent = new HttpAgent(Ed25519Identity.Generate());
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

            UserUtil.RegisterToRequestData<DataTypes.Token>(FetchTokens);
            UserUtil.RegisterToRequestData<DataTypes.Item>(FetchItems);
            UserUtil.RegisterToRequestData<DataTypes.Stat>(FetchStats);
            UserUtil.RegisterToRequestData<DataTypes.NftCollection>(FetchNfts);
            UserUtil.RegisterToRequestData<DataTypes.BoomDaoNftCollection>(FetchBoomDaoNfts);
            UserUtil.RegisterToRequestData<DataTypes.Stake>(FetchStakes);
            UserUtil.RegisterToRequestData<DataTypes.Listing>(FetchListings);

            InitializeCandidApis(CreateAgentWithRandomIdentity(), true);
        }

        private void OnDestroy()
        {
            Broadcast.Unregister<StartLogin>(StartLogin);
            Broadcast.Unregister<UserLogout>(UserLogoutHandler);

            UserUtil.UnregisterToRequestData<DataTypes.Token>(FetchTokens);
            UserUtil.UnregisterToRequestData<DataTypes.Item>(FetchItems);
            UserUtil.UnregisterToRequestData<DataTypes.Stat>(FetchStats);
            UserUtil.UnregisterToRequestData<DataTypes.NftCollection>(FetchNfts);
            UserUtil.UnregisterToRequestData<DataTypes.BoomDaoNftCollection>(FetchBoomDaoNfts);
            UserUtil.UnregisterToRequestData<DataTypes.Stake>(FetchStakes);
            UserUtil.UnregisterToRequestData<DataTypes.Listing>(FetchListings);
        }
        private void StartLogin(StartLogin arg)
        {
            CreateAgentUsingIdentityJson(arg.json, arg.useLocalHost).Forget();
        }
        private void UserLogoutHandler(UserLogout obj)
        {
            InitializeCandidApis(cachedAnonAgent.Value, true);
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
            IcpLedgerApiClient = new IcpLedgerApiClient(agent, Principal.FromText(Env.CanisterIds.ICP_LEDGER));
            StakingHubApiClient = new StakingHubApiClient(agent, Principal.FromText(Env.CanisterIds.STAKING_HUB));
            WorldApiClient = new WorldApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD));

            var userPrincipal = agent.Identity.GetPublicKey().ToPrincipal().ToText();
            var userAccountIdentity = "";
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
                UserUtil.CleanUpData();
            }
            else
            {
                userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);

                "Try Fetch User Data".Log(GetType().Name);

                //Set Login Data
                UserUtil.UpdateLoginData(agent, userPrincipal, userAccountIdentity, asAnon);

                //Fetch userData if not Anon user
                UserUtil.RequestData<DataTypes.Token>();
                UserUtil.RequestData<DataTypes.Item>();
                //UserUtil.RequestData<DataTypes.Stat>();
                UserUtil.RequestData<DataTypes.NftCollection>();
                UserUtil.RequestData<DataTypes.BoomDaoNftCollection>();
                UserUtil.RequestData<DataTypes.Stake>();
            }



            //INIT CONFIGS
            if (areDependenciesReady == false)
            {
                paymentCanisterOfferIdentifier = await CandidApiManager.Instance.WorldHub.GetAccountIdentifier(Env.CanisterIds.PAYMENT_HUB);
                paymentCanisterStakeIdentifier = await CandidApiManager.Instance.WorldHub.GetAccountIdentifier(Env.CanisterIds.STAKING_HUB);


                Debug.Log("Try Fetch Configs");
                areDependenciesReady = true;
                var entitiesConfig = await WorldApiClient.GetEntityConfigs();
                var actionConfig = await WorldApiClient.GetActionConfigs();
                Debug.Log("Configs has been Fetch");


                //Set Entity Configs
                UserUtil.UpdateData(entitiesConfig.Map(e =>
                {
                    DataTypes.EntityConfig ec = new(e);
                    return ec;
                }).ToArray());

                //Set Action Configs
                UserUtil.UpdateData(actionConfig.Map(e =>
                {
                    DataTypes.ActionConfig ac = new(e);
                    return ac;
                }).ToArray());
            }
        }

        public async UniTaskVoid CreateAgentUsingIdentityJson(string json, bool useLocalHost = false)
        {
            await UniTask.SwitchToMainThread();

            try
            {
                var identity = Identity.DeserializeJsonToIdentity(json);

                if (useLocalHost) await InitializeCandidApis(new HttpAgent(identity, new Uri("http://localhost:4943")));
                else await InitializeCandidApis(new HttpAgent(identity, new Uri("https://icp0.io/")));

                Debug.Log("You have logged in");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private async UniTask GetPagedRegistry(Extv2BoomApiClient api, DataTypes.BoomDaoNftCollection collection, uint index)
        {
            var getAccountIdentifierResult = UserUtil.GetAccountIdentifier();

            if (getAccountIdentifierResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getAccountIdentifierResult.AsErr());
                return;
            }

            var accountIdentifier = getAccountIdentifierResult.AsOk();

            var pagedRegistry = await api.GetPagedRegistry(index); // We used paged registries for Boom NFTs

            foreach (var value in pagedRegistry)
            {
                if (string.Equals(value.F1,
                    accountIdentifier.value)) // Checks that the address that owns the NFT is same as your address
                {
                    var metadataResult = await api.ExtGetTokenMetadata(value.F0);
                    string metadata = metadataResult.ValueOrDefault.AsNonfungible().Metadata.ValueOrDefault
                        .AsJson();

                    var tokenIdentifier = await WorldHub.GetTokenIdentifier(collection.canisterId, value.F0);

                    collection.tokens.Add(new()
                    {
                        tokenIdentifier = tokenIdentifier,
                        name = collection.name,
                        canister = collection.canisterId,
                        index = value.F0,
                        metadata = metadata
                    });
                }
            }
        }

        #region Fetch

        private async void FetchEntitiesHandler()
        {
            if (fetchingEntities) return;
            fetchingEntities = true;

            List<World.Models.Entity> userGameEntities = null;
            string jobId = EnqueueJobManager.Instance.EnqueueJob(() =>
            {
                (DataTypes.Item[] items, DataTypes.Stat[] stats) = userGameEntities.ProcessEntities();

                UserUtil.UpdateData(items);
                UserUtil.UpdateData(stats);
            });

            $"> > > Try to fetch game user data".Log();

            ////Fetch Game Data
            var getUserGameDataResult = await WorldApiClient.GetAllUserWorldEntities();

            if (getUserGameDataResult.Tag == World.Models.Result_4Tag.Ok)
            {
                userGameEntities = getUserGameDataResult.AsOk();
                Debug.Log($"Entities: {JsonConvert.SerializeObject(userGameEntities)}");
                EnqueueJobManager.Instance.ExecuteJob(jobId);

            }
            else
            {
                EnqueueJobManager.Instance.ExecuteJob(jobId);

                $"> > > Game User Data: Could not find Game data, msg: {getUserGameDataResult.AsErr()}".Warning();
            }

            fetchingEntities = false;
        }
        //
        private async void FetchTokens(FetchDataReq<DataTypes.Token> req)
        {
            var tokenCanisterId = req.optional == null ? "" : req.optional.ToString();

            try
            {
                var getLoginDataResult = UserUtil.GetSignInData();

                if (getLoginDataResult.Tag == UResultTag.Err)
                {
                    Debug.LogError(getLoginDataResult.AsErr());
                    return;
                }

                var loginData = getLoginDataResult.AsOk();

                ///If "tokenCanisterId" is empty then fetch all tokens
                if (string.IsNullOrEmpty(tokenCanisterId))
                {
                    var rawBalance = await CandidApiManager.Instance.IcpLedgerApiClient.AccountBalance(new AccountBalanceArgs(CandidUtil.HexStringToByteArray(loginData.accountIdentifier).ToList()));
                    var decimalCount = await CandidApiManager.Instance.IcpLedgerApiClient.Decimals();

                    var icrcLedger = new Icrc1LedgerApiClient(loginData.agent, Principal.FromText(Env.CanisterIds.ICRC_LEDGER));
                    var icrcRawBalance = await icrcLedger.Icrc1BalanceOf(new Icrc1Ledger.Models.Account(Principal.FromText(loginData.principal), new OptionalValue<List<byte>>()));
                    var icrcDecimalCount = await icrcLedger.Icrc1Decimals();
                    var name = await icrcLedger.Icrc1Name();

                    ///YOU CAN FETCH OTHER TOKENS DATA HERE AS DESIRED

                    ///

                    UserUtil.UpdateData(
                        new DataTypes.Token(Env.CanisterIds.ICP_LEDGER, "ICP", rawBalance.E8s, decimalCount.Decimals),
                        new DataTypes.Token(Env.CanisterIds.ICRC_LEDGER, name, (ulong)icrcRawBalance, icrcDecimalCount)
                        ///THEN, AFTER FETCHING MORE TOKENS DATA YOU CAN SET THEM HERE
                        );
                }
                ///If "tokenCanisterId" is ICP_LEDGER then ICP token
                else if (tokenCanisterId == Env.CanisterIds.ICP_LEDGER)
                {
                    var rawBalance = await CandidApiManager.Instance.IcpLedgerApiClient.AccountBalance(new AccountBalanceArgs(CandidUtil.HexStringToByteArray(loginData.accountIdentifier).ToList()));
                    var decimalCount = await CandidApiManager.Instance.IcpLedgerApiClient.Decimals();
                    UserUtil.UpdateData(new DataTypes.Token(Env.CanisterIds.ICP_LEDGER, "ICP", rawBalance.E8s, decimalCount.Decimals));
                }
                ///Else fetch ICRC token
                else
                {
                    var icrcLedger = new Icrc1LedgerApiClient(loginData.agent, Principal.FromText(Env.CanisterIds.ICRC_LEDGER));
                    var rawBalance = await icrcLedger.Icrc1BalanceOf(new Icrc1Ledger.Models.Account(Principal.FromText(loginData.principal), new OptionalValue<List<byte>>()));
                    var decimalCount = await icrcLedger.Icrc1Decimals();
                    var name = await icrcLedger.Icrc1Name();

                    UserUtil.UpdateData(new DataTypes.Token(Env.CanisterIds.ICRC_LEDGER, name, (ulong)rawBalance, decimalCount));
                }
            }
            catch
            {
                ///If "tokenCanisterId" is empty then fetch all tokens
                if (string.IsNullOrEmpty(tokenCanisterId))
                {
                    UserUtil.UpdateData(
                        new DataTypes.Token(Env.CanisterIds.ICP_LEDGER, "ICP", 0, 0),
                        new DataTypes.Token(Env.CanisterIds.ICRC_LEDGER, name, 0, 0)
                        );
                }
                ///If "tokenCanisterId" is ICP_LEDGER then ICP token
                else if (tokenCanisterId == Env.CanisterIds.ICP_LEDGER)
                {
                    UserUtil.UpdateData(new DataTypes.Token(Env.CanisterIds.ICP_LEDGER, "ICP", 0, 0));
                }
                ///Else fetch ICRC token
                else
                {
                    UserUtil.UpdateData(new DataTypes.Token(Env.CanisterIds.ICRC_LEDGER, name, 0, 0));
                }
            }
        }
        private void FetchItems(FetchDataReq<DataTypes.Item> req)
        {
            FetchEntitiesHandler();
        }
        private void FetchStats(FetchDataReq<DataTypes.Stat> req)
        {
            FetchEntitiesHandler();
        }

        private async void FetchNfts(FetchDataReq<DataTypes.NftCollection> req)
        {
            async UniTask GetNfts(DataTypes.NftCollection collection)
            {
                var getLoginDataResult = UserUtil.GetSignInData();

                if (getLoginDataResult.Tag == UResultTag.Err)
                {
                    Debug.LogError(getLoginDataResult.AsErr());
                    return;
                }

                var loginData = getLoginDataResult.AsOk();

                var getAgentResult = UserUtil.GetAgent();

                if (getAgentResult.Tag == UResultTag.Err)
                {
                    Debug.Log(getAgentResult.AsErr());
                    return;
                }
                var api = new ExtV2StandardApiClient(getAgentResult.AsOk(), Principal.FromText(collection.canisterId));
                List<ValueTuple<UInt32, String>> registry = await api.GetRegistry(); // In the returned ValueTuple, the UInt32 is the token index, and the String is the address that owns it

                foreach (var value in registry)
                {
                    if (string.Equals(value.Item2, loginData.accountIdentifier)) // Checks that the address that owns the NFT is same as your address
                    {
                        var tokenIdentifier = await WorldHub.GetTokenIdentifier(collection.canisterId, value.Item1);
                        collection.tokens.Add(new()
                        {
                            tokenIdentifier = tokenIdentifier,
                            name = collection.name,
                            canister = collection.canisterId,
                            index = value.Item1,
                            url = $"https://{collection.canisterId}.raw.icp0.io/?&tokenid={tokenIdentifier}&type=thumbnail"
                        });
                    }
                }
            }

            Debug.Log("Try Fetch Nft Collections");

            List<DataTypes.NftCollection> nftsCollections = new List<DataTypes.NftCollection>() // NFT collections we want to fetch that use the EXT V2 standard
	        {
                new() {name = "The Moonwalkers", canisterId = "er7d4-6iaaa-aaaaj-qac2q-cai"},
                new() {name = "Poked Bots", canisterId = "bzsui-sqaaa-aaaah-qce2a-cai"}
            };

            string jobId = EnqueueJobManager.Instance.EnqueueJob(() =>
            {
                UserUtil.UpdateData(nftsCollections.ToArray());
            });

            List<UniTask> asyncFunctions = new List<UniTask>();


            foreach (var collection in nftsCollections)
            {
                asyncFunctions.Add(GetNfts(collection));
            }

            await UniTask.WhenAll(asyncFunctions);

            //
            EnqueueJobManager.Instance.ExecuteJob(jobId);
        }

        private async void FetchBoomDaoNfts(FetchDataReq<DataTypes.BoomDaoNftCollection> req)
        {
            async UniTask GetBoomDaoNfts(DataTypes.BoomDaoNftCollection collection)
            {
                var getAgentResult = UserUtil.GetAgent();

                if (getAgentResult.Tag == UResultTag.Err)
                {
                    Debug.Log(getAgentResult.AsErr());
                    return;
                }

                var api = new Extv2BoomApiClient(getAgentResult.AsOk(), Principal.FromText(collection.canisterId));
                var result = await api.Supply(""); // Query the NFT supply so we can calculate the amount of pages in the NFT registry
                UnboundedUInt supply = (UnboundedUInt)result.Value;
                int pages = (int)supply / 10000;

                List<UniTask> asyncFunctions = new();
                for (uint i = 0; i <= pages; i++)
                {
                    asyncFunctions.Add(GetPagedRegistry(api, collection, i));
                }

                await UniTask.WhenAll(asyncFunctions);
            }

            Debug.Log("Try Fetch BoomDao Nft Collections");

            List<DataTypes.BoomDaoNftCollection> boomDaoNftsCollections = new() // NFT collections we want to fetch that were deployed by Plethora and that use Hitesh's modified EXT V2 contract
	        {
                new() {name = "Plethora Items", canisterId = Env.Nfts.BOOM_COLLECTION_CANISTER_ID },
            };

            string jobId = EnqueueJobManager.Instance.EnqueueJob(() =>
            {
                UserUtil.UpdateData(boomDaoNftsCollections.ToArray());
            });

            List<UniTask> asyncFunctions = new();

            foreach (var collection in boomDaoNftsCollections)
            {
                asyncFunctions.Add(GetBoomDaoNfts(collection));
            }

            await UniTask.WhenAll(asyncFunctions);
            //
            EnqueueJobManager.Instance.ExecuteJob(jobId);
        }

        private async void FetchStakes(FetchDataReq<DataTypes.Stake> req)
        {
            Debug.Log("Fetch Staking Data");

            var getLoginDataResult = UserUtil.GetSignInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getLoginDataResult.AsErr());
                return;
            }

            var loginData = getLoginDataResult.AsOk();

            List<Candid.StakingHub.Models.Stake> candidStake = new();
            string jobId = EnqueueJobManager.Instance.EnqueueJob(() =>
            {
                var stakes = candidStake.Map(k => new DataTypes.Stake((uint)k.Amount, k.CanisterId, k.BlockIndex.ValueOrDefault, k.TokenType));

                UserUtil.UpdateData(stakes.ToArray());

                Debug.Log("Staking Data Fetched");
            });

            candidStake = await StakingHubApiClient.GetUserStakes(loginData.principal);

            EnqueueJobManager.Instance.ExecuteJob(jobId);
        }

        private async void FetchListings(FetchDataReq<DataTypes.Listing> req)
        {
            Debug.Log("Fetch Listings");

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(Env.Nfts.BOOM_COLLECTION_CANISTER_ID));

            var listingResult = await collectionInterface.Listings();

            Dictionary<string, Extv2BoomApiClient.ListingsArg0Item> listing = new();

            foreach (var item in listingResult)
            {
                var tokenIdentifier = await CandidApiManager.Instance.WorldHub.GetTokenIdentifier(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, item.F0);
                listing.Add(tokenIdentifier, item);
            }

            Debug.Log("Apply Listings");
            UserUtil.UpdateData(listing.Map(e => new DataTypes.Listing(e.Key, e.Value)).ToArray());
        }

        #endregion
    }
}