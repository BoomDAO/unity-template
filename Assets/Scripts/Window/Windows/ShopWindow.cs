using Candid;
using Candid.IcpLedger.Models;
using Candid.World.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : Window
{
    [SerializeField] List<string> actionOfferIds;

    [SerializeField] Transform content;
    [SerializeField] Button goToBurnButton;
    [SerializeField] Button closeButton;
    [SerializeField, ShowOnly] bool buying;
    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        BroadcastState.Register<DataState<UserNodeData>>(UpdateWindow, true);
        BroadcastState.Register<DataState<WorldConfigsData>>(UpdateWindow);

    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<DataState<UserNodeData>>(UpdateWindow);
        BroadcastState.Unregister<DataState<WorldConfigsData>>(UpdateWindow);

    }

    private void UpdateWindow(DataState<WorldConfigsData> obj)
    {
        BroadcastState.TryRead<DataState<UserNodeData>>(out var coreUserDataState);

        UpdateWindow(coreUserDataState);
    }

    private void UpdateWindow(DataState<UserNodeData> obj)
    {
        var loggedIn = CandidApiManager.IsUserLoggedIn;
        Debug.Log("A");
        if (loggedIn == false) return;

        var validActionOffers = GetValidOffers(actionOfferIds);
        Debug.Log("B: "+validActionOffers.Count);

        validActionOffers.Iterate(actionOffer =>
        {

            if (actionOffer.value.ActionDataType.Tag == ActionDataTypeTag.SpendTokens)
            {
                var config = actionOffer.value.ActionDataType.AsSpendTokens();
                ActionWidget aw = WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionOffer.key,
                    textButtonContent = $"{actionOffer.key}",
                    content = $"{actionOffer.value.Name.ValueOrDefault}\n\nprice: {config.Amt}",
                    action = async (m) =>
                    {

                        double amt = config.Amt;
                        config.BaseZeroCount.TryToUInt64(out var baseZeroCount);
                        string tokenCanister = config.TokenCanister.ValueOrDefault;
                        var tokenizedAmt = CandidUtil.Tokenize(amt, baseZeroCount);

                        Debug.Log($"Call to action of id: SpendTokens - {actionOffer.key}. Tokens to spend: {tokenizedAmt}");

                        UResult<ulong, string> transferResult = default;
                        if (config.TokenCanister.HasValue == false)
                        {//IC
                            var transferArg = CandidUtil.SetupTransfer_IC(tokenizedAmt, CandidApiManager.PaymentCanisterOfferIdentifier);
                            transferResult = await TxUtil.Transfer_ICP(transferArg);
                        }
                        else
                        {//ICRC
                            var transferArg = CandidUtil.SetupTransfer_RC(tokenizedAmt, Env.CanisterIds.PAYMENT_HUB);
                            transferResult = await TxUtil.Transfer_RC(transferArg);
                        }

                        if (transferResult.Tag != UResultTag.Ok)
                        {
                            Debug.LogError("Transfer Failure, msg: "+ transferResult.AsErr());
                            return;
                        }

                        var transferHash = transferResult.AsOk();
                        var actionResult = await TxUtil.ProcessPlayerAction(new ActionArgValueTypes.SpendTokensArg(m, transferHash));

                        if (actionResult.Tag != UResultTag.Ok)
                        {
                            Debug.LogError("ActionResult Failure, msg: " + actionResult.AsErr());
                            return;
                        }

                        Debug.Log("Spend Token Success");
                    }
                }, content);
            }
            else if (actionOffer.value.ActionDataType.Tag == ActionDataTypeTag.SpendEntities)
            {
                var config = actionOffer.value.ActionDataType.AsSpendEntities();

                ActionWidget aw = WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionOffer.key,
                    textButtonContent = $"{actionOffer.key}",
                    content = $"{actionOffer.value.Name.ValueOrDefault}",
                    action = async (m) =>
                    {
                        Debug.Log("Call to action of id: SpendEntities -" + actionOffer.key);
                    }
                }, content);
            }
            else Debug.Log($"Trying to call wrong action type of id: {actionOffer.value.ActionDataType.Tag} -" + actionOffer.key);
        });
    }

    private List<KeyValue<string, ActionConfig>> GetValidOffers(List<string> actionOfferIds)
    {
        BroadcastState.TryRead<DataState<WorldConfigsData>>(out var worldConfigsData);

        if (worldConfigsData.IsReady() == false)
        {
            Debug.LogError("OffersConfig is not ready");
            return new();
        }

        List<KeyValue<string, ActionConfig>> offers = new();

        actionOfferIds.Iterate(e =>
        {
            if (UserUtil.TryGetActionConfigData(e, out var config) == false)
            {
                Debug.LogError($"id {e} doesn't exist in configs");
                return;
            }

            if((config.ActionDataType.Tag == ActionDataTypeTag.SpendTokens || config.ActionDataType.Tag == ActionDataTypeTag.SpendEntities) == false)
            {
                Debug.LogError($"id {e} action config is not of type spend tokens nor spend entities");
                return;
            }

            offers.Add(new(e, config));
        });

        return offers;
    }

    //private async void BuyItemOfferHandler_IC()
    //{
    //    BroadcastState.TryRead<DataState<OffersConfig>>(out var offersConfig);

    //    if (offersConfig.data.TryGetConfig(itemOfferId_ic, out var offerConfig) == false)
    //    {
    //        Debug.LogError($"Offer of id {itemOfferId_ic} doesn't exist");
    //        return;
    //    }

    //    buying = true;
    //    BroadcastState.TryRead<DataState<UserNodeData>>(out var coreUserDataState);

    //    //Lets update the window so button disable based on "buying" state
    //    UpdateWindow(coreUserDataState);

    //    Debug.Log("Calling Transfer");
    //    var transferResult = await TxUtil.TransferToOfferCanister_ICP(offerConfig.price);

    //    if (transferResult.State == UResultState.Err)
    //    {
    //        buying = false;

    //        Debug.Log($"Transfer failed, msg: {transferResult.AsErr()}");
    //        return;
    //    }

    //    var verifyTransResponse =
    //        await TxUtil.VerifyTx_ICP(transferResult.AsOk(), offerConfig.price, "offer", $"{itemOfferId_ic}");
    //    buying = false;

    //    if (verifyTransResponse.State == UResultState.Ok)
    //    {
    //        Debug.Log("Payment success");

    //        var okValue = verifyTransResponse.AsOk();
    //        var gameTx = okValue.gameTx;
    //        var nfts = okValue.nfts;

    //        var addedItems = gameTx.ItemsItem_.Add;

    //        BroadcastState.ForceInvoke<DataState<UserNodeData>>(coreUserDataState =>
    //        {
    //            if (gameTx != null)
    //            {
    //                addedItems.Iterate(addedItem =>
    //                {
    //                    Debug.Log($"Add item off id {addedItem.Id}, quantity: {addedItem.Quantity}");

    //                    if (coreUserDataState.data.items.TryAdd(addedItem.Id, new(addedItem.Id, addedItem.Quantity)) == false)
    //                    {
    //                        Debug.Log($"Add item off id {addedItem.Id}, quantity: {addedItem}");
    //                        coreUserDataState.data.items[addedItem.Id].quantity += addedItem.Quantity;
    //                    }
    //                });
    //            }

    //            return coreUserDataState;
    //        });
    //    }
    //    else
    //    {
    //        Debug.Log("Payment failure, msg: "+ verifyTransResponse.AsErr());
    //    }
    //}

    private async void BuyNftOfferHandler_IC()
    {

    }

    //private async void BuyItemOfferHandler_RC()
    //{
    //    BroadcastState.TryRead<DataState<IcrcData>>(out var icrcDataState);

    //    if (icrcDataState.IsReady() == false)
    //    {
    //        Debug.LogError("Icrc data must be ready");
    //        return;
    //    }
    //    BroadcastState.TryRead<DataState<OffersConfig>>(out var offersConfig);

    //    if (offersConfig.data.TryGetConfig(itemOfferId_rc, out var offerConfig) == false)
    //    {
    //        Debug.LogError($"Offer of id {itemOfferId_rc} doesn't exist");
    //        return;
    //    }

    //    buying = true;
    //    BroadcastState.TryRead<DataState<UserNodeData>>(out var coreUserDataState);
    //    //Lets update the window so button disable based on "buying" state
    //    UpdateWindow(coreUserDataState);

    //    Debug.Log("Calling Transfer");
    //    var transferResult = await TxUtil.TransferToOfferCanister_RC(offerConfig.price, icrcDataState.data.decimalCount);

    //    if (transferResult.State == UResultState.Err)
    //    {
    //        buying = false;

    //        Debug.Log($"Transfer failed, msg: {transferResult.AsErr()}");
    //        return;
    //    }

    //    var verifyTransResponse =
    //        await TxUtil.VerifyTx_RC(transferResult.AsOk(), offerConfig.price, icrcDataState.data.decimalCount, "offer", $"{itemOfferId_rc}");
    //    buying = false;

    //    if (verifyTransResponse.State == UResultState.Ok)
    //    {
    //        Debug.Log("Payment success");

    //        var okValue = verifyTransResponse.AsOk();
    //        var gameTx = okValue.gameTx;
    //        var nfts = okValue.nfts;

    //        var addedItems = gameTx.ItemsItem_.Add;

    //        BroadcastState.ForceInvoke<DataState<UserNodeData>>(coreUserDataState =>
    //        {
    //            if (gameTx != null)
    //            {
    //                addedItems.Iterate(addedItem =>
    //                {
    //                    Debug.Log($"Add item off id {addedItem.Id}, quantity: {addedItem.Quantity}");

    //                    if (coreUserDataState.data.items.TryAdd(addedItem.Id, new(addedItem.Id, addedItem.Quantity)) == false)
    //                    {
    //                        Debug.Log($"Add item off id {addedItem.Id}, quantity: {addedItem}");
    //                        coreUserDataState.data.items[addedItem.Id].quantity += addedItem.Quantity;
    //                    }
    //                });
    //            }

    //            return coreUserDataState;
    //        });
    //    }
    //    else
    //    {
    //        Debug.Log("Payment failure, msg: " + verifyTransResponse.AsErr());
    //    }
    //}
}