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
    using Candid.IcrcLedger;
    using Unity.VisualScripting;
    using Boom;
    using EdjCase.ICP.BLS;
    using Newtonsoft.Json;
    using static UserUtil;

    public class CandidApiManager : MonoBehaviour
    {
        [field : SerializeField] public string WORLD_HUB_CANISTER_ID { private set; get; } = "fgpem-ziaaa-aaaag-abi2q-cai";
        [field : SerializeField] public string WORLD_CANISTER_ID { private set; get; } = "j4n55-giaaa-aaaap-qb3wq-cai";
        [field: SerializeField] public string WORLD_COLLECTION_CANISTER_ID { private set; get; } = "6uvic-diaaa-aaaap-abgca-cai";

        public enum GameType { SinglePlayer, Multiplayer, WebsocketMultiplayer}

        // Instance
        public static CandidApiManager Instance { get; private set; }

        //Cache
        [field : SerializeField] public GameType BoomDaoGameType { private set; get; } = GameType.SinglePlayer;
        [ShowOnly, SerializeField] InitValue<IAgent> cachedAnonAgent;
        [ShowOnly, SerializeField] InitValue<string> cachedUserAddress;

        // Canister APIs
        public WorldApiClient WorldApiClient { get; private set; }
        public WorldHubApiClient WorldHub { get; private set; }

        [SerializeField, ShowOnly] string principal;
        [SerializeField, ShowOnly] bool isLoginIn;

        [SerializeField, ShowOnly] float nextUpdateIn;
        [SerializeField] float secondsToUpdateClient = 2;
        [SerializeField, ShowOnly] private long lastClientUpdate;


        [SerializeField, ShowOnly] bool inRoom;
        [SerializeField, ShowOnly] string currentRoomId;
        [SerializeField, ShowOnly] string[] usersInRoom;

        [SerializeField] bool multPlayerActionStateFetch;
        [SerializeField] bool multPlayerTokenFetch;
        [SerializeField] bool multPlayerCollectionFetch;

        bool configsRequested;

        private void Awake()
        {
            BroadcastState.Invoke(new WaitingForResponse(true));

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

            Broadcast.Register<UserLoginRequest>(FetchHandler);

            Broadcast.Register<UserLogout>(UserLogoutHandler);

            Broadcast.Register<FetchListings>(FetchHandler);


            UserUtil.AddListenerRequestData<DataTypeRequestArgs.Entity>(FetchHandler);
            UserUtil.AddListenerRequestData<DataTypeRequestArgs.ActionState>(FetchHandler);
            UserUtil.AddListenerRequestData<DataTypeRequestArgs.Token>(FetchHandler);
            UserUtil.AddListenerRequestData<DataTypeRequestArgs.NftCollection>(FetchHandler);

            InitializeCandidApis(CreateAgentWithRandomIdentity(), true).Forget();
        }

        private void OnDestroy()
        {
            Broadcast.Unregister<UserLoginRequest>(FetchHandler);

            Broadcast.Unregister<UserLogout>(UserLogoutHandler);

            Broadcast.Unregister<FetchListings>(FetchHandler);

            UserUtil.RemoveListenerRequestData<DataTypeRequestArgs.Entity>(FetchHandler);
            UserUtil.RemoveListenerRequestData<DataTypeRequestArgs.ActionState>(FetchHandler);
            UserUtil.RemoveListenerRequestData<DataTypeRequestArgs.Token>(FetchHandler);
            UserUtil.RemoveListenerRequestData<DataTypeRequestArgs.NftCollection>(FetchHandler);

            //WEBSOCKET
            if (BoomDaoGameType == CandidApiManager.GameType.WebsocketMultiplayer)
            {
                //TODO: DISCONNECT WEBSOCKET
            }
        }

        int frameCount;
        private void Update()
        {
            ++frameCount;
            if (BoomDaoGameType == GameType.Multiplayer)
            {
                nextUpdateIn = (lastClientUpdate - MainUtil.Now()) / 1000f;
                if (lastClientUpdate <= MainUtil.Now())
                {
                    lastClientUpdate = MainUtil.Now() + (long)(secondsToUpdateClient * 1000);

                    if(frameCount > 0) FetchRoomData();
                }

            }
            else if (BoomDaoGameType == GameType.SinglePlayer)
            {
            }
        }
        //


        void OnLoginCompleted(string json)
        {
            var isLoggedIn = UserUtil.IsLoggedIn(out LoginType loginType);

            if (isLoggedIn == false)
            {
                CreateAgentUsingIdentityJson(json, false).Forget();
                return;
            }

            if (isLoggedIn && loginType == LoginType.Anon)
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

            configsRequested = false;

            //WEBSOCKET
            if (BoomDaoGameType == CandidApiManager.GameType.WebsocketMultiplayer)
            {
                //TODO: DISCONNECT WEBSOCKET
            }
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
                if (UserUtil.IsUserLoggedIn() && frameCount > 0)
                {
                    //Clean up logged in user data
                    UserUtil.ClearData<DataTypes.Token>();
                    UserUtil.ClearData<DataTypes.Entity>();
                    UserUtil.ClearData<DataTypes.ActionState>();
                    UserUtil.ClearData<DataTypes.NftCollection>();
                }

                //If anon Agent is cached then set it up
                if (cachedAnonAgent.IsInit)
                {
                    userAccountIdentity = cachedUserAddress.Value;
                    agent = cachedAnonAgent.Value;

                    //Build Interfaces
                    WorldHub = new WorldHubApiClient(agent, Principal.FromText(WORLD_HUB_CANISTER_ID));
                    WorldApiClient = new WorldApiClient(agent, Principal.FromText(WORLD_CANISTER_ID));
                }
                //Else fetch required dependencies and catch it
                else
                {
                    //Build Interfaces
                    WorldHub = new WorldHubApiClient(agent, Principal.FromText(WORLD_HUB_CANISTER_ID));
                    WorldApiClient = new WorldApiClient(agent, Principal.FromText(WORLD_CANISTER_ID));

                    cachedAnonAgent.Value = agent;

                    userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);
                    cachedUserAddress.Value = userAccountIdentity;
                }


                //Set Login Data
                UserUtil.UpdateMainData(new MainDataTypes.LoginData(agent, userPrincipal, userAccountIdentity, MainDataTypes.LoginData.State.LoggedInAsAnon));
            }
            else
            {
                //Build Interfaces
                WorldHub = new WorldHubApiClient(agent, Principal.FromText(WORLD_HUB_CANISTER_ID));
                WorldApiClient = new WorldApiClient(agent, Principal.FromText(WORLD_CANISTER_ID));

                userAccountIdentity = await WorldHub.GetAccountIdentifier(userPrincipal);

                "Try Fetch User Data".Log(GetType().Name);

                //HERE: YOU CAN REQUEST FOR THE FIRST TIME ON THE GAME THE USER DATA

                //Set Login Data
                //UserUtil.Clean<DataTypes.LoginData>(new UserUtil.CleanUpType.All());
                UserUtil.UpdateMainData(new MainDataTypes.LoginData(agent, userPrincipal, userAccountIdentity, MainDataTypes.LoginData.State.LoggedIn));
                BroadcastState.Invoke(new WaitingForResponse(false));

                //USER DATA
                UserUtil.RequestData(new DataTypeRequestArgs.Entity(userPrincipal, WORLD_CANISTER_ID));

                UserUtil.RequestDataSelf<DataTypeRequestArgs.ActionState>();


                //WE REQUEST USER TOKENS

                var allTokensConfigResult = ConfigUtil.GetAllTokenConfigs();

                if (allTokensConfigResult.IsErr)
                {
                    Debug.LogWarning(allTokensConfigResult.AsErr());
                    return;
                }

                var tokensToFetch = allTokensConfigResult.AsOk();

                var tokensToFetchIds = tokensToFetch.Map(e =>
                {
                    return e.canisterId;
                });
                UserUtil.RequestData(new DataTypeRequestArgs.Token(tokensToFetchIds.ToArray()));

                //WE REQUEST USER NFTs
                var nftsToFetchResult = ConfigUtil.GetAllNftConfigs();

                if (nftsToFetchResult.IsErr)
                {
                    Debug.LogWarning(nftsToFetchResult.AsErr());
                    return;
                }

                var nftsToFetch = nftsToFetchResult.AsOk();

                var nftsToFetchIds = nftsToFetch.Map(e =>
                {
                    return e.canisterId;
                });

               UserUtil.RequestData(new DataTypeRequestArgs.NftCollection(nftsToFetchIds.ToArray()));

                //WEBSOCKET
                if (BoomDaoGameType == CandidApiManager.GameType.WebsocketMultiplayer)
                {
                    //TODO: CONNECT WEBSOCKET
                }
            }

            isLoginIn = false;

            //INIT CONFIGS
            if (!configsRequested)
            {
                configsRequested = true;
                await FetchConfigs();
            }
        }


        private async UniTask FetchConfigs()
        {
            try
            {
                //HERE: You can specify all World's Ids you want to fetch entity configs from
                string[] worlds = new string[] { WORLD_CANISTER_ID };


                //Set Configs
                var configsResult =
                    await FetchUtil.ProcessWorldCall<Dictionary<string, MainDataTypes.AllConfigs.Config>>(
                        async (worldInterface, wid) =>
                        {
                            var stableConfigs = await worldInterface.GetAllConfigs();

                            return stableConfigs.Map(e => new MainDataTypes.AllConfigs.Config(e)).ToDictionary(e => e.cid);
                        },
                        worlds
                    );

                if (configsResult.IsOk)
                {
                    var asOk = configsResult.AsOk();

                    UserUtil.UpdateMainData(new MainDataTypes.AllConfigs(asOk));
                }
                else
                {
                    throw new(configsResult.AsErr());
                }

                //Set Tokens & Nft Configs
                FetchTokenConfig().Forget();
                FetchNftConfig().Forget();

                //Set Actions
                var actionsResult =
                    await FetchUtil.ProcessWorldCall<Dictionary<string, MainDataTypes.AllAction.Action>>(
                        async (worldInterface, wid) =>
                        {
                            var stableConfigs = await worldInterface.GetAllActions();

                            return stableConfigs.Map(e => new MainDataTypes.AllAction.Action(e)).ToDictionary(e => e.aid);
                        },
                        worlds
                    );

                if (actionsResult.IsOk)
                {
                    var asOk = actionsResult.AsOk();

                    UserUtil.UpdateMainData(new MainDataTypes.AllAction(asOk));
                }
                else
                {
                    throw new(configsResult.AsErr());
                }

                BroadcastState.Invoke(new WaitingForResponse(false));
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        //
        #region Fetch
        private async UniTask FetchEntities(DataTypeRequestArgs.Entity arg)
        {
            await UniTask.SwitchToMainThread();

            var uids = arg.uids;

            if (!UserUtil.IsUserLoggedIn(out var loginData))
            {
                Debug.LogError("something went wrong, user is not logged in!");
                return;
            }

            var userUid = loginData.principal;

            if (uids.Length == 0)
            {
                uids = new string[1] { userUid };
            }

            var result = await FetchUtil.GetAllEntities(WORLD_CANISTER_ID, uids);

            if (result.IsOk)
            {
                var asOk = result.AsOk();

                asOk.Iterate(user =>
                {
                    UserUtil.UpdateData(user.Key, user.Value.ToArray());
                });
            }
            else
            {
                $"DATA of type {nameof(DataTypes.Entity)} failed to load. Message: {result.AsErr()}".Warning(nameof(CandidApiManager));
            }
        }
        private async UniTask FetchActionStates(DataTypeRequestArgs.ActionState arg)
        {
            await UniTask.SwitchToMainThread();

            var uids = arg.uids;

            if (!UserUtil.IsUserLoggedIn(out var loginData))
            {
                Debug.LogError("something went wrong, user is not logged in!");
                return;
            }

            var userUid = loginData.principal;

            if (uids.Length == 0)
            {
                uids = new string[1] { userUid };
            }

            var result = await FetchUtil.GetAllActionState(WORLD_CANISTER_ID, uids);

            if (result.IsOk)
            {
                var asOk = result.AsOk();

                asOk.Iterate(user =>
                {
                    UserUtil.UpdateData(user.Key, user.Value.ToArray());
                });
            }
            else
            {
                $"DATA of type {nameof(DataTypes.ActionState)} failed to load. Message: {result.AsErr()}".Warning(nameof(CandidApiManager));
            }
        }
        //
        private async UniTask FetchToken(DataTypeRequestArgs.Token arg)
        {
            await UniTask.SwitchToMainThread();

            var uids = arg.uids;

            if (!UserUtil.IsUserLoggedIn(out var loginData))
            {
                Debug.LogError("something went wrong, user is not logged in!");
                return;
            }

            var userUid = loginData.principal;

            if (uids.Length == 0)
            {
                uids = new string[1] { userUid };
            }

            //

            var result = await FetchUtil.GetAllTokens(arg.canisterIds, uids);

            if (result.IsOk)
            {
                var asOk = result.AsOk();

                asOk.Iterate(user =>
                {
                    UserUtil.UpdateData(user.Key, user.Value.Map(token => new DataTypes.Token(token.Key, token.Value)).ToArray());
                });
            }
            else
            {
                $"DATA of type {nameof(DataTypes.ActionState)} failed to load. Message: {result.AsErr()}".Warning(nameof(CandidApiManager));
            }
        }

        private async UniTask FetchNfts(DataTypeRequestArgs.NftCollection arg)
        {
            await UniTask.SwitchToMainThread();

            var uids = arg.uids;

            if (!UserUtil.IsUserLoggedIn(out var loginData))
            {
                Debug.LogError("something went wrong, user is not logged in!");
                return;
            }

            var userUid = loginData.principal;

            if (uids.Length == 0)
            {
                uids = new string[1] { userUid };
            }

            //
            var result = await FetchUtil.GetAllNfts(arg.canisterIds, uids);

            if (result.IsOk)
            {
                var asOk = result.AsOk();

                asOk.Iterate(user =>
                {
                    UserUtil.UpdateData(user.Key, user.Value.ToArray());
                });
            }
            else
            {
                $"DATA of type{nameof(DataTypes.ActionState)} failed to load. Message: {result.AsErr()}".Warning(nameof(CandidApiManager));
            }
        }

        //Configs
        private async UniTaskVoid FetchTokenConfig()
        {
            await UniTask.SwitchToMainThread();

            try
            {
                List<MainDataTypes.AllTokenConfigs.TokenConfig> tokens = new();

                if (ConfigUtil.QueryConfigsByTag(WORLD_CANISTER_ID, "token", out var tokensMetadata))
                {
                    foreach (var tokenMetadata in tokensMetadata)
                    {
                        if (tokenMetadata.GetConfigFieldAs<string>("canister", out var canisterId))
                        {
                            var tokenCanisterId = canisterId;

                            if (string.IsNullOrEmpty(tokenCanisterId)) tokenCanisterId = Env.CanisterIds.ICP_LEDGER;

                            if (tokenCanisterId == Env.CanisterIds.ICP_LEDGER)
                            {
                                var tokenInterface = new IcpLedgerApiClient(cachedAnonAgent.Value, Principal.FromText(tokenCanisterId));

                                var decimals = await tokenInterface.Icrc1Decimals();
                                var name = await tokenInterface.Icrc1Name();
                                var symbol = await tokenInterface.Symbol();
                                var fee = await tokenInterface.Icrc1Fee();
                                fee.TryToUInt64(out ulong _fee);


                                if (ConfigUtil.TryGetConfig(WORLD_CANISTER_ID, e =>
                                {
                                    e.GetConfigFieldAs<string>("canister", out var _canister, "");

                                    return _canister == tokenCanisterId;
                                }, out var tokenConfig))
                                {
                                    tokenConfig.GetConfigFieldAs("description", out var description, "");

                                    tokenConfig.GetConfigFieldAs("urlLogo", out var urlLogo, "");

                                    tokens.Add(new MainDataTypes.AllTokenConfigs.TokenConfig(tokenCanisterId, name, symbol.Symbol, decimals, _fee, description, urlLogo));
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

                                if (ConfigUtil.TryGetConfig(WORLD_CANISTER_ID, e =>
                                {
                                    e.GetConfigFieldAs<string>("canister", out var _canister, "");

                                    return _canister == tokenCanisterId;
                                }, out var tokenConfig))
                                {
                                    tokenConfig.GetConfigFieldAs("description", out var description, "");

                                    tokenConfig.GetConfigFieldAs("urlLogo", out var urlLogo, "");

                                    tokens.Add(new MainDataTypes.AllTokenConfigs.TokenConfig(tokenCanisterId, name, symbol, decimals, _fee, description, urlLogo));
                                }
                            }
                        }
                        else
                        {
                            $"config of tag \"token\" doesn't have field \"canister\"".Warning();
                        }
                    }
                }
                else "No Token Config found in World Config".Warning(nameof(CandidApiManager));

                if(tokens.Count > 0) UserUtil.UpdateMainData(new MainDataTypes.AllTokenConfigs(tokens.ToArray().ToDictionary(e => e.canisterId)));
                else UserUtil.UpdateMainData(new MainDataTypes.AllTokenConfigs());

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private async UniTaskVoid FetchNftConfig()
        {
            await UniTask.SwitchToMainThread();

            try
            {
                List<MainDataTypes.AllNftCollectionConfig.NftConfig> collections = new();

                if (ConfigUtil.QueryConfigsByTag(WORLD_CANISTER_ID, "nft", out var nftConfigs))
                {
                    Debug.Log($"Collections Config fetched {JsonConvert.SerializeObject(nftConfigs)}");
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

                        collections.Add(new MainDataTypes.AllNftCollectionConfig.NftConfig(canisterId, isStandard == false, collectionName, description, urlLogo));
                    });
                }
                else "No Nft Config found in World Config".Warning(nameof(CandidApiManager));

                if (collections.Count > 0)
                {
                    UserUtil.UpdateMainData(new MainDataTypes.AllNftCollectionConfig(collections.ToArray().ToDictionary(e => e.canisterId)));
                }
                else UserUtil.UpdateMainData(new MainDataTypes.AllNftCollectionConfig());
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        //Listings
        private async UniTask FetchListings()
        {
            await UniTask.SwitchToMainThread();

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }
            Debug.Log("Get Nft Listings Of " + WORLD_COLLECTION_CANISTER_ID);

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(WORLD_COLLECTION_CANISTER_ID));

            var listingResult = await collectionInterface.Listings();

            List<MainDataTypes.AllListings.Listing> listing = new();

            foreach (var item in listingResult)
            {
                var tokenIdentifier = await CandidApiManager.Instance.WorldHub.GetTokenIdentifier(WORLD_COLLECTION_CANISTER_ID, item.F0);
                listing.Add(new(tokenIdentifier, item));
            }

            UserUtil.UpdateMainData(new MainDataTypes.AllListings(listing.ToArray().ToDictionary(e => e.index)));
        }

        private void FetchRoomData()
        {
            if (UserUtil.IsUserLoggedIn(out var loginData) == false)
            {
                return;
            }


            if (EntityUtil.TryGetAllEntitiesOf<DataTypes.Entity>(EntityUtil.Queries.rooms, out var rooms, e => e))
            {
                Debug.Log($"Try Fetch Room Data Success, data: {JsonConvert.SerializeObject(rooms)}");

                var allRoomsData = new MainDataTypes.AllRoomData(rooms);
                UserUtil.UpdateMainData(allRoomsData);

                UserUtil.RequestData(new DataTypeRequestArgs.Entity(WORLD_CANISTER_ID));

                var usersInCurrentRoom = allRoomsData.GetAllUsersInCurrentRoom();
                usersInCurrentRoom ??= new string[0];

                inRoom = allRoomsData.inRoom;
                currentRoomId = allRoomsData.currentRoomId;
                usersInRoom = usersInCurrentRoom;

                if (inRoom)
                {
                    UserUtil.RequestData(new DataTypeRequestArgs.Entity(usersInCurrentRoom));

                    if (multPlayerActionStateFetch) UserUtil.RequestData(new DataTypeRequestArgs.ActionState(usersInCurrentRoom));
                    if (multPlayerTokenFetch)
                    {
                        var tokenConfigsResult = ConfigUtil.GetAllTokenConfigs();
                        if (tokenConfigsResult.IsErr)
                        {
                            Debug.LogError(tokenConfigsResult.AsErr());
                            return;
                        }
                        var tokenConfigs = tokenConfigsResult.AsOk();

                        UserUtil.RequestData(new DataTypeRequestArgs.Token(tokenConfigs.Map(e=>e.canisterId).ToArray(), usersInCurrentRoom));
                    }
                    if (multPlayerCollectionFetch)
                    {
                        var nftConfigsResult = ConfigUtil.GetAllNftConfigs();

                        if (nftConfigsResult.IsErr)
                        {
                            Debug.LogError(nftConfigsResult.AsErr());
                            return;
                        }
                        var nftConfigs = nftConfigsResult.AsOk();

                        UserUtil.RequestData(new DataTypeRequestArgs.NftCollection(nftConfigs.Map(e=>e.canisterId).ToArray(), usersInCurrentRoom));
                    }
                }
            }
            else
            {
                Debug.Log("Try Fetch Room Data Failure");
            }
        }

        #endregion

        #region FetchHandlers
        private void FetchHandler(FetchListings arg)
        {
            FetchListings().Forget();
        }
        private void FetchHandler(UserLoginRequest arg)
        {
            if (UserUtil.IsLoginRequestedPending() || UserUtil.IsUserLoggedIn()) return;

            SetAsLoginIn();
            BroadcastState.Invoke(new WaitingForResponse(true));


            PlayerPrefs.SetString("walletType", "II");

#if UNITY_WEBGL && !UNITY_EDITOR
            LoginManager.Instance.StartLoginFlowWebGl(OnLoginCompleted);
            return;
#endif
            isLoginIn = true;
            LoginManager.Instance.StartLoginFlow(OnLoginCompleted);
        }

        private void FetchHandler(FetchDataReq<DataTypeRequestArgs.Entity> req)
        {
            //$"DATA of type {nameof(DataTypeRequestArgs.Entity)} was requested, args: {JsonConvert.SerializeObject(req.arg.uids)}".Log(nameof(CandidApiManager));


            FetchEntities(req.arg).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypeRequestArgs.ActionState> req)
        {
            //$"DATA of type {nameof(DataTypeRequestArgs.ActionState)} was requested, args: {JsonConvert.SerializeObject(req.arg.uids)}".Log(nameof(CandidApiManager));

            FetchActionStates(req.arg).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypeRequestArgs.Token> req)
        {
            //$"DATA of type {nameof(DataTypeRequestArgs.Token)} was requested, args: {JsonConvert.SerializeObject(req.arg.uids)}".Log(nameof(CandidApiManager));

            FetchToken(req.arg).Forget();
        }
        private void FetchHandler(FetchDataReq<DataTypeRequestArgs.NftCollection> req)
        {
            FetchNfts(req.arg).Forget();
        }
        #endregion
    }
}