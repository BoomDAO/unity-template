using Candid.World.Models;
using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using System.Linq;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
        UserUtil.RegisterToDataChange<DataTypes.Action>(UpdateWindow);
        UserUtil.RegisterToDataChange<DataTypes.EntityConfig>(UpdateWindow, true);
    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.Entity>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.Action>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.EntityConfig>(UpdateWindow);
    }
    private void UpdateWindow(DataState<Data<DataTypes.Entity>> state)
    {
        if (UserUtil.IsDataValid<DataTypes.Entity>())
        {
            var entityConfigData = UserUtil.GetDataOfType<DataTypes.EntityConfig>();
            UpdateWindow(entityConfigData.AsOk());
        }
    }
    private void UpdateWindow(DataState<Data<DataTypes.Action>> state)
    {
        if (UserUtil.IsDataValid<DataTypes.Entity>())
        {
            var entityConfigData = UserUtil.GetDataOfType<DataTypes.EntityConfig>();
            UpdateWindow(entityConfigData.AsOk());
        }
    }

    private void UpdateWindow(DataState<Data<DataTypes.EntityConfig>> state)
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

            if (!actionOffer.HasPlugin())
            {

                var constrain = actionOffer.GetEntityConstrain().Reduce(e =>
                {
                    double amount = e.GreaterThanOrEqualQuantity.HasValue ? e.GreaterThanOrEqualQuantity.ValueOrDefault : 0;
                    string name = EntityUtil.GetName(e.GetEntityKey(), e.GetEntityKey());
                    return $"{name} x {amount}\n\n";
                },"\n");

                ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                {
                    id = key,
                    textButtonContent = $"Trade",
                    content = $"{actionOffer.GetNameUnsafe()}\n\n{constrain} -> ItemC x 1",
                    action = (actionId, customData) => { Trade(actionId, constrain).Forget(); },
                    imageContentType = new ImageContentType.Url(actionOffer.GetUrlImageUnsafe()),
                    infoWindowData = new InfoPopupWindow.WindowData(actionOffer.GetNameUnsafe(), actionOffer.GetDescriptionUnsafe())
                }, content);
            }
            else
            {
                var actionPlugin = actionOffer.GetPluginUnsafe();

                if (actionOffer.HasPluginType(ActionPluginTag.VerifyTransferIcp))
                {
                    var config = actionPlugin.AsVerifyTransferIcp();

                    ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = key,
                        textButtonContent = $"{(actionOffer.GetTagUnsafe() == "Mint" ? "Mint" : "Buy")}",
                        content = $"{actionOffer.GetNameUnsafe()}\n\nprice: {config.Amt.NotScientificNotation()} ICP",
                        action = (actionId, customData) => { BuyWithIcp(actionId, config, actionOffer).Forget(); },
                        imageContentType = new ImageContentType.Url(actionOffer.GetUrlImageUnsafe()),
                        infoWindowData = new InfoPopupWindow.WindowData(actionOffer.GetNameUnsafe(), actionOffer.GetDescriptionUnsafe())
                    }, content);
                }
                else if (actionOffer.HasPluginType(ActionPluginTag.VerifyTransferIcrc))
                {
                    var config = actionPlugin.AsVerifyTransferIcrc();

                    var tokenNameResult = UserUtil.GetPropertyFromType<DataTypes.TokenConfig, string>(config.Canister, e => e.name);
                    var tokenName = tokenNameResult.Tag == UResultTag.Ok ? tokenNameResult.AsOk() : "ICRC";

                    ActionWidget aw = WindowManager.Instance.AddWidgets<ActionWidget>(new ActionWidget.WindowData()
                    {
                        id = key,
                        textButtonContent = $"{(actionOffer.GetTagUnsafe() == "Mint" ? "Mint" : "Buy")}",
                        content = $"{actionOffer.GetNameUnsafe()}\n\nprice: {config.Amt.ToString("0." + new string('#', 339))} {tokenName}",
                        action = (actionId, customData) => { BuyWithIcrc(actionId, config, actionOffer).Forget(); }
                        ,
                        imageContentType = new ImageContentType.Url(actionOffer.GetUrlImageUnsafe()),
                        infoWindowData = new InfoPopupWindow.WindowData(actionOffer.GetNameUnsafe(), actionOffer.GetDescriptionUnsafe())
                    }, content);
                }
                else Debug.Log($"Trying to call wrong action type of id: {actionPlugin.Tag} -" + key);
            }
        });
    }
    private async UniTaskVoid Trade(string actionId, string constrain)
    {
        //await UniTask.SwitchToMainThread();

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
        //UserUtil.RequestData<DataTypes.Entity>();
        UserUtil.UpdateData<DataTypes.Entity>(resultAsOk.F1.ConvertToDataType());
        UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());
    }
    private async UniTaskVoid BuyWithIcp(string actionId, ActionPlugin.VerifyTransferIcpInfo config, DataTypes.ActionConfig actionOffer)
    {
        //await UniTask.SwitchToMainThread();

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
        if (actionOffer.Tag.ValueOrDefault.Contains("Mint"))
        {
            //UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));
            NftUtil.TryAddMintedNft(resultAsOk.F2.ToArray());
        }
        else
        {
            //UserUtil.RequestData<DataTypes.Entity>();
            UserUtil.UpdateData<DataTypes.Entity>(resultAsOk.F1.ConvertToDataType());
            UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());
        }
    }
    private async UniTaskVoid BuyWithIcrc(string actionId, ActionPlugin.VerifyTransferIcrcInfo config, DataTypes.ActionConfig actionOffer)
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));

        var tokenSymbol = "ICRC";
        var userBalance = 0D;

        var tokenAndConfigsResult = UserUtil.GetTokenAndConfigs(config.Canister);

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
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Other issue!", actionResult.AsErr().content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();

        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));
        if (actionOffer.Tag.ValueOrDefault.Contains("Mint"))
        {
            //UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));
            NftUtil.TryAddMintedNft(resultAsOk.F2.ToArray());
        }
        else
        {
            //UserUtil.RequestData<DataTypes.Entity>();
            UserUtil.UpdateData<DataTypes.Entity>(resultAsOk.F1.ConvertToDataType());
            UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());
        }
    }
    private List<KeyValue<string, DataTypes.ActionConfig>> GetValidOffers(List<string> actionOfferIds)
    {

        List<KeyValue<string, DataTypes.ActionConfig>> offers = new();

        actionOfferIds.Iterate(e =>
        {
            var getDataResult = UserUtil.GetElementOfType<DataTypes.ActionConfig>(e);
            if (getDataResult.Tag == UResultTag.Err)
            {
                return;
            }

            if (getDataResult.Tag == UResultTag.None)
            {
                return;
            }

            var config = getDataResult.AsOk();

            var actionPlugin = config.ActionPlugin.ValueOrDefault;

            if (actionPlugin != null)
            {
                if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcp && actionPlugin.Tag != ActionPluginTag.VerifyTransferIcrc) return;
            }

            offers.Add(new(e, config));
        });

        return offers;
    }


    private void DisplayActionResponse(ActionResponse resonse)
    {
        List<string> inventoryElements = new();

        //NFTs
        Dictionary<string, int> collectionsToDisplay = new();
        resonse.F2.Iterate(e =>
        {

            if (collectionsToDisplay.TryAdd(e.Canister, 1) == false) collectionsToDisplay[e.Canister] += 1;

        });

        collectionsToDisplay.Iterate(e =>
        {
            string tokenName = "Some Name";
            var fetchOwnTokenDataResult = UserUtil.GetElementOfType<DataTypes.NftCollection>(e.Key);

            if (fetchOwnTokenDataResult.IsOk)
            {
                tokenName = fetchOwnTokenDataResult.AsOk().collectionName;
            }
            else
            {
                tokenName = "Name not Found";
            }

            inventoryElements.Add($"{tokenName} x {e.Value}");
        });


        //ENTITIES
        var processedItems = resonse.F1.Filter(
                e => {
                    var localValue = UserUtil.GetElementOfType<DataTypes.Entity>($"{e.GetKey()}");

                    //if player doesn't yet have any of this entity we return true
                    if (localValue.IsErr) return true;

                    var localValueAsOk = localValue.AsOk();

                    bool differentQuantities = localValueAsOk.quantity != e.Quantity.ValueOrDefault;

                    //we don't want to display entities whose quantity value has not changed
                    if(differentQuantities == false) return false;

                    bool willLocalIncreaseValue = localValueAsOk.quantity < e.Quantity.ValueOrDefault;

                    //we don't want to display entities whose quantity value has reduced
                    if (willLocalIncreaseValue == false) return false;

                    return true;
                });

        processedItems.Iterate(e =>
        {
            var localQuantity = EntityUtil.GetCurrentQuantity(e.GetKey());
            inventoryElements.Add($"{EntityUtil.GetName(e.GetKey(), $"Gid: {(string.IsNullOrEmpty(e.Gid) ? "CurrentWorld" : e.Gid)}: Eid: {e.Eid}")} x {e.GetQuantity() - localQuantity}");
        });

        WindowManager.Instance.OpenWindow<InventoryPopupWindow>(new InventoryPopupWindow.WindowData("Earned Items", inventoryElements), 3);
    }
}