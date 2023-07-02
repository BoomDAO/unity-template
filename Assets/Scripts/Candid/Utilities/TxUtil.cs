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
using ItsJackAnton.Utility;
using Candid.extv2_boom;
using ItsJackAnton.Patterns.Broadcasts;
using Newtonsoft.Json;
using Candid.IcpLedger;
using Candid.Icrc1Ledger;
using System.Security.Principal;

public static class ActionArgValueTypes
{
    public abstract class BaseArg
    {
        public string ActionId { get; set; }

        protected BaseArg(string actionId)
        {
            ActionId = actionId;
        }

        public abstract System.Object GetGeneratedValue();
    }
    public class DefaultArg : BaseArg
    {
        public DefaultArg(string actionId) : base(actionId) { }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.DefaultInfo(ActionId);
        }
    }

    public class BurnNftArg : BaseArg
    {
        public uint Index { get; set; }

        public BurnNftArg(string actionId, uint index) : base(actionId)
        {
            this.Index = index;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.BurnNftInfo(ActionId, Index);
        }
    }

    public class VerifyTransferIcp : BaseArg
    {
        public ulong BlockIndex { get; set; }

        public VerifyTransferIcp(string actionId, ulong blockIndex) : base(actionId)
        {
            this.BlockIndex = blockIndex;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.VerifyTransferIcpInfo(ActionId, BlockIndex);
        }
    }
    public class VerifyTransferIcrc : BaseArg
    {
        public ulong BlockIndex { get; set; }

        public VerifyTransferIcrc(string actionId, ulong blockIndex) : base(actionId)
        {
            this.BlockIndex = blockIndex;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.VerifyTransferIcrcInfo(ActionId, BlockIndex);
        }
    }

    public class ClaimStakingRewardNft : BaseArg
    {
        public ClaimStakingRewardNft(string actionId) : base(actionId) { }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.ClaimStakingRewardNftInfo(ActionId);
        }
    }
    public class ClaimStakingRewardIcp : BaseArg
    {
        public ClaimStakingRewardIcp(string actionId) : base(actionId) { }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.ClaimStakingRewardIcpInfo(ActionId);
        }
    }
    public class ClaimStakingRewardIcrc : BaseArg
    {
        public ClaimStakingRewardIcrc(string actionId) : base(actionId) { }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.ClaimStakingRewardIcrcInfo(ActionId);
        }
    }
}

public static class TxUtil
{
    public async static UniTask<UResult<ActionResponse, string>> ProcessAction<T>(T arg) where T : ActionArgValueTypes.BaseArg
    {
        $"Try Process Action of type ${typeof(T).Name}, ActionId: {arg.ActionId}".Log(nameof(TxUtil));

        Result_3 verifyTransResponse = null;
        switch (arg)
        {
            case ActionArgValueTypes.DefaultArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.Default, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.BurnNftArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.BurnNft, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.VerifyTransferIcp:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.VerifyTransferIcp, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.VerifyTransferIcrc:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.VerifyTransferIcrc, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.ClaimStakingRewardNft:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.ClaimStakingRewardNft, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.ClaimStakingRewardIcp:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.ClaimStakingRewardIcp, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.ClaimStakingRewardIcrc:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessActionEntities(new ActionArg(ActionArgTag.ClaimStakingRewardIcrc, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.BaseArg:
                Debug.LogError("You cannot process an action with an BaseArg");
                break;
        }

        if (verifyTransResponse == null) return new("verifyTransResponse is null");

        if (verifyTransResponse.Tag == Result_3Tag.Err) return new(verifyTransResponse.AsErr());

        var okVal = verifyTransResponse.AsOk();

        okVal.F1 ??= new();
        okVal.F2 ??= new();
        okVal.F3 ??= new();

        $"Action Processed Success, Type: ${typeof(ActionArgValueTypes.BaseArg)}, ActionId: {arg.ActionId}".Log(nameof(TxUtil));
        return new(okVal);
    }

    public static class Transfer
    {
        #region ICP
        private static Candid.IcpLedger.Models.TransferArgs SetupArgIcp(ulong amount, string toAddress)
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
        public async static UniTask<UResult<ulong, string>> TransferIcp(ulong amount, string toAddress)
        {
            Debug.Log($"Transfer To: {toAddress}");

            var arg = SetupArgIcp(amount, toAddress);
            var result = await CandidApiManager.Instance.IcpLedgerApiClient.Transfer(arg);

            if (result.Tag == Candid.IcpLedger.Models.TransferResultTag.Ok)
            {
                UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);
                return new UResult<ulong, string>(result.AsOk());
            }
            else return new UResult<ulong, string>($"Transfer error: {result.AsErr().Tag} : {result.AsErr().Value}");
        }
        #endregion

        #region ICRC
        private static Candid.Icrc1Ledger.Models.TransferArg SetupArgIcrc(ulong amount, string toPrincipal)
        {
            return new Candid.Icrc1Ledger.Models.TransferArg(new(), new(Principal.FromText(toPrincipal), new()), amount, new(), new(), new());
        }
        public async static UniTask<UResult<ulong, string>> TransferIcrc(ulong amount, string canisterId, string toPrincipal)
        {
            Debug.Log($"Transfer To: {toPrincipal}");

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                return new(getAgentResult.AsErr());
            }

            var icrcLedgerApiClient = new Icrc1LedgerApiClient(getAgentResult.AsOk(), Principal.FromText(canisterId));

            var arg = SetupArgIcrc(amount, toPrincipal);
            var result = await icrcLedgerApiClient.Icrc1Transfer(arg);

            if (result.Tag == Candid.Icrc1Ledger.Models.TransferResultTag.Ok)
            {
                UserUtil.RequestData<DataTypes.Token>(canisterId);
                return new UResult<ulong, string>((ulong)result.AsOk());
            }
            else return new UResult<ulong, string>($"Transfer error: {result.AsErr().Tag} : {result.AsErr().Value}");
        }
        #endregion
    }

    public static class Stake
    {
        public static async Task<UResult<Null, string>> StakeIcp(double amount)
        {
            var getLoginDataResult = UserUtil.GetSignInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                return new(getLoginDataResult.AsErr());
            }

            var loginData = getLoginDataResult.AsOk();

            var getIcpBalanceResult = UserUtil.GetDataElementOfType<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);

            if (getIcpBalanceResult.Tag == UResultTag.Err)
            {
                return new(getIcpBalanceResult.AsErr());
            }

            var icpBalance = getIcpBalanceResult.AsOk();


            var tokenizedAmount = CandidUtil.ConvertToBaseUnit(amount, CandidUtil.BASE_UNIT_ICP);

            if (icpBalance.tokenizedAmount < tokenizedAmount)
            {
                return new($"Not enough \"${icpBalance.canisterId}\" currency. Current balance: {icpBalance.Amount}, required balance: {amount}");
            }

            "Calling Transfer".Log($"{nameof(TxUtil.Transfer)}");

            var transferResult = await TxUtil.Transfer.TransferIcp(tokenizedAmount, CandidApiManager.PaymentCanisterStakeIdentifier);

            if (transferResult.Tag == UResultTag.Err)
            {
                return new(transferResult.AsErr());
            }

            $"Stake Success, msg: {transferResult.AsOk()}".Log($"{nameof(TxUtil.Transfer)}");

            var updateStakeResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateIcpStakes(transferResult.AsOk(), Env.CanisterIds.STAKING_HUB, loginData.principal, 0.005.ConvertToBaseUnit(CandidUtil.BASE_UNIT_ICP));

            if (updateStakeResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
            {
                return new($"Stake Update Failure {updateStakeResult.AsErr()}");
            }

            return new(new Null());
        }

        public static async Task<UResult<Null, string>> StakeIcrc(double amount, string canisterId)
        {
            var getLoginDataResult = UserUtil.GetSignInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                return new(getLoginDataResult.AsErr());
            }

            var loginData = getLoginDataResult.AsOk();

            var getIcpBalanceResult = UserUtil.GetDataElementOfType<DataTypes.Token>(canisterId);

            if (getIcpBalanceResult.Tag == UResultTag.Err)
            {
                return new(getIcpBalanceResult.AsErr());
            }

            var icpBalance = getIcpBalanceResult.AsOk();

            var tokenizedAmount = CandidUtil.ConvertToBaseUnit(amount, CandidUtil.BASE_UNIT_ICP);

            if (icpBalance.tokenizedAmount < tokenizedAmount)
            {
                return new($"Not enough \"${icpBalance.canisterId}\" currency. Current balance: {icpBalance.Amount}, required balance: {amount}");
            }


            "Calling Transfer".Log($"{nameof(TxUtil.Transfer)}");

            var transferResult = await TxUtil.Transfer.TransferIcrc(0.00001.TokenizeToCkBtc(), canisterId, Env.CanisterIds.STAKING_HUB);

            if (transferResult.Tag == UResultTag.Err)
            {
                return new(transferResult.AsErr());
            }

            $"Stake Success, msg: {transferResult.AsOk()}".Log($"{nameof(TxUtil.Transfer)}");

            var updateStakeResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateIcrcStakes(transferResult.AsOk(), Env.CanisterIds.STAKING_HUB, loginData.principal, 0.00001.TokenizeToCkBtc(), Env.CanisterIds.ICRC_LEDGER);

            if (updateStakeResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
            {
                return new($"Stake Update Failure {updateStakeResult.AsErr()}");

            }

            return new(new Null());
        }

        public static async Task<UResult<Null, string>> StakeNft(string collectionId, Predicate<string> predicate = null)
        {
            var getLoginDataResult = UserUtil.GetSignInData();

            if (getLoginDataResult.Tag == UResultTag.Err)
            {
                return new(getLoginDataResult.AsErr());
            }

            var loginData = getLoginDataResult.AsOk();

            var nextNftResult = NftUtil.TryGetNextNft(collectionId, predicate);

            if (nextNftResult.Tag == UResultTag.Err)
            {
                return new(nextNftResult.AsErr());
            }

            var nextNft = nextNftResult.AsOk();

            var getAgentResult = UserUtil.GetAgent();

            if (getAgentResult.Tag == UResultTag.Err)
            {
                return new(getAgentResult.AsErr());
            }

            Extv2BoomApiClient collectionInterface = new(getAgentResult.AsOk(), Principal.FromText(collectionId));

            Debug.Log($"Try stake nft of index: {nextNft.index} and tid: {nextNft.tokenIdentifier}. Transferring it aidFrom: {loginData.accountIdentifier}, aidTo: {CandidApiManager.PaymentCanisterStakeIdentifier}");

            var transferResult = await collectionInterface.Transfer(new Candid.extv2_boom.Models.TransferRequest(
                1,
                new(Candid.extv2_boom.Models.UserTag.Address, loginData.accountIdentifier),
                new(),
                false,
                new(),
                new(Candid.extv2_boom.Models.UserTag.Address, CandidApiManager.PaymentCanisterStakeIdentifier),
                $"{nextNft.tokenIdentifier}"
            ));

            if (transferResult.Tag == Candid.extv2_boom.Models.TransferResponseTag.Err)
            {
                return new($"Transfer of nft tid {nextNft.tokenIdentifier} failed");
            }

            var verificationResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateExtStakes((uint)nextNft.index, Env.CanisterIds.STAKING_HUB, loginData.principal, Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

            if (verificationResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
            {
                return new($"Verification transfer of nft tid {nextNft.tokenIdentifier} failed, msg: {verificationResult.AsErr()}");
            }

            Debug.Log($"NFT Stake Success. tid: {nextNft.tokenIdentifier}");

            return new(new Null());
        }
        //
        public static async Task<UResult<Null, string>> UnstakeIcp()
        {
            Debug.Log("ICP STAKE WITHDRAWAL");

            var result = await CandidApiManager.Instance.StakingHubApiClient.DissolveIcp();

            if (result.Tag == Candid.StakingHub.Models.ResultTag.Err)
            {
                return new("ICP Withdrawal Failure result, msg: " + result.AsErr());
            }

            return new(new Null());
        }

        public static async Task<UResult<Null, string>> UnstakeIcrc(string canisterId)
        {
            Debug.Log("ICRC STAKE WITHDRAWAL");

            var result = await CandidApiManager.Instance.StakingHubApiClient.DissolveIcrc(canisterId);

            if (result.Tag == Candid.StakingHub.Models.ResultTag.Err)
            {
                return new("ICRC Withdrawal Failure result, msg: " + result.AsErr());
            }

            return new(new Null());
        }

        public static async Task<UResult<Null, string>> UnstakeNft(string collectionId, uint index)
        {
            var withdrawalResult = await CandidApiManager.Instance.StakingHubApiClient.DissolveExt(collectionId, index);

            if (withdrawalResult.Tag == Candid.StakingHub.Models.ResultTag.Err)
            {
                return new($"NFT Dissolve Failure. msg: " + withdrawalResult.AsErr());
            }

            Debug.Log($"NFT Dissolve Success.");

            return new(new Null());
        }
    }
    public static class Action
    {
        public static async Task<UResult<ActionResponse, string>> Default(string actionId)
        {
            var actionConfigResonse = UserUtil.GetDataElementOfType<DataTypes.ActionConfig>(actionId);

            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(actionConfigResonse.AsErr());
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.ActionPlugin.HasValue)
            {
                return new($"ActionId: {actionId} is not a Default type cuz its ActionPlugin has value");
            }

            var actionResult = await TxUtil.ProcessAction(new ActionArgValueTypes.DefaultArg(actionId));

            if (actionResult.Tag == UResultTag.Err)
            {
                return new("ActionResult Failure, msg: " + actionResult.AsErr());
            }

            return new(actionResult.AsOk());
        }
        public static async Task<UResult<ActionResponse, string>> BurnNft(string actionId, string collectionId)
        {
            var actionConfigResonse = UserUtil.GetDataElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(actionConfigResonse.AsErr());
            }

            var actionConfig = actionConfigResonse.AsOk();
            var actionPlugin = actionConfig.ActionPlugin.ValueOrDefault;

            if (actionPlugin != null)
            {
                if (actionPlugin.Tag != ActionPluginTag.BurnNft)
                {
                    return new($"id {actionId} is not of type {ActionPluginTag.BurnNft}. Current type: {actionPlugin.Tag}");
                }
            }

            var getNextIndexResult = NftUtil.TryGetNextNftIndex(collectionId);// Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

            if (getNextIndexResult.Tag == UResultTag.Err)
            {
                return new($"Could not find next nft to burn cuz u might not have any of the selected collection {Env.Nfts.BOOM_COLLECTION_CANISTER_ID}");
            }

            var actionResult = await TxUtil.ProcessAction(new ActionArgValueTypes.BurnNftArg(actionId, (uint)getNextIndexResult.AsOk()));

            if (actionResult.Tag == UResultTag.Err)
            {
                return new("Burn Failure, msg: " + actionResult.AsErr());
            }

            return new(actionResult.AsOk());
        }

        public static async Task<UResult<ActionResponse, string>> TransferAndVerifyIcp(string actionId)
        {
            var actionConfigResonse = UserUtil.GetDataElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(actionConfigResonse.AsErr());
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.ActionPlugin.HasValue == false)
            {
                return new($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. Action Plugin is Null");
            }

            var actionPlugin = actionConfig.ActionPlugin.ValueOrDefault;

            if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcp)
            {
                return new($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. Current type: {actionPlugin.Tag}");
            }

            var config = actionPlugin.AsVerifyTransferIcp();
            double amt = config.Amt;
            var tokenizedAmt = CandidUtil.ConvertToBaseUnit(amt, 100_000_000);

            UResult<ulong, string> transferResult = await TxUtil.Transfer.TransferIcp(tokenizedAmt, CandidApiManager.PaymentCanisterOfferIdentifier);

            if (transferResult.Tag != UResultTag.Ok)
            {
                return new(transferResult.AsErr());
            }

            var blockIndex = transferResult.AsOk();

            var actionResult = await TxUtil.ProcessAction(new ActionArgValueTypes.VerifyTransferIcp(actionId, blockIndex));

            if (actionResult.Tag != UResultTag.Ok)
            {
                return new(actionResult.AsErr());
            }



            return new(actionResult.AsOk());
        }
        public static async Task<UResult<ActionResponse, string>> TransferAndVerifyIcrc(string actionId, string canisterId)
        {
            var actionConfigResonse = UserUtil.GetDataElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(actionConfigResonse.AsErr());
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.ActionPlugin.HasValue == false)
            {
                return new($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. Action Plugin is Null");
            }

            var actionPlugin = actionConfig.ActionPlugin.ValueOrDefault;

            if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcrc)
            {
                return new($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcrc}. Current type: {actionPlugin.Tag}");
            }

            var config = actionPlugin.AsVerifyTransferIcrc();

            if(!config.BaseUnitCount.TryToUInt64(out var baseUnit))
            {
                return new($"Action Config of Id: {actionId} of type: {ActionPluginTag.VerifyTransferIcrc} doesn't have a baseUnit value");
            }

            double amt = config.Amt;
            var tokenizedAmt = CandidUtil.ConvertToBaseUnit(amt, baseUnit);

            UResult<ulong, string> transferResult = await TxUtil.Transfer.TransferIcrc(tokenizedAmt, canisterId, Env.CanisterIds.PAYMENT_HUB);

            if (transferResult.Tag != UResultTag.Ok)
            {
                return new(transferResult.AsErr());
            }

            var blockIndex = transferResult.AsOk();

            var actionResult = await TxUtil.ProcessAction(new ActionArgValueTypes.VerifyTransferIcrc(actionId, blockIndex));

            if (actionResult.Tag != UResultTag.Ok)
            {
                return new(actionResult.AsErr());
            }

            return new(actionResult.AsOk());
        }

        public static async Task<UResult<ActionResponse, string>> ClaimStakeRewardIcp(string actionId)
        {
            var actionResult = await TxUtil.ProcessAction(new ActionArgValueTypes.ClaimStakingRewardIcp(actionId));

            if (actionResult.Tag == UResultTag.Err)
            {
                return new("ICP StakeClaim Failure " + actionResult.AsErr());
            }

            return new(actionResult.AsOk());
        }
        public static async Task<UResult<ActionResponse, string>> ClaimStakeRewardIcrc(string actionId)
        {
            var actionResult = await TxUtil.ProcessAction(new ActionArgValueTypes.ClaimStakingRewardIcrc(actionId));

            if (actionResult.Tag == UResultTag.Err)
            {
                return new("ICRC StakeClaim Failure " + actionResult.AsErr());
            }

            return new(actionResult.AsOk());
        }
        public static async Task<UResult<ActionResponse, string>> ClaimStakeRewardNft(string actionId)
        {
            var actionResult = await TxUtil.ProcessAction(new ActionArgValueTypes.ClaimStakingRewardNft(actionId));

            if (actionResult.Tag == UResultTag.Err)
            {
                return new("NFT StakeClaim Failure " + actionResult.AsErr());
            }

            return new(actionResult.AsOk());
        }
    }
}