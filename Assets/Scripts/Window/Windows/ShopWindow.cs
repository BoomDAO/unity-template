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

        if (loggedIn == false) return;

        var validActionOffers = GetValidOffers(actionOfferIds);

        validActionOffers.Iterate(actionOffer =>
        {
            if (!actionOffer.value.Tag.HasValue) return;
            if (!actionOffer.value.Tag.ValueOrDefault.Contains("Offer")) return;

            if (!actionOffer.value.ActionPlugin.HasValue)
            {
                ActionWidget aw = WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionOffer.key,
                    textButtonContent = $"{actionOffer.key}",
                    content = $"{actionOffer.value.Name.ValueOrDefault}",
                    action = async (m, customData) =>
                    {
                        Debug.Log("Call to action of id: SpendEntities -" + actionOffer.key);
                        var actionResult = await TxUtil.ProcessActionEntities(new ActionArgValueTypes.DefaultArg(m));

                        if (actionResult.Tag != UResultTag.Ok)
                        {
                            Debug.LogError("ActionResult Failure, msg: " + actionResult.AsErr());
                            return;
                        }

                        var resultAsOk = actionResult.AsOk();
                        Debug.Log($"Spend Entities Success, entityCount: {resultAsOk.F1.Count}");
                        resultAsOk.F1.Debug(e => $"entity: {JsonConvert.SerializeObject(e)}");
                    }
                }, content);
            }
            else
            {
                var actionPlugin = actionOffer.value.ActionPlugin.ValueOrDefault;
                if (actionPlugin.Tag == ActionPluginTag.SpendTokens)
                {
                    var config = actionPlugin.AsSpendTokens();
                    config.BaseZeroCount.TryToUInt64(out var baseZeroCount);

                    ActionWidget aw = WindowGod.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = actionOffer.key,
                        textButtonContent = $"{actionOffer.key}",
                        content = $"{actionOffer.value.Name.ValueOrDefault}\n\nprice: {config.Amt.ToString("0." + new string('#', 339))}",
                        action = async (m, customData) =>
                        {

                            double amt = config.Amt;
                            string tokenCanister = config.TokenCanister.ValueOrDefault;
                            var tokenizedAmt = CandidUtil.Tokenize(amt, baseZeroCount);

                            UResult<ulong, string> transferResult = default;
                            if (config.TokenCanister.HasValue == false)
                            {//IC

                                double amount = 0;
                                var balanceResult = UserUtil.GetToken(Env.CanisterIds.ICP_LEDGER);
                                if (balanceResult.Tag == UResultTag.Ok) amount = balanceResult.AsOk().Amount;

                                Debug.Log($"Call to action of id: Spend ICP Tokens - {actionOffer.key}. Tokens to spend: {amt}, you have {amount}");

                                transferResult = await TxUtil.Transfer_ICP(tokenizedAmt, CandidApiManager.PaymentCanisterOfferIdentifier);
                            }
                            else
                            {//ICRC
                                double amount = 0;
                                var balanceResult = UserUtil.GetToken(Env.CanisterIds.ICRC_LEDGER);
                                if (balanceResult.Tag == UResultTag.Ok) amount = balanceResult.AsOk().Amount;

                                Debug.Log($"Call to action of id: Spend ICRC Tokens - {actionOffer.key}. Tokens to spend: {amt}, you have {amount}");
                                transferResult = await TxUtil.Transfer_RC(tokenizedAmt, Env.CanisterIds.PAYMENT_HUB);
                            }

                            if (transferResult.Tag != UResultTag.Ok)
                            {
                                Debug.LogError("Transfer Failure, msg: " + transferResult.AsErr());
                                return;
                            }

                            var transferHash = transferResult.AsOk();

                            Debug.Log($"SpendToken aid: {m},Transfer Hash: {transferHash}");

                            var actionResult = await TxUtil.ProcessActionEntities(new ActionArgValueTypes.SpendTokensArg(m, transferHash));

                            if (actionResult.Tag != UResultTag.Ok)
                            {
                                Debug.LogError("ActionResult Failure, msg: " + actionResult.AsErr());
                                return;
                            }

                            var resultAsOk = actionResult.AsOk();
                            Debug.Log($"Spend Token Success, entityCount: {resultAsOk.F1.Count}");
                            resultAsOk.F1.Debug(e => $"entity: {JsonConvert.SerializeObject(e)}");
                        }
                    }, content);
                }

                else Debug.Log($"Trying to call wrong action type of id: {actionPlugin.Tag} -" + actionOffer.key);
            }
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
                return;
            }

            if (!config.Tag.HasValue)
            {
                return;
            }
            if(!config.Tag.ValueOrDefault.Contains("Offer"))
            {
                return;
            }

            var actionPlugin = config.ActionPlugin.ValueOrDefault;

            if (actionPlugin != null)
            {
                Debug.Log($"Action Type TAG: {actionPlugin.Tag}");
                if (actionPlugin.Tag != ActionPluginTag.SpendTokens) return;
            }

            offers.Add(new(e, config));
        });

        return offers;
    }
}