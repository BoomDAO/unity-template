namespace ItsJackAnton.UI
{
    using Candid;
    using Candid.extv2_boom;
    using Candid.IcpLedger.Models;
    using EdjCase.ICP.Candid.Models;
    using global::Mono.CSharp;
    using ItsJackAnton.Patterns.Broadcasts;
    using ItsJackAnton.Utility;
    using ItsJackAnton.Values;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using TMPro;
    using Unity.VisualScripting.Antlr3.Runtime;
    using UnityEngine;
    using UnityEngine.UI;

    public class MarketPlaceWindow : Window
    {
        [SerializeField] Transform listingContent;

        [SerializeField] Transform inventoryContent;
        [SerializeField] GameObject newListingPanel;
        [SerializeField] TMP_Text newListingContentText;
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
            //WindowData windowData = (WindowData)data;
            //if (windowData == null)
            //{
            //    Debug.Log($"Window of name {gameObject.name}, requires data, data cannot be null");
            //    return;
            //}
            priceInputField.onValueChanged.AddListener(InputPriceChangeHandler);
            confirmListingButton.onClick.AddListener(ListNft);
            cancelListingButton.onClick.AddListener(CancelCreateListing);

            UserUtil.RegisterToDataChange<DataTypes.BoomDaoNftCollection>(UpdateWindow, true);
            UserUtil.RegisterToDataChange<DataTypes.Listing>(UpdateWindow);

            UserUtil.RequestData<DataTypes.Listing>();
        }

        private void OnDestroy()
        {
            priceInputField.onValueChanged.RemoveListener(InputPriceChangeHandler);
            confirmListingButton.onClick.RemoveListener(ListNft);
            cancelListingButton.onClick.RemoveListener(CancelCreateListing);

            UserUtil.UnregisterToDataChange<DataTypes.BoomDaoNftCollection>(UpdateWindow);
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
            //
            var getLoginDataResult = UserUtil.GetSignInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getLoginDataResult.AsErr());
                return;
            }

            var loginData = getLoginDataResult.AsOk();

            if (state.IsReady() == false)
            {
                //Is Fetching
                if(state.IsLoading()) Debug.LogWarning("Fetching Listings");
                else Debug.LogWarning("Problem Fetching Listing");
                return;
            }
            if(state.data.elements.Count == 0)
            {
                //There is no listings. No need to return.
                Debug.LogWarning("There is no listing yet");
            }

            foreach (var element in state.data.elements)
            {
                if(element.Value.F1.Seller.ToText() == loginData.principal)
                {
                    WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.F1.Seller}",
                        content = $"Canister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}\n\nNft ID:\n{element.Key.AddressToShort()}\n\nPrice:\n{element.Value.F1.Price.ConvertToDecimal(CandidUtil.BASE_UNIT_ICP)}\n\nSeller: YOU",
                        textButtonContent = "UNLIST",
                        action = UnlistNft,
                        customData = (Env.Nfts.BOOM_COLLECTION_CANISTER_ID, element.Key, $"{element.Value.F1.Seller}", element.Value.F1.Price)
                    }, listingContent);
                }
                else
                {
                    WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.F1.Seller}",
                        content = $"Canister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}\n\nNft ID:\n{element.Key.AddressToShort()}\n\nPrice:\n{element.Value.F1.Price.ConvertToDecimal(CandidUtil.BASE_UNIT_ICP)}\n\nSeller:\n{element.Value.F1.Seller.ToText().AddressToShort()}",
                        textButtonContent = "BUY",
                        action = Buy,
                        customData = (Env.Nfts.BOOM_COLLECTION_CANISTER_ID, element.Key, $"{element.Value.F1.Seller}", element.Value.F1.Price)
                    }, listingContent);
                }
            }


            var dataTypeResult = UserUtil.GetDataStateOfType<DataTypes.BoomDaoNftCollection>();
            if (dataTypeResult.Tag == Values.UResultTag.Ok) UpdateWindow(dataTypeResult.AsOk());
            else Debug.LogError(dataTypeResult.AsErr());
        }
        private void UpdateWindow(DataState<Data<DataTypes.BoomDaoNftCollection>> nftsState)
        {
            foreach (Transform child in inventoryContent)
            {
                Destroy(child.gameObject);
            }
            //

            var listingResult = UserUtil.GetDataElementsOfType<DataTypes.Listing>();

            if (listingResult.Tag == Values.UResultTag.Err)
            {
                //Is Fetching
                Debug.LogWarning("Fetching Listings, msg: "+ listingResult.AsErr());
                return;
            }

            if (nftsState.IsReady() == false)
            {
                //Is Fetching
                Debug.LogWarning("Fetching Nfts");
                return;
            }

            nftsState.data.elements.Do(plethoraCollection =>
            {
                plethoraCollection.Value.tokens.Iterate(token =>
                {
                    if(listingResult.AsOk().Has(e=> e.tokenIdentifier == token.tokenIdentifier))
                    {
                        //if u own the token and is listed
                        WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                        {
                            id = $"{plethoraCollection.Value.canisterId}|{token.tokenIdentifier}",
                            content = $"Canister ID:\n{plethoraCollection.Value.canisterId.AddressToShort()}\n\nNft ID:\n{token.tokenIdentifier.AddressToShort()}",
                            textButtonContent = "UNLIST",
                            action = UnlistNft,
                            customData = (plethoraCollection.Value.canisterId, token.tokenIdentifier, token.index)
                        }, inventoryContent);
                    }
                    else
                    {
                        WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                        {
                            id = $"{plethoraCollection.Value.canisterId}|{token.tokenIdentifier}",
                            content = $"Canister ID:\n{plethoraCollection.Value.canisterId.AddressToShort()}\n\nNft ID:\n{token.tokenIdentifier.AddressToShort()}",
                            textButtonContent = "LIST",
                            action = OpenMakeOfferPanel,
                            customData = (plethoraCollection.Value.canisterId, token.tokenIdentifier, token.index)
                        }, inventoryContent);
                    }
                });
            }, plethoraCollection =>
            {
                return plethoraCollection.Value.canisterId == Env.Nfts.BOOM_COLLECTION_CANISTER_ID;
            });
        }

        private async void Buy(string arg0, object customData)
        {
            BroadcastState.Invoke(new DisableButtonInteraction(true));
            (string collectionId, string nftIdentifier, string seller, ulong price) = ((string, string, string, ulong))customData;

            var getAccountIdentifierResult = UserUtil.GetAccountIdentifier();

            if (getAccountIdentifierResult.Tag == UResultTag.Err)
            {
                Debug.LogError(getAccountIdentifierResult.AsErr());
                return;
            }

            var accountIdentifier = getAccountIdentifierResult.AsOk();


            var icpBalanceResult = UserUtil.GetDataElementOfType<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);

            if(icpBalanceResult.Tag == Values.UResultTag.Err)
            {
                Debug.LogError(icpBalanceResult.AsErr());
                return;
            }

            Debug.Log($"Try buy nft of id {nftIdentifier}, price: {price.ConvertToDecimal(CandidUtil.BASE_UNIT_ICP)}, you have {icpBalanceResult.AsOk().Amount}");

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(collectionId));

            var lockResult = await collectionInterface.Lock(nftIdentifier, price, accountIdentifier.value, new());

            if (lockResult.Tag == Candid.extv2_boom.Models.Result_7Tag.Err)
            {
                BroadcastState.Invoke(new DisableButtonInteraction(false));

                Debug.LogWarning("Lock err, msg: " + lockResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Lock success, msg: " + lockResult.AsOk());

            var addressToTransferTo = lockResult.AsOk();
            Debug.Log("Transfer from: " + accountIdentifier.value);
            var transferResult = await TxUtil.Transfer.TransferIcp(price, addressToTransferTo);

            if (transferResult.Tag == Values.UResultTag.Err)
            {
                BroadcastState.Invoke(new DisableButtonInteraction(false));

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
                BroadcastState.Invoke(new DisableButtonInteraction(false));

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

            CheckForSettlement(collectionInterface);

            UserUtil.RequestData<DataTypes.BoomDaoNftCollection>();

            BroadcastState.Invoke(new DisableButtonInteraction(false));
            UserUtil.RequestData<DataTypes.Listing>();
        }

        async void CheckForSettlement(Extv2BoomApiClient collectionInterface)
        {
            if (checkingForSettlement) return;
            Debug.Log($"CheckForSettlement");

            checkingForSettlement = true;

            while (checkingForSettlement)
            {
                var settlementsResponse = await collectionInterface.HeartbeatPending();

                Debug.Log($"Settlement Count: {settlementsResponse.Count}");
                if(settlementsResponse.Count > 0)
                {
                    await collectionInterface.HeartbeatExternal();

                    continue;
                }

                checkingForSettlement = false;
            }
        }

        private void OpenMakeOfferPanel(string id, object customData)
        {
            //Disable all Inventory and offer action button
            BroadcastState.Invoke(new DisableButtonInteraction(true));
            newListingPanel.SetActive(true);
            confirmListingButton.enabled = true;

            (selectedCollectionId, selectedNftIdentifier, selectedNftIndex) = ((string, string, long))customData;

            newListingContentText.text = $"Canister ID:\n{selectedCollectionId.AddressToShort()}\n\nNft ID:\n{selectedNftIdentifier.AddressToShort()}\n\nNft Index: {selectedNftIndex}";
        }

        private async void ListNft()
        {
            if(nftPrice < 0.001)
            {
                Debug.LogError($"Listing price too low, min price is: 0.001 Icp");
                return;
            }

            confirmListingButton.enabled = false;

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == Values.UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(selectedCollectionId));

            var listingReqResult = await collectionInterface.List(new Candid.extv2_boom.Models.ListRequest(new(), new(nftPrice.ConvertToBaseUnit(CandidUtil.BASE_UNIT_ICP)), selectedNftIdentifier));

            if (listingReqResult.Tag == Candid.extv2_boom.Models.Result_3Tag.Ok)
            {
                BroadcastState.Invoke(new DisableButtonInteraction(false));
                newListingPanel.SetActive(false);
                UserUtil.RequestData<DataTypes.Listing>();

                Debug.Log("Listing success");

            }
            else
            {
                Debug.LogWarning("Error Listing, msg: " + listingReqResult.AsErr().Value.ToString());
            }
        }
        private void CancelCreateListing()
        {
            BroadcastState.Invoke(new DisableButtonInteraction(false));
            newListingPanel.SetActive(false);
        }

        private async void UnlistNft(string widgetId, object customData)
        {
            (string collectionId, string nftIdentifier, _, _) = ((string, string, string, ulong))customData;

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                Debug.Log(getAgentResult.AsErr());
                return;
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(collectionId));

            var unlistingReqResult = await collectionInterface.List(new Candid.extv2_boom.Models.ListRequest(new(), new(), nftIdentifier));

            if (unlistingReqResult.Tag == Candid.extv2_boom.Models.Result_3Tag.Err)
            {
                Debug.LogWarning("Error unListing, msg: " + unlistingReqResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Listing success");

            BroadcastState.Invoke(new DisableButtonInteraction(false));
            newListingPanel.SetActive(false);
            UserUtil.RequestData<DataTypes.Listing>();
        }
    }
}