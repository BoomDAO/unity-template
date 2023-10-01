// Ignore Spelling: metadata eid gid wid

using Boom.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class NftCollectionToFetch
{
    public string collectionId;
    public string name;
    public string description;
    public string urlLogo;
    public bool isStandard;

    public NftCollectionToFetch(string collectionId, string name, string description, string urlLogo, bool isStandard)
    {
        this.collectionId = collectionId;
        this.name = name;
        this.description = description;
        this.urlLogo = urlLogo;
        this.isStandard = isStandard;
    }
}

public static class EntityConstrainTypes
{
    public abstract class Base
    {
        protected string fieldName;
        protected Base(string wid, string gid, string eid, string constrainType, string fieldName)
        {
            Debug.Log($"### Constraint > wid: {wid} gid: {gid} eid: {eid} {constrainType} {fieldName}");
            Wid = string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid;
            Gid = gid;
            Eid = eid;
            this.fieldName = fieldName;
            ConstrainType = constrainType;
        }
        public string Wid { get; private set; }
        public string Gid { get; private set; }
        public string Eid { get; private set; }

        public string ConstrainType { get; private set; }
        public abstract bool Check(DataTypes.Entity entity);
        public string GetKey()
        {
            return $"{Wid}{Gid}{Eid}";
        }
        public abstract object GetValue();
    }

    public class EqualToString : Base
    {
        public string expectedValue;

        public EqualToString(string wid, string gid, string eid, string constrainType, string fieldName, string expectedValue) : base(wid, gid, eid, constrainType, fieldName)
        {
            this.expectedValue = expectedValue;
        }

        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<string>(fieldName, out var value))
                return false;

            return value.ToString() == expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
    public class EqualToNumber : Base
    {
        public double expectedValue;

        public EqualToNumber(string wid, string gid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, gid, eid, constrainType, fieldName)
        {
            this.expectedValue = expectedValue;
        }

        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<double>(fieldName, out var value))
                return false;

            return value == expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
   
    public class GreaterThanNumber : Base
    {
        public double expectedValue;

        public GreaterThanNumber(string wid, string gid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, gid, eid, constrainType, fieldName)
        {
            this.expectedValue = expectedValue;
        }

        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<double>(fieldName, out var value))
                return false;

            return value > expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
    public class LessThanNumber : Base
    {
        public double expectedValue;
        public LessThanNumber(string wid, string gid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, gid, eid, constrainType, fieldName)
        {
            this.expectedValue = expectedValue;
        }
        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<double>(fieldName, out var value))
                return false;

            return value < expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }

    public class GreaterThanEqualToNumber : Base
    {
        public double expectedValue;

        public GreaterThanEqualToNumber(string wid, string gid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, gid, eid, constrainType, fieldName)
        {
            this.expectedValue = expectedValue;
        }

        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<double>(fieldName, out var value))
                return false;

            return value >= expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }
    public class LessThanEqualToNumber : Base
    {
        public double expectedValue;
        public LessThanEqualToNumber(string wid, string gid, string eid, string constrainType, string fieldName, double expectedValue) : base(wid, gid, eid, fieldName, constrainType)
        {
            this.expectedValue = expectedValue;
        }
        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<double>(fieldName, out var value))
                return false;

            return value <= expectedValue;
        }

        public override object GetValue()
        {
            return expectedValue;
        }
    }

    public class GreaterThanNowTimestamp : Base
    {
        public GreaterThanNowTimestamp(string wid, string gid, string eid, string constrainType, string fieldName) : base(wid, gid, eid, constrainType, fieldName)
        {
        }
        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<ulong>(fieldName, out var value))
                return false;

            return value.NanoToMilliseconds() > MainUtil.Now();
        }

        public override object GetValue()
        {
            return null;
        }
    }
    public class LesserThanNowTimestamp : Base
    {
        public LesserThanNowTimestamp(string wid, string gid, string eid, string constrainType, string fieldName) : base(wid, gid, eid, constrainType, fieldName)
        {
        }
        public override bool Check(DataTypes.Entity entity)
        {
            if (!entity.GetFieldAs<ulong>(fieldName, out var value))
                return false;

            return value.NanoToMilliseconds() < MainUtil.Now();
        }

        public override object GetValue()
        {
            return null;
        }
    }
}

public static class ActionOutcomeTypes
{
    public abstract class Base
    {
        protected Base(Candid.World.Models.ActionOutcomeOption.OptionInfoTag outcomeType, double weight)
        {
            OutcomeType = outcomeType;
            Weight = weight;
        }
        public double Weight { get; set; }
        public Candid.World.Models.ActionOutcomeOption.OptionInfoTag OutcomeType { get; private set; }
    }

    public abstract class ActionOutcomeEditEntity : Base
    {
        public string Wid { get; private set; }
        public string Gid { get; private set; }
        public string Eid { get; private set; }

        protected ActionOutcomeEditEntity(string wid, string gid, string eid, Candid.World.Models.ActionOutcomeOption.OptionInfoTag outcomeType, double weight) : base(outcomeType, weight)
        {
            Wid = string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid;
            Gid = gid;
            Eid = eid;
        }
    }

    public class SetString : ActionOutcomeEditEntity
    {
        public string Field { get; set; }
        public string Value { get; private set; }

        public SetString(string wid, string gid, string eid, string field, string value, double weight) : base(wid, gid, eid, Candid.World.Models.ActionOutcomeOption.OptionInfoTag.SetString, weight)
        {
            Value = value;
            Field = field;
        }
    }
    public class SetNumber : ActionOutcomeEditEntity
    {
        public string Field { get; set; }
        public double Quantity { get; private set; }

        public SetNumber(string wid, string gid, string eid, string field, double quantity, double weight) : base(wid, gid, eid, Candid.World.Models.ActionOutcomeOption.OptionInfoTag.SetNumber, weight)
        {
            Quantity = quantity;
            Field = field;
        }
    }
    public class IncrementNumber : ActionOutcomeEditEntity
    {
        public string Field { get; set; }
        public double Quantity { get; private set; }

        public IncrementNumber(string wid, string gid, string eid, string field, double quantity, double weight) : base(wid, gid, eid, Candid.World.Models.ActionOutcomeOption.OptionInfoTag.IncrementNumber, weight)
        {
            Field = field;
            Quantity = quantity;
        }
    }
    public class DecrementNumber : ActionOutcomeEditEntity
    {
        public string Field { get; set; }
        public double Quantity { get; private set; }

        public DecrementNumber(string wid, string gid, string eid, string field, double quantity, double weight) : base(wid, gid, eid, Candid.World.Models.ActionOutcomeOption.OptionInfoTag.DecrementNumber, weight)
        {
            Field = field;
            Quantity = quantity;
        }
    }
    public class RenewTimestamp : ActionOutcomeEditEntity
    {
        public string Field { get; set; }
        public ulong Duration { get; private set; }

        public RenewTimestamp(string wid, string gid, string eid, string field, ulong duration, double weight) : base(wid, gid, eid, Candid.World.Models.ActionOutcomeOption.OptionInfoTag.RenewTimestamp, weight)
        {
            Field = field;
            Duration = duration;
        }
    }
    public class DeleteEntity : ActionOutcomeEditEntity
    {
        public DeleteEntity(string wid, string gid, string eid, double weight) : base(wid, gid, eid, Candid.World.Models.ActionOutcomeOption.OptionInfoTag.DeleteEntity, weight)
        {

        }
    }
    public class MintNft : Base
    {
        public string AssetId { get; set; }
        public string Canister { get; set; }
        public string Metadata { get; set; }

        public MintNft(string assetId, string canister, string metadata, double weight) : base(Candid.World.Models.ActionOutcomeOption.OptionInfoTag.MintNft, weight)
        {
            AssetId = assetId;
            Canister = canister;
            Metadata = metadata;
        }
    }
    public class MintToken : Base
    {
        public string Canister { get; set; }

        public double Quantity { get; set; }

        public MintToken(string canister, double quantity, double weight) : base(Candid.World.Models.ActionOutcomeOption.OptionInfoTag.MintToken, weight)
        {
            Canister = canister;
            Quantity = quantity;
        }
    }
}

public class ActionOutcomes
{
    public List<ActionOutcomeTypes.Base> possibleOutcomes;
    public ActionOutcomes()
    {
        this.possibleOutcomes = new();
    }
    public ActionOutcomes(List<ActionOutcomeTypes.Base> possibleOutcomes)
    {
        this.possibleOutcomes = possibleOutcomes;
    }
}

public class ActionResult
{
    public List<ActionOutcomes> outcomes;

    public ActionResult()
    {
        this.outcomes = new();
    }

    public ActionResult(List<ActionOutcomes> outcomes)
    {
        this.outcomes = outcomes;
    }

    public List<ActionOutcomeTypes.Base> GetAllPossibleOutcomes()
    {
        List<ActionOutcomeTypes.Base> allPossibleOutcomes = new();

        foreach (var outcomes in outcomes)
        {
            foreach (var possibleOutcome in outcomes.possibleOutcomes)
            {
                allPossibleOutcomes.Add(possibleOutcome);
            }
        }
        return allPossibleOutcomes;
    }

}

//

public enum EntityFieldEditType
{
    SetString,
    SetNumber,
    IncrementNumber,
    DecrementNumber,
    RenewTimestamp
}
public struct EntityFieldEditData
{
    public EntityFieldEditData(EntityFieldEditType editType, object value)
    {
        EditType = editType;
        Value = value;
    }

    public EntityFieldEditType EditType { get; set; }
    public object Value { get; set; }
}
[Preserve]
[Serializable]
public class NewEntityValues
{
    [Preserve] public string wid;
    [Preserve] public string gid;
    [Preserve] public string eid;
    [Preserve] public Dictionary<string, EntityFieldEditData> fields;
    [Preserve] public bool dispose;
    public NewEntityValues(string wid, string gid, string eid, Dictionary<string, EntityFieldEditData> fields)
    {
        this.wid = string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid;
        this.gid = gid;
        this.eid = eid;
        this.fields = fields;
        dispose = fields == null;
    }
    public string GetKey()
    {
        return $"{wid}{gid}{eid}";
    }
}

//

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
    public class TokenMetadata : Base
    {
        public string canisterId;
        public string name;
        public string symbol;
        public byte decimals;
        public ulong fee;
        public string description;
        public string urlLogo;

        public TokenMetadata(string canisterId, string name, string symbol, byte decimals, ulong fee, string description = "", string urlLogo = "")
        {
            this.canisterId = canisterId;
            this.name = name;
            this.symbol = symbol;
            this.decimals = decimals;
            this.fee = fee;
            this.description = description;
            this.urlLogo = urlLogo;
        }

        public override string GetKey()
        {
            return canisterId;
        }
    }

    [Preserve]
    [Serializable]
    public class Entity : Base
    {
        [Preserve] public string wid;
        [Preserve] public string gid;
        [Preserve] public string eid;
        [Preserve] public Dictionary<string, string> fields;

        public Entity(string wid, string gid, string eid, Dictionary<string, string> fields)
        {
            this.wid = string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid;
            this.gid = gid;
            this.eid = eid;
            this.fields = fields;
        }
        public Entity(Candid.UserNode.Models.StableEntity entity)
        {
            this.wid = string.IsNullOrEmpty(entity.Wid) ? Env.CanisterIds.WORLD : entity.Wid;
            this.gid = entity.Gid;
            this.eid = entity.Eid;
            this.fields = new();

            entity.Fields.Iterate(e =>
            {
                fields.TryAdd(e.Item1, e.Item2);
            });
        }

        public override string GetKey()
        {
            return $"{wid}{gid}{eid}";
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
        public ActionState(Candid.UserNode.Models.ActionState action)
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

        [Preserve] public string canister;
        public string name;
        public string description;
        public string urlLogo;

        [Preserve] public List<Nft> tokens = new();

        public NftCollection(string canister, string name, string description, string urlLogo)
        {
            this.canister = canister;
            this.name = name;
            this.description = description;
            this.urlLogo = urlLogo;
        }

        public override string GetKey()
        {
            return canister;
        }
    }

    [Preserve]
    [Serializable]
    public class Config : Base
    {
        public string cid;
        public string wid;
        [Preserve] public Dictionary<string, string> fields;

        public Config(string cid, string wid, Dictionary<string, string> fields)
        {
            this.wid = string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid;
            this.cid = cid;
            this.fields = fields;
        }

        public Config(Candid.World.Models.StableConfig entity, string wid)
        {
            this.wid = string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid;
            this.cid = entity.Cid;
            this.fields = new();

            entity.Fields.Iterate(e =>
            {
                fields.TryAdd(e.Item1, e.Item2);
            });
        }

        public override string GetKey()
        {
            return $"{wid}{cid}";
        }
    }

    [Preserve]
    [Serializable]
    public class Action : Base
    {
        public Action(Candid.World.Models.Action arg)
        {

            //Setup constraints
            if (arg.ActionConstraint.HasValue)
            {
                timeConstraint = arg.ActionConstraint.ValueOrDefault.TimeConstraint.ValueOrDefault;

                if(arg.ActionConstraint.ValueOrDefault.EntityConstraint.HasValue)
                {
                    entityConstraints = new();

                    foreach (var item in arg.ActionConstraint.ValueOrDefault.EntityConstraint.ValueOrDefault)
                    {
                        if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.EqualToString)
                        {
                            var expectedValue = item.Validation.AsEqualToString();
                            entityConstraints.Add(new EntityConstrainTypes.EqualToString(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.EqualToString)}", item.FieldName, expectedValue));
                        }
                        else if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.EqualToNumber)
                        {
                            var expectedValue = item.Validation.AsEqualToNumber();
                            entityConstraints.Add(new EntityConstrainTypes.EqualToNumber(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.EqualToNumber)}", item.FieldName, expectedValue));
                        }
                        else if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanNumber)
                        {
                            var expectedValue = item.Validation.AsGreaterThanNumber();
                            entityConstraints.Add(new EntityConstrainTypes.GreaterThanNumber(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.GreaterThanNumber)}", item.FieldName, expectedValue));
                        }
                        else if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanNumber)
                        {
                            var expectedValue = item.Validation.AsLessThanNumber();
                            entityConstraints.Add(new EntityConstrainTypes.LessThanNumber(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.LessThanNumber)}", item.FieldName, expectedValue));
                        }
                        else if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanEqualToNumber)
                        {
                            var expectedValue = item.Validation.AsGreaterThanEqualToNumber();
                            entityConstraints.Add(new EntityConstrainTypes.GreaterThanEqualToNumber(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.GreaterThanEqualToNumber)}", item.FieldName, expectedValue));
                        }
                        else if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanEqualToNumber)
                        {
                            var expectedValue = item.Validation.AsLessThanEqualToNumber();
                            entityConstraints.Add(new EntityConstrainTypes.LessThanEqualToNumber(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.LessThanEqualToNumber)}", item.FieldName, expectedValue));
                        }
                        else if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanNowTimestamp)
                        {
                            entityConstraints.Add(new EntityConstrainTypes.GreaterThanNowTimestamp(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.GreaterThanNowTimestamp)}", item.FieldName));
                        }
                        else if (item.Validation.Tag == Candid.World.Models.ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanNowTimestamp)
                        {
                            entityConstraints.Add(new EntityConstrainTypes.LesserThanNowTimestamp(item.Wid.ValueOrDefault, item.Gid, item.Eid, $"{nameof(EntityConstrainTypes.LesserThanNowTimestamp)}", item.FieldName));
                        }
                    }
                }
            }

            actionPlugin = arg.ActionPlugin.HasValue ? arg.ActionPlugin.ValueOrDefault : null;

            //Setup result
            actionResult = new();
            if (arg.ActionResult != null) 
            {
                List<ActionOutcomes> newOutcomes = new List< ActionOutcomes >();
                actionResult.outcomes = newOutcomes;
                foreach (var outcomes in arg.ActionResult.Outcomes)
                {
                    ActionOutcomes newPosibleOutcomes = new();
                    newOutcomes.Add(newPosibleOutcomes);

                    foreach (var outcome in outcomes.PossibleOutcomes) 
                    {
                        switch (outcome.Option.Tag)
                        {
                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.SetString:

                                var value1 = outcome.Option.AsSetString();
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.SetString(value1.Wid.ValueOrDefault, value1.Gid, value1.Eid, value1.Field, value1.Value, outcome.Weight));

                                break;
                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.SetNumber:

                                var value2 = outcome.Option.AsSetNumber();
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.SetNumber(value2.Wid.ValueOrDefault, value2.Gid, value2.Eid, value2.Field, value2.Value, outcome.Weight));

                                break;
                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.IncrementNumber:

                                var value3 = outcome.Option.AsIncrementNumber();
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.IncrementNumber(value3.Wid.ValueOrDefault, value3.Gid, value3.Eid, value3.Field, value3.Value, outcome.Weight));

                                break;
                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.DecrementNumber:

                                var value4 = outcome.Option.AsDecrementNumber();
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.DecrementNumber(value4.Wid.ValueOrDefault, value4.Gid, value4.Eid, value4.Field, value4.Value, outcome.Weight));

                                break;
                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.RenewTimestamp:

                                var value5 = outcome.Option.AsRenewTimestamp();
                                value5.Value.TryToUInt64(out var duration);
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.RenewTimestamp(value5.Wid.ValueOrDefault, value5.Gid, value5.Eid, value5.Field, duration, outcome.Weight));

                                break;

                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.DeleteEntity:

                                var value6 = outcome.Option.AsDeleteEntity();
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.DeleteEntity(value6.Wid.ValueOrDefault, value6.Gid, value6.Eid, outcome.Weight));

                                break;
                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.MintNft:

                                var value7 = outcome.Option.AsMintNft();
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.MintNft(value7.AssetId, value7.Canister, value7.Metadata, outcome.Weight));

                                break;
                            case Candid.World.Models.ActionOutcomeOption.OptionInfoTag.MintToken:

                                var value8 = outcome.Option.AsMintToken();
                                newPosibleOutcomes.possibleOutcomes.Add(new ActionOutcomeTypes.MintToken(value8.Canister, value8.Quantity, outcome.Weight));

                                break;
                        }
                    }
                }
            }


            aid = arg.Aid;
            description = arg.Description.HasValue ? arg.Description.ValueOrDefault : null;
            name = arg.Name.HasValue ? arg.Name.ValueOrDefault : null;
            tag = arg.Tag.HasValue ? arg.Tag.ValueOrDefault : null;
            imageUrl = arg.ImageUrl.HasValue ? arg.ImageUrl.ValueOrDefault : null;
        }

        public Candid.World.Models.ActionConstraint.TimeConstraintItem timeConstraint;
        public List<EntityConstrainTypes.Base> entityConstraints;
        public Candid.World.Models.ActionPlugin actionPlugin;
        public ActionResult actionResult;
        public string aid;
        public string description;
        public string name;
        public string tag;
        public string imageUrl;

        public override string GetKey()
        {
            return $"{aid}";
        }
    }

    [Preserve]
    [Serializable]
    public class Listing : Base
    {
        public Listing(string tokenIdentifier, Candid.Extv2Boom.Extv2BoomApiClient.ListingsArg0Item arg)
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

        public override string GetKey()
        {
            return $"{index}";
        }
    }
}


