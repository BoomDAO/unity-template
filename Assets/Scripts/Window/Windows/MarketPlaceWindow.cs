namespace ItsJackAnton.UI
{
    using Candid;
    using Candid.extv2_boom;
    using Candid.IcpLedger.Models;
    using EdjCase.ICP.Candid.Models;
    using global::Mono.CSharp;
    using ItsJackAnton.Patterns.Broadcasts;
    using ItsJackAnton.Utility;
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
            BroadcastState.Register<DataState<DabNftsData>>(UpdateWindow, true);
            BroadcastState.Register<DataState<ListingData>>(UpdateWindow);

            FetchListings();
        }

        private void OnDestroy()
        {
            priceInputField.onValueChanged.RemoveListener(InputPriceChangeHandler);
            confirmListingButton.onClick.RemoveListener(ListNft);
            cancelListingButton.onClick.RemoveListener(CancelCreateListing);

            FetchListings();

            BroadcastState.Unregister<DataState<DabNftsData>>(UpdateWindow);
            BroadcastState.Unregister<DataState<ListingData>>(UpdateWindow);
        }
        private void InputPriceChangeHandler(string arg0)
        {
            if (arg0.TryParseValue<double>(out var val)) nftPrice = val;
        }
        private async void UpdateWindow(DataState<ListingData> state)
        {
            foreach (Transform child in offerContent)
            {
                Destroy(child.gameObject);
            }
            //

            if(state.IsReady() == false)
            {
                //Is Fetching
                Debug.LogWarning("Fetching Listings");
                return;
            }
            if(state.data.listing.Count == 0)
            {
                //There is no listings
                Debug.LogWarning("There is no listing yet");
                return;
            }

            foreach (var element in state.data.listing)
            {
                Debug.Log("Offer of token of index: " + element.Value.F0);
                if(element.Value.F1.Seller.ToText() == CandidApiManager.UserPrincipal)
                {
                    WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.F1.Seller}",
                        content = $"Canister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}\n\nNft ID:\n{element.Key.AddressToShort()}\n\nPrice:\n{element.Value.F1.Price.DestokenizeFromIcp()}\n\nSeller: YOU",
                        textButtonContent = "UNLIST",
                        action = UnlistNft,
                        customData = (Env.Nfts.BOOM_COLLECTION_CANISTER_ID, element.Key, $"{element.Value.F1.Seller}", element.Value.F1.Price)
                    }, offerContent);
                }
                else
                {
                    WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = $"{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}|{element.Key}|{element.Value.F1.Seller}",
                        content = $"Canister ID:\n{Env.Nfts.BOOM_COLLECTION_CANISTER_ID.AddressToShort()}\n\nNft ID:\n{element.Key.AddressToShort()}\n\nPrice:\n{element.Value.F1.Price.DestokenizeFromIcp()}\n\nSeller:\n{element.Value.F1.Seller.ToText().AddressToShort()}",
                        textButtonContent = "BUY",
                        action = Buy,
                        customData = (Env.Nfts.BOOM_COLLECTION_CANISTER_ID, element.Key, $"{element.Value.F1.Seller}", element.Value.F1.Price)
                    }, offerContent);
                }
            }
        }
        private void UpdateWindow(DataState<DabNftsData> nftsState)
        {
            foreach (Transform child in inventoryContent)
            {
                Destroy(child.gameObject);
            }
            //

            if(!BroadcastState.TryRead<DataState<ListingData>>(out var listingState))
            {
                //Listing no ready
                return;
            }

            if (listingState.IsReady() == false)
            {
                //Is Fetching
                Debug.LogWarning("Fetching Listings");
                return;
            }

            if (nftsState.IsReady() == false)
            {
                //Is Fetching
                Debug.LogWarning("Fetching Nfts");
                return;
            }

            nftsState.data.plethoraNftCollections.Do(plethoraCollection =>
            {
                plethoraCollection.tokens.Iterate(token =>
                {
                    if(listingState.data.listing.Has(e=> e.Key == token.tokenIdentifier))
                    {
                        //if u own the token and is listed
                        WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                        {
                            id = $"{plethoraCollection.canisterId}|{token.tokenIdentifier}",
                            content = $"Canister ID:\n{plethoraCollection.canisterId.AddressToShort()}\n\nNft ID:\n{token.tokenIdentifier.AddressToShort()}",
                            textButtonContent = "UNLIST",
                            action = UnlistNft,
                            customData = (plethoraCollection.canisterId, token.tokenIdentifier, token.index)
                        }, inventoryContent);
                    }
                    else
                    {
                        WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                        {
                            id = $"{plethoraCollection.canisterId}|{token.tokenIdentifier}",
                            content = $"Canister ID:\n{plethoraCollection.canisterId.AddressToShort()}\n\nNft ID:\n{token.tokenIdentifier.AddressToShort()}",
                            textButtonContent = "LIST",
                            action = OpenMakeOfferPanel,
                            customData = (plethoraCollection.canisterId, token.tokenIdentifier, token.index)
                        }, inventoryContent);
                    }
                });
            }, plethoraCollection =>
            {
                return plethoraCollection.canisterId == Env.Nfts.BOOM_COLLECTION_CANISTER_ID;
            });
        }

        private async void FetchListings()
        {
            BroadcastState.ForceInvoke<DataState<ListingData>>(e =>
            {
                e.SetAsLoading();
                return e;
            });

            Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(Env.Nfts.BOOM_COLLECTION_CANISTER_ID));

            var listingResult = await collectionInterface.Listings();

            Dictionary<string, Extv2BoomApiClient.ListingsArg0Item> listing = new();

            foreach (var item in listingResult)
            {
                var tokenIdentifier = await CandidApiManager.Instance.WorldHub.GetTokenIdentifier(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, item.F0);
                listing.Add(tokenIdentifier, item);
            }

            BroadcastState.ForceInvoke<DataState<ListingData>>(e =>
            {

                e.data = new(listing);
                e.SetAsReady();
                return e;
            });
        }

        private async void Buy(string arg0, object customData)
        {
            (string collectionId, string nftIdentifier, string seller, ulong price) = ((string, string, string, ulong))customData;

            var icpBalanceResult = UserUtil.GetToken(Env.CanisterIds.ICP_LEDGER);

            if(icpBalanceResult.Tag == Values.UResultTag.Err)
            {
                Debug.Log(icpBalanceResult.AsErr());
                return;
            }

            Debug.Log($"Try buy nft of id {nftIdentifier}, price: {price.DestokenizeFromIcp()}, you have {icpBalanceResult.AsOk().Amount}");

            Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(collectionId));

            var lockResult = await collectionInterface.Lock(nftIdentifier, price, CandidApiManager.UserAccountIdentity, new());

            if (lockResult.Tag != Candid.extv2_boom.Models.Result_7Tag.Ok)
            {
                BroadcastState.Invoke(new ToggleActionWidgetState(true));
                FetchListings();
                Debug.LogWarning("Lock err, msg: " + lockResult.AsErr().Value.ToString());
                return;
            }

            Debug.Log("Lock success, msg: " + lockResult.AsOk());

            var addressToTransferTo = lockResult.AsOk();
            Debug.Log("Transfer from: " + CandidApiManager.UserAccountIdentity);
            var transferResult = await TxUtil.Transfer_ICP(price, addressToTransferTo);

            if (transferResult.Tag == Values.UResultTag.Err)
            {
                BroadcastState.Invoke(new ToggleActionWidgetState(true));
                FetchListings();
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
                FetchListings();

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

            CandidApiManager.Instance.FetchUserNfts();
            //BroadcastState.ForceInvoke<DataState<DabNftsData>>(e =>
            //{
            //    if(e.data.plethoraNftCollections.TryLocate(e=>e.canisterId == collectionId, out var collection) == false)
            //    {
            //        collection = new() { canisterId = collectionId, tokens = new()};
            //        e.data.plethoraNftCollections.Add(collection);
            //    }
            //    collection.tokens.Add(new DabNftDetails() { })
            //    return e;
            //});

            BroadcastState.Invoke(new ToggleActionWidgetState(true));
            FetchListings();
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
            BroadcastState.Invoke(new ToggleActionWidgetState(false));
            listingPanel.SetActive(true);
            confirmListingButton.enabled = true;

            (selectedCollectionId, selectedNftIdentifier, selectedNftIndex) = ((string, string, long))customData;

            listingContentText.text = $"Canister ID:\n{selectedCollectionId.AddressToShort()}\n\nNft ID:\n{selectedNftIdentifier.AddressToShort()}\n\nNft Index: {selectedNftIndex}";
        }

        private async void ListNft()
        {
            if(nftPrice < 0.001)
            {
                Debug.LogError($"Listing price too low, min price is: 0.001 Icp");
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
                        FetchListings();

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
            listingPanel.SetActive(false);
        }

        private async void UnlistNft(string widgetId, object customData)
        {
            (string collectionId, string nftIdentifier, _, _) = ((string, string, string, ulong))customData;

            if (!BroadcastState.TryRead<DataState<DabNftsData>>(out var nfts))
            {
                Debug.LogWarning("DabNftsData not found");
                return;
            }

            if (!nfts.IsReady())
            {
                Debug.LogWarning("DabNftsData not ready");
                return;
            }

            Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(collectionId));

            var listingReqResult = await collectionInterface.List(new Candid.extv2_boom.Models.ListRequest(new(), new(), nftIdentifier));

            if (listingReqResult.Tag == Candid.extv2_boom.Models.Result_3Tag.Err)
            {
                Debug.LogWarning("Error Listing, msg: " + listingReqResult.AsErr().Value.ToString());
                return;


            }

            Debug.Log("Listing success");

            BroadcastState.Invoke(new ToggleActionWidgetState(true));
            listingPanel.SetActive(false);
            FetchListings();
        }
    }
}