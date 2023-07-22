using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using Candid.World.Models;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StakeWindow : Window
{
    [SerializeField] string claimIcpStakeRewardActionId = "stakeIcp";
    [SerializeField] string claimRcpStakeRewardActionId = "stakeRc";
    [SerializeField] string claimNftpStakeRewardActionId = "stakeNft";

    [SerializeField] TextMeshProUGUI stakeText;

    [SerializeField] Button stakeIcpButton;
    [SerializeField] Button unstakeIcpButton;
    [SerializeField] Button claimIcpStakeRewardButton;

    //[SerializeField] TMP_Text stakedIcrcText;
    [SerializeField] Button stakeIcrcButton;
    [SerializeField] Button unstakeIcrcButton;
    [SerializeField] Button claimRcStakeRewardButton;

    //[SerializeField] TMP_Text stakedNftText;
    [SerializeField] Button stakeNftButton;
    [SerializeField] Button unstakeNftButton;
    [SerializeField] Button claimNftStakeRewardButton;

    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        stakeIcpButton.onClick.AddListener(OnIcpStakeHandler);
        unstakeIcpButton.onClick.AddListener(OnIcpUnstakeHandler);
        claimIcpStakeRewardButton.onClick.AddListener(ClaimIcpStakeRewardHandler);

        stakeIcrcButton.onClick.AddListener(OnIcrcStakeHandler);
        unstakeIcrcButton.onClick.AddListener(OnIcrcUnstakeHandler);
        claimRcStakeRewardButton.onClick.AddListener(ClaimIcrcStakeRewardHandler);

        stakeNftButton.onClick.AddListener(OnNftStakeHandler);
        unstakeNftButton.onClick.AddListener(OnNftUnstakeHandler);
        claimNftStakeRewardButton.onClick.AddListener(ClaimNftStakeRewardHandler);

        UserUtil.RegisterToDataChange<DataTypes.Stake>(UpdateWindow, true);
    }

    public void OnDestroy()
    {
        stakeIcpButton.onClick.RemoveListener(OnIcpStakeHandler);
        unstakeIcpButton.onClick.RemoveListener(OnIcpUnstakeHandler);
        claimIcpStakeRewardButton.onClick.RemoveListener(ClaimIcpStakeRewardHandler);

        stakeIcrcButton.onClick.RemoveListener(OnIcrcStakeHandler);
        unstakeIcrcButton.onClick.RemoveListener(OnIcrcUnstakeHandler);
        claimRcStakeRewardButton.onClick.RemoveListener(ClaimIcrcStakeRewardHandler);

        stakeNftButton.onClick.RemoveListener(OnNftStakeHandler);
        unstakeNftButton.onClick.RemoveListener(OnNftUnstakeHandler);
        claimNftStakeRewardButton.onClick.RemoveListener(ClaimNftStakeRewardHandler);

        UserUtil.UnregisterToDataChange<DataTypes.Stake>(UpdateWindow);
    }

    private void UpdateWindow(DataState<Data<DataTypes.Stake>> obj)
    {
        if (obj.IsReady())
        {
            if(obj.data.elements.Count == 0) stakeText.text = "You don't have Stakes...";
            else stakeText.text = obj.data.elements.Reduce(e => $"Type: {e.Value.TokenType} | Id: {e.Value.CanisterId} | Amt :{e.Value.Amount} | Index?: {e.Value.BlockIndex}", ",\n");
        }
        else
        {
            stakeText.text = "Loading...";
        }
    }

    #region Handlers
    private void OnIcpStakeHandler()
    {
        OnIcpStake().Forget();
    }
    private void OnIcpUnstakeHandler()
    {
        OnIcpUnstake().Forget();
    }
    private void ClaimIcpStakeRewardHandler()
    {
        ClaimIcpStakeReward().Forget();
    }

    private void OnIcrcStakeHandler()
    {
        OnIcrcStake().Forget();
    }
    private void OnIcrcUnstakeHandler()
    {
        OnIcrcUnstake().Forget();
    }
    private void ClaimIcrcStakeRewardHandler()
    {
        ClaimIcrcStakeReward().Forget();
    }

    private void OnNftStakeHandler()
    {
        OnNftStake().Forget();
    }
    private void OnNftUnstakeHandler()
    {
        OnNftUnstake().Forget();
    }
    private void ClaimNftStakeRewardHandler()
    {
        ClaimNftStakeReward().Forget();
    }
    #endregion

    private async UniTaskVoid OnIcpStake()
    {
        //await UniTask.SwitchToMainThread();

        var amountToStake = 0.005D;

        BroadcastState.Invoke(new WaitingForResponse(true));


        var result = await ActionUtil.Stake.StakeToken(amountToStake);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case StakeErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("You don't have enough ICP", $"Requirements:\n{$"{amountToStake} ICP\n\nYou need to deposit some ICP"}"), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid OnIcpUnstake()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Stake.UnstakeToken();

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case UnstakeErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Insufficient Balance", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid ClaimIcpStakeReward()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Action.ClaimStakeRewardIcp(claimIcpStakeRewardActionId);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case ActionErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Insufficient Balance", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = result.AsOk();
        DisplayActionResponse(result.AsOk());
        UserUtil.UpdateData<DataTypes.Entity>(resultAsOk.F1.ConvertToDataType());
        UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    //
    private async UniTaskVoid OnIcrcStake()
    {
        //await UniTask.SwitchToMainThread();

        var userTokenResult = UserUtil.GetElementOfType<DataTypes.TokenConfig>(Env.CanisterIds.ICRC_LEDGER);

        //CHECK FOR ERR
        var tokenName = "ICRC";
        if (userTokenResult.Tag == UResultTag.Ok)
        {
            tokenName = userTokenResult.AsOk().name;
        }

        var amountToStake = 1D;

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Stake.StakeToken(amountToStake, Env.CanisterIds.ICRC_LEDGER);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case StakeErrType.InsufficientBalance content:
                    Window infoPopup = null;
                    infoPopup = WindowManager.Instance.OpenWindow<InfoPopupWindow>(
                        new InfoPopupWindow.WindowData(
                            $"You don't have enough {tokenName}",
                            $"Requirements:\n{$"{tokenName} x {amountToStake.NotScientificNotation()}"}\n\nYou need to mint more \"{tokenName}\"",
                            new(new($"Mint ${tokenName}", () =>
                            {
                                if (infoPopup != null) infoPopup.Close();
                                Close();
                                WindowManager.Instance.OpenWindow<MintTestTokensWindow>(null, 1);
                            }
                    ))), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }
        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICRC_LEDGER);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid OnIcrcUnstake()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Stake.UnstakeToken(Env.CanisterIds.ICRC_LEDGER);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case UnstakeErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Insufficient Balance", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.STAKING_HUB);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid ClaimIcrcStakeReward()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Action.ClaimStakeRewardIcrc(claimRcpStakeRewardActionId);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case ActionErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Insufficient Balance", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = result.AsOk();
        DisplayActionResponse(result.AsOk());
        UserUtil.UpdateData<DataTypes.Entity>(resultAsOk.F1.ConvertToDataType());
        UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    //
    private async UniTaskVoid OnNftStake()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Stake.StakeNft(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case StakeErrType.InsufficientBalance content:
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

        UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid OnNftUnstake()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Stake.UnstakeNft(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case UnstakeErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Insufficient Balance", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new WaitingForResponse(false));
    }
    private async UniTaskVoid ClaimNftStakeReward()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var result = await ActionUtil.Action.ClaimStakeRewardNft(claimNftpStakeRewardActionId);

        if (result.Tag == UResultTag.Err)
        {
            switch (result.AsErr())
            {
                case ActionErrType.InsufficientBalance content:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Insufficient Balance", content.Content), 3);
                    break;
                default:
                    WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Some other issue!", result.AsErr().Content), 3);
                    break;
            }

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = result.AsOk();
        DisplayActionResponse(result.AsOk());
        UserUtil.UpdateData<DataTypes.Entity>(resultAsOk.F1.ConvertToDataType());
        UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());
        BroadcastState.Invoke(new WaitingForResponse(false));
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
