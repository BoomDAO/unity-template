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
using Boom.Patterns.Broadcasts;
using Boom;
using Candid.Extv2Boom;
using static EntityFieldEdit;


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

//PROCESS ACTION RESULT
public class ProcessedActionResponse
{
    public class Outcomes
    {
        public string uid;
        public Dictionary<string, NewEntityEdits> entityEdits = new();
        public List<MintNft> nfts;
        public List<TransferIcrc> tokens;

        public Outcomes(string uid, List<ActionOutcomeOption> outcomes)
        {
            this.uid = uid;
            this.entityEdits = new();
            this.nfts = new();
            this.tokens = new();

            outcomes.Iterate(e =>
            {
                switch (e.Option.Tag)
                {
                    case ActionOutcomeOption.OptionInfoTag.UpdateEntity:

                        string wid = CandidApiManager.Instance.WORLD_CANISTER_ID;
                        string eid;
                        string fieldName;
                        Dictionary<string, EntityFieldEdit.Base> allFieldsToEdit;

                        var updateEntity = e.Option.AsUpdateEntity();

                        if (!string.IsNullOrEmpty(updateEntity.Wid.ValueOrDefault)) wid = updateEntity.Wid.ValueOrDefault;
                        eid = updateEntity.Eid;

                        var entityKey = $"{wid}{eid}";


                        foreach (var update in updateEntity.Updates)
                        {
                            switch (update.Tag)
                            {
                                case UpdateEntityTypeTag.SetText:

                                    var val1 = update.AsSetText();
                                    fieldName = val1.FieldName;
                                    var fieldValue1 = val1.FieldValue;

                                    if (!entityEdits.TryGetValue(entityKey, out var entityToEdit1))
                                    {
                                        entityToEdit1 = new(wid, eid, new());
                                        entityEdits.Add(entityKey, entityToEdit1);
                                    }

                                    allFieldsToEdit = entityToEdit1.fields;


                                    if (allFieldsToEdit != null)
                                    {
                                        if (!allFieldsToEdit.TryAdd(fieldName, new EntityFieldEdit.SetText((string)fieldValue1)))
                                        {
                                            allFieldsToEdit[fieldName] = new EntityFieldEdit.SetText((string)fieldValue1);
                                        }
                                    }

                                    break;
                                case UpdateEntityTypeTag.SetNumber:

                                    var val2 = update.AsSetNumber();

                                    fieldName = val2.FieldName;
                                    var fieldValue2 = val2.FieldValue;

                                    if (!entityEdits.TryGetValue(entityKey, out var entityToEdit2))
                                    {
                                        entityToEdit2 = new(wid, eid, new());
                                        entityEdits.Add(entityKey, entityToEdit2);
                                    }

                                    allFieldsToEdit = entityToEdit2.fields;

                                    if (allFieldsToEdit != null)
                                    {
                                        if (fieldValue2.Tag == Candid.World.Models.SetNumber.FieldValueInfoTag.Number)
                                        {
                                            var v = new EntityFieldEdit.SetNumber(new EntityFieldEdit.Numeric.ValueType.Number(fieldValue2.AsNumber()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                allFieldsToEdit[fieldName] = v;
                                            }
                                        }
                                        else
                                        {
                                            var v = new EntityFieldEdit.SetNumber(new EntityFieldEdit.Numeric.ValueType.Formula(fieldValue2.AsFormula()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                allFieldsToEdit[fieldName] = v;
                                            }
                                        }
                                    }

                                    break;
                                case UpdateEntityTypeTag.IncrementNumber:

                                    var val3 = update.AsIncrementNumber();

                                    fieldName = val3.FieldName;
                                    var fieldValue3 = val3.FieldValue;

                                    if (!entityEdits.TryGetValue(entityKey, out var entityToEdit3))
                                    {
                                        entityToEdit3 = new(wid, eid, new());
                                        entityEdits.Add(entityKey, entityToEdit3);
                                    }

                                    allFieldsToEdit = entityToEdit3.fields;

                                    if (allFieldsToEdit != null)
                                    {
                                        if (fieldValue3.Tag == Candid.World.Models.IncrementNumber.FieldValueInfoTag.Number)
                                        {
                                            var v = new EntityFieldEdit.IncrementNumber(new EntityFieldEdit.Numeric.ValueType.Number(fieldValue3.AsNumber()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                var currentValue = allFieldsToEdit[fieldName];

                                                allFieldsToEdit[fieldName] = new EntityFieldEdit.SetNumber(new EntityFieldEdit.Numeric.ValueType.Number((v.Value as EntityFieldEdit.Numeric.ValueType.Number).Value + ((currentValue as EntityFieldEdit.Numeric).Value as EntityFieldEdit.Numeric.ValueType.Number).Value));
                                            }
                                        }
                                        else
                                        {
                                            var v = new EntityFieldEdit.IncrementNumber(new EntityFieldEdit.Numeric.ValueType.Formula(fieldValue3.AsFormula()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                allFieldsToEdit[fieldName] = v;
                                            }
                                        }
                                    }

                                    break;
                                case UpdateEntityTypeTag.DecrementNumber:

                                    var val4 = update.AsDecrementNumber();

                                    fieldName = val4.FieldName;
                                    var fieldValue4 = val4.FieldValue;

                                    if (!entityEdits.TryGetValue(entityKey, out var entityToEdit4))
                                    {
                                        entityToEdit4 = new(wid, eid, new());
                                        entityEdits.Add(entityKey, entityToEdit4);
                                    }

                                    allFieldsToEdit = entityToEdit4.fields;

                                    if (allFieldsToEdit != null)
                                    {
                                        if (fieldValue4.Tag == Candid.World.Models.DecrementNumber.FieldValueInfoTag.Number)
                                        {
                                            var v = new EntityFieldEdit.DecrementNumber(new EntityFieldEdit.Numeric.ValueType.Number(fieldValue4.AsNumber()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                var currentValue = allFieldsToEdit[fieldName];

                                                allFieldsToEdit[fieldName] = new EntityFieldEdit.SetNumber(new EntityFieldEdit.Numeric.ValueType.Number((v.Value as EntityFieldEdit.Numeric.ValueType.Number).Value + ((currentValue as EntityFieldEdit.Numeric).Value as EntityFieldEdit.Numeric.ValueType.Number).Value));
                                            }
                                        }
                                        else
                                        {
                                            var v = new EntityFieldEdit.DecrementNumber(new EntityFieldEdit.Numeric.ValueType.Formula(fieldValue4.AsFormula()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                allFieldsToEdit[fieldName] = v;
                                            }
                                        }
                                    }

                                    break;
                                case UpdateEntityTypeTag.RenewTimestamp:

                                    var val5 = update.AsRenewTimestamp();

                                    fieldName = val5.FieldName;
                                    var fieldValue5 = val5.FieldValue;

                                    if (!entityEdits.TryGetValue(entityKey, out var entityToEdit5))
                                    {
                                        entityToEdit5 = new(wid, eid, new());
                                        entityEdits.Add(entityKey, entityToEdit5);
                                    }

                                    allFieldsToEdit = entityToEdit5.fields;

                                    if (allFieldsToEdit != null)
                                    {
                                        if (fieldValue5.Tag == Candid.World.Models.RenewTimestamp.FieldValueInfoTag.Number)
                                        {
                                            var v = new EntityFieldEdit.RenewTimestamp(new EntityFieldEdit.Numeric.ValueType.Number(fieldValue5.AsNumber()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                var currentValue = allFieldsToEdit[fieldName];

                                                allFieldsToEdit[fieldName] = new EntityFieldEdit.SetNumber(new EntityFieldEdit.Numeric.ValueType.Number((v.Value as EntityFieldEdit.Numeric.ValueType.Number).Value + ((currentValue as EntityFieldEdit.Numeric).Value as EntityFieldEdit.Numeric.ValueType.Number).Value));
                                            }
                                        }
                                        else
                                        {
                                            var v = new EntityFieldEdit.RenewTimestamp(new EntityFieldEdit.Numeric.ValueType.Formula(fieldValue5.AsFormula()));
                                            if (!allFieldsToEdit.TryAdd(fieldName, v))
                                            {
                                                allFieldsToEdit[fieldName] = v;
                                            }
                                        }
                                    }

                                    break;
                                case UpdateEntityTypeTag.DeleteEntity:

                                    var val6 = update.AsDeleteEntity();

                                    if (!entityEdits.TryGetValue(entityKey, out var entityToEdit6))
                                    {
                                        entityToEdit6 = new(wid, eid, null);
                                        entityEdits.Add(entityKey, entityToEdit6);
                                    }

                                    break;
                            }
                        }

                        break;
                    case ActionOutcomeOption.OptionInfoTag.TransferIcrc:

                        tokens.Add(e.Option.AsTransferIcrc());
                        break;
                    case ActionOutcomeOption.OptionInfoTag.MintNft:

                        nfts.Add(e.Option.AsMintNft());
                        break;
                }
            });
        }
    }

    public Outcomes callerOutcomes;
    public Outcomes targetOutcomes;
    public Outcomes worldOutcomes;

    public ProcessedActionResponse(ActionReturn actionReturn)
    {
        var callerOutcomeResult = actionReturn.CallerOutcomes;
        var targetOutcomeResult = actionReturn.TargetOutcomes;
        var worldOutcomeResult = actionReturn.WorldOutcomes;

        if (callerOutcomeResult.HasValue)
        {
            callerOutcomes = new(actionReturn.CallerPrincipalId, callerOutcomeResult.ValueOrDefault);
        }
        if (targetOutcomeResult.HasValue)
        {
            targetOutcomes = new(actionReturn.TargetPrincipalId.ValueOrDefault, targetOutcomeResult.ValueOrDefault);
        }
        if (worldOutcomeResult.HasValue)
        {
            worldOutcomes = new(actionReturn.WorldPrincipalId, worldOutcomeResult.ValueOrDefault);
        }
    }
}
//UTILS
public static class ActionUtil
{

    public static bool TryGetTriesLeft(string actionId, SubAction subAction, out ulong triesLeft)
    {
        triesLeft = 0;

        if (subAction.TimeConstraint == null)
        {
            triesLeft = 1;
            return true;
        }

        if(subAction.TimeConstraint.actionTimeInterval == null)
        {
            triesLeft = 1;
            return true;
        }

        var intervalStartTs = UserUtil.GetPropertyFromTypeSelf<DataTypes.ActionState, ulong>(actionId, e => e.intervalStartTs, 0);
        var actionCount = UserUtil.GetPropertyFromTypeSelf<DataTypes.ActionState, ulong>(actionId, e => e.actionCount, 0);

        var actionsPerInterval = subAction.TimeConstraint.actionTimeInterval.ActionsPerInterval;

        if (actionsPerInterval == 0)
        {
            return false;
        }

        var intervalDuration = subAction.TimeConstraint.actionTimeInterval.IntervalDuration;

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

    private static void UpdateActionData(string actionId, ulong intervalStartTs, ulong actionCount)
    {
        UserUtil.UpdateDataSelf(new DataTypes.ActionState(
            actionId,
            actionCount,
            intervalStartTs
        ));
    }


    public static readonly Dictionary<string, int> actionsInProcess = new();

    private static void IncrementActionProcessCount(string actionId)
    {
        if (!actionsInProcess.TryAdd(actionId, 1))
        {
            actionsInProcess[actionId] += 1;
        }

        Broadcast.Invoke<OnActionInProcessCountChange>(new OnActionInProcessCountChange(actionId));
    }
    private static void ReduceActionProcessCount(string actionId)
    {
        if (actionsInProcess.ContainsKey(actionId))
        {
            if (actionsInProcess[actionId] > 0)
            {
                actionsInProcess[actionId] -= 1;
                if (actionsInProcess[actionId] > 0)
                {
                    actionsInProcess.Remove(actionId);
                }
                Broadcast.Invoke<OnActionInProcessCountChange>(new OnActionInProcessCountChange(actionId));
            }

        }
    }
    public static int GetActionProcessingCount(string actionId)
    {
        actionsInProcess.TryGetValue(actionId, out int count);

        return count;
    }


    //MAIN FUNCTION TO PROCESS AN ACTION
    public async static UniTask<UResult<ProcessedActionResponse, ActionErrType.Base>> ProcessAction(string actionId, List<Field> args = default)
    {
        args ??= new();

        IncrementActionProcessCount(actionId);

        $"Try Process Action, ActionId: {actionId}".Log(nameof(ActionUtil));
        //Execute Action
        Result3 actionResponse = await CandidApiManager.Instance.WorldApiClient.ProcessAction(new ActionArg(actionId, args));

        if (actionResponse.Tag == Result3Tag.Err)
        {
            ReduceActionProcessCount(actionId);

            return new(new ActionErrType.Other(actionResponse.AsErr()));
        }

        var okVal = actionResponse.AsOk();

        ProcessedActionResponse processedActionResponse = new(okVal);

        //REFINE OUTCOMES
        processedActionResponse.RefineActionOutcomes(args);


        if(CandidApiManager.Instance.BoomDaoGameType == CandidApiManager.GameType.SinglePlayer)
        {
            List<string> outcomeUids = new();

            //CALLER OUTCOMES
            if (processedActionResponse.callerOutcomes != null)
            {
                if (processedActionResponse.callerOutcomes.entityEdits.Count > 0)
                {
                    EntityUtil.ApplyEntityEdits(processedActionResponse.callerOutcomes);
                    outcomeUids.Add(processedActionResponse.callerOutcomes.uid);
                }

                //NFTS
                if (processedActionResponse.callerOutcomes.nfts.Count > 0)
                {
                    NftUtil.TryAddMintedNft(processedActionResponse.callerOutcomes.uid, processedActionResponse.callerOutcomes.nfts.ToArray());

                    CoroutineManagerUtil.DelayAction(() =>
                    {
                        UserUtil.RequestData(new DataTypeRequestArgs.NftCollection(processedActionResponse.callerOutcomes.nfts.Map(e => e.Canister).ToArray(), processedActionResponse.callerOutcomes.uid));
                    }, 20f, CandidApiManager.Instance.transform);
                }

                //TOKENS
                if (processedActionResponse.callerOutcomes.tokens.Count > 0)
                {
                    TokenUtil.IncrementTokenByDecimal(processedActionResponse.callerOutcomes.uid, processedActionResponse.callerOutcomes.tokens.Map<TransferIcrc, (string canister, double decimalAmount)>(e => (e.Canister, e.Quantity)).ToArray());
                }
            }
            //TARGET OUTCOMES
            if (processedActionResponse.targetOutcomes != null)
            {
                if (processedActionResponse.targetOutcomes.entityEdits.Count > 0)
                {
                    EntityUtil.ApplyEntityEdits(processedActionResponse.targetOutcomes);
                    outcomeUids.Add(processedActionResponse.targetOutcomes.uid);
                }

                //NFTS
                if (processedActionResponse.targetOutcomes.nfts.Count > 0)
                {
                    NftUtil.TryAddMintedNft(processedActionResponse.targetOutcomes.uid, processedActionResponse.targetOutcomes.nfts.ToArray());

                    CoroutineManagerUtil.DelayAction(() =>
                    {
                        UserUtil.RequestData(new DataTypeRequestArgs.NftCollection(processedActionResponse.targetOutcomes.nfts.Map(e => e.Canister).ToArray(), processedActionResponse.targetOutcomes.uid));
                    }, 20f, CandidApiManager.Instance.transform);
                }

                //TOKENS
                if (processedActionResponse.targetOutcomes.tokens.Count > 0)
                {
                    TokenUtil.IncrementTokenByDecimal(processedActionResponse.targetOutcomes.uid, processedActionResponse.targetOutcomes.tokens.Map<TransferIcrc, (string canister, double decimalAmount)>(e => (e.Canister, e.Quantity)).ToArray());
                }
            }
            //WORLD OUTCOMES
            if (processedActionResponse.worldOutcomes != null)
            {
                if (processedActionResponse.worldOutcomes.entityEdits.Count > 0)
                {
                    EntityUtil.ApplyEntityEdits(processedActionResponse.worldOutcomes);
                    outcomeUids.Add(processedActionResponse.worldOutcomes.uid);
                }

                //NFTS
                if (processedActionResponse.worldOutcomes.nfts.Count > 0)
                {
                    NftUtil.TryAddMintedNft(processedActionResponse.worldOutcomes.uid, processedActionResponse.worldOutcomes.nfts.ToArray());

                    CoroutineManagerUtil.DelayAction(() =>
                    {
                        UserUtil.RequestData(new DataTypeRequestArgs.NftCollection(processedActionResponse.worldOutcomes.nfts.Map(e => e.Canister).ToArray(), processedActionResponse.worldOutcomes.uid));
                    }, 20f, CandidApiManager.Instance.transform);
                }

                //TOKENS
                if (processedActionResponse.worldOutcomes.tokens.Count > 0)
                {
                    TokenUtil.IncrementTokenByDecimal(processedActionResponse.worldOutcomes.uid, processedActionResponse.worldOutcomes.tokens.Map<TransferIcrc, (string canister, double decimalAmount)>(e => (e.Canister, e.Quantity)).ToArray());
                }
            }

            if (outcomeUids.Count > 0)
            {
                CoroutineManagerUtil.DelayAction(() =>
                {
                     UserUtil.RequestData(new DataTypeRequestArgs.Entity(outcomeUids.ToArray()));
                }, 20f, CandidApiManager.Instance.transform);
            }
        }
        //else
        //{
        //    //CALLER OUTCOMES
        //    if (processedActionResponse.callerOutcomes != null)
        //    {
        //        if (processedActionResponse.callerOutcomes.entityEdits.Count > 0)
        //        {
        //            EntityUtil.ApplyEntityEdits(processedActionResponse.callerOutcomes);
        //        }

        //        //NFTS
        //        if (processedActionResponse.callerOutcomes.nfts.Count > 0)
        //        {
        //            NftUtil.TryAddMintedNft(processedActionResponse.callerOutcomes.uid, processedActionResponse.callerOutcomes.nfts.ToArray());
        //        }

        //        //TOKENS
        //        if (processedActionResponse.callerOutcomes.tokens.Count > 0)
        //        {
        //            TokenUtil.IncrementTokenByDecimal(processedActionResponse.callerOutcomes.uid, processedActionResponse.callerOutcomes.tokens.Map<TransferIcrc, (string canister, double decimalAmount)>(e => (e.Canister, e.Quantity)).ToArray());
        //        }
        //    }
        //    //TARGET OUTCOMES
        //    if (processedActionResponse.targetOutcomes != null)
        //    {
        //        if (processedActionResponse.targetOutcomes.entityEdits.Count > 0)
        //        {
        //            EntityUtil.ApplyEntityEdits(processedActionResponse.targetOutcomes);
        //        }

        //        //NFTS
        //        if (processedActionResponse.targetOutcomes.nfts.Count > 0)
        //        {
        //            NftUtil.TryAddMintedNft(processedActionResponse.targetOutcomes.uid, processedActionResponse.targetOutcomes.nfts.ToArray());
        //        }

        //        //TOKENS
        //        if (processedActionResponse.targetOutcomes.tokens.Count > 0)
        //        {
        //            TokenUtil.IncrementTokenByDecimal(processedActionResponse.targetOutcomes.uid, processedActionResponse.targetOutcomes.tokens.Map<TransferIcrc, (string canister, double decimalAmount)>(e => (e.Canister, e.Quantity)).ToArray());
        //        }
        //    }
        //    //WORLD OUTCOMES
        //    if (processedActionResponse.worldOutcomes != null)
        //    {
        //        if (processedActionResponse.worldOutcomes.entityEdits.Count > 0)
        //        {
        //            EntityUtil.ApplyEntityEdits(processedActionResponse.worldOutcomes);
        //        }

        //        //NFTS
        //        if (processedActionResponse.worldOutcomes.nfts.Count > 0)
        //        {
        //            NftUtil.TryAddMintedNft(processedActionResponse.worldOutcomes.uid, processedActionResponse.worldOutcomes.nfts.ToArray());
        //        }

        //        //TOKENS
        //        if (processedActionResponse.worldOutcomes.tokens.Count > 0)
        //        {
        //            TokenUtil.IncrementTokenByDecimal(processedActionResponse.worldOutcomes.uid, processedActionResponse.worldOutcomes.tokens.Map<TransferIcrc, (string canister, double decimalAmount)>(e => (e.Canister, e.Quantity)).ToArray());
        //        }
        //    }
        //}


        $"Action Processed Success, ActionId: {actionId}, outcome: {JsonConvert.SerializeObject(processedActionResponse)}".Log(nameof(ActionUtil));

        ReduceActionProcessCount(actionId);

        return new(processedActionResponse);
    }

    public static class Transfer
    {
        //ICP
        public async static UniTask<UResult<ulong, TransferErrType.Base>> TransferIcp(IcpTx tx)
        {
            //CHECK LOGIN
            var principalResult = UserUtil.GetPrincipal();

            if (principalResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.LogIn($"You cannot execute this function, you might not be logged in or maybe you are as anon.\n More details:\n{principalResult.AsErr()}"));

            }
            var principal = principalResult.AsOk().Value;

            //CHECK USER BALANCE

            var tokenDetailsResult = TokenUtil.GetTokenDetails(principal, Env.CanisterIds.ICP_LEDGER);

            if (tokenDetailsResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.Other(tokenDetailsResult.AsErr()));
            }

            var (token, metadata) = tokenDetailsResult.AsOk();

            var requiredBaseUnitAmount = CandidUtil.ConvertToBaseUnit(tx.Amount, metadata.decimals);

            if (token.baseUnitAmount < requiredBaseUnitAmount + metadata.fee)
            {
                return new(new TransferErrType.InsufficientBalance($"Not enough \"${Env.CanisterIds.ICP_LEDGER}\" currency. Current balance: {token.baseUnitAmount.ConvertToDecimal(metadata.decimals).NotScientificNotation()}, required balance: {tx.Amount}"));
            }

            //UPDATE LOCAL STATE
            TokenUtil.DecrementTokenByBaseUnit(principal, (Env.CanisterIds.ICP_LEDGER, requiredBaseUnitAmount + metadata.fee));

            //SETUP INTERFACE
            var tokenInterface = new IcpLedgerApiClient(UserUtil.GetAgent().AsOk(), Principal.FromText(Env.CanisterIds.ICP_LEDGER));

            //SETUP ARGS
            var toAddress = await CandidUtil.ToAddress(tx.ToPrincipal);
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
            $"Transfer to address: {toAddress},\n amount {tx.Amount},\n baseUnitAmount: {requiredBaseUnitAmount},\n decimals: {metadata.decimals},\n fee: {metadata.fee}".Log(nameof(ActionUtil));
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
                TokenUtil.IncrementTokenByBaseUnit(principal, (Env.CanisterIds.ICP_LEDGER, requiredBaseUnitAmount + metadata.fee));

                return new(new TransferErrType.Transfer($"{result.AsErr().Tag}: {result.AsErr().Value}"));
            }
        }
        //ICRC
        public async static UniTask<UResult<ulong, TransferErrType.Base>> TransferIcrc(IcrcTx tx)
        {
            //CHECK LOGIN
            if (tx.Canister == Env.CanisterIds.ICP_LEDGER) return new(new TransferErrType.Other($"You cannot use this function to transfer ICRC, try using \"{nameof(TransferIcp)}\""));

            var principalResult = UserUtil.GetPrincipal();

            if (principalResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.LogIn($"You cannot execute this function, you might not be logged in or maybe you are as anon.\n More details:\n{principalResult.AsErr()}"));

            }
            var principal = principalResult.AsOk().Value;

            //CHECK USER BALANCE
            var tokenDetailsResult = TokenUtil.GetTokenDetails(principal, tx.Canister);

            if (tokenDetailsResult.Tag == UResultTag.Err)
            {
                return new(new TransferErrType.Other(tokenDetailsResult.AsErr()));
            }

            var (token, metadata) = tokenDetailsResult.AsOk();

            var requiredBaseUnitAmount = CandidUtil.ConvertToBaseUnit(tx.Amount, metadata.decimals);

            if (token.baseUnitAmount < requiredBaseUnitAmount + metadata.fee)
            {
                return new(new TransferErrType.InsufficientBalance($"Not enough \"${tx.Canister}\" currency. Current balance: {token.baseUnitAmount.ConvertToDecimal(metadata.decimals).NotScientificNotation()}, required balance: {tx.Amount}"));
            }

            //UPDATE LOCAL STATE
            TokenUtil.DecrementTokenByBaseUnit(principal, (tx.Canister, requiredBaseUnitAmount + metadata.fee));

            //SETUP INTERFACE
            var tokenInterface = new IcrcLedgerApiClient(UserUtil.GetAgent().AsOk(), Principal.FromText(tx.Canister));

            //SETUP ARGS
            var arg = new Candid.IcrcLedger.Models.TransferArgs(
                (UnboundedUInt)requiredBaseUnitAmount,
                new(),
                new((UnboundedUInt)metadata.fee),
                new(),
                new(),
                new(Principal.FromText(tx.ToPrincipal),
                new()));

            //TRANSFER
            $"Transfer to principal: {tx.ToPrincipal},\n amount {tx.Amount},\n baseUnitAmount: {requiredBaseUnitAmount},\n decimals: {metadata.decimals},\n fee: {metadata.fee}".Log(nameof(ActionUtil));
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
                TokenUtil.IncrementTokenByBaseUnit(principal, (tx.Canister, requiredBaseUnitAmount + metadata.fee));

                return new(new TransferErrType.Transfer($"{result.AsErr().Tag}: {result.AsErr().Value}"));
            }
        }

        public async static UniTask<UResult<ulong, TransferErrType.Base>> TransferNft(string canister, string tokenIdentifier, string to)
        {
            //CHECK LOGIN

            var getLoginTypeResult = UserUtil.GetLoginType();

            if (getLoginTypeResult == UserUtil.LoginType.None)
            {
                return new(new TransferErrType.LogIn("You must log in"));
            }

            if (getLoginTypeResult == UserUtil.LoginType.Anon)
            {
                return new(new TransferErrType.LogIn("You cannot execute this function as anon"));
            }

            var uid = UserUtil.GetPrincipal().AsOk().Value;
            var fromPrincipal = Principal.FromText(uid);

            //UPDATE LOCAL STATE

            var removalResult = NftUtil.TryRemoveNftByIdentifier(uid, canister, tokenIdentifier);

            if (removalResult.IsErr)
            {
                return new(new TransferErrType.InsufficientBalance(removalResult.AsErr()));
            }
            var removedNft = removalResult.AsOk();

            //SETUP INTERFACE
            Extv2BoomApiClient tokenInterface = new(UserUtil.GetAgent().AsOk(), Principal.FromText(canister));

            //SETUP ARGS
            var arg = new Candid.Extv2Boom.Models.TransferRequest(
                amount : (UnboundedUInt)1,
                from : new(Candid.Extv2Boom.Models.UserTag.Principal, fromPrincipal),
                memo : new(),
                notify : false,
                subaccount : new(),
                to : new(Candid.Extv2Boom.Models.UserTag.Principal, Principal.FromText(to)),
                token : tokenIdentifier
            );

            //TRANSFER
            $"Transfer to principal: {to},\n nft of id: {tokenIdentifier}".Log(nameof(ActionUtil));
            var result = await tokenInterface.ExtTransfer(arg);

            //CHECK SUCCESS
            if (result.Tag == Candid.Extv2Boom.Models.TransferResponseTag.Ok)
            {
                var blockIndex = (ulong)result.AsOk();
                $"BlockIndex Transfer: {blockIndex}".Log(nameof(ActionUtil));
                return new(blockIndex);
            }
            else
            {
                NftUtil.TryAddMintedNft(uid, removedNft);
                return new(new TransferErrType.Transfer($"{result.AsErr().Tag}: {result.AsErr().Value}"));
            }
        }
    }
}