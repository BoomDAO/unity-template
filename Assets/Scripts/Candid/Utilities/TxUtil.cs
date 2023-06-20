using Candid;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ItsJackAnton.Values;
using System.Collections.Generic;
using Candid.World.Models;
using System.Threading.Tasks;

public static class ActionArgValueTypes
{
    public abstract class BaseArg { };
    public class BurnNftArg : BaseArg
    {
        public string ActionId { get; set; }
        public string Aid { get; set; }
        public int Index { get; set; }

        public BurnNftArg(string actionId, int index, string aid)
        {
            this.ActionId = actionId;
            this.Index = index;
            this.Aid = aid;
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
    }
    public class SpendEntitiesArg : BaseArg
    {
        public string ActionId { get; set; }

        public SpendEntitiesArg(string actionId)
        {
            this.ActionId = actionId;
        }
    }
    public class ClaimStakingRewardArg : BaseArg
    {
        public string ActionId { get; set; }

        public ClaimStakingRewardArg(string actionId)
        {
            this.ActionId = actionId;
        }
    }
}

public static class TxUtil
{
    public async static UniTask<UResult<List<Entity>, string>> ProcessPlayerAction<T>(T arg) where T : ActionArgValueTypes.BaseArg
    {
        Result_5 verifyTransResponse = null;
        switch (arg)
        {
            case ActionArgValueTypes.SpendTokensArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessPlayerAction(new ActionArg(ActionArgTag.SpendTokens, arg));
                break;
            case ActionArgValueTypes.SpendEntitiesArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessPlayerAction(new ActionArg(ActionArgTag.SpendEntities, arg));
                break;
            case ActionArgValueTypes.BurnNftArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessPlayerAction(new ActionArg(ActionArgTag.BurnNft, arg));
                break;
            case ActionArgValueTypes.ClaimStakingRewardArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessPlayerAction(new ActionArg(ActionArgTag.ClaimStakingReward, arg));
                break;
            case ActionArgValueTypes.BaseArg:
                Debug.LogError("You cannot process an action with an BaseArg");
                break;
        }
        var result = new UResult<List<Entity>, string>();

        if (verifyTransResponse == null) return result.Err("verifyTransResponse is null");

        if (verifyTransResponse.Tag == Result_5Tag.Err) return result.Err(verifyTransResponse.AsErr());

        var okVal = verifyTransResponse.AsOk();
        return result.Ok(okVal);
    }

    #region ICP
    public async static UniTask<UResult<ulong, string>> Transfer_ICP(Candid.IcpLedger.Models.TransferArgs arg)
    {
        var result = await CandidApiManager.Instance.IcpLedgerApiClient.Transfer(arg);

        if (result.Tag == Candid.IcpLedger.Models.TransferResultTag.Ok)
        {
            UserUtil.UpdateBalanceReq_Icp();
            return new UResult<ulong, string>(result.AsOk());
        }
        else return new UResult<ulong, string>($"Transfer error: {result.AsErr().Tag} : {result.AsErr().Value}");
    }

    public async static UniTask<UResult<ulong, string>> TransferToStakeCanister_ICP(Candid.IcpLedger.Models.TransferArgs arg)
    {
        Debug.Log($"Transfer To: {CandidApiManager.PaymentCanisterStakeIdentifier}");

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
    public async static UniTask<UResult<ulong, string>> Transfer_RC(Candid.Icrc1Ledger.Models.TransferArg arg)
    {
        Debug.Log($"Transfer To: {CandidApiManager.PaymentCanisterOfferIdentifier}");

        var result = await CandidApiManager.Instance.rcLedgerApiClient.Icrc1Transfer(arg);

        if (result.Tag == Candid.Icrc1Ledger.Models.TransferResultTag.Ok)
        {
            UserUtil.UpdateBalanceReq_Rc();
            return new UResult<ulong, string>((ulong)result.AsOk());
        }
        else return new UResult<ulong, string>($"Transfer error: {result.AsErr().Tag} : {result.AsErr().Value}");
    }

    public async static UniTask<UResult<ulong, string>> TransferToStakeCanister_RC(Candid.Icrc1Ledger.Models.TransferArg arg)
    {
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