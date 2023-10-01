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

public class ShopWindow : Window
{
    //[SerializeField] GameObject loadingStateGo;

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
        UserUtil.RegisterToDataChange<DataTypes.Entity>(UpdateWindow);
        UserUtil.RegisterToDataChange<DataTypes.ActionState>(UpdateWindow);
        UserUtil.RegisterToDataChange<DataTypes.Config>(UpdateWindow, true);
    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.Entity>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.ActionState>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.Config>(UpdateWindow);
    }
    private void UpdateWindow(DataState<Data<DataTypes.Entity>> state)
    {
        if (UserUtil.IsDataValid<DataTypes.Entity>())
        {
            var entityConfigData = UserUtil.GetDataOfType<DataTypes.Config>();
            UpdateWindow(entityConfigData.AsOk());
        }
    }
    private void UpdateWindow(DataState<Data<DataTypes.ActionState>> state)
    {
        if (UserUtil.IsDataValid<DataTypes.Entity>())
        {
            var entityConfigData = UserUtil.GetDataOfType<DataTypes.Config>();
            UpdateWindow(entityConfigData.AsOk());
        }
    }

    private void UpdateWindow(DataState<Data<DataTypes.Config>> state)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        var getIsLoginResult = UserUtil.GetLogInType();

        if (getIsLoginResult.Tag == UResultTag.Err)
        {
            Debug.LogError(getIsLoginResult.AsErr());
            return;
        }

        bool isLoggedIn = getIsLoginResult.AsOk() == UserUtil.LoginType.User;

        if (isLoggedIn == false) return;

        var validActionOffers = GetValidOffers(actionOfferIds);

        validActionOffers.Iterate(actionOfferKeyValue =>
        {
            var key = actionOfferKeyValue.key;
            var actionOffer = actionOfferKeyValue.value;

            if (actionOffer.actionPlugin == null)
            {
                var constrain = actionOffer.entityConstraints?.Reduce(e =>
                {
                    if(e.ConstrainType != $"{nameof(EntityConstrainTypes.GreaterThanEqualToNumber)}")
                    {
                        return $"ConstrainType Error! "+ e.ConstrainType;
                    }
                    double amount = (double) e.GetValue();
                    string name = e.GetKey();

                    if (EntityUtil.GetConfigFieldAs<string>(e.GetConfigId(), "name", out var configName)) name = configName; 

                    return $"{name} x {amount}\n\n";
                },"\n");

                ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = key,
                    textButtonContent = $"Trade Item",
                    content = $"{actionOffer.name}\n\n{constrain} -> ItemC x 1",
                    action = (actionId, customData) => { Trade(actionId, constrain).Forget(); },
                    imageContentType = new ImageContentType.Url(actionOffer.imageUrl),
                    infoWindowData = new InfoPopupWindow.WindowData(actionOffer.name, actionOffer.description)
                }, content);
            }
            else
            {
                var actionPluginResult = actionOffer.actionPlugin;

                if (actionOffer.HasPluginType(ActionPluginTag.VerifyTransferIcp))
                {
                    var config = actionPluginResult.AsVerifyTransferIcp();

                    ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = key,
                        textButtonContent = $"{(actionOffer.tag == "Mint" ? "Mint Nft" : "Buy Item")}",
                        content = $"{actionOffer.name}\n\nprice: {config.Amt.NotScientificNotation()} ICP",
                        action = (actionId, customData) => { BuyWithIcp(actionId, config, actionOffer).Forget(); },
                        imageContentType = new ImageContentType.Url(actionOffer.imageUrl),
                        infoWindowData = new InfoPopupWindow.WindowData(actionOffer.name, actionOffer.description)
                    }, content);
                }
                else if (actionOffer.HasPluginType(ActionPluginTag.VerifyTransferIcrc))
                {
                    var config = actionPluginResult.AsVerifyTransferIcrc();

                    var tokenNameResult = UserUtil.GetPropertyFromType<DataTypes.TokenMetadata, string>(config.Canister, e => e.name);
                    var tokenName = tokenNameResult.Tag == UResultTag.Ok ? tokenNameResult.AsOk() : "ICRC";

                    ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = key,
                        textButtonContent = $"{(actionOffer.tag == "Mint" ? "Mint Nft" : "Buy Item")}",
                        content = $"{actionOffer.name}\n\nprice: {config.Amt.ToString("0." + new string('#', 339))} {tokenName}",
                        action = (actionId, customData) => { BuyWithIcrc(actionId, config, actionOffer).Forget(); }
                        ,
                        imageContentType = new ImageContentType.Url(actionOffer.imageUrl),
                        infoWindowData = new InfoPopupWindow.WindowData(actionOffer.name, actionOffer.description)
                    }, content);
                }
                else if (actionOffer.HasPluginType(ActionPluginTag.VerifyBurnNfts))
                {
                    var plugin = actionPluginResult.AsVerifyBurnNfts();

                    string possibleOutcoemsContent = actionOffer.actionResult.GetAllPossibleOutcomes().Filter(e =>
                    {
                        return e.OutcomeType == ActionOutcomeOption.OptionInfoTag.IncrementNumber;
                    }).Reduce(k =>
                    {
                        if (k is ActionOutcomeTypes.IncrementNumber receiveQuantity)
                        {
                            var quantity = receiveQuantity.Quantity;

                            EntityUtil.GetConfigFieldAs<string>(receiveQuantity.GetConfigId(), "name", out var configName, receiveQuantity.GetConfigId());

                            return $"{configName} x {quantity}";
                        }
                        else
                        {
                            return $"Wrong type: {k.GetType()}";
                        }
                    });


                    ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = key,
                        textButtonContent = $"Burn Nft",
                        content = $"Posible Rewards:\n\n{possibleOutcoemsContent}",
                        action = (actionId, customData) => { BurnNftHandler(actionId); }
                      ,
                        imageContentType = new ImageContentType.Url(actionOffer.imageUrl),
                        infoWindowData = new InfoPopupWindow.WindowData(actionOffer.name, actionOffer.description)
                    }, content);
                }
                else Debug.Log($"Trying to call wrong action type of id: {actionPluginResult.Tag} -" + key);
            }
        });
    }

    private void BurnNftHandler(string actionid)
    {
        BurnNft(actionid).Forget();
    }

    private async UniTaskVoid BurnNft(string actionid)
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));


        var nextNftIndexResult = NftUtil.TryGetNextNftIndex(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        if (nextNftIndexResult.IsErr)
        {
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", nextNftIndexResult.AsErr()), 3);
            BroadcastState.Invoke(new WaitingForResponse(false));

            return;
        }

        var result = await ActionUtil.Action.VerifyBurnNfts(actionid, Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        if (result.Tag == UResultTag.Err)
        {
            //UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));
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

        var actionResult = await ActionUtil.Action.Default(actionId);

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
        //EntityUtil.IncrementNumber(resultAsOk.receivedEntities.ToArray());
        //EntityUtil.Decrementnumber(resultAsOk.spentEntities.ToArray());

    }
    private async UniTaskVoid BuyWithIcp(string actionId, ActionPlugin.VerifyTransferIcpInfo config, DataTypes.Action actionOffer)
    {
        BroadcastState.Invoke(new WaitingForResponse(true));

        var actionResult = await ActionUtil.Action.TransferAndVerifyIcp(actionId);

        //CHECK FOR ERR
        if (actionResult.Tag == UResultTag.Err)
        {
            var actionError = actionResult.AsErr();

            switch (actionError)
            {
                case ActionErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You don't have enough ICP", $"Requirements:\n{$"{config.Amt} ICP\n\nYou need to deposit some ICP"}"), 3);
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
        //if (actionOffer.tag.Contains("Mint"))
        //{
        //    NftUtil.TryAddMintedNft(resultAsOk.nfts.ToArray());
        //}
        //else
        //{
        //    EntityUtil.IncrementNumber(resultAsOk.receivedEntities.ToArray());
        //}
    }
    private async UniTaskVoid BuyWithIcrc(string actionId, ActionPlugin.VerifyTransferIcrcInfo config, DataTypes.Action actionOffer)
    {
        BroadcastState.Invoke(new WaitingForResponse(true));

        var tokenSymbol = "ICRC";
        var userBalance = 0D;

        var tokenAndConfigsResult = TokenUtil.GetTokenDetails(config.Canister);

        if (tokenAndConfigsResult.Tag == UResultTag.Ok)
        {
            var (token, tokenConfigs) = tokenAndConfigsResult.AsOk();

            tokenSymbol = tokenConfigs.symbol;
            userBalance = token.baseUnitAmount.ConvertToDecimal(tokenConfigs.decimals);
        }

        $"Required ICRC: {config.Amt} Balance: {userBalance}".Log();

        var actionResult = await ActionUtil.Action.TransferAndVerifyIcrc(actionId, config.Canister);

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
                            $"Requirements:\n{$"{tokenSymbol} x {config.Amt}"}\n\nYou need to mint more \"{tokenSymbol}\"",
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
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Other issue! "+ actionResult.AsErr().GetType().Name, actionResult.AsErr().content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();

        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));
        //if (actionOffer.tag.Contains("Mint"))
        //{
        //    NftUtil.TryAddMintedNft(resultAsOk.nfts.ToArray());
        //}
        //else
        //{
        //    EntityUtil.IncrementNumber(resultAsOk.receivedEntities.ToArray());
        //}
    }
    private List<KeyValue<string, DataTypes.Action>> GetValidOffers(List<string> actionOfferIds)
    {

        List<KeyValue<string, DataTypes.Action>> offers = new();

        actionOfferIds.Iterate(e =>
        {
            var getDataResult = UserUtil.GetElementOfType<DataTypes.Action>(e);
            if (getDataResult.Tag == UResultTag.Err)
            {
                return;
            }

            if (getDataResult.Tag == UResultTag.None)
            {
                return;
            }

            var config = getDataResult.AsOk();

            var actionPlugin = config.actionPlugin;

            if (actionPlugin != null)
            {
                if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcp &&
                actionPlugin.Tag != ActionPluginTag.VerifyTransferIcrc &&
                actionPlugin.Tag != ActionPluginTag.VerifyBurnNfts) return;
            }

            offers.Add(new(e, config));
        });

        return offers;
    }


    private void DisplayActionResponse(ProcessedActionResponse resonse)
    {
        List<string> inventoryElements = new();

        //NFTs
        Dictionary<string, int> collectionsToDisplay = new();
        resonse.nfts.Iterate(e =>
        {

            if (collectionsToDisplay.TryAdd(e.Canister, 1) == false) collectionsToDisplay[e.Canister] += 1;

        });

        collectionsToDisplay.Iterate(e =>
        {
            string tokenName = "Some Name";
            var fetchOwnTokenDataResult = UserUtil.GetElementOfType<DataTypes.NftCollection>(e.Key);

            if (fetchOwnTokenDataResult.IsOk)
            {
                tokenName = fetchOwnTokenDataResult.AsOk().name;
            }
            else
            {
                tokenName = "Name not Found";
            }

            inventoryElements.Add($"{tokenName} x {e.Value}");
        });


        //ENTITIES
        resonse.incrementNumberEntities.Iterate(e =>
        {
            if (!EntityUtil.GetConfigFieldAs<string>(e.Value.GetConfigId(), "name", out var configName)) return;
            if (!e.Value.GetFieldAs("quantity", out double configQuantity)) return;

            if (e.Value.TryGetConfig(out var config)) inventoryElements.Add($"{configName} x {configQuantity}");
            else inventoryElements.Add($"{e.Value.GetKey()} x {configQuantity}");
        });

        WindowManager.Instance.OpenWindow<InventoryPopupWindow>(new InventoryPopupWindow.WindowData("Earned Items", inventoryElements), 3);
    }
}