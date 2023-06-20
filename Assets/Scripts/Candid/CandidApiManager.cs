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
using Candid.WorldHub;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace Candid
{
    public class CandidApiManager : MonoBehaviour
    {
        public enum CanisterActionType
        {
            Update,
            Query
        }
        [SerializeField, ShowOnly] bool isUserLoggedIn;
        [SerializeField, ShowOnly] bool isAnonLoggedIn;
        public static bool IsUserLoggedIn { get { return Instance.isUserLoggedIn; } }
        public static bool IsAnonLoggedIn { get { return Instance.isAnonLoggedIn; } }
        public static bool IsLoggedIn { get { return IsUserLoggedIn || IsAnonLoggedIn; } }


        // ICP.NET
        public IAgent Agent { get; private set; }
        public InitValue<IAgent> cachedAnonAgent;

        // Canister APIs
        public WorldApiClient WorldApiClient { get; private set; }
        public WorldHubApiClient WorldHub { get; private set; }
        public StakingHubApiClient StakingHubApiClient { get; private set; }

        public IcpLedgerApiClient IcpLedgerApiClient { get; private set; }
        public Icrc1LedgerApiClient rcLedgerApiClient { get; private set; }

        [SerializeField, ShowOnly] string paymentCanisterOfferIdentifier;
        [SerializeField, ShowOnly] string paymentCanisterStakeIdentifier;
        public static string PaymentCanisterOfferIdentifier { get { return Instance.paymentCanisterOfferIdentifier; } }
        public static string PaymentCanisterStakeIdentifier { get { return Instance.paymentCanisterStakeIdentifier; } }


        [SerializeField, ShowOnly] string userPrincipal;
        [SerializeField, ShowOnly] string userAccountIdentity;
        public static string UserPrincipal { get { return Instance.userPrincipal; } }
        public static string UserAccountIdentity { get { return Instance.userAccountIdentity; } }
        public InitValue<string> cachedUserAddress;

        bool areDependenciesReady;

        // Instance
        public static CandidApiManager Instance;


        private void Awake()
        {
            //Debug.Log(EnqueueJobManager.Instance);

            Instance = this;

            Broadcast.Register<UserLogout>(UserLogoutHandler);
            Broadcast.Register<FetchBalanceReqIcp>(FetchIcpBalanceReqHandler);
            Broadcast.Register<FetchckBalanceReqIcrc>(FetchIcrcBalanceReqHandler);

            InitializeCandidApis(CreateAgentWithRandomIdentity(), true);
        }
        public IAgent CreateAgentWithRandomIdentity(bool useLocalHost = false)
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


        private void OnDestroy()
        {
            Broadcast.Unregister<UserLogout>(UserLogoutHandler);
            Broadcast.Unregister<FetchBalanceReqIcp>(FetchIcpBalanceReqHandler);
            Broadcast.Unregister<FetchckBalanceReqIcrc>(FetchIcrcBalanceReqHandler);
        }

        private void UserLogoutHandler(UserLogout obj)
        {
            Debug.Log("Log out and log in as anon");
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
            this.Agent = agent;

            WorldHub = new WorldHubApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD_HUB));
            IcpLedgerApiClient = new IcpLedgerApiClient(agent, Principal.FromText(Env.CanisterIds.ICP_LEDGER));
            rcLedgerApiClient = new Icrc1LedgerApiClient(agent, Principal.FromText(Env.CanisterIds.ICRC_LEDGER));
            StakingHubApiClient = new StakingHubApiClient(agent, Principal.FromText(Env.CanisterIds.STAKING_HUB));
            WorldApiClient = new WorldApiClient(agent, Principal.FromText(Env.CanisterIds.WORLD));

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

                BroadcastState.Invoke(new DataState<WorldConfigsData>(new WorldConfigsData(entitiesConfig, actionConfig)));
            }


            userPrincipal = agent.Identity.GetPublicKey().ToPrincipal().ToText();
            if (asAnon)
            {
                if (cachedAnonAgent.IsInit && cachedAnonAgent.IsInit)
                {
                    userAccountIdentity = cachedUserAddress.Value;
                }
                else
                {
                    cachedAnonAgent.Value = agent;
                    userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);
                    cachedUserAddress.Value = userAccountIdentity;
                }

                UserCleanup();
            }
            else
            {
                userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);

                //Fetch Core User Data
                TryFetchGameData();

                //FETCH NFTS DATA
                FetchUserNfts();
            }
        }

        private void UserCleanup()
        {
            BroadcastState.ForceInvoke<DataState<UserNodeData>>((e) =>
            {
                e.Clear();
                return e;
            });
            BroadcastState.ForceInvoke<DataState<DabNftsData>>((e) =>
            {
                e.Clear();
                return e;
            });
            BroadcastState.ForceInvoke<DataState<IcpData>>((e) =>
            {
                e.Clear();
                return e;
            });
            BroadcastState.ForceInvoke<DataState<IcrcData>>((e) =>
            {
                e.Clear();
                return e;
            });

            isAnonLoggedIn = true;
            isUserLoggedIn = false;
            Broadcast.Invoke<AnonLogin>(new AnonLogin(userPrincipal, userAccountIdentity));
        }

        private async UniTask TryFetchGameData()
        {
            List<World.Models.Entity> userGameEntities = null;
            string jobId = EnqueueJobManager.Instance.EnqueueJob(() =>
            {
                BroadcastState.ForceInvoke(new DataState<UserNodeData>(new UserNodeData(userGameEntities)));
            });

            $"> > > Try to fetch game user data. Public key to use: {userPrincipal}".Log();

            ////Fetch Game Data
            var getUserGameDataResult = await WorldApiClient.GetAllUserWorldEntities();

            if (getUserGameDataResult.Tag == World.Models.Result_5Tag.Ok)
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


                isUserLoggedIn = true;
                isAnonLoggedIn = false;
                UserUtil.UpdateBalanceReq_Icp();
                UserUtil.UpdateBalanceReq_Rc();
                FetchStakesReqHandler();

                Broadcast.Invoke<UserLogin>(new UserLogin(userPrincipal, userAccountIdentity));
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }


        // EXAMPLE FUNCTION - PLEASE MOVE TO A BETTER PLACE LATER
        public async UniTask FetchUserNfts()
        {
            List<DabNftCollection> nonPlethoraNftCollections = new List<DabNftCollection>() // NFT collections we want to fetch that use the EXT V2 standard
	        {
                new() {name = "The Moonwalkers", canisterId = "er7d4-6iaaa-aaaaj-qac2q-cai"},
                new() {name = "Poked Bots", canisterId = "bzsui-sqaaa-aaaah-qce2a-cai"}
            };

            List<DabNftCollection> plethoraNftCollections = new List<DabNftCollection>() // NFT collections we want to fetch that were deployed by Plethora and that use Hitesh's modified EXT V2 contract
	        {
                new() {name = "Plethora Items", canisterId = Env.Nfts.BOOM_COLLECTION },//"4qmvs-qyaaa-aaaal-ab2rq-cai"},
            };

            string jobId = EnqueueJobManager.Instance.EnqueueJob(() =>
            {
                BroadcastState.ForceInvoke<DataState<DabNftsData>>(e =>
                {
                    e.data = new(nonPlethoraNftCollections, plethoraNftCollections);
                    e.SetAsReady();
                    return e;
                });
            });

            List<UniTask> asyncFunctions = new List<UniTask>();


            foreach (var collection in nonPlethoraNftCollections)
            {
                asyncFunctions.Add(GetNonPlethoraNfts(collection));
            }

            foreach (var collection in plethoraNftCollections)
            {
                asyncFunctions.Add(GetPlethoraNfts(collection));
            }


            await UniTask.WhenAll(asyncFunctions);

            //
            EnqueueJobManager.Instance.ExecuteJob(jobId);
        }

        private async UniTask GetNonPlethoraNfts(DabNftCollection collection)
        {
            var api = new ExtV2StandardApiClient(Agent, Principal.FromText(collection.canisterId));
            List<ValueTuple<UInt32, String>> registry = await api.GetRegistry(); // In the returned ValueTuple, the UInt32 is the token index, and the String is the address that owns it

            foreach (var value in registry)
            {
                if (string.Equals(value.Item2, userAccountIdentity)) // Checks that the address that owns the NFT is same as your address
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
        private async UniTask GetPlethoraNfts(DabNftCollection collection)
        {
            var api = new Extv2BoomApiClient(Agent, Principal.FromText(collection.canisterId));
            var result = await api.Supply(""); // Query the NFT supply so we can calculate the amount of pages in the NFT registry
            UnboundedUInt supply = (UnboundedUInt)result.Value;
            int pages = (int)supply / 10000;

            List<UniTask> asyncFunctions = new List<UniTask>();
            for (uint i = 0; i <= pages; i++)
            {
                asyncFunctions.Add(GetPagedRegistry(api, collection, i));
            }

            await UniTask.WhenAll(asyncFunctions);
        }

        private async UniTask GetPagedRegistry(Extv2BoomApiClient api, DabNftCollection collection, uint index)
        {
            var pagedRegistry = await api.GetPagedRegistry(index); // We used paged registries for Boom NFTs

            foreach (var value in pagedRegistry)
            {
                if (string.Equals(value.F1,
                    userAccountIdentity)) // Checks that the address that owns the NFT is same as your address
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

        private async void FetchIcpBalanceReqHandler(FetchBalanceReqIcp req)
        {
            var rawBalance = await CandidApiManager.Instance.IcpLedgerApiClient.AccountBalance(new AccountBalanceArgs(CandidUtil.HexStringToByteArray(CandidApiManager.UserAccountIdentity).ToList()));
            UserUtil.SetBalanceIcp((long)rawBalance.E8s);
        }
        private async void FetchIcrcBalanceReqHandler(FetchckBalanceReqIcrc req)
        {
            Debug.Log("Try Fetch ICRC balance");
            var rawBalance = await CandidApiManager.Instance.rcLedgerApiClient.Icrc1BalanceOf(new Icrc1Ledger.Models.Account(Principal.FromText(UserPrincipal), new OptionalValue<List<byte>>()));
            var decimalCount = await CandidApiManager.Instance.rcLedgerApiClient.Icrc1Decimals();
            var name = await CandidApiManager.Instance.rcLedgerApiClient.Icrc1Name();

            UserUtil.SetBalanceIcrc(name, (long)rawBalance, decimalCount);
        }

        private async void FetchStakesReqHandler()
        {
            var result = await StakingHubApiClient.GetUserStakes(userPrincipal);

            BroadcastState.ForceInvoke<DataState<StakeData>>(e =>
            {
                e.data.stakes = e.data.stakes ?? new();
                if (result != null)
                {
                    result.Debug(j => $"id: {j.CanisterId}, type {j.TokenType}, index: {j.Index}, amt: {j.Amount}");
                    e.data.stakes.Clear();
                    result.Iterate(k =>
                    {
                        e.data.stakes.Add(new Stake((uint)k.Amount, k.CanisterId, k.Index.ValueOrDefault, k.TokenType));
                    });
                }
                else
                {
                    Debug.Log("Stake is NULL");
                }
                e.SetAsReady();

                return e;
            });
        }
    }
}