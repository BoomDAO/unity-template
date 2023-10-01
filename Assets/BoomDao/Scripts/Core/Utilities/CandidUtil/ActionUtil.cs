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
using Candid.IcrcLedger;
using Candid.IcpLedger;
using WebSocketSharp;
using System;
using Newtonsoft.Json;

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
}

//PROCESS ACTION RESULT
public class ProcessedActionResponse
{
    public Dictionary<string, NewEntityValues> setStringEntities = new();
    public Dictionary<string, NewEntityValues> setNumberEntities = new();
    public Dictionary<string, NewEntityValues> incrementNumberEntities = new();
    public Dictionary<string, NewEntityValues> decrementNumberEntities = new();
    public Dictionary<string, NewEntityValues> renewTimestamp = new();
    public Dictionary<string, NewEntityValues> deletedEntities = new();
    public List<MintNft> nfts;
    public List<MintToken> tokens;

    public ProcessedActionResponse(
        Dictionary<string, NewEntityValues> setStringEntities,
        Dictionary<string, NewEntityValues> setNumberEntities,
        Dictionary<string, NewEntityValues> incrementNumberEntities,
        Dictionary<string, NewEntityValues> decrementNumberEntities,
        Dictionary<string, NewEntityValues> renewTimestamp,
        Dictionary<string, NewEntityValues> deletedEntities,
        List<MintNft> nfts, List<MintToken> tokens)
    {
        this.setStringEntities = setStringEntities;
        this.setNumberEntities = setNumberEntities;
        this.incrementNumberEntities = incrementNumberEntities;
        this.decrementNumberEntities = decrementNumberEntities;
        this.renewTimestamp = renewTimestamp;
        this.deletedEntities = deletedEntities;
        this.nfts = nfts;
        this.tokens = tokens;
    }
}
//UTILS
public static class ActionUtil
{
    #region Get Action Details
    public static bool HasPluginType(this DataTypes.Action action, ActionPluginTag type)
    {
        if (action.actionPlugin == null) return false;

        return action.actionPlugin.Tag == type;
    }
    #endregion

    public static bool TryGetTriesLeft(string actionId, out ulong triesLeft)
    {
        triesLeft = 0;
        var actionResult = UserUtil.GetElementOfType<DataTypes.Action>(actionId);

        if (actionResult.IsErr)
        {
            Debug.LogError(actionResult.AsErr());
            return false;
        }

        var asOk = actionResult.AsOk();

        if (asOk.timeConstraint == null)
        {
            triesLeft = 1;
            return true;
        }

        var intervalStartTs = UserUtil.GetPropertyFromType<DataTypes.ActionState, ulong>(actionId, e => e.intervalStartTs, 0);
        var actionCount = UserUtil.GetPropertyFromType<DataTypes.ActionState, ulong>(actionId, e => e.actionCount, 0);

        if (asOk.timeConstraint.ActionsPerInterval.TryToUInt64(out var actionsPerInterval) == false)
        {
            Debug.Log("Converting \"timeConstrain.ActionsPerInterval\" to UInt64 failed");
            return false;
        }

        if (actionsPerInterval == 0)
        {
            return false;
        }

        if (asOk.timeConstraint.IntervalDuration.TryToUInt64(out var intervalDuration) == false)
        {
            Debug.Log("Converting \"timeConstrain.IntervalDuration\" to UInt64 failed");
            return false;
        }

        var timeConstrainToCompareWith = intervalStartTs.NanoToMilliseconds() + intervalDuration.NanoToMilliseconds();
        if (timeConstrainToCompareWith < MainUtil.Now())
        {
            triesLeft = actionsPerInterval;
            return true;
        }
        else if (actionCount < actionsPerInterval)
        {
            triesLeft = actionsPerInterval - actionCount;
            return true;
        }
        else
        {
            triesLeft = 0;
            return true;
        }
    }
    //GENERIC CHECK BEFORE PROCESSING AN ACTION

    private static UResult<Null, ActionErrType.Base> ValidateAction(string actionId, out (ulong actionCount, ulong intervalStartTs) newActionData)
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
        var intervalStartTs = UserUtil.GetPropertyFromType<DataTypes.ActionState, ulong>(actionId, e => e.intervalStartTs, 0);
        var actionCount = UserUtil.GetPropertyFromType<DataTypes.ActionState, ulong>(actionId, e => e.actionCount, 0);

        ////

        //Check for Action State

        var areActionsValid = UserUtil.IsDataValid<DataTypes.Action>(); //TODO: REMOVE

        if (!areActionsValid) return new(new ActionErrType.Other("Action State has not been loaded yet")); //TODO: REMOVE

        var actionResult = UserUtil.GetElementOfType<DataTypes.Action>(actionId);

        if (actionResult.IsErr) return new(new ActionErrType.Other(actionResult.AsErr()));

        var action = actionResult.AsOk();

        if (action.timeConstraint != null)
        {
            var timeConstrain = action.timeConstraint;

            if (timeConstrain.ActionsPerInterval.TryToUInt64(out var actionsPerInterval) == false) return new(new ActionErrType.Other("Converting \"timeConstrain.ActionsPerInterval\" to UInt64 failed"));

            if (actionsPerInterval == 0) return new(new ActionErrType.Other("The actionsPerInterval of this action is currently set to 0, which effectively disables it."));

            if (timeConstrain.IntervalDuration.TryToUInt64(out var intervalDuration) == false) return new(new ActionErrType.Other("Converting \"timeConstrain.IntervalDuration\" to UInt64 failed"));

            var timeConstrainToCompareWith = intervalStartTs.NanoToMilliseconds() + intervalDuration.NanoToMilliseconds();
            if (timeConstrainToCompareWith < MainUtil.Now())
            {
                actionCount = 1;
                intervalStartTs = (ulong)(MainUtil.Now() + 5f.SecondsToMilli()).MilliToNano();
            }
            else if (actionCount < actionsPerInterval)
            {
                ++actionCount;
            }
            else
            {
                var secondsLeft = (timeConstrainToCompareWith - MainUtil.Now()).MilliToSeconds();
                return new(new ActionErrType.ActionsPerInterval($"You have reached the max ({actionsPerInterval}) tries for this time interval. You have tried already {actionCount} times.\nYou must wait: {(secondsLeft > 0 ? secondsLeft : 1)} secs"));
            }

            newActionData = (actionCount, intervalStartTs);
        }

        var checkForRequiremensResult = EntityUtil.MeetEntityRequirements(action);

        if (checkForRequiremensResult.IsErr) return new(new ActionErrType.Other(checkForRequiremensResult.AsErr()));

        if (checkForRequiremensResult.AsOk() == false) return new(new ActionErrType.EntityConstrain("You don't meet the entity requirement"));

        return new(new Null());//SUCCESS
    }
    public static UResult<Null, ActionErrType.Base> ValidateAction(string actionId)
    {
        return ValidateAction(actionId, out var outVal);
    }

    private static void UpdateActionData(string actionId, ulong intervalStartTs, ulong actionCount)
    {
        UserUtil.UpdateData(new DataTypes.ActionState(
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

        Dictionary<string, NewEntityValues> setStringEntities = new();
        Dictionary<string, NewEntityValues> setNumberEntities = new();
        Dictionary<string, NewEntityValues> incrementNumberEntities = new();
        Dictionary<string, NewEntityValues> decrementNumberEntities = new();
        Dictionary<string, NewEntityValues> renewTimestamp = new();
        Dictionary<string, NewEntityValues> deletedEntities = new();
        List<MintNft> nfts = new();
        List<MintToken> tokens = new();

        okVal.Iterate(e =>
        {
            string wid = Env.CanisterIds.WORLD;
            string gid;
            string eid;
            string fieldName;
            object fieldValue;
            Dictionary<string, EntityFieldEditData> allFieldsToEdit;
            var entityKey = "";

            switch (e.Option.Tag)
            {
                case ActionOutcomeOption.OptionInfoTag.SetString:

                    var val1 = e.Option.AsSetString();
                    if (!string.IsNullOrEmpty(val1.Wid.ValueOrDefault)) wid = val1.Wid.ValueOrDefault;
                    gid = val1.Gid;
                    eid = val1.Eid;
                    fieldName = val1.Field;
                    fieldValue = val1.Value;

                    entityKey = $"{wid}{gid}{eid}";

                    if (!setStringEntities.TryGetValue(entityKey, out var entityToEdit1))
                    {
                        entityToEdit1 = new(wid, gid, eid, new Dictionary<string, EntityFieldEditData>());
                        setStringEntities.Add(entityKey, entityToEdit1);
                    }

                    allFieldsToEdit = entityToEdit1.fields;


                    if (allFieldsToEdit != null)
                    {
                        if (!allFieldsToEdit.TryAdd(fieldName, new EntityFieldEditData(EntityFieldEditType.SetString, fieldValue)))
                        {
                            allFieldsToEdit[fieldName] = new EntityFieldEditData(EntityFieldEditType.SetString, fieldValue);
                        }
                    }

                    break;
                case ActionOutcomeOption.OptionInfoTag.SetNumber:

                    var val2 = e.Option.AsSetNumber();
                    if (!string.IsNullOrEmpty(val2.Wid.ValueOrDefault)) wid = val2.Wid.ValueOrDefault;
                    gid = val2.Gid;
                    eid = val2.Eid;
                    fieldName = val2.Field;
                    fieldValue = val2.Value;

                    entityKey = $"{wid}{gid}{eid}";

                    if (!setNumberEntities.TryGetValue(entityKey, out var entityToEdit2))
                    {
                        entityToEdit2 = new(wid, gid, eid, new Dictionary<string, EntityFieldEditData>());
                        setNumberEntities.Add(entityKey, entityToEdit2);
                    }

                    allFieldsToEdit = entityToEdit2.fields;

                    if (allFieldsToEdit != null)
                    {
                        if (!allFieldsToEdit.TryAdd(fieldName, new EntityFieldEditData(EntityFieldEditType.SetNumber, fieldValue)))
                        {
                            allFieldsToEdit[fieldName] = new EntityFieldEditData(EntityFieldEditType.SetNumber, fieldValue);
                        }
                    }

                    break;
                case ActionOutcomeOption.OptionInfoTag.IncrementNumber:

                    var val3 = e.Option.AsIncrementNumber();
                    if (!string.IsNullOrEmpty(val3.Wid.ValueOrDefault)) wid = val3.Wid.ValueOrDefault;
                    gid = val3.Gid;
                    eid = val3.Eid;
                    fieldName = val3.Field;
                    fieldValue = val3.Value;

                    entityKey = $"{wid}{gid}{eid}";

                    if (!incrementNumberEntities.TryGetValue(entityKey, out var entityToEdit3))
                    {
                        entityToEdit3 = new(wid, gid, eid, new Dictionary<string, EntityFieldEditData>());
                        incrementNumberEntities.Add(entityKey, entityToEdit3);
                    }

                    allFieldsToEdit = entityToEdit3.fields;

                    if (allFieldsToEdit != null)
                    {
                        if (!allFieldsToEdit.TryAdd(fieldName, new EntityFieldEditData(EntityFieldEditType.IncrementNumber, fieldValue)))
                        {
                            var currentValue = allFieldsToEdit[fieldName];
                            var newValue = (double)currentValue.Value + (double)fieldValue;
                            allFieldsToEdit[fieldName] = new EntityFieldEditData(EntityFieldEditType.IncrementNumber, newValue);
                        }
                    }

                    break;
                case ActionOutcomeOption.OptionInfoTag.DecrementNumber:

                    var val4 = e.Option.AsDecrementNumber();
                    if (!string.IsNullOrEmpty(val4.Wid.ValueOrDefault)) wid = val4.Wid.ValueOrDefault;
                    gid = val4.Gid;
                    eid = val4.Eid;
                    fieldName = val4.Field;
                    fieldValue = val4.Value;

                    entityKey = $"{wid}{gid}{eid}";

                    if (!decrementNumberEntities.TryGetValue(entityKey, out var entityToEdit4))
                    {
                        entityToEdit4 = new(wid, gid, eid, new Dictionary<string, EntityFieldEditData>());
                        decrementNumberEntities.Add(entityKey, entityToEdit4);
                    }

                    allFieldsToEdit = entityToEdit4.fields;

                    if (allFieldsToEdit != null)
                    {
                        if (!allFieldsToEdit.TryAdd(fieldName, new EntityFieldEditData(EntityFieldEditType.DecrementNumber, fieldValue)))
                        {
                            var currentValue = allFieldsToEdit[fieldName];
                            var newValue = (double)currentValue.Value + (double)fieldValue;
                            allFieldsToEdit[fieldName] = new EntityFieldEditData(EntityFieldEditType.DecrementNumber, newValue);
                        }
                    }

                    break;
                case ActionOutcomeOption.OptionInfoTag.RenewTimestamp:

                    var val5 = e.Option.AsRenewTimestamp();
                    if (!string.IsNullOrEmpty(val5.Wid.ValueOrDefault)) wid = val5.Wid.ValueOrDefault;
                    gid = val5.Gid;
                    eid = val5.Eid;
                    fieldName = val5.Field;
                    fieldValue = val5.Value;

                    entityKey = $"{wid}{gid}{eid}";

                    if (!renewTimestamp.TryGetValue(entityKey, out var entityToEdit5))
                    {
                        entityToEdit5 = new(wid, gid, eid, new Dictionary<string, EntityFieldEditData>());
                        renewTimestamp.Add(entityKey, entityToEdit5);
                    }

                    allFieldsToEdit = entityToEdit5.fields;

                    if (allFieldsToEdit != null)
                    {
                        if (!allFieldsToEdit.TryAdd(fieldName, new EntityFieldEditData(EntityFieldEditType.RenewTimestamp, fieldValue)))
                        {
                            allFieldsToEdit[fieldName] = new EntityFieldEditData(EntityFieldEditType.RenewTimestamp, fieldValue);
                        }
                    }

                    break;
                case ActionOutcomeOption.OptionInfoTag.DeleteEntity:

                    var val6 = e.Option.AsDeleteEntity();
                    if (!string.IsNullOrEmpty(val6.Wid.ValueOrDefault)) wid = val6.Wid.ValueOrDefault;
                    gid = val6.Gid;
                    eid = val6.Eid;

                    entityKey = $"{wid}{gid}{eid}";

                    if (!deletedEntities.TryGetValue(entityKey, out var entityToEdit6))
                    {
                        entityToEdit6 = new(wid, gid, eid, null);
                        deletedEntities.Add(entityKey, entityToEdit6);
                    }

                    break;
                case ActionOutcomeOption.OptionInfoTag.MintToken:

                    tokens.Add(e.Option.AsMintToken());
                    break;
                case ActionOutcomeOption.OptionInfoTag.MintNft:

                    nfts.Add(e.Option.AsMintNft());
                    break;
            }
        });

        //TRY UPDATE LOCAL DATA

        //ENTITIES
        LinkedList<NewEntityValues> entityEdits = new();
        foreach (var item in setStringEntities)
            entityEdits.AddLast(item.Value);
        foreach (var item in setNumberEntities)
            entityEdits.AddLast(item.Value);
        foreach (var item in incrementNumberEntities)
            entityEdits.AddLast(item.Value);
        foreach (var item in decrementNumberEntities)
            entityEdits.AddLast(item.Value);
        foreach (var item in renewTimestamp)
            entityEdits.AddLast(item.Value);
        foreach (var item in deletedEntities)
            entityEdits.AddLast(item.Value);

        //NFTS
        EntityUtil.EditEntities(entityEdits.ToArray());

        if (nfts.Count > 0)
        {
            NftUtil.TryAddMintedNft(nfts.ToArray());
        }

        //TOKENS
        if (tokens.Count > 0)
        {
            TokenUtil.IncrementTokenByDecimal(tokens.Map<MintToken, (string canister, double decimalAmount)>(e=> (e.Canister, e.Quantity)).ToArray());
        }
        var processActionResponse = new ProcessedActionResponse(setStringEntities, setNumberEntities, incrementNumberEntities, decrementNumberEntities, renewTimestamp, deletedEntities, nfts, tokens);

        $"Action Processed Success, Type: ${typeof(ActionArgValueTypes.BaseArg)}, ActionId: {arg.ActionId}, outcome: {JsonConvert.SerializeObject(processActionResponse)}".Log(nameof(ActionUtil));

        return new(processActionResponse);
    }

    public static class Transfer
    {
        //ICP
        public async static UniTask<UResult<ulong, TransferErrType.Base>> TransferIcp(double amount, string toAddress)
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

            var tokenDetailsResult = TokenUtil.GetTokenDetails(Env.CanisterIds.ICP_LEDGER);

            if (tokenDetailsResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.Other(tokenDetailsResult.AsErr()));
            }

            var (token, metadata) = tokenDetailsResult.AsOk();

            var requiredBaseUnitAmount = CandidUtil.ConvertToBaseUnit(amount, metadata.decimals);

            if (token.baseUnitAmount < requiredBaseUnitAmount + metadata.fee)
            {
                return new(new TransferErrType.InsufficientBalance($"Not enough \"${Env.CanisterIds.ICP_LEDGER}\" currency. Current balance: {token.baseUnitAmount.ConvertToDecimal(metadata.decimals).NotScientificNotation()}, required balance: {amount}"));
            }

            //UPDATE LOCAL STATE
            TokenUtil.DecrementTokenByBaseUnit((Env.CanisterIds.ICP_LEDGER, requiredBaseUnitAmount + metadata.fee));

            //SETUP INTERFACE
            var tokenInterface = new IcpLedgerApiClient(UserUtil.GetAgent().AsOk(), Principal.FromText(Env.CanisterIds.ICP_LEDGER));

            //SETUP ARGS
            List<byte> addressBytes = CandidUtil.HexStringToByteArray(toAddress).ToList();
            var arg = new TransferArgs
            {
                To = addressBytes,
                Amount = new Candid.IcpLedger.Models.Tokens(requiredBaseUnitAmount),
                Fee = new Candid.IcpLedger.Models.Tokens(metadata.fee),
                CreatedAtTime = OptionalValue<TimeStamp>.NoValue(),
                Memo = new ulong(),
                FromSubaccount = new(),
            };

            //TRANSFER
            $"Transfer to address: {toAddress},\n amount {amount},\n baseUnitAmount: {requiredBaseUnitAmount},\n decimals: {metadata.decimals},\n fee: {metadata.fee}".Log(nameof(ActionUtil));
            var result = await tokenInterface.Transfer(arg);

            //CHECK SUCCESS
            if (result.Tag == Candid.IcpLedger.Models.TransferResultTag.Ok)
            {
                var blockIndex = result.AsOk();
                $"BlockIndex Transfer: {blockIndex}".Log();
                return new(blockIndex);
            }
            else
            {
                //Due to failure restore to previews value
                TokenUtil.IncrementTokenByBaseUnit((Env.CanisterIds.ICP_LEDGER, requiredBaseUnitAmount + metadata.fee));

                return new(new TransferErrType.Transfer($"{result.AsErr().Tag}: {result.AsErr().Value}"));
            }
        }
        //ICRC
        public async static UniTask<UResult<ulong, TransferErrType.Base>> TransferIcrc(double amount, string canisterId, string toPrincipal)
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
            var tokenDetailsResult = TokenUtil.GetTokenDetails(canisterId);

            if (tokenDetailsResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.Other(tokenDetailsResult.AsErr()));
            }

            var (token, metadata) = tokenDetailsResult.AsOk();

            var requiredBaseUnitAmount = CandidUtil.ConvertToBaseUnit(amount, metadata.decimals);

            if (token.baseUnitAmount < requiredBaseUnitAmount + metadata.fee)
            {
                return new(new TransferErrType.InsufficientBalance($"Not enough \"${canisterId}\" currency. Current balance: {token.baseUnitAmount.ConvertToDecimal(metadata.decimals).NotScientificNotation()}, required balance: {amount}"));
            }

            //UPDATE LOCAL STATE
            TokenUtil.DecrementTokenByBaseUnit((canisterId, requiredBaseUnitAmount + metadata.fee));

            //SETUP INTERFACE
            var tokenInterface = new IcrcLedgerApiClient(UserUtil.GetAgent().AsOk(), Principal.FromText(canisterId));

            //SETUP ARGS
            var arg = new Candid.IcrcLedger.Models.TransferArgs(
                (UnboundedUInt)requiredBaseUnitAmount,
                new(),
                new((UnboundedUInt)metadata.fee),
                new(),
                new(),
                new(Principal.FromText(toPrincipal),
                new()));

            //TRANSFER
            $"Transfer to principal: {toPrincipal},\n amount {amount},\n baseUnitAmount: {requiredBaseUnitAmount},\n decimals: {metadata.decimals},\n fee: {metadata.fee}".Log(nameof(ActionUtil));
            var result = await tokenInterface.Icrc1Transfer(arg);

            //CHECK SUCCESS
            if (result.Tag == Candid.IcrcLedger.Models.TransferResultTag.Ok)
            {
                var blockIndex = (ulong)result.AsOk();
                $"BlockIndex Transfer: {blockIndex}".Log(nameof(ActionUtil));
                return new(blockIndex);
            }
            else
            {   //Due to failure restore to previews value
                TokenUtil.IncrementTokenByBaseUnit((canisterId, requiredBaseUnitAmount + metadata.fee));

                return new(new TransferErrType.Transfer($"{result.AsErr().Tag}: {result.AsErr().Value}"));
            }
        }
    }
    public static class Action
    {
        //DEFAULT
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> Default(string actionId)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateAction(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionResult = UserUtil.GetElementOfType<DataTypes.Action>(actionId);

            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionResult.AsErr()));
            }

            var action = actionResult.AsOk();

            if (action.actionPlugin != null)
            {
                return new(new ActionErrType.WrongActionType($"ActionId: {actionId} is not a Default type cuz its ActionPlugin has value"));
            }

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionReturnValueResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.DefaultArg(actionId));

            //CHECK FOR SUCCESS
            if (actionReturnValueResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionReturnValueResult.AsErr()));
            }

            return new(actionReturnValueResult.AsOk());
        }
        //BURN
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> VerifyBurnNfts(string actionId, string collectionId, params uint[] indexes)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateAction(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionResult = UserUtil.GetElementOfType<DataTypes.Action>(actionId);
            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionResult.AsErr()));
            }

            var action = actionResult.AsOk();

            if (action.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyBurnNfts}. ActionPlugin is Null"));
            }

            var actionPlugin = action.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.VerifyBurnNfts)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyBurnNfts}. Current type: {actionPlugin.Tag}"));
            }

            //CHECK BALANCE

            List<uint> nftsToBurnIndexes = null;
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

                    nftsToBurnIndexes = nftsToBurnResult.AsOk().Map(e => e.index).ToList();
                }
                //If indexes were specified
                else
                {
                    //ENSURE GIVEN IDNEXES CONTAIN THE REQUIRED METADATA

                    nftsToBurnIndexes = indexes.ToList();
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

                    nftsToBurnIndexes = new(1) { getNextIndexResult.AsOk() };
                }
                //If indexes were specified
                else
                {
                    nftsToBurnIndexes = indexes.ToList();
                }
            }

            //Fetch specified nfts to restore them upon failure
            LinkedList<DataTypes.NftCollection.Nft> tempNfts = new();
            foreach (var nftIndex in nftsToBurnIndexes)
            {
                var nftResult = NftUtil.TryGetNft(collectionId, nftIndex);

                if (nftResult.IsOk) tempNfts.AddLast(nftResult.AsOk());
            }

            foreach (var nftIndex in nftsToBurnIndexes)
            {
                var tryRemoveNftIndexResult = NftUtil.TryRemoveNftByIndex(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, nftIndex);
                if (tryRemoveNftIndexResult.IsErr)
                {
                    return new(new ActionErrType.Other(tryRemoveNftIndexResult.AsErr()));
                }
            }

            //PROCESS ACTION
            UpdateActionData(actionId, newActionData.intervalStartTs, newActionData.actionCount);
            var actionReturnValueResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.VerifyBurnNftsArg(actionId, nftsToBurnIndexes));

            //CHECK FOR SUCCESS
            if (actionReturnValueResult.Tag == UResultTag.Err)
            {
                //Restore removed nfts due to error
                NftUtil.TryAddMintedNft(tempNfts.ToArray());

                return new(new ActionErrType.ActionExecutionFailure(actionReturnValueResult.AsErr()));
            }

            return new(actionReturnValueResult.AsOk());
        }

        //TRANSFER AND VERIFY
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> TransferAndVerifyIcp(string actionId)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateAction(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionResult = UserUtil.GetElementOfType<DataTypes.Action>(actionId);
            if (actionResult.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionResult.AsErr()));
            }

            var action = actionResult.AsOk();

            if (action.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. ActionPlugin is Null"));
            }

            var actionPlugin = action.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcp)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. Current type: {actionPlugin.Tag}"));
            }

            //SETUP BALANCE REQUIREMENT
            var plugin = actionPlugin.AsVerifyTransferIcp();
            double amount = plugin.Amt;

            //TRANSFER
            var transferResult = await ActionUtil.Transfer.TransferIcp(amount, CandidApiManager.PaymentHubIdentifier);

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
            var actionReturnValueResult = await ActionUtil.ProcessAction(new ActionArgValueTypes.VerifyTransferIcp(actionId, blockIndex));

            //CHECK FOR SUCCESS
            if (actionReturnValueResult.Tag != UResultTag.Ok)
            {
                return new(new ActionErrType.ActionExecutionFailure(actionReturnValueResult.AsErr()));
            }

            return new(actionReturnValueResult.AsOk());
        }
        public static async UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> TransferAndVerifyIcrc(string actionId, string canisterId)
        {
            //CHECK CONSTRAINS
            var canProcessActionResult = ValidateAction(actionId, out var newActionData);
            if (canProcessActionResult.Tag == UResultTag.Err)
            {
                return new(canProcessActionResult.AsErr());
            }

            //CHECK ACTION TYPE
            var actionResponse = UserUtil.GetElementOfType<DataTypes.Action>(actionId);
            if (actionResponse.Tag == UResultTag.Err)
            {
                return new(new ActionErrType.Other(actionResponse.AsErr()));
            }

            var action = actionResponse.AsOk();

            if (action.actionPlugin == null)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcp}. ActionPlugin is Null"));
            }

            var actionPlugin = action.actionPlugin;

            if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcrc)
            {
                return new(new ActionErrType.WrongActionType($"id {actionId} is not of type {ActionPluginTag.VerifyTransferIcrc}. Current type: {actionPlugin.Tag}"));
            }

            //SETUP BALANCE REQUIREMENT
            var plugin = actionPlugin.AsVerifyTransferIcrc();
            double amount = plugin.Amt;

            //TRANSFER
            var transferResult = await ActionUtil.Transfer.TransferIcrc(amount, canisterId, Env.CanisterIds.PAYMENT_HUB);

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
    }
}