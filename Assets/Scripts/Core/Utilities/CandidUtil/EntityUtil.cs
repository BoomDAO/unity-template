
using Boom.Values;
using Candid.World.Models;
using UnityEngine;
using static Candid.World.Models.ActionOutcomeOption.OptionInfo;
// Ignore Spelling: Util

internal static class EntityUtil
{
    public static string GetKey(this Entity entity)
    {
        return $"{entity.Wid}{entity.Gid}{entity.Eid}";
    }
    public static string GetKey(this ReceiveEntityQuantityInfo entity)
    {
        var wid = entity.F0.GetValueOrDefault();

        if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;
        return $"{wid}{entity.F1}{entity.F2}";
    }
    //HAS CONFIG
    public static bool HasConfig(string entityId, out DataTypes.EntityConfig config)
    {
        config = default;
        var result = UserUtil.GetElementOfType<DataTypes.EntityConfig>(entityId);
        if (result.IsOk) config = result.AsOk();
        return result.IsOk;
    }

    // GET NAME
    public static string GetName(string entityId, string defaultValue = "None")
    {
        if (HasConfig(entityId, out var config))
        {
            if (config.Name.HasValue) return config.Name.ValueOrDefault;
        }
        return defaultValue;
    }

    // GET DESCRIPTION
    public static string GetDescription(string entityId, string defaultValue = "None")
    {
        if (HasConfig(entityId, out var config))
        {
            if (config.Description.HasValue) return config.Description.ValueOrDefault;
        }
        return defaultValue;
    }

    // GET IMAGE URL
    public static string GetImageUrl(string entityId, string defaultValue = "None")
    {
        if (HasConfig(entityId, out var config))
        {
            if (config.ImageUrl.HasValue) return config.ImageUrl.ValueOrDefault;
        }
        return defaultValue;
    }

    // GET OBJECT URL
    public static string GetObjetUrl(string entityId, string defaultValue = "None")
    {
        if (HasConfig(entityId, out var config))
        {
            if (config.ObjectUrl.HasValue) return config.ObjectUrl.ValueOrDefault;
        }
        return defaultValue;
    }

    // GET RARITY
    public static string GetRarity(string entityId, string defaultValue = "None")
    {
        if (HasConfig(entityId, out var config))
        {
            if (config.Rarity.HasValue) return config.Rarity.ValueOrDefault;
        }
        return defaultValue;
    }

    // GET TAG
    public static string GetTag(string entityId, string defaultValue = "None")
    {
        if (HasConfig(entityId, out var config))
        {
            if(string.IsNullOrEmpty(config.Tag)) return defaultValue;

            return config.Tag;
        }
        return defaultValue;
    }

    // GET MERADATA
    public static string GetMetadata(string entityId, string defaultValue = "None")
    {
        if (HasConfig(entityId, out var config))
        {
            if (string.IsNullOrEmpty(config.Metadata)) return defaultValue;

            return config.Metadata;
        }
        return defaultValue;
    }

    // GET QUANTITY
    public static double GetCurrentQuantity(string emtityId)
    {
        var result = UserUtil.GetElementOfType<DataTypes.Entity>(emtityId);
        if (result.IsErr) return default;

        return result.AsOk().quantity.GetValueOrDefault();
    }
    public static double GetQuantity(this Entity entity)
    {
        return entity.Quantity.ValueOrDefault;
    }


    /// <summary>
    /// Will return true if all requirements are met
    /// </summary>
    /// <param name="constrains"></param>
    /// <returns></returns>
    public static UResult<bool, string> MeetEntityRequirements(params ActionConstraint.EntityConstraintItemItem[] constrains)
    {
        var data = UserUtil.GetElementsOfType<DataTypes.Entity>();

        if (data.Tag == UResultTag.Err) return new(data.AsErr());

        var asOk = data.AsOk();

        foreach (var constrain in constrains)
        {
            bool doContinue = false;
            foreach (var ownEntity in asOk)
            {
                if ($"{ownEntity.wid}{ownEntity.gid}{ownEntity.eid}" == $"{constrain.WorldId}{constrain.GroupId}{constrain.EntityId}")
                {
                    int a = 0, b = 0;

                    if (constrain.GreaterThanOrEqualQuantity.HasValue)
                    {
                        ++a;
                        if (constrain.GreaterThanOrEqualQuantity.ValueOrDefault <= ownEntity.quantity)
                        {
                            ++b;
                        }
                    }

                    if (constrain.LessThanQuantity.HasValue)
                    {
                        ++a;
                        if (constrain.LessThanQuantity.ValueOrDefault > ownEntity.quantity)
                        {
                            ++b;
                        }
                    }

                    if (constrain.EqualToAttribute.HasValue)
                    {
                        ++a;
                        if (constrain.EqualToAttribute.ValueOrDefault == ownEntity.attribute)
                        {
                            ++b;
                        }
                    }

                    if (constrain.NotExpired.HasValue)
                    {
                        ++a;
                        if (constrain.NotExpired.ValueOrDefault ? ownEntity.lastTs >= Time.time : ownEntity.lastTs < Time.time)
                        {
                            ++b;
                        }
                    }

                    doContinue = a == b;
                    break;
                }
            }

            if (doContinue) continue;

            return new(false);
        }

        return new(true);
    }
}