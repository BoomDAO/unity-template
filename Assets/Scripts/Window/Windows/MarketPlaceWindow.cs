namespace ItsJackAnton.UI
{
    using Candid;
    using Candid.extv2_boom;
    using Candid.IcpLedger.Models;
    using EdjCase.ICP.Candid.Models;
    using ItsJackAnton.Patterns.Broadcasts;
    using ItsJackAnton.Utility;
    using System;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MarketPlaceWindow : Window
    {
        [SerializeField] Transform offerContent;

        [SerializeField] Transform inventoryContent;
        [SerializeField] GameObject listingPanel;
        [SerializeField] TMP_Text listingContentText;
        [SerializeField] Button confirmListingButton;
        [SerializeField] Button cancelListingButton;
        [SerializeField] TMP_InputField priceInputField;
        [SerializeField, ShowOnly] double nftPrice;
        [SerializeField, ShowOnly] string selectedCollectionId;
        [SerializeField, ShowOnly] string selectedNftIdentifier;
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
            //WindowData windowData = (WindowData)data;
            //if (windowData == null)
            //{
            //    Debug.Log($"Window of name {gameObject.name}, requires data, data cannot be null");
            //    return;
            //}
            priceInputField.onValueChanged.AddListener(InputPriceChangeHandler);
            confirmListingButton.onClick.AddListener(ListNft);
            cancelListingButton.onClick.AddListener(CancelCreateListing);
            BroadcastState.Register<DataState<DabNftsData>>(UpdateWindow, true);
            UpdateOfferPanel();
        }

        private void OnDestroy()
        {
            priceInputField.onValueChanged.RemoveListener(InputPriceChangeHandler);
            confirmListingButton.onClick.RemoveListener(ListNft);
            cancelListingButton.onClick.RemoveListener(CancelCreateListing);
            BroadcastState.Unregister<DataState<DabNftsData>>(UpdateWindow);
        }
        private void InputPriceChangeHandler(string arg0)
        {
            if (arg0.TryParseValue<double>(out var val)) nftPrice = val;
        }

        private void UpdateWindow(DataState<DabNftsData> obj)
        {
            foreach (Transform child in inventoryContent)
            {
                Destroy(child.gameObject);
            }

            if (obj.IsReady())
            {
                obj.data.plethoraNftCollections.Do(plethoraCollection =>
                {
                    plethoraCollection.tokens.Iterate(token =>
                    {
                        WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                        {
                            id = $"{plethoraCollection.canisterId}|{token.tokenIdentifier}",
                            content = $"Canister ID:\n{plethoraCollection.canisterId.AddressToShort()}\n\nNft ID:\n{token.tokenIdentifier.AddressToShort()}",
                            textButtonContent = "LIST",
                            action = OpenMakeOfferPanel
                        }, inventoryContent);
                    });
                }, plethoraCollection =>
                {
                    return plethoraCollection.canisterId == Env.Nfts.BOOM_COLLECTION;
                });
            }
            else
            {
                Debug.LogWarning("DabNftsData not ready");
            }
        }

        private async void UpdateOfferPanel()
        {
            foreach (Transform child in offerContent)
            {
                Destroy(child.gameObject);
            }
            Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(Env.Nfts.BOOM_COLLECTION));

            var listingResult = await collectionInterface.Listings();

            if(listingResult.Count > 0)
            {


                foreach(var element in listingResult)
                {
                    Debug.Log("Offer of token of index: "+ element.F0);

                    var tokenIdentifier = await CandidApiManager.Instance.WorldHub.GetTokenIdentifier(Env.Nfts.BOOM_COLLECTION, element.F0);
                    WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{Env.Nfts.BOOM_COLLECTION}|{tokenIdentifier}|{element.F1.Price}|{element.F1.Seller}",
                        content = $"Canister ID:\n{Env.Nfts.BOOM_COLLECTION.AddressToShort()}\n\nNft ID:\n{tokenIdentifier.AddressToShort()}\n\nPrice:\n{element.F1.Price.DestokenizeFromIcp()}\n\nSeller:\n{element.F1.Seller.ToText().AddressToShort()}",
                        textButtonContent = "BUY",
                        action = Buy
                    }, offerContent);
                }
            }
            else
            {
                Debug.LogWarning("There is no listing yet");
            }
        }

        private async void Buy(string arg0)
        {
            var canisterIdAndNftIndex = arg0.Split('|');
            var collectionId = canisterIdAndNftIndex[0];
            var nftIdentifier = canisterIdAndNftIndex[1];
            canisterIdAndNftIndex[2].TryParseValue(out ulong price);
            var seller = canisterIdAndNftIndex[3];

            BroadcastState.TryRead<DataState<IcpData>>(out var icpAmt);

            Debug.Log($"Try buy nft of id {nftIdentifier}, price: {price.DestokenizeFromIcp()}, you have {((ulong)icpAmt.data.amt).DestokenizeFromIcp()}");

            Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(collectionId));

            var lockResult = await collectionInterface.Lock(nftIdentifier, price, CandidApiManager.UserAccountIdentity, new());

            if (lockResult.Tag != Candid.extv2_boom.Models.Result_7Tag.Ok)
            {
                BroadcastState.Invoke(new ToggleActionWidgetState(true));
                UpdateOfferPanel();
                Debug.LogWarning("Lock err, msg: " + lockResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Lock success, msg: " + lockResult.AsOk());

            var addressToTransferTo = lockResult.AsOk();
            Debug.Log("Transfer from: " + CandidApiManager.UserAccountIdentity);
            var transferArg = CandidUtil.SetupTransfer_IC(price, addressToTransferTo);
            var transferResult = await TxUtil.Transfer_ICP(transferArg);

            if (transferResult.Tag == Values.UResultTag.Err)
            {
                BroadcastState.Invoke(new ToggleActionWidgetState(true));
                UpdateOfferPanel();
                Debug.LogError($"Error transfering money, {transferResult.AsErr()}");
                return;
            }
            else if (transferResult.Tag == Values.UResultTag.None)
            {
                Debug.LogError("Error, transfer result is not setup");
                return;
            }

            var settleResult = await collectionInterface.Settle(nftIdentifier);

            if (settleResult.Tag == Candid.extv2_boom.Models.Result_3Tag.Err)
            {
                BroadcastState.Invoke(new ToggleActionWidgetState(true));
                UpdateOfferPanel();

                var err = settleResult.AsErr();

                if(err.Tag == Candid.extv2_boom.Models.CommonErrorTag.InvalidToken)
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

            BroadcastState.Invoke(new ToggleActionWidgetState(true));
            UpdateOfferPanel();
        }

        private void OpenMakeOfferPanel(string id)
        {
            //Disable all Inventory and offer action button
            BroadcastState.Invoke(new ToggleActionWidgetState(false));
            listingPanel.SetActive(true);
            confirmListingButton.enabled = true;

            var canisterIdAndNftIndex = id.Split('|');
            selectedCollectionId = canisterIdAndNftIndex[0];
            selectedNftIdentifier = canisterIdAndNftIndex[1];

            listingContentText.text = $"Canister ID:\n{selectedCollectionId.AddressToShort()}\n\nNft ID:\n{selectedNftIdentifier.AddressToShort()}";
        }

        private async void ListNft()
        {
            if(nftPrice < 0.0001)
            {
                Debug.LogError($"Listing price too low, min price is: 0.0001 Icp");
                return;
            }

            confirmListingButton.enabled = false;

            if (BroadcastState.TryRead<DataState<DabNftsData>>(out var nfts))
            {
                if (nfts.IsReady())
                {
                    Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(selectedCollectionId));

                    var listingReqResult = await collectionInterface.List(new Candid.extv2_boom.Models.ListRequest(new(), new(nftPrice.TokenizeToIcp()), selectedNftIdentifier));

                    if(listingReqResult.Tag == Candid.extv2_boom.Models.Result_3Tag.Ok)
                    {
                        BroadcastState.Invoke(new ToggleActionWidgetState(true));
                        listingPanel.gameObject.SetActive(false);
                        UpdateOfferPanel();

                        Debug.Log("Listing success");

                    }
                    else
                    {
                        Debug.LogWarning("Error Listing, msg: "+ listingReqResult.AsErr().Value.ToString());

                    }
                }
                else
                {
                    Debug.LogWarning("DabNftsData not ready");
                }
            }
            else
            {
                Debug.LogWarning("DabNftsData not found");
            }
        }
        private void CancelCreateListing()
        {
            BroadcastState.Invoke(new ToggleActionWidgetState(true));
            listingPanel.gameObject.SetActive(false);
        }
    }
}