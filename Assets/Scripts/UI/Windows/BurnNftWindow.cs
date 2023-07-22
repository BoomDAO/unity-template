using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using Candid.World.Models;
using Cysharp.Threading.Tasks;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BurnNftWindow : Window
{
    [SerializeField] string burnNftActionId;
    [SerializeField] TextMeshProUGUI possibleOutcoemsContentText;
    [SerializeField] ListenToToggleInteract burnButton;
    [SerializeField] Button closeButton;
    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        UserUtil.RegisterToDataChange<DataTypes.ActionConfig>(UpdateWindow, true);
        UserUtil.RegisterToDataChange<DataTypes.NftCollection>(UpdateWindow, true);

        burnButton.Btn.onClick.AddListener(BurnHandler);

        burnNftActionId = "burn_nft_tiket";
    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.ActionConfig>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.NftCollection>(UpdateWindow);
        burnButton.Btn.onClick.RemoveListener(BurnHandler);
    }
    private void UpdateWindow(DataState<Data<DataTypes.ActionConfig>> state)
    {
        if (state.IsReady())
        {
            if(state.data.elements.TryLocate(e => e.Key == burnNftActionId, out var keyValActionConfig))
            {
                var actionConfig = keyValActionConfig.Value;

                actionConfig.ActionResult.Outcomes.Once(k =>
                {
                    possibleOutcoemsContentText.text = k.PossibleOutcomes.Reduce(s =>
                    {
                        switch (s.Option.Tag)
                        {
                            case ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity:
                                var option = s.Option.AsReceiveEntityQuantity();
                                var quantity = option.F3;
                                return $"{EntityUtil.GetName(option.GetKey())} x {quantity}";

                            default:
                                return "";
                        }
                    });
                });
            }
        }
    }
    private void UpdateWindow(DataState<Data<DataTypes.NftCollection>> state)
    {
        //if (state.IsReady() == false) BroadcastState.Invoke(new WaitingForResponse(true, "Loading Dependencies"));
        //else BroadcastState.Invoke(new WaitingForResponse(false));

        var nftCountResult = NftUtil.GetNftCount(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        if (nftCountResult.IsErr)
        {
            burnButton.ToggleForceDisable(true);

            return;
        }

        var nftCount = nftCountResult.AsOk();
        bool hasRequiredNfts = nftCount > 0;

        if (hasRequiredNfts)
        {
            burnButton.ToggleForceDisable(false);
        }
        else
        {
            burnButton.ToggleForceDisable(true);
        }
    }

    private void BurnHandler()
    {
        Burn().Forget();
    }

    private async UniTaskVoid Burn()
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

        var result = await ActionUtil.Action.BurnNft(burnNftActionId, Env.Nfts.BOOM_COLLECTION_CANISTER_ID, nextNftIndexResult.AsOk());

        if (result.Tag == UResultTag.Err)
        {
            UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));

            switch (result.AsErr())
            {
                case ActionErrType.InsufficientBalance content:
                    Window infoPopup = null;
                    infoPopup = WindowManager.Instance.OpenWindow<InfoPopupWindow>(
                    new InfoPopupWindow.WindowData(
                        $"You don't a nft to stake",
                        $"Requires 1 NFT From Collection:\n {Env.Nfts.BOOM_COLLECTION_CANISTER_ID}",
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

        UserUtil.UpdateData<DataTypes.Entity>(resultAsOk.F1.ConvertToDataType());
        UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());
    }

    private void DisplayActionResponse(ActionResponse resonse)
    {
        List<string> inventoryElements = new();

        var processedItems = resonse.F1.Filter(
                e => {
                    var localValue = UserUtil.GetElementOfType<DataTypes.Entity>($"{e.Gid}{e.Eid}");

                    //if player doesn't yet have any of this entity we return true
                    if (localValue.IsErr) return true;

                    var localValueAsOk = localValue.AsOk();

                    bool differentQuantities = localValueAsOk.quantity != e.Quantity.ValueOrDefault;

                    //we don't want to display entities whose quantity value has not changed
                    if (differentQuantities == false) return false;

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