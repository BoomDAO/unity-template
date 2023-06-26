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

public class StakeWidrawWindow : Window
{
    [SerializeField] string claimIcpStakeRewardActionId = "stakeIcp";
    [SerializeField] string claimRcpStakeRewardActionId = "stakeRc";
    [SerializeField] string claimNftpStakeRewardActionId = "stakeNft";

    [SerializeField] TMP_Text stakeText;

    [SerializeField] Button stakeIcpButton;
    [SerializeField] Button withdrawIcpButton;
    [SerializeField] Button claimIcpStakeRewardButton;

    //[SerializeField] TMP_Text stakedIcrcText;
    [SerializeField] Button stakeIcrcButton;
    [SerializeField] Button withdrawIcrcButton;
    [SerializeField] Button claimRcStakeRewardButton;

    //[SerializeField] TMP_Text stakedNftText;
    [SerializeField] Button stakeNftButton;
    [SerializeField] Button withdrawNftButton;
    [SerializeField] Button claimNftStakeRewardButton;

    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        stakeIcpButton.onClick.AddListener(OnIcpStake);
        withdrawIcpButton.onClick.AddListener(OnIcpWithDraw);
        claimIcpStakeRewardButton.onClick.AddListener(ClaimIcpStakeReward);

        stakeIcrcButton.onClick.AddListener(OnIcrcStake);
        withdrawIcrcButton.onClick.AddListener(OnIcrcWithDraw);
        claimRcStakeRewardButton.onClick.AddListener(ClaimRcStakeReward);

        stakeNftButton.onClick.AddListener(OnNftStake);
        withdrawNftButton.onClick.AddListener(OnNftWithDraw);
        claimNftStakeRewardButton.onClick.AddListener(ClaimNftStakeReward);

        BroadcastState.Register<DataState<StakeData>>(UpdateWindow, true);
    }

    public void OnDestroy()
    {
        stakeIcpButton.onClick.RemoveListener(OnIcpStake);
        withdrawIcpButton.onClick.RemoveListener(OnIcpWithDraw);
        claimIcpStakeRewardButton.onClick.RemoveListener(ClaimIcpStakeReward);

        stakeIcrcButton.onClick.RemoveListener(OnIcrcStake);
        withdrawIcrcButton.onClick.RemoveListener(OnIcrcWithDraw);
        claimRcStakeRewardButton.onClick.RemoveListener(ClaimRcStakeReward);

        stakeNftButton.onClick.RemoveListener(OnNftStake);
        withdrawNftButton.onClick.RemoveListener(OnNftWithDraw);
        claimNftStakeRewardButton.onClick.RemoveListener(ClaimNftStakeReward);

        BroadcastState.Unregister<DataState<StakeData>>(UpdateWindow);
    }

    private void UpdateWindow(DataState<StakeData> obj)
    {
        //BroadcastState.TryRead<DataState<IcrcData>>(out var icrcDataState);
        if (obj.IsReady())
        {
            stakeText.text = obj.data.stakes.Reduce(e => $"Type: {e.TokenType} | Id: {e.CanisterId} | Amt :{e.Amount} | Index?: {e.Index}\n");
        }
        else
        {
            stakeText.text = "None Stakes...";
        }
    }

    private async void OnIcpStake()
    {
        Debug.Log("Calling Transfer");
        var transferResult = await TxUtil.Transfer_ICP(0.005.TokenizeToIcp(), CandidApiManager.PaymentCanisterStakeIdentifier);

        if (transferResult.Tag == UResultTag.Err)
        {
            Debug.Log($"Stake Failure, msg: {transferResult.AsErr()}");
            return;
        }
        Debug.Log($"Stake Success, msg: {transferResult.AsOk()}");
        var updateStakeResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateIcpStakes(transferResult.AsOk(), Env.CanisterIds.STAKING_HUB, CandidApiManager.UserPrincipal, 0.005.TokenizeToIcp());

        if(updateStakeResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
        {
            Debug.LogError($"Stake Update Failure {updateStakeResult.AsErr()}");
        }
    }
    private async void OnIcpWithDraw()
    {
        Debug.Log("ICP STAKE WITHDRAWAL");

        var result = await CandidApiManager.Instance.StakingHubApiClient.DissolveIcp();

        if(result.Tag == Candid.StakingHub.Models.ResultTag.Ok) Debug.Log("ICP Withdrawal Ok result " + result.AsOk());
        else Debug.Log("ICP Withdrawal Failure result " + result.AsErr());

        UserUtil.UpdateBalanceReq_Icp();
    }

    private async void ClaimIcpStakeReward()
    {
        Debug.Log("ICP STAKE CLAIM");

        var actionResult = await TxUtil.ProcessActionEntities(new ActionArgValueTypes.ClaimStakingRewardArg(claimIcpStakeRewardActionId));

        if (actionResult.Tag == UResultTag.Ok)
        {
            Debug.Log("ICP StakeClaim Success " + actionResult.AsOk());
        }
        else
        {
            Debug.LogError("ICP StakeClaim Failure " + actionResult.AsErr());
        }
    }
    //
    private async void OnIcrcStake()
    {
        Debug.Log("Calling Transfer");
        var transferResult = await TxUtil.Transfer_RC(0.00001.TokenizeToCkBtc(), Env.CanisterIds.STAKING_HUB);

        if (transferResult.Tag == UResultTag.Err)
        {
            Debug.LogError($"Stake Failure, msg: {transferResult.AsErr()}");
            return;
        }
        Debug.Log($"Stake Success, msg: {transferResult.AsOk()}");
        var updateStakeResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateIcrcStakes(transferResult.AsOk(), Env.CanisterIds.STAKING_HUB, CandidApiManager.UserPrincipal, 0.00001.TokenizeToCkBtc(), Env.CanisterIds.ICRC_LEDGER);

        if (updateStakeResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
        {
            Debug.LogError($"Stake Update Failure {updateStakeResult.AsErr()}");
        }
    }
    private async void OnIcrcWithDraw()
    {
        Debug.Log("ICRC STAKE WITHDRAWAL");

        var result = await CandidApiManager.Instance.StakingHubApiClient.DissolveIcrc(Env.CanisterIds.STAKING_HUB);

        if (result.Tag == Candid.StakingHub.Models.ResultTag.Ok) Debug.Log("ICRC Withdrawal Ok result " + result.AsOk());
        else Debug.Log("ICRC Withdrawal Failure result " + result.AsErr());

        UserUtil.UpdateBalanceReq_Rc();
    }
    private async void ClaimRcStakeReward()
    {
        Debug.Log("ICRC STAKE CLAIM");

        var actionResult = await TxUtil.ProcessActionEntities(new ActionArgValueTypes.ClaimStakingRewardArg(claimRcpStakeRewardActionId));

        if (actionResult.Tag == UResultTag.Ok)
        {
            Debug.Log("ICRC StakeClaim Success " + actionResult.AsOk());
        }
        else
        {
            Debug.LogError("ICRC StakeClaim Failure " + actionResult.AsErr());
        }
    }
    //
    private async void OnNftStake()
    {
        if(DabNftUtil.TryGetNextNftIndex(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, out var index, Env.Nfts.NFT_OF_USAGE_TO_BURN))
        {
            Debug.Log($"Try stake nft of index "+ index);

            Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(Env.Nfts.BOOM_COLLECTION_CANISTER_ID));

            var transferResult = await collectionInterface.Transfer(new Candid.extv2_boom.Models.TransferRequest(
                1,
                new(Candid.extv2_boom.Models.UserTag.Address, CandidApiManager.UserAccountIdentity),
                new(),
                false,
                new(),
                new(Candid.extv2_boom.Models.UserTag.Address, CandidApiManager.PaymentCanisterStakeIdentifier),
                $"{index}"
            ));

            if(transferResult.Tag == Candid.extv2_boom.Models.TransferResponseTag.Ok)
            {
                //var collectionIdentifierResult = await CandidApiManager.Instance.CoreApiClient.GetAccountIdentifier(Env.Nfts.BOOM_COLLECTION);
                var verificationResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateExtStakes((uint)index, CandidApiManager.PaymentCanisterStakeIdentifier, CandidApiManager.UserAccountIdentity, Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

                if(verificationResult.Tag == Candid.StakingHub.Models.ResponseTag.Success)
                {
                    Debug.Log($"NFT Stake Success. Index {index}");
                }
                else
                {
                    Debug.LogError($"Verification transfer of nft index {index} failed");
                }
            }
            else
            {
                Debug.LogError($"Transfer of nft index {index} failed");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to fetch a nft index");

        }
    }
    private async void OnNftWithDraw()
    {
        BroadcastState.TryRead<DataState<StakeData>>(out var stakeData);
        if (stakeData.IsReady())
        {
            //Look for the first stake element of type nft, get its index and use it as the nft to withdraw
            var withdrawalResult = await CandidApiManager.Instance.StakingHubApiClient.DissolveExt(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, 0);

            if(withdrawalResult.Tag == Candid.StakingHub.Models.ResultTag.Ok)
            {
            }
            else
            {

            }
        }
        else
        {
            Debug.LogWarning("Stake Data not ready");
        }
    }
    private async void ClaimNftStakeReward()
    {
        Debug.Log("NFT STAKE CLAIM");
        var actionResult = await TxUtil.ProcessActionEntities(new ActionArgValueTypes.ClaimStakingRewardArg(claimNftpStakeRewardActionId));

        if (actionResult.Tag == UResultTag.Ok)
        {
            Debug.Log("NFT StakeClaim Success " + actionResult.AsOk());
        }
        else
        {
            Debug.LogError("NFT StakeClaim Failure " + actionResult.AsErr());
        }
    }
}
