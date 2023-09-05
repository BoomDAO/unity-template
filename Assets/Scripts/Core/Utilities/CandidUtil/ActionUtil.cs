using Candid;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Boom.Values;
using System.Collections.Generic;
using Candid.World.Models;
using Candid.IcpLedger.Models;
using EdjCase.ICP.Candid.Models;
using System.Linq;
using Boom.Utility;
using Candid.Extv2Boom;
using Candid.IcrcLedger;
using Candid.IcpLedger;
using WebSocketSharp;
using System;

//TRANSFER ERROR TYPES
public static class TransferErrType
{
    public abstract class Base
    {
        public string content;
        public string Content { get { return $"{GetType()}: {content}"; } }

        protected Base(string content)
        {
            this.content = content;
        }
    }
    public class LogIn : Base
    {
        public LogIn(string msg) : base(msg)
        {

        }
    }
    public class InsufficientBalance : Base
    {
        public InsufficientBalance(string msg) : base(msg)
        {

        }
    }
    public class Transfer : Base
    {
        public Transfer(string msg) : base(msg)
        {

        }
    }
    public class Other : Base
    {
        public Other(string msg) : base(msg)
        {

        }
    }
}
//STAKE ERROR TYPES
public static class StakeErrType
{
    public abstract class Base
    {
        public string content;
        public string Content { get { return $"{GetType()}: {content}"; } }

        protected Base(string content)
        {
            this.content = content;
        }
    }
    public class LogIn : Base
    {
        public LogIn(string msg) : base(msg)
        {

        }
    }
    public class InsufficientBalance : Base
    {
        public InsufficientBalance(string msg) : base(msg)
        {

        }
    }
    public class Transfer : Base
    {
        public Transfer(string msg) : base(msg)
        {

        }
    }
    public class UpdateStake : Base
    {
        public UpdateStake(string msg) : base(msg)
        {

        }
    }
    public class Other : Base
    {
        public Other(string msg) : base(msg)
        {

        }
    }
}
//UNSTAKE ERROR TYPES
public static class UnstakeErrType
{
    public abstract class Base
    {
        private string content;
        public string Content { get { return $"{GetType()}: {content}"; } }

        protected Base(string content)
        {
            this.content = content;
        }
    }
    public class LogIn : Base
    {
        public LogIn(string msg) : base(msg)
        {

        }
    }

    public class InsufficientBalance : Base
    {
        public InsufficientBalance(string msg) : base(msg)
        {

        }
    }

    public class Other : Base
    {
        public Other(string msg) : base(msg)
        {

        }
    }
}
//ACTION ERROR TYPES
public static class ActionErrType
{
    public abstract class Base
    {
        public string content;
        public string Content { get { return $"{GetType()}: {content}"; } }

        protected Base(string content)
        {
            this.content = content;
        }
    }

    public class LogIn : Base
    {
        public LogIn(string msg) : base(msg)
        {

        }
    }

    //ACTION CONSTRAINS
    public class ActionExecutionFailure : Base
    {
        public ActionExecutionFailure(string msg) : base(msg)
        {
        }
    }
    public class ActionsPerInterval : Base
    {
        public ActionsPerInterval(string msg) : base(msg)
        {
        }
    }
    public class EntityConstrain : Base
    {
        public EntityConstrain(string msg) : base(msg)
        {
        }
    }
    public class WrongActionType : Base
    {
        public WrongActionType(string content) : base(content)
        {
        }
    }

    //BALANCE
    public class InsufficientBalance : Base
    {
        public InsufficientBalance(string msg) : base(msg)
        {

        }
    }

    //TRANSFER
    public class Transfer : Base
    {
        public Transfer(string msg) : base(msg)
        {

        }
    }

    //OTHER
    public class Other : Base
    {
        public Other(string msg) : base(msg)
        {
        }
    }
}
//ACTION ARGUMENTS
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

    public class VerifyBurnNftsArg : BaseArg
    {
        public List<uint> Indexes { get; set; }

        public VerifyBurnNftsArg(string actionId, List<uint> indexes) : base(actionId)
        {
            this.Indexes = indexes;
        }

        public override System.Object GetGeneratedValue()
        {
            return new ActionArg.VerifyBurnNftsInfo(ActionId, Indexes);
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

//PROCESS ACTION RESULT
public class ProcessedActionResponse
{
    public List<MintNft> nfts;
    public List<MintToken> tokens;
    public List<DataTypes.Entity> receivedEntities;
    public List<DataTypes.Entity> spentEntities;
    public List<DataTypes.Entity> reducedExpiration;
    public List<DataTypes.Entity> renewedExpiration;
    public List<DataTypes.Entity> setAttribute;
    public List<DataTypes.Entity> deletedEntities;

    public ProcessedActionResponse(List<MintNft> nfts, List<MintToken> tokens, List<DataTypes.Entity> receivedEntities, List<DataTypes.Entity> spentEntities, List<DataTypes.Entity> reducedExpiration, List<DataTypes.Entity> renewedExpiration, List<DataTypes.Entity> setAttribute, List<DataTypes.Entity> deletedEntities)
    {
        this.nfts = nfts;
        this.tokens = tokens;
        this.receivedEntities = receivedEntities;
        this.spentEntities = spentEntities;
        this.reducedExpiration = reducedExpiration;
        this.renewedExpiration = renewedExpiration;
        this.setAttribute = setAttribute;
        this.deletedEntities = deletedEntities;
    }
}
//UTILS
public static class ActionUtil
{
    #region Get Action Details
    public static bool HasPluginType(this DataTypes.ActionConfig actionConfig, ActionPluginTag type)
    {
        if (actionConfig.actionPlugin == null) return false;

        return actionConfig.actionPlugin.Tag == type;
    }
    #endregion

    //GENERIC CHECK BEFORE PROCESSING AN ACTION
    private static UResult<Null, ActionErrType.Base> ValidateActionConfig(string actionId, out (ulong actionCount, ulong intervalStartTs) newActionData)
    {
        newActionData = default;
        //Check Login State
        var getLoginTypeResult = UserUtil.GetLogInType();

        if (getLoginTypeResult.Tag == UResultTag.Err)
        {
            return new(new ActionErrType.LogIn(getLoginTypeResult.AsErr()));
        }

        if(getLoginTypeResult.AsOk() == UserUtil.LoginType.Anon)
        {
            return new(new ActionErrType.LogIn("You cannot execute this function as anon"));
        }

        //If action exist
        var intervalStartTs = UserUtil.GetPropertyFromType<DataTypes.Action, ulong>(actionId, e => e.intervalStartTs, 0);
        var actionCount = UserUtil.GetPropertyFromType<DataTypes.Action, ulong>(actionId, e => e.actionCount, 0);

        ////

        //Check for config

        var isActionConfigsValid = UserUtil.IsDataValid<DataTypes.ActionConfig>(); //TODO: REMOVE

        if (!isActionConfigsValid) return new(new ActionErrType.Other("Action Config has not been loaded yet")); //TODO: REMOVE

        var actionConfigResult = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);

        if (actionConfigResult.IsErr) return new(new ActionErrType.Other(actionConfigResult.AsErr()));

        var actionConfig = actionConfigResult.AsOk();

        if (actionConfig.timeConstraint != null)
        {
            var timeConstrain = actionConfig.timeConstraint;

            if (timeConstrain.ActionsPerInterval.TryToUInt64(out var actionsPerIntervalConfig) == false) return new(new ActionErrType.Other("Converting \"timeConstrain.ActionsPerInterval\" to UInt64 failed"));

            if (actionsPerIntervalConfig == 0) return new(new ActionErrType.Other("The actionsPerInterval of this action is currently set to 0, which effectively disables it."));

            if (timeConstrain.IntervalDuration.TryToUInt64(out var intervalDurationConfig) == false) return new(new ActionErrType.Other("Converting \"timeConstrain.IntervalDuration\" to UInt64 failed"));

            var timeConstrainToCompareWith = intervalStartTs.NanoToMilliseconds() + intervalDurationConfig.NanoToMilliseconds();
            if (timeConstrainToCompareWith < MainUtil.Now())
            {
                actionCount = 1;
                intervalStartTs = (ulong)(MainUtil.Now() + 5f.SecondsToMilli()).MilliToNano();
            }
            else if (actionCount < actionsPerIntervalConfig)
            {
                ++actionCount;
            }
            else
            {
                var secondsLeft = (timeConstrainToCompareWith - MainUtil.Now()).MilliToSeconds();
                return new(new ActionErrType.ActionsPerInterval($"You have reached the max ({actionsPerIntervalConfig}) tries for this time interval. You have tried already {actionCount} times.\nYou must wait: {(secondsLeft > 0 ? secondsLeft : 1)} secs"));
            }

            newActionData = (actionCount, intervalStartTs);
        }

        if (actionConfig.entityConstraints != null)
        {
            var entityConstrain = actionConfig.entityConstraints;

            var checkForRequiremensResult = EntityUtil.MeetEntityRequirements(entityConstrain.ToArray());

            if (checkForRequiremensResult.IsErr) return new(new ActionErrType.Other(checkForRequiremensResult.AsErr()));

            if (checkForRequiremensResult.AsOk() == false) return new(new ActionErrType.EntityConstrain("You don't meet the entity requirement"));
        }

        return new(new Null());//SUCCESS
    }

    private static void UpdateActionData(string actionId, ulong intervalStartTs, ulong actionCount)
    {
        UserUtil.UpdateData(new DataTypes.Action(
            actionId,
            actionCount,
            intervalStartTs
        ));
    }

    //MAIN FUNCTION TO PROCESS AN ACTION
    private async static UniTask<UResult<ProcessedActionResponse, string>> ProcessAction<T>(T arg) where T : ActionArgValueTypes.BaseArg
    {
        $"Try Process Action of type ${typeof(T).Name}, ActionId: {arg.ActionId}".Log(nameof(ActionUtil));

        //Execute Action
        Result_4 verifyTransResponse = null;
        switch (arg)
        {
            case ActionArgValueTypes.DefaultArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(ActionArgTag.Default, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.VerifyBurnNftsArg:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(ActionArgTag.VerifyBurnNfts, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.VerifyTransferIcp:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(ActionArgTag.VerifyTransferIcp, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.VerifyTransferIcrc:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(ActionArgTag.VerifyTransferIcrc, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.ClaimStakingRewardNft:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(ActionArgTag.ClaimStakingRewardNft, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.ClaimStakingRewardIcp:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(ActionArgTag.ClaimStakingRewardIcp, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.ClaimStakingRewardIcrc:
                verifyTransResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(ActionArgTag.ClaimStakingRewardIcrc, arg.GetGeneratedValue()));
                break;
            case ActionArgValueTypes.BaseArg:
                Debug.LogError("You cannot process an action with an BaseArg");
                break;
        }

        if (verifyTransResponse == null) return new("verifyTransResponse is null");

        if (verifyTransResponse.Tag == Result_4Tag.Err)
        {
            return new(verifyTransResponse.AsErr());
        }

        var okVal = verifyTransResponse.AsOk();

        List<MintNft> nfts = new();
        List<MintToken> tokens = new();
        List<DataTypes.Entity> receivedEntities = new();
        List<DataTypes.Entity> spentEntities = new();
        List<DataTypes.Entity> reducedExpiration = new();
        List<DataTypes.Entity> renewedExpiration = new();
        List<DataTypes.Entity> setAttribute = new();
        List<DataTypes.Entity> deletedEntities = new();

        okVal.Iterate(e =>
        {
            switch (e.Option.Tag)
            {
                case ActionOutcomeOption.OptionInfoTag.MintToken:
                    //Debug.Log($"RESULT: MINT TOKENs: {JsonConvert.SerializeObject(e.ConvertToDataType())}");
                    tokens.Add(e.Option.AsMintToken());
                    break;
                case ActionOutcomeOption.OptionInfoTag.MintNft:
                    //Debug.Log($"RESULT: MINT NFTs: {JsonConvert.SerializeObject(e.ConvertToDataType())}");
                    nfts.Add(e.Option.AsMintNft());
                    break;
                case ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity:
                    //Debug.Log($"RESULT: ReceiveEntityQuantity: {JsonConvert.SerializeObject(e.ConvertToDataType())}");
                    receivedEntities.Add(e.ConvertToDataType());
                    break;
                case ActionOutcomeOption.OptionInfoTag.SpendEntityQuantity:
                    //Debug.Log($"RESULT: SpendEntityQuantity: {JsonConvert.SerializeObject(e.ConvertToDataType())}");
                    var asSpendEntityQuantity = e.Option.AsSpendEntityQuantity();
                    spentEntities.Add(e.ConvertToDataType());
                    break;
                case ActionOutcomeOption.OptionInfoTag.ReduceEntityExpiration:
                    //Debug.Log($"RESULT: ReduceEntityExpiration: {JsonConvert.SerializeObject(e.ConvertToDataType())}");
                    reducedExpiration.Add(e.ConvertToDataType());
                    break;
                case ActionOutcomeOption.OptionInfoTag.RenewEntityExpiration:
                    renewedExpiration.Add(e.ConvertToDataType());
                    break;
                case ActionOutcomeOption.OptionInfoTag.SetEntityAttribute:
                    //Debug.Log($"RESULT: SetEntityAttribute: {JsonConvert.SerializeObject(e.ConvertToDataType())}");
                    setAttribute.Add(e.ConvertToDataType());
                    break;
                case ActionOutcomeOption.OptionInfoTag.DeleteEntity:
                    //Debug.Log($"RESULT: DeleteEntity: {JsonConvert.SerializeObject(e.ConvertToDataType())}");
                    deletedEntities.Add(e.ConvertToDataType());
                    break;
            }
        });

        var processActionResponse = new ProcessedActionResponse(nfts, tokens, receivedEntities, spentEntities, reducedExpiration, renewedExpiration, setAttribute, deletedEntities);

        $"Action Processed Success, Type: ${typeof(ActionArgValueTypes.BaseArg)}, ActionId: {arg.ActionId}".Log(nameof(ActionUtil));

        return new(processActionResponse);
    }

    public static class Transfer
    {
        //ICP
        public async static UniTask<UResult<ulong, TransferErrType.Base>> TransferIcp(double amount, string toAddress, bool updateBalance = true)
        {
            //CHECK LOGIN
            var getLoginTypeResult = UserUtil.GetLogInType();

            if (getLoginTypeResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.LogIn(getLoginTypeResult.AsErr()));
            }

            if (getLoginTypeResult.AsOk() == UserUtil.LoginType.Anon)
            {
                return new(new TransferErrType.LogIn("You cannot execute this function as anon"));
            }

            //CHECK USER BALANCE

            var tokenAndConfigsResult = UserUtil.GetTokenAndConfigs(Env.CanisterIds.ICP_LEDGER);

            if (tokenAndConfigsResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.Other(tokenAndConfigsResult.AsErr()));
            }

            var (token, configs) = tokenAndConfigsResult.AsOk();

            var requiredBaseUnitAmount = CandidUtil.ConvertToBaseUnit(amount, configs.decimals);

            if (token.baseUnitAmount < requiredBaseUnitAmount)
            {
                return new(new TransferErrType.InsufficientBalance($"Not enough \"${Env.CanisterIds.ICP_LEDGER}\" currency. Current balance: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}, required balance: {amount}"));
            }

            //SETUP INTERFACE
            var tokenInterface = new IcpLedgerApiClient(UserUtil.GetAgent().AsOk(), Principal.FromText(Env.CanisterIds.ICP_LEDGER));

            //SETUP ARGS
            List<byte> addressBytes = CandidUtil.HexStringToByteArray(toAddress).ToList();
            var arg = new TransferArgs
            {
                To = addressBytes,
                Amount = new Candid.IcpLedger.Models.Tokens(requiredBaseUnitAmount),
                Fee = new Candid.IcpLedger.Models.Tokens(configs.fee),
                CreatedAtTime = OptionalValue<TimeStamp>.NoValue(),
                Memo = new ulong(),
                FromSubaccount = new(),
            };

            //TRANSFER
            $"Transfer to address: {toAddress},\n amount {amount},\n baseUnitAmount: {requiredBaseUnitAmount},\n decimals: {configs.decimals},\n fee: {configs.fee}".Log(nameof(ActionUtil));
            var result = await tokenInterface.Transfer(arg);

            //CHECK SUCCESS
            if (result.Tag == Candid.IcpLedger.Models.TransferResultTag.Ok)
            {
                if (updateBalance) TokenUtil.DecrementTokenByBaseUnit(Env.CanisterIds.ICP_LEDGER, requiredBaseUnitAmount + configs.fee); //UserUtil.RequestData<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);

                var blockIndex = result.AsOk();
                $"BlockIndex Transfer: {blockIndex}".Log();
                return new(blockIndex);
            }
            else return new(new TransferErrType.Transfer($"{result.AsErr().Tag}: {result.AsErr().Value}"));
        }
        //ICRC
        public async static UniTask<UResult<ulong, TransferErrType.Base>> TransferIcrc(double amount, string canisterId, string toPrincipal, bool updateBalance = true)
        {
            //CHECK LOGIN
            if (canisterId == Env.CanisterIds.ICP_LEDGER) return new(new TransferErrType.Other($"You cannot use this function to transfer ICP, try using \"{nameof(TransferIcp)}\""));

            var getLoginTypeResult = UserUtil.GetLogInType();

            if (getLoginTypeResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.LogIn(getLoginTypeResult.AsErr()));
            }

            if (getLoginTypeResult.AsOk() == UserUtil.LoginType.Anon)
            {
                return new(new TransferErrType.LogIn("You cannot execute this function as anon"));
            }

            //CHECK USER BALANCE
            var tokenAndConfigsResult = UserUtil.GetTokenAndConfigs(canisterId);

            if (tokenAndConfigsResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.Other(tokenAndConfigsResult.AsErr()));
            }

            var (token, configs) = tokenAndConfigsResult.AsOk();

            var requiredBaseUnitAmount = CandidUtil.ConvertToBaseUnit(amount, configs.decimals);

            if (token.baseUnitAmount < requiredBaseUnitAmount)
            {
                return new(new TransferErrType.InsufficientBalance($"Not enough \"${canisterId}\" currency. Current balance: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}, required balance: {amount}"));
            }

            //SETUP INTERFACE
            var tokenInterface = new IcrcLedgerApiClient(UserUtil.GetAgent().AsOk(), Principal.FromText(canisterId));

            //SETUP ARGS
            var arg = new Candid.IcrcLedger.Models.TransferArgs(
                (UnboundedUInt)requiredBaseUnitAmount,
                new(),
                new((UnboundedUInt)configs.fee),
                new(),
                new(),
                new(Principal.FromText(toPrincipal),
                new()));

            //TRANSFER
            $"Transfer to principal: {toPrincipal},\n amount {amount},\n baseUnitAmount: {requiredBaseUnitAmount},\n decimals: {configs.decimals},\n fee: {configs.fee}".Log(nameof(ActionUtil));
            var result = await tokenInterface.Icrc1Transfer(arg);

            //CHECK SUCCESS
            if (result.Tag == Candid.IcrcLedger.Models.TransferResultTag.Ok)
            {
                if (updateBalance) TokenUtil.DecrementTokenByBaseUnit(canisterId, requiredBaseUnitAmount + configs.fee);// UserUtil.RequestData<DataTypes.Token>(canisterId);

                var blockIndex = (ulong)result.AsOk();
                $"BlockIndex Transfer: {blockIndex}".Log(nameof(ActionUtil));
                return new(blockIndex);
            }
            else return new(new TransferErrType.Transfer($"{result.AsErr().Tag}: {result.AsErr().Value}"));
        }
    }
    public static class Stake
    {
        //ICP & ICRC
        public static async UniTask<UResult<Null, StakeErrType.Base>> StakeToken(double amount, string canisterId = "")
        {
            if (string.IsNullOrEmpty(canisterId)) canisterId = Env.CanisterIds.ICP_LEDGER;
            Debug.Log($"STAKE {canisterId}");

            //IF ICP
            if (canisterId == Env.CanisterIds.ICP_LEDGER)
            {
                //TRANSFER TO STAKING HUB
                var transferResult = await ActionUtil.Transfer.TransferIcp(amount, CandidApiManager.StakingHubIdentifier);

                if (transferResult.Tag == UResultTag.Err)
                {
                    switch (transferResult.AsErr())
                    {
                        case TransferErrType.LogIn errType:
                            return new(new StakeErrType.LogIn(errType.content));
                        case TransferErrType.InsufficientBalance errType:
                            return new(new StakeErrType.InsufficientBalance(errType.content));
                        case TransferErrType.Transfer errType:
                            return new(new StakeErrType.Transfer(errType.content));
                        case TransferErrType.Other errType:
                            return new(new StakeErrType.Other(errType.content));
                    }
                }

                //SETUP BALANCE REQUIREMENT
                var tokenConfigResult = UserUtil.GetElementOfType<DataTypes.TokenConfig>(canisterId);
                if (tokenConfigResult.IsErr) return new(new StakeErrType.Other(tokenConfigResult.AsErr()));
                var requiredBaseUnitAmount = amount.ConvertToBaseUnit(tokenConfigResult.AsOk().decimals);

                //UPDATE STAKE
                var updateStakeResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateIcpStakes(transferResult.AsOk(), Env.CanisterIds.STAKING_HUB, UserUtil.GetPrincipal().AsOk().Value, requiredBaseUnitAmount);

                //CHECK FOR STAKE UPDATE ERROR
                if (updateStakeResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
                {
                    return new(new StakeErrType.UpdateStake($"Stake Update Failure {updateStakeResult.AsErr()}"));
                }
            }
            //IF ICRC
            else
            {
                //TRANSFER TO STAKING HUB
                var transferResult = await ActionUtil.Transfer.TransferIcrc(amount, canisterId, Env.CanisterIds.STAKING_HUB);

                if (transferResult.Tag == UResultTag.Err)
                {
                    switch (transferResult.AsErr())
                    {
                        case TransferErrType.LogIn errType:
                            return new(new StakeErrType.LogIn(errType.content));
                        case TransferErrType.InsufficientBalance errType:
                            return new(new StakeErrType.InsufficientBalance(errType.content));
                        case TransferErrType.Transfer errType:
                            return new(new StakeErrType.Transfer(errType.content));
                        case TransferErrType.Other errType:
                            return new(new StakeErrType.Other(errType.content));
                    }
                }

                //SETUP BALANCE REQUIREMENT
                var tokenConfigResult = UserUtil.GetElementOfType<DataTypes.TokenConfig>(canisterId);
                if (tokenConfigResult.IsErr) return new(new StakeErrType.Other(tokenConfigResult.AsErr()));
                var requiredBaseUnitAmount = amount.ConvertToBaseUnit(tokenConfigResult.AsOk().decimals);

                //UPDATE STAKE
                var updateStakeResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateIcrcStakes(transferResult.AsOk(), Env.CanisterIds.STAKING_HUB, UserUtil.GetPrincipal().AsOk().Value, requiredBaseUnitAmount, canisterId);

                //CHECK FOR STAKE UPDATE ERROR
                if (updateStakeResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
                {
                    return new(new StakeErrType.UpdateStake($"Stake Update Failure {updateStakeResult.AsErr()}"));
                }
            }

            $"Stake Success".Log($"{nameof(ActionUtil.Transfer)}");
            return new(new Null());
        }
        public static async UniTask<UResult<Null, UnstakeErrType.Base>> UnstakeToken(string canisterId = "")
        {
            if (string.IsNullOrEmpty(canisterId)) canisterId = Env.CanisterIds.ICP_LEDGER;

            //CHECK LOG IN
            var getLoginTypeResult = UserUtil.GetLogInType();

            if (getLoginTypeResult.Tag == UResultTag.Err)
            {
                return new(new UnstakeErrType.LogIn(getLoginTypeResult.AsErr()));
            }

            if (getLoginTypeResult.AsOk() == UserUtil.LoginType.Anon)
            {
                return new(new UnstakeErrType.LogIn("You cannot execute this function as anon"));
            }

            //CHECK STAKED BALANCE
            var getStakesResult = UserUtil.GetElementsOfType<DataTypes.Stake>();

            if (getStakesResult.Tag == UResultTag.Err)
                return new(new UnstakeErrType.Other(getStakesResult.AsErr()));

            var stakes = getStakesResult.AsOk();

            var stake = stakes.Locate(e =>
            {
                return e.canisterId == canisterId;
            });

            long stakeBaseUnitBalance = stake == null ? 0 : stake.amount;

            if (stakeBaseUnitBalance == 0)
                return new(new UnstakeErrType.InsufficientBalance($"You don't have enough staked ICRC from canister: {canisterId}"));

            if(canisterId == Env.CanisterIds.ICP_LEDGER)
            {

                var result = await CandidApiManager.Instance.StakingHubApiClient.DissolveIcp();

                if (result.Tag == Candid.StakingHub.Models.ResultTag.Err)
                {
                    return new(new UnstakeErrType.Other("ICP Withdrawal Failure result, msg: " + result.AsErr()));
                }
            }
            else
            {
                var result = await CandidApiManager.Instance.StakingHubApiClient.DissolveIcrc(canisterId);

                if (result.Tag == Candid.StakingHub.Models.ResultTag.Err)
                {
                    return new(new UnstakeErrType.Other("ICRC Withdrawal Failure result, msg: " + result.AsErr()));
                }
            }

            return new(new Null());
        }

        //NFTs
        public static async UniTask<UResult<Null, StakeErrType.Base>> StakeNft(string collectionId, uint? index = null)
        {
            var getLoginTypeResult = UserUtil.GetLogInType();

            if (getLoginTypeResult.Tag == UResultTag.Err)
            {
                return new(new StakeErrType.LogIn(getLoginTypeResult.AsErr()));
            }

            if (getLoginTypeResult.AsOk() == UserUtil.LoginType.Anon)
            {
                return new(new StakeErrType.LogIn("You cannot execute this function as anon"));
            }

            var getLoginDataResult = UserUtil.GetLogInData();
            var loginData = getLoginDataResult.AsOk();

            DataTypes.NftCollection.Nft nftDetails = null;

            //CHECK BALANCE
            if (index.HasValue)
            {
                var nftResult = NftUtil.TryGetNft(collectionId, index.Value);

                if (nftResult.IsErr)
                    return new(new StakeErrType.InsufficientBalance(nftResult.AsErr()));

                nftDetails = nftResult.AsOk();
            }
            else
            {
                var nextNftResult = NftUtil.TryGetNextNft(collectionId);

                if (nextNftResult.Tag == UResultTag.Err)
                {
                    return new(new StakeErrType.InsufficientBalance(nextNftResult.AsErr()));
                }

                nftDetails = nextNftResult.AsOk();
            }

            Extv2BoomApiClient collectionInterface = new(loginData.agent, Principal.FromText(collectionId));

            Debug.Log($"Try stake nft of index: {nftDetails.index} and tid: {nftDetails.tokenIdentifier}. Transferring it aidFrom: {loginData.accountIdentifier}, aidTo: {CandidApiManager.StakingHubIdentifier}");

            var transferResult = await collectionInterface.Transfer(new Candid.Extv2Boom.Models.TransferRequest(
                1,
                new(Candid.Extv2Boom.Models.UserTag.Address, loginData.accountIdentifier),
                new(),
                false,
                new(),
                new(Candid.Extv2Boom.Models.UserTag.Address, CandidApiManager.StakingHubIdentifier),
                $"{nftDetails.tokenIdentifier}"
            ));

            if (transferResult.Tag == Candid.Extv2Boom.Models.TransferResponseTag.Err)
            {
                return new(new StakeErrType.Transfer($"Transfer of nft tid {nftDetails.tokenIdentifier} failed"));
            }

            var verificationResult = await CandidApiManager.Instance.StakingHubApiClient.UpdateExtStakes((uint)nftDetails.index, Env.CanisterIds.STAKING_HUB, loginData.principal, Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

            if (verificationResult.Tag == Candid.StakingHub.Models.ResponseTag.Err)
            {
                return new(new StakeErrType.UpdateStake($"Verification transfer of nft tid {nftDetails.tokenIdentifier} failed, msg: {verificationResult.AsErr()}"));
            }

            Debug.Log($"NFT Stake Success. tid: {nftDetails.tokenIdentifier}");

            return new(new Null());
        }
        public static async UniTask<UResult<Null, UnstakeErrType.Base>> UnstakeNft(string collectionId, uint? index = null)
        {
            //CHECK LOG IN
            var getLoginTypeResult = UserUtil.GetLogInType();

            if (getLoginTypeResult.Tag == UResultTag.Err)
                return new(new UnstakeErrType.LogIn(getLoginTypeResult.AsErr()));

            if (getLoginTypeResult.AsOk() == UserUtil.LoginType.Anon)
                return new(new UnstakeErrType.LogIn("You cannot execute this function as anon"));

            //CHECK BALANCE
            var getStakesResult = UserUtil.GetElementsOfType<DataTypes.Stake>();

            if (getStakesResult.Tag == UResultTag.Err)
                return new(new UnstakeErrType.Other(getStakesResult.AsErr()));

            var stakes = getStakesResult.AsOk();

            if (index.HasValue) {

                var nextNftStake = stakes.Locate(e =>
                {
                    if (e.blockIndex.TryParseValue(out uint nftIndex) == false) return false;

                    return e.canisterId == collectionId && nftIndex == index;
                });

                if (nextNftStake == null)
                    return new(new UnstakeErrType.InsufficientBalance($"You have no nft to unstake from collection {collectionId}"));
            }
            else
            {
                var nextNftStake = stakes.Locate(e => e.canisterId == collectionId && e.blockIndex != null);

                if (nextNftStake == null)
                    return new(new UnstakeErrType.InsufficientBalance($"You have no nft to unstake from collection {collectionId}"));

                if (!nextNftStake.blockIndex.TryParseValue(out uint nftIndex))
                    return new(new UnstakeErrType.Other($"Nft Index of nft collectionId: {collectionId} could not be parsed"));

                index = nftIndex;
            }

            var withdrawalResult = await CandidApiManager.Instance.StakingHubApiClient.DissolveExt(collectionId, index.Value);

            if (withdrawalResult.Tag == Candid.StakingHub.Models.ResultTag.Err)
                return new(new UnstakeErrType.Other($"NFT Dissolve Failure. msg: " + withdrawalResult.AsErr()));

            Debug.Log($"NFT Dissolve Success.");

            return new(new Null());
        }
    }
    public static class Action
    {
        //DEFAULT
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> Default(string actionId)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateActionConfig(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionConfigResonse = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);

            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionConfigResonse.AsErr()));
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.actionPlugin != null)
            {
                return new(new ActionErrType.WrongActionType($"ActionId: {actionId} is not a Default type cuz its ActionPlugin has value"));
            }

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.DefaultArg(actionId));

            //CHECK FOR SUCCESS
            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionResult.AsErr()));
            }

            return new(actionResult.AsOk());
        }
        //BURN
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> VerifyBurnNfts(string actionId, string collectionId, params uint[] indexes)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateActionConfig(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionConfigResonse = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionConfigResonse.AsErr()));
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyBurnNfts}. ActionPlugin is Null"));
            }

            var actionPlugin = actionConfig.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.VerifyBurnNfts)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyBurnNfts}. Current type: {actionPlugin.Tag}"));
            }

            //CHECK BALANCE

            List<uint> nftsToBurn = null;
            indexes ??= new uint[0];

            var burnNftActionPlugin = actionPlugin.AsVerifyBurnNfts();
            var optionalRequiredMetadata = burnNftActionPlugin.RequiredNftMetadata;

            var canisterId = burnNftActionPlugin.Canister;
            var hasRequirements = optionalRequiredMetadata.HasValue && optionalRequiredMetadata.GetValueOrDefault().Count() > 0;
            var requiredMetadata = hasRequirements ? optionalRequiredMetadata.GetValueOrDefault().ToArray() : null;

            //If has requirements
            if (hasRequirements)
            {
                //If indexes were not specified
                if (indexes.Length == 0)
                {
          
                    Func<DataTypes.NftCollection.Nft, string, bool> predicate = (nft, requirement) =>
                    {
                        $"Nft metadata Comparison. Nft metadata: {nft.metadata} ==  requirement: {requirement}= {(nft.metadata == requirement)}".Log();

                        return nft.metadata == requirement;
                    };

                    var nftsToBurnResult = NftUtil.Filter(canisterId, predicate, requiredMetadata);

                    if (nftsToBurnResult.IsErr)
                        return new(new ActionErrType.InsufficientBalance(nftsToBurnResult.AsErr()));

                    nftsToBurn = nftsToBurnResult.AsOk().Map(e => e.index).ToList();
                }
                //If indexes were specified
                else
                {
                    //ENSURE GIVEN IDNEXES CONTAIN THE REQUIRED METADATA

                    nftsToBurn = indexes.ToList();
                    Func<DataTypes.NftCollection.Nft, uint, bool> predicate = (nft, requiredIndex) => nft.index == requiredIndex;

                    var specifiedNftsResult = NftUtil.Filter(canisterId, predicate, indexes);

                    if(specifiedNftsResult.IsErr)
                        return new(new ActionErrType.InsufficientBalance(specifiedNftsResult.AsErr()));

                    var specifiedNfts = specifiedNftsResult.AsOk();

                    for (int i = 0; i < requiredMetadata.Length; i++)
                    {
                        for (int j = 0; j < specifiedNfts.Count; j++)
                        {
                            if (requiredMetadata[i] == specifiedNfts[j].metadata)
                            {
                                goto continueWithMainLoop;
                            }
                        }

                        return new(new ActionErrType.InsufficientBalance($"You dont have nft with metadata: {requiredMetadata[i]} in the collection: {canisterId}"));
                        continueWithMainLoop: continue;
                    }
                }
            }
            else
            {
                //If indexes were not specified
                if (indexes.Length == 0)
                {
                    var getNextIndexResult = NftUtil.TryGetNextNftIndex(collectionId);

                    if (getNextIndexResult.Tag == UResultTag.Err)
                    {
                        return new(new ActionErrType.InsufficientBalance($"Could not find next nft to burn cuz u might not have any from the selected collection {collectionId}"));
                    }

                    nftsToBurn = new(1) { getNextIndexResult.AsOk() };
                }
                //If indexes were specified
                else
                {
                    nftsToBurn = indexes.ToList();
                }
            }

            foreach (var nftIndex in nftsToBurn)
            {
                var tryRemoveNftIndexResult = NftUtil.TryRemoveNftByIndex(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, nftIndex);
                if (tryRemoveNftIndexResult.IsErr)
                {
                    return new(new ActionErrType.Other(tryRemoveNftIndexResult.AsErr()));
                }
            }

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.VerifyBurnNftsArg(actionId, nftsToBurn));

            //CHECK FOR SUCCESS
            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionResult.AsErr()));
            }

            return new(actionResult.AsOk());
        }

        //TRANSFER AND VERIFY
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> TransferAndVerifyIcp(string actionId, bool updateBalance = true)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateActionConfig(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionConfigResonse = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionConfigResonse.AsErr()));
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. ActionPlugin is Null"));
            }

            var actionPlugin = actionConfig.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcp)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. Current type: {actionPlugin.Tag}"));
            }

            //SETUP BALANCE REQUIREMENT
            var config = actionPlugin.AsVerifyTransferIcp();
            double amount = config.Amt;

            //TRANSFER
            var transferResult = await ActionUtil.Transfer.TransferIcp(amount, CandidApiManager.PaymentHubIdentifier, updateBalance);

            if (transferResult.Tag == UResultTag.Err)
            {
                switch (transferResult.AsErr())
                {
                    case TransferErrType.LogIn errType:
                        return new(new ActionErrType.LogIn(errType.content));
                    case TransferErrType.InsufficientBalance errType:
                        return new(new ActionErrType.InsufficientBalance(errType.content));
                    case TransferErrType.Transfer errType:
                        return new(new ActionErrType.Transfer(errType.content));
                    case TransferErrType.Other errType:
                        return new(new ActionErrType.Other(errType.content));
                }
            }

            var blockIndex = transferResult.AsOk();

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.VerifyTransferIcp(actionId, blockIndex));

            //CHECK FOR SUCCESS
            if (actionResult.Tag != UResultTag.Ok)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionResult.AsErr()));
            }

            return new(actionResult.AsOk());
        }
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> TransferAndVerifyIcrc(string actionId, string canisterId, bool updateBalance = true)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateActionConfig(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionConfigResonse = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionConfigResonse.AsErr()));
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. ActionPlugin is Null"));
            }

            var actionPlugin = actionConfig.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcrc)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcrc}. Current type: {actionPlugin.Tag}"));
            }

            //SETUP BALANCE REQUIREMENT
            var config = actionPlugin.AsVerifyTransferIcrc();
            double amount = config.Amt;

            //TRANSFER
            var transferResult = await ActionUtil.Transfer.TransferIcrc(amount, canisterId, Env.CanisterIds.PAYMENT_HUB, updateBalance);

            if (transferResult.Tag == UResultTag.Err)
            {
                switch (transferResult.AsErr())
                {
                    case TransferErrType.LogIn errType:
                        return new(new ActionErrType.LogIn(errType.content));
                    case TransferErrType.InsufficientBalance errType:
                        return new(new ActionErrType.InsufficientBalance(errType.content));
                    case TransferErrType.Transfer errType:
                        return new(new ActionErrType.Transfer(errType.content));
                    case TransferErrType.Other errType:
                        return new(new ActionErrType.Other(errType.content));
                }
            }

            var blockIndex = transferResult.AsOk();

            $"TEST ActionArgValueTypes.VerifyTransferIcrc:   actionId: {actionId}  blockIndex: {blockIndex}".Log(nameof(ActionUtil));
            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.VerifyTransferIcrc(actionId, blockIndex));

            //CHECK FOR SUCCESS
            if (actionResult.Tag != UResultTag.Ok)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionResult.AsErr()));
            }

            return new(actionResult.AsOk());
        }

        //CLAIM STAKE REWARD
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> ClaimStakeRewardIcp(string actionId)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateActionConfig(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionConfigResonse = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionConfigResonse.AsErr()));
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.ClaimStakingRewardIcp}. ActionPlugin is Null"));
            }

            var actionPlugin = actionConfig.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.ClaimStakingRewardIcp)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.ClaimStakingRewardIcp}. Current type: {actionPlugin.Tag}"));
            }

            //SETUP CLAIM REWARD REQUIREMENT
            var requirement = actionPlugin.AsClaimStakingRewardIcp().RequiredAmount;

            var tokenConfigResult = UserUtil.GetElementOfType<DataTypes.TokenConfig>(Env.CanisterIds.ICP_LEDGER);

            if (tokenConfigResult.IsErr)
            {
                return new(new ActionErrType.Other(tokenConfigResult.AsErr()));
            }

            var tokenContig = tokenConfigResult.AsOk();

            var baseUnitRequirement = requirement.ConvertToBaseUnit(tokenContig.decimals);

            //CHECK STAKE BALANCE
            var getStakesResult = UserUtil.GetElementsOfType<DataTypes.Stake>();

            if (getStakesResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(getStakesResult.AsErr()));
            }

            var stakes = getStakesResult.AsOk();

            var stake = stakes.Locate(e =>
            {
                return e.canisterId == Env.CanisterIds.ICP_LEDGER;
            });

            ulong stakeBaseUnitBalance = stake == null ? 0 : stake.amount;

            if (stakeBaseUnitBalance < baseUnitRequirement)
            {
                return new(new ActionErrType.InsufficientBalance($"You don't have enough staked ICP from canister: {Env.CanisterIds.ICP_LEDGER}\nrequirement: {requirement}\nstaked: {stakeBaseUnitBalance.ConvertToDecimal(tokenContig.decimals).NotScientificNotation()}"));
            }

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.ClaimStakingRewardIcp(actionId));

            //CHECK FOR SUCCESS
            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionResult.AsErr()));
            }

            return new(actionResult.AsOk());
        }
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> ClaimStakeRewardIcrc(string actionId)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateActionConfig(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionConfigResonse = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionConfigResonse.AsErr()));
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.ClaimStakingRewardIcrc}. ActionPlugin is Null"));
            }

            var actionPlugin = actionConfig.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.ClaimStakingRewardIcrc)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.ClaimStakingRewardIcrc}. Current type: {actionPlugin.Tag}"));
            }

            //SETUP CLAIM REWARD REQUIREMENT
            var unwrappedPlugin = actionPlugin.AsClaimStakingRewardIcrc();
            var canisterId = unwrappedPlugin.Canister;
            var requirement = unwrappedPlugin.RequiredAmount;

            var tokenConfigResult = UserUtil.GetElementOfType<DataTypes.TokenConfig>(canisterId);

            if (tokenConfigResult.IsErr)
            {
                return new(new ActionErrType.Other(tokenConfigResult.AsErr()));
            }

            var tokenContig = tokenConfigResult.AsOk();

            var baseUnitRequirement = requirement.ConvertToBaseUnit(tokenContig.decimals);

            //CHECK STAKE BALANCE
            var getStakesResult = UserUtil.GetElementsOfType<DataTypes.Stake>();

            if (getStakesResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(getStakesResult.AsErr()));
            }

            var stakes = getStakesResult.AsOk();

            var stake = stakes.Locate(e =>
            {
                return e.canisterId == canisterId;
            });

            ulong stakeBaseUnitBalance = stake == null ? 0 : stake.amount;

            if (stakeBaseUnitBalance < baseUnitRequirement)
            {
                return new(new ActionErrType.InsufficientBalance($"You don't have enough staked {tokenContig.symbol} from canister: {canisterId}\nrequirement: {requirement}\nstaked: {stakeBaseUnitBalance.ConvertToDecimal(tokenContig.decimals).NotScientificNotation()}"));
            }

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.ClaimStakingRewardIcrc(actionId));

            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other("ICRC StakeClaim Failure " + actionResult.AsErr()));
            }

            return new(actionResult.AsOk());
        }
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> ClaimStakeRewardNft(string actionId)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateActionConfig(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionConfigResonse = UserUtil.GetElementOfType<DataTypes.ActionConfig>(actionId);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionConfigResonse.AsErr()));
            }

            var actionConfig = actionConfigResonse.AsOk();

            if (actionConfig.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.ClaimStakingRewardNft}. ActionPlugin is Null"));
            }

            var actionPlugin = actionConfig.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.ClaimStakingRewardNft)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.ClaimStakingRewardNft}. Current type: {actionPlugin.Tag}"));
            }

            //SETUP CLAIM REWARD REQUIREMENT
            var unwrappedPlugin = actionPlugin.AsClaimStakingRewardNft();
            var canisterId = unwrappedPlugin.Canister;
            var requirement = unwrappedPlugin.RequiredAmount;
            requirement.TryToUInt64(out var _requirement);

            //CHECK STAKE BALANCE
            var getStakesResult = UserUtil.GetElementsOfType<DataTypes.Stake>();

            if (getStakesResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(getStakesResult.AsErr()));
            }

            var stakes = getStakesResult.AsOk();

            ulong nftStakeCount = (ulong)CollectionUtil.Count(stakes, e =>
            {
                return e.canisterId.Contains(canisterId);
            });

            if (nftStakeCount < _requirement)
            {
                return new(new ActionErrType.InsufficientBalance($"You don't have enough staked NFT from canister: {canisterId}\nrequirement: {_requirement}\nstaked: {nftStakeCount}"));
            }

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.ClaimStakingRewardNft(actionId));

            //CHECK FOR SUCCESS
            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionResult.AsErr()));
            }

            return new(actionResult.AsOk());
        }
    }
}