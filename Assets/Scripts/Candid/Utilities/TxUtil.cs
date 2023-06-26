using Candid;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ItsJackAnton.Values;
using System.Collections.Generic;
using Candid.World.Models;
using System.Threading.Tasks;
using Candid.IcpLedger.Models;
using EdjCase.ICP.Candid.Models;
using System.Linq;
using System;

public static class ActionArgValueTypes
{
    public abstract class BaseArg
    {
        public abstract System.Object GetGeneratedValue();
    };
    public class BurnNftArg : BaseArg
    {
        public string ActionId { get; set; }
        public uint Index { get; set; }

        public BurnNftArg(string actionId, uint index)
        {
            this.ActionId = actionId;
            this.Index = index;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.BurnNftInfo(ActionId, Index);
        }
    }
    public class SpendTokensArg : BaseArg
    {
        public string ActionId { get; set; }
        public ulong Hash { get; set; }

        public SpendTokensArg(string actionId, ulong hash)
        {
            this.ActionId = actionId;
            this.Hash = hash;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.SpendTokensInfo(ActionId, Hash);
        }
    }
    public class DefaultArg : BaseArg
    {
        public string ActionId { get; set; }

        public DefaultArg(string actionId)
        {
            this.ActionId = actionId;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.DefaultInfo(ActionId);
        }
    }
    public class ClaimStakingRewardArg : BaseArg
    {
        public string ActionId { get; set; }

        public ClaimStakingRewardArg(string actionId)
        {
            this.ActionId = actionId;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.ClaimStakingRewardInfo(ActionId);
        }
    }
}

public static class TxUtil
{
    public async static UniTask<UResult<Response, string>> ProcessActionEntities<T>(T arg) where T : ActionArgValueTypes.BaseArg
    {
        Result_5 verifyTransResponse = null;
        switch (arg)
        {
            case ActionArgValueTypes.SpendTokensArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.SpendTokens, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.DefaultArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.Default, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.BurnNftArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.BurnNft, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.ClaimStakingRewardArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.ClaimStakingReward, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.BaseArg:
                Debug.LogError("You cannot process an action with an BaseArg");
                break;
        }

        if (verifyTransResponse == null) return new("verifyTransResponse is null");

        if (verifyTransResponse.Tag == Result_5Tag.Err) return new(verifyTransResponse.AsErr());

        var okVal = verifyTransResponse.AsOk();

        okVal.F1 ??= new();
        okVal.F2 ??= new();
        okVal.F3 ??= new();

        return new(okVal);
    }

    #region ICP
    private static Candid.IcpLedger.Models.TransferArgs SetupTransfer_IC(ulong amount, string toAddress)
    {
        List<byte> addressBytes = CandidUtil.HexStringToByteArray(toAddress).ToList();

        var transferArgs = new TransferArgs
        {
            To = addressBytes,
            Amount = new Candid.IcpLedger.Models.Tokens(amount),
            Fee = new Candid.IcpLedger.Models.Tokens(10000),
            CreatedAtTime = OptionalValue<TimeStamp>.NoValue(),
            Memo = new ulong(),
            FromSubaccount = new(),
        };

        return transferArgs;
    }
    public async static UniTask<UResult<ulong, string>> Transfer_ICP(ulong amount, string toAddress)
    {
        var arg = SetupTransfer_IC(amount, toAddress);
        var result = await CandidApiManager.Instance.IcpLedgerApiClient.Transfer(arg);

        if (result.Tag == Candid.IcpLedger.Models.TransferResultTag.Ok)
        {
            UserUtil.UpdateBalanceReq_Icp();
            return new UResult<ulong, string>(result.AsOk());
        }
        else return new UResult<ulong, string>($"Transfer error: {result.AsErr().Tag} : {result.AsErr().Value}");
    }
    #endregion


    #region ICRC
    private static Candid.Icrc1Ledger.Models.TransferArg SetupTransfer_RC(ulong amount, string toPrincipal)
    {
        return new Candid.Icrc1Ledger.Models.TransferArg(new(), new(Principal.FromText(toPrincipal), new()), amount, new(), new(), new());
    }
    public async static UniTask<UResult<ulong, string>> Transfer_RC(ulong amount, string toPrincipal)
    {
        Debug.Log($"Transfer To: {CandidApiManager.PaymentCanisterOfferIdentifier}");

        var arg = SetupTransfer_RC(amount, toPrincipal);

        var result = await CandidApiManager.Instance.rcLedgerApiClient.Icrc1Transfer(arg);

        if (result.Tag == Candid.Icrc1Ledger.Models.TransferResultTag.Ok)
        {
            UserUtil.UpdateBalanceReq_Rc();
            return new UResult<ulong, string>((ulong)result.AsOk());
        }
        else return new UResult<ulong, string>($"Transfer error: {result.AsErr().Tag} : {result.AsErr().Value}");
    }
    #endregion
}