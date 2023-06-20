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
    [SerializeField] TMP_Text stakeText;

    [SerializeField] Button stakeIcpButton;
    [SerializeField] Button withdrawIcpButton;

    [SerializeField] TMP_Text stakedIcrcText;
    [SerializeField] Button stakeIcrcButton;
    [SerializeField] Button withdrawIcrcButton;

    [SerializeField] TMP_Text stakedNftText;
    [SerializeField] Button stakeNftButton;
    [SerializeField] Button withdrawNftButton;

    [SerializeField] Transaction stakeContent;

    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        stakeIcpButton.onClick.AddListener(OnIcpStake);
        withdrawIcpButton.onClick.AddListener(OnIcpWithDraw);

        stakeIcrcButton.onClick.AddListener(OnIcrcStake);
        withdrawIcrcButton.onClick.AddListener(OnIcrcWithDraw);

        stakeNftButton.onClick.AddListener(OnNftStake);
        withdrawNftButton.onClick.AddListener(OnNftWithDraw);

        BroadcastState.Register<DataState<StakeData>>(UpdateWindow, true);
    }

    public void OnDestroy()
    {
        stakeIcpButton.onClick.RemoveListener(OnIcpStake);
        withdrawIcpButton.onClick.RemoveListener(OnIcpWithDraw);

        stakeIcrcButton.onClick.RemoveListener(OnIcrcStake);
        withdrawIcrcButton.onClick.RemoveListener(OnIcrcWithDraw);

        stakeNftButton.onClick.RemoveListener(OnNftStake);
        withdrawNftButton.onClick.RemoveListener(OnNftWithDraw);

        BroadcastState.Unregister<DataState<StakeData>>(UpdateWindow);
    }

    private void UpdateWindow(DataState<StakeData> obj)
    {
        BroadcastState.TryRead<DataState<IcrcData>>(out var icrcDataState);
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
        //Debug.Log("Calling Transfer");
        //var transferResult = await TxUtil.TransferToStakeCanister_ICP(Env.Stacking.ICP_STAKE);

        //if (transferResult.State == UResultState.Err)
        //{
        //    Debug.Log($"Transfer failed, msg: {transferResult.AsErr()}");
        //    return;
        //}

        //var verifyStakeResult = await TxUtil.VerifyStake_ICP(transferResult.AsOk(), Env.Stacking.ICP_STAKE);

        //if(verifyStakeResult.State == UResultState.Ok) Debug.Log("Stake Ok Result: " + verifyStakeResult.AsOk());
        //else Debug.Log("Stake Err Result: " + verifyStakeResult.AsErr());
    }
    private async void OnIcpWithDraw()
    {
        var result = await CandidApiManager.Instance.StakingHubApiClient.DissolveIcp();

        if(result.Tag == Candid.StakingHub.Models.ResultTag.Ok) Debug.Log("Withdrawal Ok result " + result.AsOk());
        else Debug.Log("Withdrawal Failure result " + result.AsErr());

        UserUtil.UpdateBalanceReq_Icp();
    }
    private async void OnIcrcStake()
    {
        //BroadcastState.TryRead<DataState<IcrcData>>(out var icrcDataState);

        //if(icrcDataState.IsReady() == false)
        //{
        //    Debug.LogError("Icrc data must be ready");
        //    return;
        //}

        //Debug.Log("Calling Transfer");
        //Debug.Log("Current RC Balance A: "+ icrcDataState.data.amt);
        //var transferResult = await TxUtil.TransferToStakeCanister_RC(Env.Stacking.ICRC_STAKE, icrcDataState.data.decimalCount);
        //Debug.Log("Current RC Balance B: " + icrcDataState.data.amt);

        //if (transferResult.State == UResultState.Err)
        //{
        //    Debug.Log($"Transfer failed, msg: {transferResult.AsErr()}");
        //    return;
        //}

        //var verifyStakeResult = await TxUtil.VerifyStake_RC(transferResult.AsOk(), Env.Stacking.ICRC_STAKE, icrcDataState.data.decimalCount);

        //if (verifyStakeResult.State == UResultState.Ok) Debug.Log("Stake Ok Result: " + verifyStakeResult.AsOk());
        //else Debug.Log("Stake Err Result: " + verifyStakeResult.AsErr());
    }
    private async void OnIcrcWithDraw()
    {
        await CandidApiManager.Instance.StakingHubApiClient.DissolveIcrc(Env.CanisterIds.ICRC_LEDGER);
    }
    private async void OnNftStake()
    {
        if(DabNftUtil.TryGetNextNftIndex(Env.Nfts.BOOM_COLLECTION, out var index, Env.Nfts.NFT_OF_USAGE_TO_BURN))
        {
            Debug.Log($"Try stake nft of index "+ index);

            Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(Env.Nfts.BOOM_COLLECTION));

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
                var verificationResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateExtStakes((uint)index, CandidApiManager.PaymentCanisterStakeIdentifier, CandidApiManager.UserAccountIdentity, Env.Nfts.BOOM_COLLECTION);

                if(verificationResult.Tag == Candid.StakingHub.Models.ResponseTag.Success)
                {
                    Debug.Log($"NFT Stake Success. Index {index}");
                }
                else
                {
                    Debug.Log($"Verification transfer of nft index {index} failed");
                }
            }
            else
            {
                Debug.Log($"Transfer of nft index {index} failed");
            }
        }
        else
        {
            Debug.Log($"Failed to fetch a nft index");

        }
    }
    private async void OnNftWithDraw()
    {
        BroadcastState.TryRead<DataState<StakeData>>(out var stakeData);
        if (stakeData.IsReady())
        {
            //Look for the first stake element of type nft, get its index and use it as the nft to withdraw
            var withdrawalResult = await CandidApiManager.Instance.StakingHubApiClient.DissolveExt(Env.Nfts.BOOM_COLLECTION, 0);

            if(withdrawalResult.Tag == Candid.StakingHub.Models.ResultTag.Ok)
            {
            }
            else
            {

            }
        }
        else
        {
            Debug.Log("Stake Data not ready");
        }
    }


}
