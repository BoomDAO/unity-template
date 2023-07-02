using Candid;
using Candid.IcpLedger.Models;
using Candid.World.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : Window
{
    [SerializeField] List<string> actionOfferIds;

    [SerializeField] Transform content;
    [SerializeField] Button closeButton;
    [SerializeField, ShowOnly] bool buying;
    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        UserUtil.RegisterToDataChange<DataTypes.Item>(UpdateWindow, true);
        UserUtil.RegisterToDataChange<DataTypes.EntityConfig>(UpdateWindow);

    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.Item>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.EntityConfig>(UpdateWindow);

    }

    private void UpdateWindow(DataState<Data<DataTypes.EntityConfig>> state)
    {
        var itemsResult = UserUtil.GetDataElementsOfType<DataTypes.Item>();

        if(itemsResult.Tag == UResultTag.Err)
        {
            Debug.LogError("Something went wrong updating shop window, msg: " + itemsResult.AsErr());
            return;
        }

        UserUtil.UpdateData(itemsResult.AsOk().ToArray());
    }

    private void UpdateWindow(DataState<Data<DataTypes.Item>> state)
    {
        var getIsLoginResult = UserUtil.GetSignInType();

        if (getIsLoginResult.Tag == UResultTag.Err)
        {
            Debug.LogError(getIsLoginResult.AsErr());
            return;
        }

        bool isLoggedIn = getIsLoginResult.AsOk() == UserUtil.SigningType.user;

        if (isLoggedIn == false) return;

        var validActionOffers = GetValidOffers(actionOfferIds);

        validActionOffers.Iterate(actionOffer =>
        {
            if (!actionOffer.value.Tag.HasValue) return;
            if (!actionOffer.value.Tag.ValueOrDefault.Contains("Offer")) return;

            if (!actionOffer.value.ActionPlugin.HasValue)
            {
                var constrain = "";
                if (actionOffer.value.ActionConstraint.HasValue)
                {
                    if (actionOffer.value.ActionConstraint.ValueOrDefault.EntityConstraint.HasValue)
                    {
                        constrain = actionOffer.value.ActionConstraint.ValueOrDefault.EntityConstraint.ValueOrDefault.Reduce(e =>
                        {
                            double amount = e.GreaterThanOrEqualQuantity.HasValue ? e.GreaterThanOrEqualQuantity.ValueOrDefault : 0;

                            return $"{e.EntityId} x {amount}";
                        });
                    }
                }

                ActionWidget aw = WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionOffer.key,
                    textButtonContent = $"{actionOffer.key}",
                    content = $"{actionOffer.value.Name.ValueOrDefault}\nItems to Spend:\n{constrain}",
                    action = async (actionId, customData) =>
                    {
                        BroadcastState.Invoke(new DisableButtonInteraction(true));
                        var actionResult = await TxUtil.Action.Default(actionId);

                        if (actionResult.Tag == UResultTag.Err)
                        {
                            Debug.LogError(actionResult.AsErr());
                            return;
                        }

                        var resultAsOk = actionResult.AsOk();

                        Debug.Log($"Spend Entities Success, entityCount: {resultAsOk.F1.Count}");
                        resultAsOk.F1.Debug(e => $"entity: {JsonConvert.SerializeObject(e)}");

                        UserUtil.RequestData<DataTypes.Item>();

                        BroadcastState.Invoke(new DisableButtonInteraction(false));
                    }
                }, content);
            }
            else
            {
                var actionPlugin = actionOffer.value.ActionPlugin.ValueOrDefault;
                if (actionPlugin.Tag == ActionPluginTag.VerifyTransferIcp)
                {
                    var config = actionPlugin.AsVerifyTransferIcp();

                    ActionWidget aw = WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = actionOffer.key,
                        textButtonContent = $"{actionOffer.key}",
                        content = $"{actionOffer.value.Name.ValueOrDefault}\n\nprice: {config.Amt.NotScientificNotation()}",
                        action = async (actionId, customData) =>
                        {
                            BroadcastState.Invoke(new DisableButtonInteraction(true));

                            var actionResult = await TxUtil.Action.TransferAndVerifyIcp(actionId);

                            if (actionResult.Tag == UResultTag.Err)
                            {
                                Debug.LogError(actionResult.AsErr());
                                return;
                            }

                            var resultAsOk = actionResult.AsOk();

                            Debug.Log($"Spend Token Success, entityCount: {resultAsOk.F1.Count}");
                            resultAsOk.F1.Debug(e => $"entity: {JsonConvert.SerializeObject(e)}");

                            UserUtil.RequestData<DataTypes.Item>();

                            BroadcastState.Invoke(new DisableButtonInteraction(false));
                        }
                    }, content);
                }
                else if (actionPlugin.Tag == ActionPluginTag.VerifyTransferIcrc)
                {
                    var config = actionPlugin.AsVerifyTransferIcrc();
                    config.BaseUnitCount.TryToUInt64(out var baseZeroCount);

                    ActionWidget aw = WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = actionOffer.key,
                        textButtonContent = $"{actionOffer.key}",
                        content = $"{actionOffer.value.Name.ValueOrDefault}\n\nprice: {config.Amt.ToString("0." + new string('#', 339))}",
                        action = async (actionId, customData) =>
                        {
                            BroadcastState.Invoke(new DisableButtonInteraction(true));

                            var actionResult = await TxUtil.Action.TransferAndVerifyIcrc(actionId, Env.CanisterIds.ICRC_LEDGER);

                            if (actionResult.Tag == UResultTag.Err)
                            {
                                Debug.LogError(actionResult.AsErr());
                                return;
                            }

                            var resultAsOk = actionResult.AsOk();

                            Debug.Log($"Spend Token Success, entityCount: {resultAsOk.F1.Count}");
                            resultAsOk.F1.Debug(e => $"entity: {JsonConvert.SerializeObject(e)}");

                            UserUtil.RequestData<DataTypes.Item>();
                            BroadcastState.Invoke(new DisableButtonInteraction(false));
                        }
                    }, content);
                }


                else Debug.Log($"Trying to call wrong action type of id: {actionPlugin.Tag} -" + actionOffer.key);
            }
        });
    }

    private List<KeyValue<string, DataTypes.ActionConfig>> GetValidOffers(List<string> actionOfferIds)
    {

        List<KeyValue<string, DataTypes.ActionConfig>> offers = new();

        actionOfferIds.Iterate(e =>
        {
            var getDataResult = UserUtil.GetDataElementOfType<DataTypes.ActionConfig>(e);
            if (getDataResult.Tag == UResultTag.Err)
            {
                return;
            }

            if (getDataResult.Tag == UResultTag.None)
            {
                return;
            }

            var config = getDataResult.AsOk();

            if (!config.Tag.ValueOrDefault.Contains("Offer"))
            {
                return;
            }

            var actionPlugin = config.ActionPlugin.ValueOrDefault;

            if (actionPlugin != null)
            {
                if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcp && actionPlugin.Tag != ActionPluginTag.VerifyTransferIcrc) return;
            }

            offers.Add(new(e, config));
        });

        return offers;
    }
}