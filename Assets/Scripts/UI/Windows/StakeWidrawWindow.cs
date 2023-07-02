using Candid;
using Candid.extv2_boom;
using Candid.IcpLedger.Models;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using Mono.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Env;

public class StakeWidrawWindow : Window
{
    [SerializeField] string claimIcpStakeRewardActionId = "stakeIcp";
    [SerializeField] string claimRcpStakeRewardActionId = "stakeRc";
    [SerializeField] string claimNftpStakeRewardActionId = "stakeNft";

    [SerializeField] TMP_Text stakeText;

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
        stakeIcpButton.onClick.AddListener(OnIcpStake);
        unstakeIcpButton.onClick.AddListener(OnIcpUnstake);
        claimIcpStakeRewardButton.onClick.AddListener(ClaimIcpStakeReward);

        stakeIcrcButton.onClick.AddListener(OnIcrcStake);
        unstakeIcrcButton.onClick.AddListener(OnIcrcUnstake);
        claimRcStakeRewardButton.onClick.AddListener(ClaimRcStakeReward);

        stakeNftButton.onClick.AddListener(OnNftStake);
        unstakeNftButton.onClick.AddListener(OnNftUnstake);
        claimNftStakeRewardButton.onClick.AddListener(ClaimNftStakeReward);

        UserUtil.RegisterToDataChange<DataTypes.Stake>(UpdateWindow, true);
    }

    public void OnDestroy()
    {
        stakeIcpButton.onClick.RemoveListener(OnIcpStake);
        unstakeIcpButton.onClick.RemoveListener(OnIcpUnstake);
        claimIcpStakeRewardButton.onClick.RemoveListener(ClaimIcpStakeReward);

        stakeIcrcButton.onClick.RemoveListener(OnIcrcStake);
        unstakeIcrcButton.onClick.RemoveListener(OnIcrcUnstake);
        claimRcStakeRewardButton.onClick.RemoveListener(ClaimRcStakeReward);

        stakeNftButton.onClick.RemoveListener(OnNftStake);
        unstakeNftButton.onClick.RemoveListener(OnNftUnstake);
        claimNftStakeRewardButton.onClick.RemoveListener(ClaimNftStakeReward);

        UserUtil.UnregisterToDataChange<DataTypes.Stake>(UpdateWindow);
    }

    private void UpdateWindow(DataState<Data<DataTypes.Stake>> obj)
    {
        if (obj.IsReady())
        {
            stakeText.text = obj.data.elements.Reduce(e => $"Type: {e.Value.TokenType} | Id: {e.Value.CanisterId} | Amt :{e.Value.Amount} | Index?: {e.Value.BlockIndex}\n");
        }
        else
        {
            stakeText.text = "None Stakes...";
        }
    }

    private async void OnIcpStake()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Stake.StakeIcp(0.005);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }

        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    private async void OnIcpUnstake()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Stake.UnstakeIcp();

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }

        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    private async void ClaimIcpStakeReward()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Action.ClaimStakeRewardIcp(claimIcpStakeRewardActionId);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }

        UserUtil.RequestData<DataTypes.Item>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    //
    private async void OnIcrcStake()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Stake.StakeIcrc(0.00001, Env.CanisterIds.ICRC_LEDGER);

        if (result.Tag == UResultTag.Err) Debug.LogError(result.AsErr());

        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICRC_LEDGER);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    private async void OnIcrcUnstake()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Stake.UnstakeIcrc(Env.CanisterIds.STAKING_HUB);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }

        UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.STAKING_HUB);
        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    private async void ClaimRcStakeReward()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Action.ClaimStakeRewardIcrc(claimRcpStakeRewardActionId);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }

        UserUtil.RequestData<DataTypes.Item>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    //
    private async void OnNftStake()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Stake.StakeNft(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }

        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    private async void OnNftUnstake()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var getStakesResult =  UserUtil.GetDataElementsOfType<DataTypes.Stake>();

        if(getStakesResult.Tag == UResultTag.Err)
        {
            Debug.LogError("Fetching Stakes Failure, msg: " + getStakesResult.AsErr());
            return;
        }

        var stakes = getStakesResult.AsOk();

        var nextNftStake = stakes.Locate(e => e.CanisterId == Env.Nfts.BOOM_COLLECTION_CANISTER_ID && e.BlockIndex != null);

        if (nextNftStake != null)
        {
            Debug.LogError("Stake Data not ready");
            return;
        }

        if(!nextNftStake.BlockIndex.TryParseValue(out uint nftIndex))
        {
            Debug.LogError("Nft Index could not be parsed");
            return;
        }

        var result = await TxUtil.Stake.UnstakeNft(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, nftIndex);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }

        UserUtil.RequestData<DataTypes.Stake>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
    private async void ClaimNftStakeReward()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));
        var result = await TxUtil.Action.ClaimStakeRewardNft(claimNftpStakeRewardActionId);

        if (result.Tag == UResultTag.Err)
        {
            Debug.LogError(result.AsErr());
            return;
        }
        UserUtil.RequestData<DataTypes.Item>();
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
}
