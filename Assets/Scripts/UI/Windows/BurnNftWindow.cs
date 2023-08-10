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

                actionConfig.actionResult.Outcomes.Once(k =>
                {
                    possibleOutcoemsContentText.text = k.PossibleOutcomes.Reduce(s =>
                    {
                        switch (s.Option.Tag)
                        {
                            case ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity:
                                var option = s.Option.AsReceiveEntityQuantity();
                                var quantity = option.Quantity;
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

        var result = await ActionUtil.Action.VerifyBurnNfts(burnNftActionId, Env.Nfts.BOOM_COLLECTION_CANISTER_ID);//, nextNftIndexResult.AsOk());

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

        EntityUtil.IncrementCurrentQuantity(resultAsOk.receivedEntities.ToArray());
    }

    private void DisplayActionResponse(ProcessedActionResponse resonse)
    {
        List<string> inventoryElements = new();

        resonse.receivedEntities.Iterate(e =>
        {
            inventoryElements.Add($"{EntityUtil.GetName(e.GetKey(), $"Key: {e.GetKey()}: ")} x {e.quantity}");
        });

        WindowManager.Instance.OpenWindow<InventoryPopupWindow>(new InventoryPopupWindow.WindowData("Earned Items", inventoryElements), 3);
    }
}