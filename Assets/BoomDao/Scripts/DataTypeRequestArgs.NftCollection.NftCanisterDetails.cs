// Ignore Spelling: metadata eid gid wid

using Boom.Patterns.Broadcasts;
using Boom.Utility;
using Candid;
using Candid.World.Models;
using EdjCase.ICP.Agent.Agents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using static MainDataTypes.AllNftCollectionConfig;

public class NftCollectionToFetch
{
    public string collectionId;

    public NftCollectionToFetch(string collectionId)
    {
        this.collectionId = collectionId;
    }
}

public static class EntityConstrainTypes
{
    public abstract class Base
    {
        protected Base(string wid, string eid, string constrainType)
        {
            //Debug.Log($"### Constraint > wid: {wid} gid: {gid} eid: {eid} {constrainType} {fieldName}");
            Wid = string.IsNullOrEmpty(wid) ? CandidApiManager.Instance.WORLD_CANISTER_ID : wid;
            Eid = eid;
            ConstrainType = constrainType;
        }
        public string Wid { get; private set; }
        public string Eid { get; private set; }

        public string ConstrainType { get; private set; }
        public abstract bool Check(Dictionary<string, DataTypes.Entity> entities);
        public string GetKey()
        {
            return $"{Wid}{Eid}";
        }
        public abstract object GetValue();
    }

    public class EqualToText : Base
    {
        protected string fieldName;

        public string expectedValue;

        public EqualToText(string wid, string eid, string constrainType, string fieldName, string expectedValue) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;

            this.expectedValue = expectedValue;
        }

        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            return field.ToString() == expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
    public class ContainsText : Base
    {
        protected string fieldName;

        public string expectedValue;

        public ContainsText(string wid, string eid, string constrainType, string fieldName, string expectedValue) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;

            this.expectedValue = expectedValue;
        }

        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            return field.Contains(expectedValue);
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
    public class EqualToNumber : Base
    {
        protected string fieldName;

        public double expectedValue;

        public EqualToNumber(string wid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;

            this.expectedValue = expectedValue;
        }

        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            if (field.TryParseValue<double>(out var value) == false)
            {
                Debug.LogError("Failed when parsing field value");
                return false;
            }

            return value == expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }

    public class GreaterThanNumber : Base
    {
        protected string fieldName;

        public double expectedValue;

        public GreaterThanNumber(string wid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;

            this.expectedValue = expectedValue;
        }

        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            if (field.TryParseValue<double>(out var value) == false)
            {
                Debug.LogError("Failed when parsing field value");
                return false;
            }

            return value > expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
    public class LessThanNumber : Base
    {
        protected string fieldName;

        public double expectedValue;
        public LessThanNumber(string wid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;

            this.expectedValue = expectedValue;
        }
        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            if (field.TryParseValue<double>(out var value) == false)
            {
                Debug.LogError("Failed when parsing field value");
                return false;
            }

            return value < expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }

    public class GreaterThanEqualToNumber : Base
    {
        protected string fieldName;

        public double expectedValue;

        public GreaterThanEqualToNumber(string wid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;

            this.expectedValue = expectedValue;
        }

        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            if (field.TryParseValue<double>(out var value) == false)
            {
                Debug.LogError("Failed when parsing field value");
                return false;
            }

            return value >= expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
    public class LessThanEqualToNumber : Base
    {
        protected string fieldName;

        public double expectedValue;
        public LessThanEqualToNumber(string wid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;

            this.expectedValue = expectedValue;
        }
        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            if (field.TryParseValue<double>(out var value) == false)
            {
                Debug.LogError("Failed when parsing field value");
                return false;
            }

            return value <= expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }

    public class GreaterThanNowTimestamp : Base
    {
        protected string fieldName;

        public GreaterThanNowTimestamp(string wid, string eid, string constrainType, string fieldName) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;
        }
        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            if (field.TryParseValue<ulong>(out var value) == false)
            {
                Debug.LogError("Failed when parsing field value");
                return false;
            }

            return value.NanoToMilliseconds() > MainUtil.Now();
        }

        public override object GetValue()
        {
            return null;
        }
    }
    public class LesserThanNowTimestamp : Base
    {
        protected string fieldName;

        public LesserThanNowTimestamp(string wid, string eid, string constrainType, string fieldName) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;
        }
        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {

            if (entities.TryGetValue(Eid, out var entity) == false)
            {
                Debug.LogError("Failed when finding entity");

                return false;
            }

            if (entity.fields.TryGetValue(fieldName, out var field) == false)
            {
                Debug.LogError("Failed when finding field");

                return false;
            }

            if (field.TryParseValue <ulong>(out var value) == false)
            {
                Debug.LogError("Failed when parsing field value");
                return false;
            }

            return value.NanoToMilliseconds() < MainUtil.Now();
        }

        public override object GetValue()
        {
            return null;
        }
    }

    public class ExistField : Base
    {
        protected string fieldName;

        public bool value;
        public ExistField(string wid, string eid, string constrainType, string fieldName, bool value) : base(wid, eid, constrainType)
        {
            this.fieldName = fieldName;
            this.value = value;
        }

        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.TryGetValue(Eid, out var entity) == false) return false;

            if (entity.fields.TryGetValue(fieldName, out var field) == false) return false;

            return true;
        }

        public override object GetValue()
        {
            return value;
        }
    }
    public class Exist : Base
    {
        public bool value;

        public Exist(string wid, string eid, string constrainType, bool value) : base(wid, eid, constrainType)
        {
            this.value = value;
        }

        public override bool Check(Dictionary<string, DataTypes.Entity> entities)
        {
            if (entities.ContainsKey(Eid)) return true;
            return false;
        }

        public override object GetValue()
        {
            return value;
        }
    }
}

public class TimeConstraint
{
    public class ActionTimeInterval
    {
        public ActionTimeInterval(ulong actionsPerInterval, ulong intervalDuration)
        {
            ActionsPerInterval = actionsPerInterval;
            IntervalDuration = intervalDuration;
        }

        public ulong ActionsPerInterval { get; set; }
        public ulong IntervalDuration { get; set; }
    }
    public TimeConstraint(Candid.World.Models.ActionConstraint.TimeConstraintValue timeConstraint)
    {
        if (timeConstraint.ActionTimeInterval.HasValue)
        {
            var _actionTimeInterval = timeConstraint.ActionTimeInterval.ValueOrDefault;
            _actionTimeInterval.ActionsPerInterval.TryToUInt64(out ulong actionsPerInterval);
            _actionTimeInterval.IntervalDuration.TryToUInt64(out ulong intervalDuration);
            actionTimeInterval = new(actionsPerInterval, intervalDuration);
        }

        if (timeConstraint.ActionExpirationTimestamp.HasValue)
        {
            timeConstraint.ActionExpirationTimestamp.ValueOrDefault.TryToUInt64(out ulong _actionExpirationTimestamp);
            actionExpirationTimestamp = actionExpirationTimestamp;
        }
        else actionExpirationTimestamp = null;
    }

    public ulong? actionExpirationTimestamp { get; set; }
    public ActionTimeInterval actionTimeInterval { get; set; }

}
public class SubAction
{
    public List<ActionOutcome> Outcomes { get; set; }

    public bool HasConstraint { get; set; }

    public TimeConstraint TimeConstraint { get; set; }
    public List<EntityConstrainTypes.Base> EntityConstraints { get; set; }
    public IcpTx IcpConstraint { get; set; }
    public List<IcrcTx> IcrcConstraint { get; set; }
    public List<NftTx> NftConstraint { get; set; }
    public ActionResult ActionResult { get; set; }

    public SubAction(Candid.World.Models.SubAction subAction)
    {
        HasConstraint = subAction.ActionConstraint.HasValue;
        Outcomes = subAction.ActionResult.Outcomes;

        if (HasConstraint)
        {
            var constraints = subAction.ActionConstraint.GetValueOrDefault();
            //SETUP TIME CONSTRAINT
            if (constraints.TimeConstraint.HasValue) TimeConstraint = new(constraints.TimeConstraint.ValueOrDefault);

            //SETUP ENTITY CONSTRAINT
            EntityConstraints = new();
            foreach (var item in constraints.EntityConstraint)
            {
                if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.EqualToText)
                {
                    var expectedValue = item.EntityConstraintType.AsEqualToText();
                    this.EntityConstraints.Add(new EntityConstrainTypes.EqualToText(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.EqualToText)}", expectedValue.FieldName, expectedValue.Value));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.ContainsText)
                {
                    var expectedValue = item.EntityConstraintType.AsContainsText();
                    this.EntityConstraints.Add(new EntityConstrainTypes.ContainsText(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.ContainsText)}", expectedValue.FieldName, expectedValue.Value));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.EqualToNumber)
                {
                    var expectedValue = item.EntityConstraintType.AsEqualToNumber();
                    this.EntityConstraints.Add(new EntityConstrainTypes.EqualToNumber(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.EqualToNumber)}", expectedValue.FieldName, expectedValue.Value));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.GreaterThanNumber)
                {
                    var expectedValue = item.EntityConstraintType.AsGreaterThanNumber();
                    this.EntityConstraints.Add(new EntityConstrainTypes.GreaterThanNumber(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.GreaterThanNumber)}", expectedValue.FieldName, expectedValue.Value));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.LessThanNumber)
                {
                    var expectedValue = item.EntityConstraintType.AsLessThanNumber();
                    this.EntityConstraints.Add(new EntityConstrainTypes.LessThanNumber(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.LessThanNumber)}", expectedValue.FieldName, expectedValue.Value));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.GreaterThanEqualToNumber)
                {
                    var expectedValue = item.EntityConstraintType.AsGreaterThanEqualToNumber();
                    this.EntityConstraints.Add(new EntityConstrainTypes.GreaterThanEqualToNumber(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.GreaterThanEqualToNumber)}", expectedValue.FieldName, expectedValue.Value));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.LessThanEqualToNumber)
                {
                    var expectedValue = item.EntityConstraintType.AsLessThanEqualToNumber();
                    this.EntityConstraints.Add(new EntityConstrainTypes.LessThanEqualToNumber(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.LessThanEqualToNumber)}", expectedValue.FieldName, expectedValue.Value));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.GreaterThanNowTimestamp)
                {
                    var expectedValue = item.EntityConstraintType.AsGreaterThanNowTimestamp();

                    this.EntityConstraints.Add(new EntityConstrainTypes.GreaterThanNowTimestamp(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.GreaterThanNowTimestamp)}", expectedValue.FieldName));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.LessThanNowTimestamp)
                {
                    var expectedValue = item.EntityConstraintType.AsLessThanNowTimestamp();

                    this.EntityConstraints.Add(new EntityConstrainTypes.LesserThanNowTimestamp(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.LesserThanNowTimestamp)}", expectedValue.FieldName));
                }
                else if (item.EntityConstraintType.Tag == EntityConstraintTypeTag.ExistField)
                {
                    var expectedValue = item.EntityConstraintType.AsExistField();
                    this.EntityConstraints.Add(new EntityConstrainTypes.ExistField(item.Wid.ValueOrDefault, item.Eid, $"{nameof(EntityConstrainTypes.ExistField)}", expectedValue.FieldName, expectedValue.Value));
                }
            }

            if (constraints.IcpConstraint.HasValue) IcpConstraint = constraints.IcpConstraint.ValueOrDefault;
            IcrcConstraint = constraints.IcrcConstraint;
            NftConstraint = new();

            constraints.NftConstraint.Iterate(e =>
            {
                if (e.NftConstraintType.Tag == NftTx.NftConstraintTypeInfoTag.Transfer)
                {
                    var transferConstraint = e.NftConstraintType.AsTransfer();

                    if(string.IsNullOrEmpty(transferConstraint.ToPrincipal)) NftConstraint.Add(new(e.Canister, e.Metadata, new NftTx.NftConstraintTypeInfo(NftTx.NftConstraintTypeInfoTag.Transfer, new NftTransfer("0000000000000000000000000000000000000000000000000000001"))));
                    else NftConstraint.Add(e);
                }
                else
                {
                    NftConstraint.Add(e);
                }
            });
        }

        ActionResult = subAction.ActionResult;
    }
}

//

public static class EntityFieldEdit
{
    public abstract class Base { }

    public class SetText : Base
    {
        public string Value { get; set; }

        public SetText(string value)
        {
            Value = value;
        }
    }
    public class ReplaceText : Base
    {
        public ReplaceText(string oldText, string newText)
        {
            OldText = oldText;
            NewText = newText;
        }

        public string OldText { get; set; }
        public string NewText { get; set; }
    }


    public abstract class Numeric : Base
    { 
        public static class ValueType
        {
            public abstract class Base { }
            public class Number : Base
            {
                public double Value { get; set; }

                public Number(double value)
                {
                    Value = value;
                }
            }
            public class Formula : Base
            {
                public string Value { get; set; }

                public Formula(string value)
                {
                    Value = value;
                }
            }
        }

        public ValueType.Base Value { get; set; }

        protected Numeric(ValueType.Base value)
        {
            Value = value;
        }
    }

    public class SetNumber : Numeric
    {
        public SetNumber(ValueType.Base value) : base(value)
        {
        }
    }
    public class IncrementNumber : Numeric
    {
        public IncrementNumber(ValueType.Base value) : base(value)
        {
        }
    }
    public class DecrementNumber : Numeric
    {
        public DecrementNumber(ValueType.Base value) : base(value)
        {
        }
    }
    public class RenewTimestamp : Numeric
    {
        public RenewTimestamp(ValueType.Base value) : base(value)
        {
        }
    }
}
[Preserve]
[Serializable]
public class NewEntityEdits
{
    [Preserve] public string wid;
    [Preserve] public string eid;
    [Preserve] public Dictionary<string, EntityFieldEdit.Base> fields;
    [Preserve] public bool dispose;
    public NewEntityEdits(string wid, string eid, Dictionary<string, EntityFieldEdit.Base> fields)
    {
        this.wid = string.IsNullOrEmpty(wid) ? CandidApiManager.Instance.WORLD_CANISTER_ID : wid;
        this.eid = eid;
        this.fields = fields;
        dispose = fields == null;
    }
    public string GetKey()
    {
        return $"{wid}{eid}";
    }
}

//


public static class DataTypeRequestArgs
{
    public class Base
    {
        public string[] uids;

        protected Base(params string[] usersUid)
        {
            this.uids = usersUid;
        }
    }

    internal class Entity : Base
    {
        public Entity() : base("self") { }
        public Entity(params string[] usersUid) : base(usersUid)
        {
        }
    }

    internal class ActionState : Base
    {
        public ActionState() : base("self") { }
        public ActionState(params string[] usersUid) : base(usersUid)
        {
        }
    }

    internal class Token : Base
    {
        public readonly string[] canisterIds;
        public Token() : base("self") { }
        public Token(string[] canisterIds, params string[] usersUid) : base(usersUid)
        {
            this.canisterIds = canisterIds;
        }
    }

    internal class NftCollection : Base
    {
        public readonly string []canisterIds;
        public NftCollection() : base("self") { }
        public NftCollection(string[] canisterIds, params string[] usersUid) : base(usersUid)
        {
            this.canisterIds = canisterIds;
        }
    }
}

public static class DataTypes
{
    public abstract class Base : Boom.IDisposable
    {
        public bool isScheduleForDisposal;

        public abstract string GetKey();

        public void ScheduleDisposal()
        {
            isScheduleForDisposal = true;
        }

        public bool CanDispose()
        {
            return isScheduleForDisposal;
        }
    }

    [Preserve]
    [Serializable]
    public class Entity : Base
    {
        [Preserve] public string wid;
        [Preserve] public string eid;
        [Preserve] public Dictionary<string, string> fields;

        public Entity(string wid, string eid, Dictionary<string, string> fields)
        {
            this.wid = string.IsNullOrEmpty(wid) ? CandidApiManager.Instance.WORLD_CANISTER_ID : wid;
            this.eid = eid;
            this.fields = fields;
        }
        public Entity(string wid, Candid.World.Models.StableEntity entity)
        {
            this.wid = wid;
            this.eid = entity.Eid;
            this.fields = new();

            foreach(var field in entity.Fields)
            {
                fields.Add(field.FieldName, field.FieldValue);
            }
        }

        public override string GetKey()
        {
            return $"{eid}";
        }
    }

    [Preserve]
    [Serializable]
    public class ActionState : Base
    {
        public string actionId;
        public ulong actionCount;
        public ulong intervalStartTs;

        public ActionState(string actionId, ulong actionCount, ulong intervalStartTs)
        {
            this.actionId = actionId;
            this.actionCount = actionCount;
            this.intervalStartTs = intervalStartTs;
        }
        public ActionState(Candid.World.Models.ActionState action)
        {
            this.actionId = action.ActionId;
            action.ActionCount.TryToUInt64(out actionCount);
            action.IntervalStartTs.TryToUInt64(out intervalStartTs);
        }

        public override string GetKey()
        {
            return actionId;
        }
    }

    [Preserve]
    [Serializable]
    public class Token : Base
    {
        public string canisterId;
        public ulong baseUnitAmount;
        public Token(string canisterId, ulong baseUnitAmount)
        {
            this.canisterId = canisterId;
            this.baseUnitAmount = baseUnitAmount;
        }

        public override string GetKey()
        {
            return canisterId;
        }
    }

    [Preserve]
    [Serializable]
    public class NftCollection : Base
    {
        [Preserve]
        [Serializable]
        public class Nft
        {
            [Preserve] public string canister;
            [Preserve] public uint index;
            [Preserve] public string tokenIdentifier;
            public string url;
            [Preserve] public string metadata;

            public Nft(string canister, uint index, string tokenIdentifier, string url, string metadata)
            {
                this.canister = canister;
                this.index = index;
                this.tokenIdentifier = tokenIdentifier;
                this.url = url;
                this.metadata = metadata;
            }
        }

        [Preserve] public string canisterId;


        [Preserve] public List<Nft> tokens = new();

        public NftCollection(string canister)
        {
            this.canisterId = canister;
        }

        public override string GetKey()
        {
            return canisterId;
        }
    }
}

public class MainDataTypes
{
    public abstract class Base : IBroadcastState
    {
        protected Base()
        {
        }
    }


    [Preserve]
    [Serializable]
    public class AllConfigs : Base
    {
        public class Config
        {
            public string cid;
            [Preserve] public Dictionary<string, string> fields;

            public Config(string cid, Dictionary<string, string> fields)
            {
                this.cid = cid;
                this.fields = fields;
            }

            public Config(Candid.World.Models.StableConfig entity)
            {
                this.cid = entity.Cid;
                this.fields = new();

                foreach (var field in entity.Fields)
                {
                    fields.Add(field.FieldName, field.FieldValue);
                }
            }
        }

        public Dictionary<string, Dictionary<string, Config>> configs; //worldId -> cid -> config

        public AllConfigs()
        {
            this.configs = new();
        }
        public AllConfigs(Dictionary<string, Dictionary<string, Config>> configs)
        {
            this.configs = configs;
        }
    }

    [Preserve]
    [Serializable]
    public class AllAction : Base
    {
        public class Action
        {
            public Action(Candid.World.Models.Action arg)
            {
                //Setup constraints
                callerAction = arg.CallerAction.HasValue ? new(arg.CallerAction.ValueOrDefault) : null;
                targetAction = arg.TargetAction.HasValue ? new(arg.TargetAction.ValueOrDefault) : null;
                worldAction = arg.WorldAction.HasValue ? new(arg.WorldAction.ValueOrDefault) : null;

                aid = arg.Aid;
            }

            public SubAction callerAction;
            public SubAction targetAction;
            public SubAction worldAction;

            public string aid;
        }

        public Dictionary<string, Dictionary<string, Action>> actions; //worldId -> aid -> action

        public AllAction()
        {
            this.actions = new();
        }

        public AllAction(Dictionary<string, Dictionary<string, Action>> actions)
        {
            this.actions = actions;
        }
    }

    [Preserve]
    [Serializable]
    public class AllTokenConfigs : Base
    {
        public class TokenConfig
        {
            public string canisterId;
            public string name;
            public string symbol;
            public byte decimals;
            public ulong fee;
            public string description;
            public string urlLogo;

            public TokenConfig() { }
            public TokenConfig(string canisterId, string name, string symbol, byte decimals, ulong fee, string description = "", string urlLogo = "")
            {
                this.canisterId = canisterId;
                this.name = name;
                this.symbol = symbol;
                this.decimals = decimals;
                this.fee = fee;
                this.description = description;
                this.urlLogo = urlLogo;
            }
        }

        public Dictionary<string, TokenConfig> configs; //canisterId -> config

        public AllTokenConfigs() { configs = new(); }

        public AllTokenConfigs(Dictionary<string, TokenConfig> configs)
        {
            this.configs = configs;
        }
    }


    public class AllNftCollectionConfig : Base
    {
        [Preserve]
        [Serializable]
        public class NftConfig
        {

            [Preserve] public string canisterId;
            public bool isBoomDaoStandard;
            public string name;
            public string description;
            public string urlLogo;

            public NftConfig() { }
            public NftConfig(string canister, bool isBoomDaoStandard, string name, string description, string urlLogo)
            {
                this.canisterId = canister;
                this.isBoomDaoStandard = isBoomDaoStandard;

                this.name = name;
                this.description = description;
                this.urlLogo = urlLogo;
            }
        }

        public Dictionary<string, NftConfig> configs; //canisterId -> config

        public AllNftCollectionConfig() { configs = new(); }

        public AllNftCollectionConfig(Dictionary<string, NftConfig> configs)
        {
            this.configs = configs;
        }
    }

    [Preserve]
    [Serializable]
    public class LoginData : Base
    {
        public enum State
        {
            Logedout,
            LoginRequested,
            LoggedIn,
            LoggedInAsAnon,
        }
        public IAgent agent;
        public string principal;
        public string accountIdentifier;
        public State state;
        public long updateTs;
        public LoginData() 
        {
            this.state = State.Logedout;
        }
        public LoginData(IAgent agent, string principal, string accountIdentifier, State state)
        {
            this.agent = agent;
            this.principal = principal;
            this.accountIdentifier = accountIdentifier;
            this.state = state;
            updateTs = MainUtil.Now();
        }
    }

    [Preserve]
    [Serializable]
    public class AllRoomData : Base
    {
        public class RoomData
        {
            public string roomId;
            public int userCount;
            public string[] users;

            public RoomData(string roomId, string[] users)
            {
                this.roomId = roomId;
                this.userCount = users.Length;
                this.users = users;
            }
        }
        public bool inRoom;
        public string currentRoomId;
        public RoomData currentRoom;
        public Dictionary<string, RoomData> rooms = new();
        public long updateTs;

        public AllRoomData() { }
        public AllRoomData(IEnumerable<DataTypes.Entity> roomEntities)
        {
            updateTs = MainUtil.Now();

            //
            if (UserUtil.IsUserLoggedIn(out var loginData) == false)
            {
                return;
            }

            foreach (var roomEntity in roomEntities)
            {
                roomEntity.GetFieldAsDouble("userCount", out var userCount);

                if (userCount > 0)
                {
                    if (roomEntity.GetFieldAsString("users", out var users))
                    {
                        var usersInRoom = users.Split(',');
                        var room = new RoomData(roomEntity.eid, usersInRoom);
                        this.rooms.TryAdd(roomEntity.eid, room);

                        if (users.Contains(loginData.principal))
                        {
                            currentRoomId = roomEntity.eid;
                            currentRoom = room;
                            inRoom = true;
                            break;
                        }
                    }
                }
            }
        }

        public string[] GetAllUsersInCurrentRoom()
        {
            if (inRoom)
            {
                if (rooms.TryGetValue(currentRoomId, out var room)) return room.users;
                else Debug.LogWarning("Could not find users of room id: " + currentRoomId);
            }
            return null;
        }
    }

    [Preserve]
    [Serializable]
    public class AllListings : Base
    {
        [Preserve]
        [Serializable]
        public class Listing : Base
        {
            public Listing(string tokenIdentifier, Candid.Extv2Boom.Extv2BoomApiClient.ListingsReturnArg0.ListingsReturnArg0Element arg)
            {
                this.tokenIdentifier = tokenIdentifier;
                index = arg.F0;
                details = arg.F1;
                metadataLegacy = arg.F2;
            }

            public string tokenIdentifier;
            public uint index;
            public Candid.Extv2Boom.Models.Listing details;
            public Candid.Extv2Boom.Models.MetadataLegacy metadataLegacy;
        }

        public Dictionary<uint, Listing> listings;

        public AllListings()
        {
        }
        public AllListings(Dictionary<uint, Listing> listings)
        {
            this.listings = listings;
        }
    }
}






