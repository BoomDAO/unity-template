namespace Boom.UI
{
    using Candid;
    using Candid.Extv2Boom;
    using EdjCase.ICP.Candid.Models;
    using Boom.Patterns.Broadcasts;
    using Boom.Utility;
    using Boom.Values;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Cysharp.Threading.Tasks;
    using static DataTypes;
    using System.Collections.Generic;

    public class MarketPlaceWindow : Window
    {
        [SerializeField] Button openNftToListPanelButton;
        [SerializeField] GameObject nftToListPanel;

        [SerializeField] Transform listingContent;
        [SerializeField] Transform nftToListContent;

        [SerializeField] TextMeshProUGUI loadingVisualListing;
        [SerializeField] TextMeshProUGUI loadingVisualNftToList;

        [SerializeField] GameObject mintButton;
        [SerializeField] GameObject newListingPanel;
        [SerializeField] TextMeshProUGUI newListingContentText;
        [SerializeField] Button confirmListingButton;
        [SerializeField] Button cancelListingButton;
        [SerializeField] TMP_InputField priceInputField;
        [SerializeField, ShowOnly] double nftPrice;
        [SerializeField, ShowOnly] string selectedNftCollectionCanisterId;
        [SerializeField, ShowOnly] string selectedNftIdentifier;
        [SerializeField, ShowOnly] long selectedNftIndex;

        [SerializeField, ShowOnly] bool checkingForSettlement;
        public class WindowData
        {
            //DATA
        }
        public override bool RequireUnlockCursor()
        {
            return false;//or true
        }
        public override Type[] GetConflictWindow()
        {
            return new Type[2] { typeof(InventoryWindow), typeof(LoginWindow) };
        }
        public override void Setup(object data)
        {
            mintButton.SetActive(false);

            openNftToListPanelButton.onClick.AddListener(() =>
            {
                nftToListPanel.SetActive(true);
            });

            priceInputField.onValueChanged.AddListener(InputPriceChangeHandler);
            confirmListingButton.onClick.AddListener(ListNftHandler);
            cancelListingButton.onClick.AddListener(CancelMakeOfferPanel);

            UserUtil.AddListenerDataChangeSelf<DataTypes.NftCollection>(UpdateWindow, true);
            UserUtil.AddListenerMainDataChange<MainDataTypes.AllListings>(UpdateWindow);


            Broadcast.Invoke<FetchListings>();
        }

        private void OnDestroy()
        {
            priceInputField.onValueChanged.RemoveListener(InputPriceChangeHandler);
            confirmListingButton.onClick.RemoveListener(ListNftHandler);
            cancelListingButton.onClick.RemoveListener(CancelMakeOfferPanel);

            UserUtil.RemoveListenerDataChangeSelf<DataTypes.NftCollection>(UpdateWindow);
            UserUtil.RemoveListenerMainDataChange<MainDataTypes.AllListings>(UpdateWindow);
        }
        private void InputPriceChangeHandler(string arg0)
        {
            if (arg0.TryParseValue<double>(out var val)) nftPrice = val;
        }
        private void UpdateWindow(MainDataTypes.AllListings state)
        {
            foreach (Transform child in listingContent)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in nftToListContent)
            {
                Destroy(child.gameObject);
            }

            loadingVisualListing.text = "";
            //
            var getLoginDataResult = UserUtil.GetLogInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getLoginDataResult.AsErr());
                return;
            }

            var loginData = getLoginDataResult.AsOk();

            if (UserUtil.IsMainDataValid<MainDataTypes.AllListings>() == false)
            {
                Debug.LogWarning("Listings is not yet valit");
                return;
            }

            if (state.listings.Count == 0)
            {
                loadingVisualListing.text = "No listings yet";

                Debug.LogWarning("There is no listing yet");
            }

            foreach (var element in state.listings)
            {
                if (element.Value.details.Seller.ToText() == loginData.principal)
                {
                    WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.details.Seller}",
                        content = $"Nft ID:\n{element.Value.tokenIdentifier.SimplifyAddress()}\n\nIndex:\n{element.Value.index}\n\nPrice:\n{element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nSeller: YOU",
                        textButtonContent = "UNLIST",
                        action = (a, b) => UnlistNft(a, b).Forget(),

                        customData = (
                            CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID,
                            element.Value.tokenIdentifier,
                            $"{element.Value.details.Seller}",
                            element.Value.details.Price),

                        imageContentType = new ImageContentType.Url($"https://{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID}.raw.icp0.io/?&tokenid={element.Value.tokenIdentifier}&type=thumbnail"),
                        infoWindowData = new($"Your NFT Listing Info", $"Price:\n{element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nSeller: YOU\n\nNft ID:\n{element.Value.tokenIdentifier.SimplifyAddress()}\n\nCanister ID:\n{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID.SimplifyAddress()}")
                    }, listingContent);
                }
                else
                {
                    WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.details.Seller}",
                        content = $"Nft ID:\n{element.Value.tokenIdentifier.SimplifyAddress()}\n\nSeller:\n{element.Value.details.Seller.ToText().SimplifyAddress()}",
                        textButtonContent = $"BUY {element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP",
                        action = (a, b) => Buy(a, b).Forget(),

                        customData = (
                            CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID,
                            element.Value.tokenIdentifier,
                            $"{element.Value.details.Seller}",
                            element.Value.details.Price),

                        imageContentType = new ImageContentType.Url($"https://{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID}.raw.icp0.io/?&tokenid={element.Value.tokenIdentifier}&type=thumbnail"),
                        infoWindowData = new($"NFT Listing Info", $"Price:\n{element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nSeller: {element.Value.details.Seller.ToText().SimplifyAddress()}\n\nNft ID:\n{element.Value.tokenIdentifier.SimplifyAddress()}\n\nCanister ID:\n{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID.SimplifyAddress()}")
                    }, listingContent);
                }
            }

            var principalResult = UserUtil.GetPrincipal();

            if (principalResult.Tag == UResultTag.Err)
            {
                Debug.Log(principalResult.AsErr());
                return;
            }
            var principal = principalResult.AsOk().Value;

            var nftCollectionsResult = UserUtil.GetDataSelf<DataTypes.NftCollection>();

            if (nftCollectionsResult.Tag == Values.UResultTag.Ok) UpdateWindow(nftCollectionsResult.AsOk());
            else Debug.LogWarning(nftCollectionsResult.AsErr());
        }
        private void UpdateWindow(Data<DataTypes.NftCollection> nftsState)
        {
            var principalResult = UserUtil.GetPrincipal();

            if (principalResult.Tag == UResultTag.Err)
            {
                Debug.Log(principalResult.AsErr());
                return;
            }
            var principal = principalResult.AsOk().Value;

            foreach (Transform child in nftToListContent)
            {
                Destroy(child.gameObject);
            }

            loadingVisualNftToList.text = "";
            //

            var listingResult = UserUtil.GetMainData<MainDataTypes.AllListings>();

            if (listingResult.Tag == UResultTag.Err)
            {
                //Is Fetching
                Debug.LogWarning("Fetching Listings, msg: "+ listingResult.AsErr());
                if (UserUtil.IsFetchingData<DataTypes.NftCollection>())
                {
                    loadingVisualNftToList.text = "Loading...";
                }

                return;
            }

            var listings = listingResult.AsOk();

            var nftCountResult = NftUtil.GetNftCount(principal, CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID);

            if (nftCountResult.Tag == Boom.Values.UResultTag.Err)
            {
                loadingVisualNftToList.text = "Loading...";
                return;
            }

            var nftCount = nftCountResult.AsOk();

            mintButton.SetActive(nftCount == 0);

            if (nftCount == 0)
            {
                //There is no listings. No need to return.
                loadingVisualNftToList.text = "You don't have any NFTs to list";

                Debug.LogWarning("You dont have any Nft from collection: "+ CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID);
            }
            else
            {
                if(nftsState.elements.TryGetValue(CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID, out var nftCollection))
                {
                    nftCollection.tokens.Iterate(token =>
                    {
                        var url = "https://i.postimg.cc/65smkh6B/BoomDao.jpg"; //token.url;
                        if (listings.listings.TryLocate(e => e.Value.tokenIdentifier == token.tokenIdentifier, out var listing))
                        {
                            //if u own the token and is listed
                            WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                            {
                                id = $"{nftCollection.canisterId}|{token.tokenIdentifier}",
                                content = $"Nft ID:\n{token.tokenIdentifier.SimplifyAddress()}\n\nPrice:\n{listing.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP",
                                textButtonContent = "UNLIST",
                                action = (a, b) => UnlistNft(a, b).Forget(),

                                customData = (
                                    nftCollection.canisterId,
                                    token.tokenIdentifier,
                                    //we ignore the two values below
                                    "", (ulong)0),

                                imageContentType = new ImageContentType.Url(url),
                                infoWindowData = new($"NFT Listing Info", $"Price:\n{listing.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nNft ID:\n{token.tokenIdentifier.SimplifyAddress()}\n\nCanister ID:\n{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID.SimplifyAddress()}")
                            }, nftToListContent);
                        }
                        else
                        {
                            WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                            {
                                id = $"{nftCollection.canisterId}|{token.tokenIdentifier}",
                                content = $"Nft ID:\n{token.tokenIdentifier.SimplifyAddress()}",
                                textButtonContent = "LIST",
                                action = OpenMakeOfferPanel,

                                customData = (
                                    nftCollection.canisterId,
                                    token.tokenIdentifier,
                                    token.index),

                                imageContentType = new ImageContentType.Url(url),
                                infoWindowData = new($"NFT Info", $"Nft ID:\n{token.tokenIdentifier.SimplifyAddress()}\n\nCanister ID:\n{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID.SimplifyAddress()}")
                            }, nftToListContent);
                        }
                    });
                }
                else
                {
                    Debug.LogError($"Failure to find config of nft collectionId: {CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID}");
                    return;
                }
            }
        }


        private async UniTaskVoid CheckForSettlement(Extv2BoomApiClient collectionInterface)
        {
            if (checkingForSettlement) return;

            checkingForSettlement = true;

            while (checkingForSettlement)
            {
                var settlementsResponse = await collectionInterface.HeartbeatPending();

                if (settlementsResponse.Count > 0)
                {
                    await collectionInterface.HeartbeatExternal();

                    continue;
                }

                checkingForSettlement = false;
            }
        }
        private async UniTaskVoid Buy(string arg0, object customData)
        {
            //await UniTask.SwitchToMainThread();


            BroadcastState.Invoke(new WaitingForResponse(true));
            (string collectionId, string nftIdentifier, string seller, ulong price) = ((string, string, string, ulong))customData;

            var loginDataResult = UserUtil.GetLogInData();

            if (loginDataResult.IsErr)
            {
                Debug.LogError(loginDataResult.AsErr());
                return;
            }

            var loginData = loginDataResult.AsOk();

            var accountIdentifier = loginData.accountIdentifier;
            var principal = loginData.principal;
            var agent = loginData.agent;

            var tokenDataResult = TokenUtil.GetTokenDetails(principal, Env.CanisterIds.ICP_LEDGER);

            if (tokenDataResult.IsErr)
            {
                Debug.LogError("Could not find Icp token Config");
                return;
            }
            var tokenData = tokenDataResult.AsOk();

            if (tokenData.token.baseUnitAmount < price)
            {
                WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You don't have enough ICP", $"Requirements:\n{$"{price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nYou need to deposit some ICP"}"), 3);
                return;
            }
            Extv2BoomApiClient collectionInterface = new(agent, Principal.FromText(collectionId));

            var lockResult = await collectionInterface.Lock(nftIdentifier, price, accountIdentifier, new());

            if (lockResult.Tag == Candid.Extv2Boom.Models.Result7Tag.Err)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));

                Debug.LogWarning("Lock err, msg: " + lockResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Lock success, msg: " + lockResult.AsOk());

            var addressToTransferTo = lockResult.AsOk();
            Debug.Log("Transfer from: " + accountIdentifier + " Price: " + price);

            var transferResult = await ActionUtil.Transfer.TransferIcp(new Candid.World.Models.IcpTx(CandidUtil.ConvertToDecimal(price, tokenData.configs.decimals), addressToTransferTo));

            if (transferResult.Tag == Values.UResultTag.Err)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));

                Debug.LogError($"Error transfering money, {transferResult.AsErr()}");
                return;
            }

            var settleResult = await collectionInterface.Settle(nftIdentifier);

            if (settleResult.Tag == Candid.Extv2Boom.Models.Result3Tag.Err)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));

                var err = settleResult.AsErr();

                if(err.Tag == Candid.Extv2Boom.Models.CommonErrorTag.InvalidToken)
                {
                    Debug.LogError($"Settle failure, invalid token, msg: {err.AsInvalidToken()}");
                }
                else
                {
                    Debug.LogError($"Settle failure, other issue, msg: {err.AsOther()}");
                }

                return;
            }

            Debug.Log("Settle success");

            CheckForSettlement(collectionInterface).Forget();

            //

            if (ConfigUtil.QueryConfigsByTag(CandidApiManager.Instance.WORLD_CANISTER_ID, "nft", out var nftConfigs))
            {
                nftConfigs.Iterate(e =>
                {
                    if (!e.GetConfigFieldAs<string>("canister", out var _canisterId))
                    {
                        Debug.LogError($"config of tag \"nft\" doesn't have field \"canister\"");

                        return;
                    }

                    if (_canisterId != collectionId) return;

                    if (!e.GetConfigFieldAs<string>("name", out var collectionName))
                    {
                        Debug.LogWarning($"config of tag \"nft\" doesn't have field \"collectionName\"");
                    }
                    if (!e.GetConfigFieldAs<string>("description", out var description))
                    {
                        Debug.LogWarning($"config of tag \"nft\" doesn't have field \"description\"");
                    }
                    if (!e.GetConfigFieldAs<string>("urlLogo", out var urlLogo))
                    {
                        Debug.LogWarning($"config of tag \"nft\" doesn't have field \"urlLogo\"");
                    }

                    if (!e.GetConfigFieldAs<bool>("isStandard", out var isStandard))
                    {
                        Debug.LogError($"config of tag \"nft\" doesn't have field \"isStandard\"");

                        return;
                    }

                     UserUtil.RequestData(new DataTypeRequestArgs.NftCollection(new[] { collectionId }, principal));
                });
            }

            //

            BroadcastState.Invoke(new WaitingForResponse(false));

            UserUtil.ClearMainData<MainDataTypes.AllListings>();

            Broadcast.Invoke<FetchListings>();
        }
        private void OpenMakeOfferPanel(string id, object customData)
        {
            //Disable all Inventory and offer action button
            newListingPanel.SetActive(true);
            confirmListingButton.enabled = true;

            (selectedNftCollectionCanisterId, selectedNftIdentifier, selectedNftIndex) = ((string, string, uint))customData;

            newListingContentText.text = $"Canister ID:\n{CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID.SimplifyAddress()}\n\nNft ID:\n{selectedNftIdentifier.SimplifyAddress()}\n\nNft Index: {selectedNftIndex}";
        }
        private void CancelMakeOfferPanel()
        {
            newListingPanel.SetActive(false);
        }

        private void ListNftHandler()
        {
            ListNft().Forget();
        }
        private async UniTaskVoid ListNft()
        {
            //await UniTask.SwitchToMainThread();

            BroadcastState.Invoke(new WaitingForResponse(true));

            if (nftPrice < 0.001)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));

                Debug.LogError($"Listing price too low, min price is: 0.001 Icp");
                return;
            }

            confirmListingButton.enabled = false;

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == Values.UResultTag.Err)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID));

            var listingReqResult = await collectionInterface.List(new Candid.Extv2Boom.Models.ListRequest(new(), new(nftPrice.ConvertToBaseUnit(CandidUtil.ICP_DECIMALS)), selectedNftIdentifier));

            if (listingReqResult.Tag == Candid.Extv2Boom.Models.Result3Tag.Ok)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));
                newListingPanel.SetActive(false);
                Close();

                Broadcast.Invoke<FetchListings>();

                Debug.Log("Listing success");

            }
            else
            {
                Debug.LogWarning("Error Listing, msg: " + listingReqResult.AsErr().Value.ToString());
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
        }
        private async UniTaskVoid UnlistNft(string widgetId, object customData)
        {
            //await UniTask.SwitchToMainThread();

            BroadcastState.Invoke(new WaitingForResponse(true));

            (string collectionId, string nftIdentifier, _, _) = ((string, string, string, ulong))customData;

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(collectionId));

            Debug.Log($"Try Unlist Nft of id: {nftIdentifier}\n from collection: {collectionId}");

            var unlistingReqResult = await collectionInterface.List(new Candid.Extv2Boom.Models.ListRequest(new(), new(), nftIdentifier));

            if (unlistingReqResult.Tag == Candid.Extv2Boom.Models.Result3Tag.Err)
            {
                Debug.LogWarning("Error unListing, msg: " + unlistingReqResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Unlisting success");

            BroadcastState.Invoke(new WaitingForResponse(false));
            newListingPanel.SetActive(false);

            UserUtil.ClearMainData<MainDataTypes.AllListings>();

            Broadcast.Invoke<FetchListings>();
        }

        public override void Close()
        {
            if(nftToListPanel.activeSelf) nftToListPanel.SetActive(false);
            else base.Close();
        }
    }
}