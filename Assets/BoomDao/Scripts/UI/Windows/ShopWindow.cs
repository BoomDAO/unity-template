using Candid.World.Models;
using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using Boom;
using System.Linq;
using EdjCase.ICP.Candid.Models;
using static Env;
using Candid;

public class ShopWindow : Window
{
    //[SerializeField] GameObject loadingStateGo;

    [SerializeField] string listOfValidActionsConfig = "shop_window_actions";

    [SerializeField] Transform content;
    [SerializeField] Button closeButton;
    [SerializeField, ShowOnly] bool buying;
    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        UserUtil.AddListenerDataChangeSelf<DataTypes.Entity>(UpdateWindow);
        UserUtil.AddListenerDataChangeSelf<DataTypes.ActionState>(UpdateWindow);
        UserUtil.AddListenerMainDataChange<MainDataTypes.AllConfigs>(UpdateWindow, true);
    }

    private void OnDestroy()
    {
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.Entity>(UpdateWindow);
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.ActionState>(UpdateWindow);
        UserUtil.RemoveListenerMainDataChange<MainDataTypes.AllConfigs>(UpdateWindow);
    }
    private void UpdateWindow(Data<DataTypes.Entity> state)
    {
        var principalResult =  UserUtil.GetPrincipal();

        if (principalResult.IsErr)
        {
            Debug.LogError(principalResult.AsErr());

            return;
        }

        if (UserUtil.IsDataValid<DataTypes.Entity>(principalResult.AsOk().Value))
        {
            var entityConfigData = UserUtil.GetMainData<MainDataTypes.AllConfigs>();
            UpdateWindow(entityConfigData.AsOk());
        }
    }
    private void UpdateWindow(Data<DataTypes.ActionState> state)
    {
        var principalResult = UserUtil.GetPrincipal();

        if (principalResult.IsErr)
        {
            Debug.LogError(principalResult.AsErr());

            return;
        }

        if (UserUtil.IsDataValid<DataTypes.Entity>(principalResult))
        {
            var entityConfigData = UserUtil.GetMainData<MainDataTypes.AllConfigs>();
            UpdateWindow(entityConfigData.AsOk());
        }
    }

    private void UpdateWindow(MainDataTypes.AllConfigs state)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        if (!ConfigUtil.TryGetConfig(CandidApiManager.Instance.WORLD_CANISTER_ID, listOfValidActionsConfig, out var config))
        {
            Debug.LogError("Could not find config of id: " + listOfValidActionsConfig);
            return;
        }

        var configFields = config.fields;

        configFields.Iterate(e =>
        {
            string actionId = e.Key;
            string actionType = e.Value;

            if (!ConfigUtil.TryGetConfig(CandidApiManager.Instance.WORLD_CANISTER_ID, actionId, out var actionConfig))
            {
                Debug.LogError("Could not find config of id: " + listOfValidActionsConfig);
                return;
            }

            actionConfig.fields.TryGetValue("name", out string name);
            actionConfig.fields.TryGetValue("description", out string description);
            actionConfig.fields.TryGetValue("imageUrl", out string imageUrl);

            if (!ConfigUtil.TryGetAction(CandidApiManager.Instance.WORLD_CANISTER_ID, actionId, out var action))
            {
                Debug.LogError("Could not find action of id: " + actionId);

                return;
            }

            var callerSubAction = action.callerAction;


            if (actionType == "trade")
            {
                if(!ConfigUtil.TryGetActionPart<List<EntityConstrainTypes.Base>>(actionId, e => e.callerAction.EntityConstraints, out var entityConstraints))
                {
                    Debug.LogError("Could not find entities constraints of action of id: " + actionId);

                    return;
                }

                var constrain = entityConstraints.Reduce(e =>
                {
                    if (e.ConstrainType != $"{nameof(EntityConstrainTypes.GreaterThanEqualToNumber)}")
                    {
                        return $"ConstrainType Error! " + e.ConstrainType;
                    }
                    double amount = (double)e.GetValue();
                    string name = e.GetKey();

                    if (ConfigUtil.GetConfigFieldAs<string>(CandidApiManager.Instance.WORLD_CANISTER_ID, e.Eid, "name", out var configName)) name = configName;

                    return $"{name} x {amount}\n\n";
                }, "\n");

                ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionId,
                    textButtonContent = $"Trade Item",
                    content = $"{name}\n\nItemA x 1 -> ItemC x 1",
                    action = (actionId, customData) => { Trade(actionId, "constraints").Forget(); },
                    imageContentType = new ImageContentType.Url(imageUrl),
                    infoWindowData = new InfoPopupWindow.WindowData(name, description)
                }, content);
            }
            else if (actionType == "verifyICP")
            {
                if (!ConfigUtil.TryGetActionPart<IcpTx>(actionId, e => e.callerAction.IcpConstraint, out var txConstraint))
                {
                    Debug.LogError("Could not find icp constraint of action of id: " + actionId);

                    return;
                }

                ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionId,
                    textButtonContent = $"{(actionId == "spend_icp_to_mint_test_nft" ? "Mint Nft" : "Buy Item")}",
                    content = $"{name}\n\nprice: {txConstraint.Amount} ICP",
                    action = (actionId, customData) => { BuyWithIcp(actionId, txConstraint).Forget(); },
                    imageContentType = new ImageContentType.Url(imageUrl),
                    infoWindowData = new InfoPopupWindow.WindowData(name, description)
                }, content);
            }
            else if (actionType == "verifyICRC")
            {
                if (!ConfigUtil.TryGetActionPart<List<IcrcTx>>(actionId, e => e.callerAction.IcrcConstraint, out var txConstraints))
                {
                    Debug.LogError("Could not find icrc constraint of action of id: " + actionId);

                    return;
                }

                if(txConstraints.Count == 0)
                {
                    Debug.LogError("empty icrc constraint of action of id: " + actionId);

                    return;
                }

                var txConstraint = txConstraints[0];

                ConfigUtil.TryGetTokenConfig(txConstraint.Canister, out var tokenConfig);

                var tokenName = tokenConfig != null ? tokenConfig.name : "ICRC";

                ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionId,
                    textButtonContent = $"Buy Item",
                    content = $"{name}\n\nprice: {txConstraint.Amount} {tokenName}", //{config.Amt.ToString("0." + new string('#', 339))}
                    action = (actionId, customData) => { BuyWithIcrc(actionId, txConstraint).Forget(); }
                    ,
                    imageContentType = new ImageContentType.Url(imageUrl),
                    infoWindowData = new InfoPopupWindow.WindowData(name, description)
                }, content);
            }
            else if (actionType == "verifyNftBurn")
            {
                if (!ConfigUtil.TryGetActionPart<ActionResult>(actionId, e => e.callerAction.ActionResult, out var actionResult))
                {
                    Debug.LogError("Could not find action result of action of id: " + actionId);

                    return;
                }
                if (!ConfigUtil.TryGetActionPart<List<NftTx>>(actionId, e => e.callerAction.NftConstraint, out var txConstraints))
                {
                    Debug.LogError("Could not find nft constraint of action of id: " + actionId);

                    return;
                }

                if (txConstraints.Count == 0)
                {
                    Debug.LogError("empty nft constraint of action of id: " + actionId);

                    return;
                }

                var txConstraint = txConstraints[0];
                var possibleOutcomes = actionResult.Outcomes[0].PossibleOutcomes;

                string possibleOutcoemsContent = possibleOutcomes.Filter(e =>
                {
                    if(e.Option.Tag != ActionOutcomeOption.OptionInfoTag.UpdateEntity)
                    {
                        return false;
                    }

                    var asUpdateEntity = e.Option.AsUpdateEntity();

                    return asUpdateEntity.Updates.Has(e=> e.Tag == UpdateEntityTypeTag.IncrementNumber);
                }).Reduce(k =>
                {
                    var asUpdateEntity = k.Option.AsUpdateEntity();
                    var asIncrementNumber = asUpdateEntity.Updates.First(e => e.Tag == UpdateEntityTypeTag.IncrementNumber);

                    ConfigUtil.GetConfigFieldAs<string>(CandidApiManager.Instance.WORLD_CANISTER_ID, asUpdateEntity.Eid, "name", out var configName, asUpdateEntity.Eid);

                    return $"{configName} x {(asIncrementNumber.AsIncrementNumber().FieldValue.Tag == IncrementNumber.FieldValueInfoTag.Number? asIncrementNumber.AsIncrementNumber().FieldValue.Value : "some formula")}";
                });


                ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionId,
                    textButtonContent = $"Burn Nft",
                    content = $"Nft Burn Posible Rewards:\n\n{possibleOutcoemsContent}",
                    action = (actionId, customData) => { BurnNftHandler(actionId, txConstraint); }
                  ,
                    imageContentType = new ImageContentType.Url(imageUrl),
                    infoWindowData = new InfoPopupWindow.WindowData(name, description)
                }, content);
            }
            else if (actionType == "verifyNftHold")
            {
                if (!ConfigUtil.TryGetActionPart<ActionResult>(actionId, e => e.callerAction.ActionResult, out var actionResult))
                {
                    Debug.LogError("Could not find action result of action of id: " + actionId);

                    return;
                }
                if (!ConfigUtil.TryGetActionPart<List<NftTx>>(actionId, e => e.callerAction.NftConstraint, out var txConstraints))
                {
                    Debug.LogError("Could not find nft constraint of action of id: " + actionId);

                    return;
                }

                if (txConstraints.Count == 0)
                {
                    Debug.LogError("empty nft constraint of action of id: " + actionId);

                    return;
                }

                var txConstraint = txConstraints[0];
                var possibleOutcomes = actionResult.Outcomes[0].PossibleOutcomes;

                string possibleOutcoemsContent = possibleOutcomes.Filter(e =>
                {
                    if (e.Option.Tag != ActionOutcomeOption.OptionInfoTag.UpdateEntity)
                    {
                        return false;
                    }

                    var asUpdateEntity = e.Option.AsUpdateEntity();

                    return asUpdateEntity.Updates.Has(e => e.Tag == UpdateEntityTypeTag.IncrementNumber);
                }).Reduce(k =>
                {
                    var asUpdateEntity = k.Option.AsUpdateEntity();
                    var asIncrementNumber = asUpdateEntity.Updates.First(e => e.Tag == UpdateEntityTypeTag.IncrementNumber);

                    ConfigUtil.GetConfigFieldAs<string>(CandidApiManager.Instance.WORLD_CANISTER_ID, asUpdateEntity.Eid, "name", out var configName, asUpdateEntity.Eid);

                    return $"{configName} x {(asIncrementNumber.AsIncrementNumber().FieldValue.Tag == IncrementNumber.FieldValueInfoTag.Number ? asIncrementNumber.AsIncrementNumber().FieldValue.Value : "some formula")}";
                });


                ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = actionId,
                    textButtonContent = $"Check Nft Holdings",
                    content = $"Nft Hold Posible Rewards:\n\n{possibleOutcoemsContent}",
                    action = (actionId, customData) => { CheckHoldNftHandler(actionId, txConstraint); }
                  ,
                    imageContentType = new ImageContentType.Url(imageUrl),
                    infoWindowData = new InfoPopupWindow.WindowData(name, description)
                }, content);
            }
            else
            {
                Debug.LogError("action type not handled: " + actionType);

            }
        });
    }

    private void BurnNftHandler(string actionid, NftTx tx)
    {
        BurnNft(actionid, tx).Forget();
    }
    private void CheckHoldNftHandler(string actionid, NftTx tx)
    {
        CheckHoldNft(actionid, tx).Forget();
    }

    private async UniTaskVoid BurnNft(string actionid, NftTx tx)
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));

        var principalResult = UserUtil.GetPrincipal();

        if (principalResult.Tag == UResultTag.Err)
        {
            Debug.Log(principalResult.AsErr());
            return;
        }
        var principal = principalResult.AsOk().Value;

        if (tx.NftConstraintType.Tag == NftTx.NftConstraintTypeInfoTag.Hold)
        {
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData(">Some other issue!", "You cannot burn a nft for a hold constraint"), 3);
            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }
        var transferConstraint = tx.NftConstraintType.AsTransfer();

        var nextNftIndexResult = NftUtil.TryGetNextNft(principal, tx.Canister);

        if (nextNftIndexResult.IsErr)
        {
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData(">>Some other issue!", nextNftIndexResult.AsErr()), 3);
            BroadcastState.Invoke(new WaitingForResponse(false));

            return;
        }

        var nextNft = nextNftIndexResult.AsOk();

        var transferResult = await ActionUtil.Transfer.TransferNft(tx.Canister, nextNft.tokenIdentifier, transferConstraint.ToPrincipal);

        if (transferResult.IsErr)
        {
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData(">>>Some other issue!", transferResult.AsErr().content), 3);
            BroadcastState.Invoke(new WaitingForResponse(false));

            return;
        }

        var result = await ActionUtil.ProcessAction(actionid);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr().content);

            switch (result.AsErr())
            {
                case ActionErrType.InsufficientBalance content:
                    Window infoPopup = null;
                    infoPopup = WindowManager.Instance.OpenWindow<InfoPopupWindow>(
                    new InfoPopupWindow.WindowData(
                        $"You don't a nft to burn",
                        $"{content.content}",
                        new(
                            new(
                                $"Mint a Nft",
                                () =>
                                {
                                    if (infoPopup != null) infoPopup.Close();
                                    Close();
                                    WindowManager.Instance.OpenWindow<MintTestTokensWindow>(null, 1);
                                }
                               )
                            )),
                        3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = result.AsOk();

        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid CheckHoldNft(string actionid, NftTx tx)
    {
        //await UniTask.SwitchToMainThread();

        var principalResult = UserUtil.GetPrincipal();

        if (principalResult.Tag == UResultTag.Err)
        {
            Debug.Log(principalResult.AsErr());
            return;
        }
        var principal = principalResult.AsOk().Value;

        BroadcastState.Invoke(new WaitingForResponse(true));


        var nextNftIndexResult = NftUtil.TryGetNextNftIndex(principal, tx.Canister);

        if (nextNftIndexResult.IsErr)
        {
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", nextNftIndexResult.AsErr()), 3);
            BroadcastState.Invoke(new WaitingForResponse(false));

            return;
        }

        var result = await ActionUtil.ProcessAction(actionid);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr().content);

            switch (result.AsErr())
            {
                case ActionErrType.InsufficientBalance content:
                    Window infoPopup = null;
                    infoPopup = WindowManager.Instance.OpenWindow<InfoPopupWindow>(
                    new InfoPopupWindow.WindowData(
                        $"You don't a nft to burn",
                        $"{content.content}",
                        new(
                            new(
                                $"Mint a Nft",
                                () =>
                                {
                                    if (infoPopup != null) infoPopup.Close();
                                    Close();
                                    WindowManager.Instance.OpenWindow<MintTestTokensWindow>(null, 1);
                                }
                               )
                            )),
                        3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = result.AsOk();

        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));
    }


    private async UniTaskVoid Trade(string actionId, string constrain)
    {
        BroadcastState.Invoke(new WaitingForResponse(true));

        var actionResult = await ActionUtil.ProcessAction(actionId);

        if (actionResult.Tag == UResultTag.Err)
        {
            var actionError = actionResult.AsErr();

            switch (actionError)
            {
                case ActionErrType.EntityConstrain content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Oops you dont meet requirements", $"Requirements:\n{constrain}"), 3);
                    break;
                case ActionErrType.ActionsPerInterval content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Time constrain!", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Other issue!", actionResult.AsErr().content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();

        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid BuyWithIcp(string actionId, IcpTx icpConstraint)
    {
        BroadcastState.Invoke(new WaitingForResponse(true));

        var blockIndexResult = await ActionUtil.Transfer.TransferIcp(icpConstraint);

        if (blockIndexResult.IsErr)
        {
            var error = blockIndexResult.AsErr();
            switch (error)
            {
                case TransferErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You don't have enough ICP", $"Requirements:\n{$"{icpConstraint.Amount} ICP\n\nYou need to deposit some ICP"}"), 3);
                    break;
                case TransferErrType.Transfer content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Transfer error!", content.Content), 3);
                    break;
                case TransferErrType.LogIn content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You must login!", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Other issue!", "Unknown..."), 3);
                    break;
            }
        }

        var actionResult = await ActionUtil.ProcessAction(actionId);

        //CHECK FOR ERR
        if (actionResult.Tag == UResultTag.Err)
        {
            var actionError = actionResult.AsErr();

            switch (actionError)
            {
                case ActionErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You don't have enough ICP", $"Requirements:\n{$"{icpConstraint.Amount} ICP\n\nYou need to deposit some ICP"}"), 3);
                    break;
                case ActionErrType.ActionsPerInterval content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Time constrain!", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Other issue!", actionResult.AsErr().content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();

        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid BuyWithIcrc(string actionId, IcrcTx icrcConstraint)
    {
        var principalResult = UserUtil.GetPrincipal();

        if (principalResult.Tag == UResultTag.Err)
        {
            Debug.Log(principalResult.AsErr());
            return;
        }
        var principal = principalResult.AsOk().Value;

        BroadcastState.Invoke(new WaitingForResponse(true));

        var tokenSymbol = "ICRC";
        var userBalance = 0D;


        var tokenAndConfigsResult = TokenUtil.GetTokenDetails(principal, icrcConstraint.Canister);

        if (tokenAndConfigsResult.Tag == UResultTag.Ok)
        {
            var (token, tokenConfigs) = tokenAndConfigsResult.AsOk();

            tokenSymbol = tokenConfigs.symbol;
            userBalance = token.baseUnitAmount.ConvertToDecimal(tokenConfigs.decimals);
        }
        $"Required ICRC: {icrcConstraint.Amount} Balance: {userBalance}".Log();


        var blockIndexResult = await ActionUtil.Transfer.TransferIcrc(icrcConstraint);

        if (blockIndexResult.IsErr)
        {
            var error = blockIndexResult.AsErr();
            switch (error)
            {
                case TransferErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You don't have enough ICP", $"Requirements:\n{$"{icrcConstraint.Amount} ICP\n\nYou need to deposit some ICP"}"), 3);
                    break;
                case TransferErrType.Transfer content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Transfer error!", content.Content), 3);
                    break;
                case TransferErrType.LogIn content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You must login!", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Other issue!", "Unknown..."), 3);
                    break;
            }
        }

        var actionResult = await ActionUtil.ProcessAction(actionId);

        //CHECK FOR ERR
        if (actionResult.Tag == UResultTag.Err)
        {
            var actionError = actionResult.AsErr();

            switch (actionError)
            {
                case ActionErrType.InsufficientBalance content:

                    Window infoPopup = null;
                    infoPopup = WindowManager.Instance.OpenWindow<InfoPopupWindow>(
                        new InfoPopupWindow.WindowData(
                    $"You don't have enough {tokenSymbol}",
                            $"Requirements:\n{$"{tokenSymbol} x {icrcConstraint.Amount}"}\n\nYou need to mint more \"{tokenSymbol}\"",
                            new(new($"Mint ${tokenSymbol}", () =>
                            {
                                if (infoPopup != null) infoPopup.Close();
                                Close();
                                WindowManager.Instance.OpenWindow<MintTestTokensWindow>(null, 1);
                            }
                    ))), 3);
                    break;
                case ActionErrType.ActionsPerInterval content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Time constrain!", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Other issue! " + actionResult.AsErr().GetType().Name, actionResult.AsErr().content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();

        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));
    }

    private void DisplayActionResponse(ProcessedActionResponse resonse)
    {
        List<string> inventoryElements = new();

        //NFTs
        Dictionary<string, int> collectionsToDisplay = new();

        if (resonse.callerOutcomes == null) return;


        resonse.callerOutcomes.nfts.Iterate(e =>
        {

            if (collectionsToDisplay.TryAdd(e.Canister, 1) == false) collectionsToDisplay[e.Canister] += 1;

        });

        collectionsToDisplay.Iterate(e =>
        {
            if (ConfigUtil.TryGetNftCollectionConfig(e.Key, out var collectionConfig) == false)
            {
                return;
            }

            inventoryElements.Add($"{(collectionConfig != null ? collectionConfig.name : "Name not Found")} x {e.Value}");
        });

        //Tokens
        resonse.callerOutcomes.tokens.Iterate(e =>
        {
            if (ConfigUtil.TryGetTokenConfig(e.Canister, out var tokenConfig) == false)
            {
                return;
            }

            inventoryElements.Add($"{(tokenConfig != null? tokenConfig.name : "ICRC")} x {e.Quantity}");
        });


        //ENTITIES
        resonse.callerOutcomes.entityEdits.Iterate(e =>
        {
            if (e.Value.fields.Has(k => k.Value is EntityFieldEdit.IncrementNumber) == false) return;

            if (!ConfigUtil.GetConfigFieldAs<string>(CandidApiManager.Instance.WORLD_CANISTER_ID, e.Value.eid, "name", out var configName)) return;
            if (!e.Value.GetEditedFieldAsNumeber("quantity", out double quantity)) return;

            if (e.Value.TryGetConfig(CandidApiManager.Instance.WORLD_CANISTER_ID, out var config)) inventoryElements.Add($"{configName} x {quantity}");
            else inventoryElements.Add($"{e.Value.GetKey()} x {quantity}");
        });

        WindowManager.Instance.OpenWindow<InventoryPopupWindow>(new InventoryPopupWindow.WindowData("Earned Items", inventoryElements), 3);
    }
}