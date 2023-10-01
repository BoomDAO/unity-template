
using Boom.Utility;
using Boom.Values;
using Candid.World.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
// Ignore Spelling: Util

public enum ConfigQueryType
{
    Eid,
    Gid
}
internal static class EntityUtil
{
    public static string BuildConfigId(string id, string wid = "")
    {
        if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;

        return $"{wid}{id}";
    }

    public static string GetConfigId(this DataTypes.Entity entity, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        var wid = entity.wid;
        if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;

        return configQueryType == ConfigQueryType.Eid? $"{wid}{entity.eid}" : $"{wid}{entity.gid}";
    }
    public static string GetConfigId(this NewEntityValues entity, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        var wid = entity.wid;
        if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;

        return configQueryType == ConfigQueryType.Eid ? $"{wid}{entity.eid}" : $"{wid}{entity.gid}";
    }
    public static string GetConfigId(this EntityConstrainTypes.Base entity, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        var wid = entity.Wid;
        if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;

        return configQueryType == ConfigQueryType.Eid ? $"{wid}{entity.Eid}" : $"{wid}{entity.Gid}";
    }
    public static string GetConfigId(this ActionOutcomeTypes.ActionOutcomeEditEntity entity, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        var wid = entity.Wid;
        if (string.IsNullOrEmpty(wid)) wid = Env.CanisterIds.WORLD;

        return configQueryType == ConfigQueryType.Eid ? $"{wid}{entity.Eid}" : $"{wid}{entity.Gid}";
    }

    //TRY GET CONFGIS
    public static bool TryGetConfig(string entityConfigKey, out DataTypes.Config entityConfig)
    {
        entityConfig = default;
        var config = UserUtil.GetElementOfType<DataTypes.Config>(entityConfigKey);


        if (config.IsOk)
        {
            entityConfig = config.AsOk();
        }

        return config.IsOk;
    }

    public static bool TryGetConfig(this NewEntityValues entity, out DataTypes.Config entityConfig, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        return TryGetConfig(entity.GetConfigId(configQueryType), out entityConfig);
    }
    public static bool TryGetConfig(this DataTypes.Entity entity, out DataTypes.Config entityConfig, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        return TryGetConfig(entity.GetConfigId(configQueryType), out entityConfig);
    }
    public static bool TryGetConfig(this EntityConstrainTypes.Base entity, out DataTypes.Config entityConfig, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        return TryGetConfig(entity.GetConfigId(configQueryType), out entityConfig);
    }
    public static bool TryGetConfig(Predicate<DataTypes.Config> predicate, out DataTypes.Config returnValue)
    {
        returnValue = default;

        var result = UserUtil.GetElementsOfType<DataTypes.Config>();
        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());

            return false;
        }

        var elements = result.AsOk();

        foreach (var item in elements)
        {
            if (predicate(item))
            {
                returnValue = item;
                return true;
            }
        }

        return false;
    }
    public static bool QueryConfigs(Predicate<DataTypes.Config> predicate, out LinkedList<DataTypes.Config> returnValue)
    {
        returnValue = default;

        var result = UserUtil.GetDataOfType<DataTypes.Config>();

        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());

            return false;
        }

        returnValue = new();

        foreach (var item in result.AsOk().data.elements)
        {
            if (predicate(item.Value)) returnValue.AddLast(item.Value);
        }

        return true;
    }
    public static bool QueryConfigsByTag(string tag, out LinkedList<DataTypes.Config> returnValue)
    {
        return QueryConfigs(e =>
        {
            if (!e.fields.TryGetValue("tag", out var value)) return false;

            return value.ToString() == tag;
        }, out returnValue);
    }

    public static bool GetConfigFieldAs<T>(string configId, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;

        var result = UserUtil.GetElementOfType<DataTypes.Config>(configId);
        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());

            return false;
        }

        if (!result.AsOk().fields.TryGetValue(fieldName, out var value)) return false;

        if (value.TryParseValue<T>(out returnValue))
        {
            return true;
        }
        Debug.LogError($"Error on \"value\" type, current type: {value.GetType()}, desired type is: {typeof(T)}");
        return false;
    }
    public static bool GetConfigFieldAs<T>(this DataTypes.Entity entity, string fieldName, out T returnValue, T defaultValue = default)
    {
        return GetConfigFieldAs(entity.GetConfigId(), fieldName, out returnValue, defaultValue);
    }
    public static bool GetConfigFieldAs<T>(this DataTypes.Config config, string fieldName, out T returnValue, T defaultValue = default)
    {
        return GetConfigFieldAs(config.GetKey(), fieldName, out returnValue, defaultValue);
    }
    public static bool GetConfigFieldAs<T>(this NewEntityValues newEntityValues, string fieldName, out T returnValue, T defaultValue = default, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        return GetConfigFieldAs(newEntityValues.GetConfigId(configQueryType), fieldName, out returnValue, defaultValue);
    }
    public static bool GetConfigFieldAs<T>(this ActionOutcomeTypes.ActionOutcomeEditEntity newEntityValues, string fieldName, out T returnValue, T defaultValue = default, ConfigQueryType configQueryType = ConfigQueryType.Eid)
    {
        return GetConfigFieldAs(newEntityValues.GetConfigId(configQueryType), fieldName, out returnValue, defaultValue);
    }

    public static bool GetFieldAs<T>(string entityKey, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;

        var result = UserUtil.GetElementOfType<DataTypes.Entity>(entityKey);
        if (result.IsErr)
        {
            return false;
        }

        if (!result.AsOk().fields.TryGetValue(fieldName, out var value)) return false;

        if (value.TryParseValue<T>(out returnValue))
        {
            return true;
        }
        Debug.LogError($"Error on \"value\" type, current type: {value.GetType()}, desired type is: {typeof(T)}");
        return false;
    }
    public static bool GetFieldAs<T>(this DataTypes.Entity entity, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;

        if (!entity.fields.TryGetValue(fieldName, out var value)) return false;

        if (value.TryParseValue<T>(out returnValue))
        {
            return true;
        }
        Debug.LogError($"Error on \"value\" type, current type: {value.GetType()}, desired type is: {typeof(T)}");
        return false;
    }
    public static bool GetFieldAs<T>(this NewEntityValues newEntityValues, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;

        if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

        try
        {
            returnValue = (T)edit.Value;
            return true;
        }
        catch
        {
            Debug.LogError($"Error on \"value\" type, current type: {edit.Value.GetType()}, desired type is: {typeof(T)}");
            return false;
        }
    }

    /// <summary>
    /// Will return true if all requirements are met
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static UResult<bool, string> MeetEntityRequirements(DataTypes.Action action)
    {
        if (action.entityConstraints != null)
        {
            var entitiesResult = UserUtil.GetElementsOfType<DataTypes.Entity>();

            if (entitiesResult.Tag == UResultTag.Err) return new(entitiesResult.AsErr());

            var entitiesResultAsOk = entitiesResult.AsOk();

            foreach (var constrain in action.entityConstraints)
            {
                bool fail = true;

                foreach (var ownEntity in entitiesResultAsOk)
                {
                    if(constrain.GetKey() == ownEntity.GetKey())
                    {
                        if (constrain.Check(ownEntity))
                        {
                            fail = false;
                            break;
                        }
                    }
                }

                if (fail) return new(false);

            }

            return new(true);
        }
        else return new(true);
    }

    public static void EditEntities(params NewEntityValues[] newEntityValuesArr)
    {
        if (!UserUtil.IsDataValid<DataTypes.Entity>())
        {
            Debug.LogError("Error, entity has not been loaded yet");

            return;
        }

        var currentEntities = UserUtil.GetDataOfType<DataTypes.Entity>().AsOk().data.elements;


        foreach (NewEntityValues newEntityValues in newEntityValuesArr) { 
            
            if(!newEntityValues.dispose)
            {
                if (!currentEntities.TryGetValue(newEntityValues.GetKey(), out var currentEntity))
                {
                    currentEntity = new(newEntityValues.wid, newEntityValues.gid, newEntityValues.eid, new());
                    currentEntities.Add(currentEntity.GetKey(), currentEntity);
                }

                foreach (var field in newEntityValues.fields)
                {
                    Debug.Log($"FIELD TO EDIT, id: {field.Key}, edit type: {field.Value.EditType}, editValue: {field.Value.Value} ");
                    var editData = field.Value;

                    if(editData.EditType == EntityFieldEditType.SetString)
                    {

                        if (!currentEntity.fields.TryAdd(field.Key, editData.Value.ToString()))
                        {
                            currentEntity.fields[field.Key] = editData.Value.ToString();
                        }
                    }
                    else if (editData.EditType == EntityFieldEditType.SetNumber)
                    {
                        if (!currentEntity.fields.TryAdd(field.Key, editData.Value.ToString()))
                        {
                            currentEntity.fields[field.Key] = editData.Value.ToString();
                        }
                    }
                    else if (editData.EditType == EntityFieldEditType.IncrementNumber)
                    {
                        if (!newEntityValues.GetFieldAs<double>(field.Key, out var value)) continue;


                        if (!currentEntity.fields.TryAdd(field.Key, value.ToString()))
                        {
                            if(!currentEntity.GetFieldAs<double>(field.Key, out var currentValue)) continue;

                            currentEntity.fields[field.Key] = (currentValue + value).ToString();
                        }
                    }
                    else if (editData.EditType == EntityFieldEditType.DecrementNumber)
                    {
                        if (!newEntityValues.GetFieldAs<double>(field.Key, out var value)) continue;

                        if (!currentEntity.GetFieldAs<double>(field.Key, out var currentValue)) continue;

                        currentEntity.fields[field.Key] = (currentValue - value).ToString();
                    }
                    else
                    {
                        if (!currentEntity.fields.TryAdd(field.Key, editData.Value.ToString()))
                        {
                            currentEntity.fields[field.Key] = editData.Value.ToString();
                        }
                    }
                }
            }
            else
            {
                currentEntities.Remove(newEntityValues.GetKey());
            }

        }

        UserUtil.UpdateData<DataTypes.Entity>(new DataTypes.Entity[0]);
    }
}