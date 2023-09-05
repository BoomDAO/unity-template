using Boom.Utility;
using Candid.World.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CandidUtil
{
    public const byte ICP_DECIMALS = 8;

    public static byte[] HexStringToByteArray(string hexString)
    {
        var bytes = new byte[hexString.Length / 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = System.Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

    public static ulong ConvertToBaseUnit(this double value, byte decimals)//Zero
    {
        var baseUnitCount = decimals == 0 ? 0 : (ulong)Mathf.Pow(10, decimals);


        return (ulong)(baseUnitCount * value);
    }
    public static double ConvertToDecimal(this ulong value, byte decimals)//Zero
    {
        var baseUnitCount = decimals == 0 ? 0 : (ulong)Mathf.Pow(10, decimals);


        return value / (double)baseUnitCount;
    }

    public static DataTypes.Entity ConvertToDataType(this Entity entity)
    {
        double? quantity = null;
        string? attribute = null;
        ulong? lastTs = null;
        if (entity.Quantity.HasValue) quantity = entity.Quantity.ValueOrDefault;
        if (entity.Attribute.HasValue) attribute = entity.Attribute.ValueOrDefault;
        if (entity.Expiration.HasValue)
        {
            lastTs = 0;
            if (entity.Expiration.ValueOrDefault.TryToUInt64(out var val)) lastTs = val;
        }
        return new DataTypes.Entity(entity.Wid, entity.Gid, entity.Eid, quantity, attribute, lastTs);
    }
    public static DataTypes.Entity[] ConvertToDataType(this IEnumerable<Entity> entities)
    {
        return entities.Map(e => e.ConvertToDataType()).ToArray();
    }
    public static DataTypes.Action ConvertToDataType(this Action action)
    {
        action.ActionCount.TryToUInt64(out var actionCount);
        action.IntervalStartTs.TryToUInt64(out var startTs);

        var newEntity = new DataTypes.Action(action.ActionId, actionCount, startTs);

        return newEntity;
    }
    public static DataTypes.Action[] ConvertToDataType(this IEnumerable<Action> entities)
    {
        return entities.Map(e => e.ConvertToDataType()).ToArray();
    }

    public static DataTypes.Entity ConvertToDataType(this ActionOutcomeOption actionOutcomeOption)
    {
        if(actionOutcomeOption.Option.Tag == ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity)
        {
            var val = actionOutcomeOption.Option.AsReceiveEntityQuantity();
            var wid = val.Wid.GetValueOrDefault();
            if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;

            return new DataTypes.Entity(string.IsNullOrEmpty(wid)? Env.CanisterIds.WORLD : wid, val.Gid, val.Eid, val.Quantity, null, null);
        }
        else if (actionOutcomeOption.Option.Tag == ActionOutcomeOption.OptionInfoTag.SpendEntityQuantity)
        {
            var val = actionOutcomeOption.Option.AsSpendEntityQuantity();
            var wid = val.Wid.GetValueOrDefault();
            if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;
            return new DataTypes.Entity(string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid, val.Gid, val.Eid, val.Quantity, null, null);
        }
        else if (actionOutcomeOption.Option.Tag == ActionOutcomeOption.OptionInfoTag.ReduceEntityExpiration)
        {
            var val = actionOutcomeOption.Option.AsReduceEntityExpiration();
            var wid = val.Wid.GetValueOrDefault();
            if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;
            return new DataTypes.Entity(string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid, val.Gid, val.Eid, null, null, (ulong)val.Duration);

        }
        else if (actionOutcomeOption.Option.Tag == ActionOutcomeOption.OptionInfoTag.RenewEntityExpiration)
        {
            var val = actionOutcomeOption.Option.AsRenewEntityExpiration();
            var wid = val.Wid.GetValueOrDefault();
            if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD; ;
            return new DataTypes.Entity(string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid, val.Gid, val.Eid, null, null, (ulong)val.Duration);

        }
        else if (actionOutcomeOption.Option.Tag == ActionOutcomeOption.OptionInfoTag.SetEntityAttribute)
        {
            var val = actionOutcomeOption.Option.AsSetEntityAttribute();
            var wid = val.Wid.GetValueOrDefault();
            if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;
            return new DataTypes.Entity(string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid, val.Gid, val.Eid, null, val.Attribute, null);

        }
        else if (actionOutcomeOption.Option.Tag == ActionOutcomeOption.OptionInfoTag.DeleteEntity)
        {
            var val = actionOutcomeOption.Option.AsDeleteEntity();
            var wid = val.Wid.GetValueOrDefault();
            if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;
            return new DataTypes.Entity(string.IsNullOrEmpty(wid) ? Env.CanisterIds.WORLD : wid, val.Gid, val.Eid, null, null, null);
        }
        return null;
    }
    public static DataTypes.Entity ConvertToDataType(this Candid.UserNode.Models.Entity entity)
    {
        double? quantity = null;
        string? attribute = null;
        ulong? lastTs = null;
        if (entity.Quantity.HasValue) quantity = entity.Quantity.ValueOrDefault;
        if (entity.Attribute.HasValue) attribute = entity.Attribute.ValueOrDefault;
        if (entity.Expiration.HasValue)
        {
            lastTs = 0;
            if (entity.Expiration.ValueOrDefault.TryToUInt64(out var val)) lastTs = val;
        }
        return new DataTypes.Entity(entity.Wid, entity.Gid, entity.Eid, quantity, attribute, lastTs);
    }
    public static DataTypes.Entity[] ConvertToDataType(this IEnumerable<Candid.UserNode.Models.Entity> entities)
    {
        return entities.Map(e => e.ConvertToDataType()).ToArray();
    }

    public static DataTypes.Action ConvertToDataType(this Candid.UserNode.Models.Action action)
    {
        action.ActionCount.TryToUInt64(out var actionCount);
        action.IntervalStartTs.TryToUInt64(out var startTs);

        var newEntity = new DataTypes.Action(action.ActionId, actionCount, startTs);

        return newEntity;
    }
    public static DataTypes.Action[] ConvertToDataType(this IEnumerable<Candid.UserNode.Models.Action> entities)
    {
        return entities.Map(e => e.ConvertToDataType()).ToArray();
    }
}
