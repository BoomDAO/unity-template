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
        [SerializeField, ShowOnly] string selectedCollectionId;
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

            UserUtil.RegisterToDataChange<DataTypes.NftCollection>(UpdateWindow, true);
            UserUtil.RegisterToDataChange<DataTypes.Listing>(UpdateWindow);

            UserUtil.RequestData<DataTypes.Listing>();
        }

        private void OnDestroy()
        {
            priceInputField.onValueChanged.RemoveListener(InputPriceChangeHandler);
            confirmListingButton.onClick.RemoveListener(ListNftHandler);
            cancelListingButton.onClick.RemoveListener(CancelMakeOfferPanel);

            UserUtil.UnregisterToDataChange<DataTypes.NftCollection>(UpdateWindow);
            UserUtil.UnregisterToDataChange<DataTypes.Listing>(UpdateWindow);
        }
        private void InputPriceChangeHandler(string arg0)
        {
            if (arg0.TryParseValue<double>(out var val)) nftPrice = val;
        }
        private void UpdateWindow(DataState<Data<DataTypes.Listing>> state)
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

            if (state.IsReady() == false)
            {
                //Is Fetching
                if(state.IsLoading())
                {
                    Debug.LogWarning("Fetching Listings");
                    loadingVisualListing.text = "Loading...";
                }
                else Debug.LogWarning("Problem Fetching Listing");
                return;
            }

            if (state.data.elements.Count == 0)
            {
                loadingVisualListing.text = "No listings yet";

                Debug.LogWarning("There is no listing yet");
            }

            foreach (var element in state.data.elements)
            {
                if (element.Value.details.Seller.ToText() == loginData.principal)
                {
                    WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.details.Seller}",
                        content = $"Nft ID:\n{element.Value.tokenIdentifier.AddressToShort()}\n\nIndex:\n{element.Value.index}\n\nPrice:\n{element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nSeller: YOU",
                        textButtonContent = "UNLIST",
                        action = (a, b) => UnlistNft(a, b).Forget(),

                        customData = (
                            Env.Nfts.BOOM_COLLECTION_CANISTER_ID,
                            element.Value.tokenIdentifier,
                            $"{element.Value.details.Seller}",
                            element.Value.details.Price),

                        imageContentType = new ImageContentType.Url($"https://{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}.raw.icp0.io/?&tokenid={element.Value.tokenIdentifier}&type=thumbnail"),
                        infoWindowData = new($"Your NFT Listing Info", $"Price:\n{element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nSeller: YOU\n\nNft ID:\n{element.Value.tokenIdentifier.AddressToShort()}\n\nCanister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}")
                    }, listingContent);
                }
                else
                {
                    WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.details.Seller}",
                        content = $"Nft ID:\n{element.Value.tokenIdentifier.AddressToShort()}\n\nSeller:\n{element.Value.details.Seller.ToText().AddressToShort()}",
                        textButtonContent = $"BUY {element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP",
                        action = (a, b) => Buy(a, b).Forget(),

                        customData = (
                            Env.Nfts.BOOM_COLLECTION_CANISTER_ID,
                            element.Value.tokenIdentifier,
                            $"{element.Value.details.Seller}",
                            element.Value.details.Price),

                        imageContentType = new ImageContentType.Url($"https://{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}.raw.icp0.io/?&tokenid={element.Value.tokenIdentifier}&type=thumbnail"),
                        infoWindowData = new($"NFT Listing Info", $"Price:\n{element.Value.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nSeller: {element.Value.details.Seller.ToText().AddressToShort()}\n\nNft ID:\n{element.Value.tokenIdentifier.AddressToShort()}\n\nCanister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}")
                    }, listingContent);
                }
            }


            var dataTypeResult = UserUtil.GetDataOfType<DataTypes.NftCollection>();
            if (dataTypeResult.Tag == Values.UResultTag.Ok) UpdateWindow(dataTypeResult.AsOk());
            else Debug.LogWarning(dataTypeResult.AsErr());
        }
        private void UpdateWindow(DataState<Data<DataTypes.NftCollection>> nftsState)
        {
            foreach (Transform child in nftToListContent)
            {
                Destroy(child.gameObject);
            }

            loadingVisualNftToList.text = "";
            //

            var listingResult = UserUtil.GetElementsOfType<DataTypes.Listing>();

            if (listingResult.Tag == Values.UResultTag.Err)
            {
                //Is Fetching
                Debug.LogWarning("Fetching Listings, msg: "+ listingResult.AsErr());
                if (nftsState.IsLoading())
                {
                    loadingVisualNftToList.text = "Loading...";
                }

                return;
            }

            var nftCountResult = NftUtil.GetNftCount(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

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

                Debug.LogWarning("You dont have any Nft from collection: "+ Env.Nfts.BOOM_COLLECTION_CANISTER_ID);
            }

            nftsState.data.elements.Once(plethoraCollection =>
            {
                plethoraCollection.Value.tokens.Iterate(token =>
                {

                    var url = token.url;
                    if (listingResult.AsOk().TryLocate(e=> e.tokenIdentifier == token.tokenIdentifier, out var listing))
                    {
                        //if u own the token and is listed
                        WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                        {
                            id = $"{plethoraCollection.Value.canister}|{token.tokenIdentifier}",
                            content = $"Nft ID:\n{token.tokenIdentifier.AddressToShort()}\n\nPrice:\n{listing.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP",
                            textButtonContent = "UNLIST",
                            action = (a, b) => UnlistNft(a, b).Forget(),

                            customData = (
                                plethoraCollection.Value.canister,
                                token.tokenIdentifier,
                                //we ignore the two values below
                                "",(ulong)0),

                            imageContentType = new ImageContentType.Url(url),
                            infoWindowData = new($"NFT Listing Info", $"Price:\n{listing.details.Price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nNft ID:\n{token.tokenIdentifier.AddressToShort()}\n\nCanister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}")
                        }, nftToListContent);
                    }
                    else
                    {
                        WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                        {
                            id = $"{plethoraCollection.Value.canister}|{token.tokenIdentifier}",
                            content = $"Nft ID:\n{token.tokenIdentifier.AddressToShort()}",
                            textButtonContent = "LIST",
                            action = OpenMakeOfferPanel,

                            customData = (
                                plethoraCollection.Value.canister,
                                token.tokenIdentifier,
                                token.index),

                            imageContentType = new ImageContentType.Url(url),
                            infoWindowData = new($"NFT Info", $"Nft ID:\n{token.tokenIdentifier.AddressToShort()}\n\nCanister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}")
                        }, nftToListContent);
                    }
                });
            }, plethoraCollection =>
            {
                return plethoraCollection.Value.canister == Env.Nfts.BOOM_COLLECTION_CANISTER_ID;
            });
        }


        private async UniTaskVoid CheckForSettlement(Extv2BoomApiClient collectionInterface)
        {
            if (checkingForSettlement) return;
            Debug.Log($"CheckForSettlement");

            checkingForSettlement = true;

            while (checkingForSettlement)
            {
                var settlementsResponse = await collectionInterface.HeartbeatPending();

                Debug.Log($"Settlement Count: {settlementsResponse.Count}");
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

            var getAccountIdentifierResult = UserUtil.GetAccountIdentifier();

            if (getAccountIdentifierResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getAccountIdentifierResult.AsErr());
                return;
            }

            var accountIdentifier = getAccountIdentifierResult.AsOk();


            var icpBalanceResult = UserUtil.GetElementOfType<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);

            if(icpBalanceResult.Tag == Values.UResultTag.Err)
            {
                Debug.LogError(icpBalanceResult.AsErr());
                return;
            }

            if(icpBalanceResult.AsOk().baseUnitAmount < price)
            {
                WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You don't have enough ICP", $"Requirements:\n{$"{price.ConvertToDecimal(CandidUtil.ICP_DECIMALS)} ICP\n\nYou need to deposit some ICP"}"), 3);
                return;
            }

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(collectionId));

            var lockResult = await collectionInterface.Lock(nftIdentifier, price, accountIdentifier.value, new());

            if (lockResult.Tag == Candid.Extv2Boom.Models.Result_9Tag.Err)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));

                Debug.LogWarning("Lock err, msg: " + lockResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Lock success, msg: " + lockResult.AsOk());

            var addressToTransferTo = lockResult.AsOk();
            Debug.Log("Transfer from: " + accountIdentifier.value);
            var transferResult = await ActionUtil.Transfer.TransferIcp(price, addressToTransferTo);

            if (transferResult.Tag == Values.UResultTag.Err)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));

                Debug.LogError($"Error transfering money, {transferResult.AsErr()}");
                return;
            }

            var settleResult = await collectionInterface.Settle(nftIdentifier);

            if (settleResult.Tag == Candid.Extv2Boom.Models.Result_5Tag.Err)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));

                var err = settleResult.AsErr();

                if(err.Tag == Candid.Extv2Boom.Models.CommonError__1Tag.InvalidToken)
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

            UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));

            BroadcastState.Invoke(new WaitingForResponse(false));
            UserUtil.Clean<DataTypes.Listing>();
            UserUtil.RequestData<DataTypes.Listing>();
        }
        private void OpenMakeOfferPanel(string id, object customData)
        {
            //Disable all Inventory and offer action button
            newListingPanel.SetActive(true);
            confirmListingButton.enabled = true;

            (selectedCollectionId, selectedNftIdentifier, selectedNftIndex) = ((string, string, uint))customData;

            newListingContentText.text = $"Canister ID:\n{selectedCollectionId.AddressToShort()}\n\nNft ID:\n{selectedNftIdentifier.AddressToShort()}\n\nNft Index: {selectedNftIndex}";
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

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(selectedCollectionId));

            var listingReqResult = await collectionInterface.List(new Candid.Extv2Boom.Models.ListRequest(new(), new(nftPrice.ConvertToBaseUnit(CandidUtil.ICP_DECIMALS)), selectedNftIdentifier));

            if (listingReqResult.Tag == Candid.Extv2Boom.Models.Result_5Tag.Ok)
            {
                BroadcastState.Invoke(new WaitingForResponse(false));
                newListingPanel.SetActive(false);
                Close();
                UserUtil.RequestData<DataTypes.Listing>();

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

            if (unlistingReqResult.Tag == Candid.Extv2Boom.Models.Result_5Tag.Err)
            {
                Debug.LogWarning("Error unListing, msg: " + unlistingReqResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Unlisting success");

            BroadcastState.Invoke(new WaitingForResponse(false));
            newListingPanel.SetActive(false);

            UserUtil.Clean<DataTypes.Listing>();
            UserUtil.RequestData<DataTypes.Listing>();
        }

        public override void Close()
        {
            if(nftToListPanel.activeSelf) nftToListPanel.SetActive(false);
            else base.Close();
        }
    }
}