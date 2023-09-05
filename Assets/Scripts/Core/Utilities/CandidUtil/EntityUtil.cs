
using Boom.Values;
using Candid.World.Models;
using System.Collections.Generic;
using UnityEngine;
// Ignore Spelling: Util

internal static class EntityUtil
{
    public static string GetKey(this ActionConstraint.EntityConstraintItemItem actionConfig)
    {
        return $"{actionConfig.Wid}{actionConfig.Gid}{actionConfig.Eid}";
    }
    public static string GetKey(this Entity entity)
    {
        return $"{entity.Wid}{entity.Gid}{entity.Eid}";
    }
    public static string GetKey(this ReceiveEntityQuantity entity)
    {
        var wid = entity.Wid.GetValueOrDefault();
        if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;

        return $"{wid}{entity.Gid}{entity.Eid}";
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
    public static string GetName(string key, string defaultValue = "None")
    {
        if (HasConfig(key, out var config))
        {
            if (string.IsNullOrEmpty(config.name) == false) return config.name;
        }
        return defaultValue;
    }

    // GET DESCRIPTION
    public static string GetDescription(string key, string defaultValue = "None")
    {
        if (HasConfig(key, out var config))
        {
            if (string.IsNullOrEmpty(config.description) == false) return config.description;
        }
        return defaultValue;
    }

    // GET IMAGE URL
    public static string GetImageUrl(string key, string defaultValue = "None")
    {
        if (HasConfig(key, out var config))
        {
            if (string.IsNullOrEmpty(config.imageUrl) == false) return config.imageUrl;
        }
        return defaultValue;
    }

    // GET OBJECT URL
    public static string GetObjetUrl(string key, string defaultValue = "None")
    {
        if (HasConfig(key, out var config))
        {
            if (string.IsNullOrEmpty(config.objectUrl) == false) return config.objectUrl;
        }
        return defaultValue;
    }

    // GET RARITY
    public static string GetRarity(string key, string defaultValue = "None")
    {
        if (HasConfig(key, out var config))
        {
            if (string.IsNullOrEmpty(config.rarity) == false) return config.rarity;
        }
        return defaultValue;
    }

    // GET TAG
    public static string GetTag(string key, string defaultValue = "None")
    {
        if (HasConfig(key, out var config))
        {
            if (string.IsNullOrEmpty(config.tag) == false) return config.tag;
        }
        return defaultValue;
    }

    // GET MERADATA
    public static string GetMetadata(string key, string defaultValue = "None")
    {
        if (HasConfig(key, out var config))
        {
            if (string.IsNullOrEmpty(config.metadata) == false) return config.metadata;
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
                if ($"{ownEntity.wid}{ownEntity.gid}{ownEntity.eid}" == $"{constrain.Wid}{constrain.Gid}{constrain.Eid}")
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

    public static double IncrementCurrentQuantity(params DataTypes.Entity[] valuesToIncrementBy)
    {
        List<DataTypes.Entity> newValues = new();
        double currentQuantity = 0;

        for (int i = 0; i < valuesToIncrementBy.Length; i++)
        {
            var element = valuesToIncrementBy[i];
            currentQuantity = UserUtil.GetPropertyFromType<DataTypes.Entity, double>(element.GetKey(), e => e.quantity.GetValueOrDefault(), 0);

            if(element.quantity != null)
            {
                element.quantity += currentQuantity;
                newValues.Add(element);
            }
        }

        UserUtil.UpdateData<DataTypes.Entity>(newValues.ToArray());

        return currentQuantity;
    }
    public static double DecrementCurrentQuantity(params DataTypes.Entity[] valuesToIncrementBy)
    {
        List<DataTypes.Entity> newValues = new();
        double currentQuantity = 0;

        for (int i = 0; i < valuesToIncrementBy.Length; i++)
        {
            var element = valuesToIncrementBy[i];
            currentQuantity = UserUtil.GetPropertyFromType<DataTypes.Entity, double>(element.GetKey(), e => e.quantity.GetValueOrDefault(), 0);

            if(currentQuantity > 0)
            {
                if (element.quantity != null)
                {
                    element.quantity = currentQuantity - element.quantity;

                    if (element.quantity < 0) element.quantity = 0;

                    newValues.Add(element);
                }
            }
        }

        UserUtil.UpdateData<DataTypes.Entity>(newValues.ToArray());

        return currentQuantity;
    }
}