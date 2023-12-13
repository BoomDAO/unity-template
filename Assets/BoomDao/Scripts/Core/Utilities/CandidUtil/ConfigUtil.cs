using Boom.Values;
using Candid;
using Candid.World.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigUtil
{
    //TRY GET CONFGIS
    public static bool TryGetConfig(string worldId, string configId, out MainDataTypes.AllConfigs.Config returnValue)
    {
        returnValue = default;

        var result = UserUtil.GetMainData<MainDataTypes.AllConfigs>();


        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());
            return false;
        }

        var allConfigs = result.AsOk();

        if(allConfigs.configs.TryGetValue(worldId, out var worldConfigs) == false)
        {
            Debug.LogError($"Could not find configs from world of id: {worldId}");
            return false;
        }

        if(worldConfigs.TryGetValue(configId, out returnValue) == false)
        {
            Debug.LogError($"Could not find config of id: {configId} in world of id: {worldId}");

            return false;
        }

        return result.IsOk;
    }
    public static bool TryGetConfig(string worldId, Predicate<MainDataTypes.AllConfigs.Config> predicate, out MainDataTypes.AllConfigs.Config returnValue)
    {
        returnValue = default;

        var result = UserUtil.GetMainData<MainDataTypes.AllConfigs>();


        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());
            return false;
        }

        var allConfigs = result.AsOk();

        if (allConfigs.configs.TryGetValue(worldId, out var worldConfigs) == false)
        {
            Debug.LogError($"Could not find configs from world of id: {worldId}");
            return false;
        }


        foreach (var item in worldConfigs)
        {
            if (predicate(item.Value))
            {
                returnValue = item.Value;
                return true;
            }
        }

        return false;
    }

    public static bool QueryConfigs(string worldId, Predicate<MainDataTypes.AllConfigs.Config> predicate, out LinkedList<MainDataTypes.AllConfigs.Config> returnValue)
    {
        returnValue = default;

        var configsResult = UserUtil.GetMainData<MainDataTypes.AllConfigs>();


        if (configsResult.IsErr)
        {
            Debug.LogError(configsResult.AsErr());
            return false;
        }

        var allConfigs = configsResult.AsOk();

        if (allConfigs.configs.TryGetValue(worldId, out var worldConfigs) == false)
        {
            Debug.LogError($"Could not find configs from world of id: {worldId}");
            return false;
        }

        returnValue = new();

        foreach (var item in worldConfigs)
        {
            if (predicate(item.Value)) returnValue.AddLast(item.Value);
        }

        return true;
    }
    public static bool QueryConfigsByTag(string worldId, string tag, out LinkedList<MainDataTypes.AllConfigs.Config> returnValue)
    {
        return QueryConfigs(worldId, e =>
        {
            if (!e.fields.TryGetValue("tag", out var value)) return false;

            return value.Contains(tag);
        }, out returnValue);
    }

    //

    public static bool TryGetConfig(this NewEntityEdits entity, string worldId, out MainDataTypes.AllConfigs.Config entityConfig)
    {
        return TryGetConfig(worldId, entity.eid, out entityConfig);
    }
    public static bool TryGetConfig(this DataTypes.Entity entity, string worldId, out MainDataTypes.AllConfigs.Config entityConfig)
    {
        return TryGetConfig(worldId, entity.eid, out entityConfig);
    }
    public static bool TryGetConfig(this EntityConstrainTypes.Base entity, string worldId, out MainDataTypes.AllConfigs.Config entityConfig)
    {
        return TryGetConfig(worldId, entity.Eid, out entityConfig);
    }

    public static bool GetConfigFieldAs<T>(string worldId, string configId, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;

        if (!TryGetConfig(worldId, configId, out var config))
        {
            return false;
        }

        if (!config.fields.TryGetValue(fieldName, out var value))
        {
            Debug.LogError($"Failure to find in config of id: {configId} a field of name {fieldName}");

            return false;
        }

        if (value.TryParseValue<T>(out returnValue) == false)
        {
            Debug.LogError($"Failure to parse config field of id: {configId} to {typeof(T).Name}");

            return false;
        }

        return true;
    }
    public static bool GetConfigFieldAs<T>(this DataTypes.Entity entity, string fieldName, out T returnValue, T defaultValue = default)
    {
        return GetConfigFieldAs(entity.wid, entity.eid, fieldName, out returnValue, defaultValue);
    }
    public static bool GetConfigFieldAs<T>(this NewEntityEdits newEntityValues, string worldId, string fieldName, out T returnValue, T defaultValue = default)
    {
        return GetConfigFieldAs(worldId, newEntityValues.eid, fieldName, out returnValue, defaultValue);
    }
    public static bool GetConfigFieldAs<T>(this MainDataTypes.AllConfigs.Config config, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;
        if (!config.fields.TryGetValue(fieldName, out var value)) return false;

        if (value.TryParseValue<T>(out returnValue))
        {
            return true;
        }
        return false;
    }

    //ACTIONS
    public static bool TryGetAction(string worldId, string actionId, out MainDataTypes.AllAction.Action returnValue)
    {
        returnValue = default;

        var result = UserUtil.GetMainData<MainDataTypes.AllAction>();


        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());
            return false;
        }

        var allConfigs = result.AsOk();

        if (allConfigs.actions.TryGetValue(worldId, out var worldActions) == false)
        {
            Debug.LogError($"Could not find configs from world of id: {worldId}");
            return false;
        }

        if (worldActions.TryGetValue(actionId, out returnValue) == false)
        {
            Debug.LogError($"Could not find config of id: {actionId} in world of id: {worldId}");

            return false;
        }

        return true;
    }
    public static bool TryGetActionPart<T>(this string actionId, Func<MainDataTypes.AllAction.Action, T> func, out T returnValue)
    {
        returnValue = default;

        if (!ConfigUtil.TryGetAction(CandidApiManager.Instance.WORLD_CANISTER_ID, actionId, out var action))
        {
            Debug.LogError("Could not find action of id: " + actionId);

            return false;
        }


        try
        {
            returnValue = func(action);
        }
        catch
        {
            return false;
        }


        return true;
    }

    //TOKENS
    public static UResult<LinkedList<MainDataTypes.AllTokenConfigs.TokenConfig>, string> GetAllTokenConfigs()
    {
        var result = UserUtil.GetMainData<MainDataTypes.AllTokenConfigs>();

        if (result.IsErr)
        {
            return new(result.AsErr());
        }

        var allConfigs = result.AsOk();

        LinkedList<MainDataTypes.AllTokenConfigs.TokenConfig> configs = new();

        foreach ( var tokenConfig in allConfigs.configs) 
        {
            configs.AddLast(tokenConfig.Value);
        }

        return new(configs);
    }
    public static bool TryGetTokenConfig(string canisterId, out MainDataTypes.AllTokenConfigs.TokenConfig returnValue)
    {
        returnValue = default;

        var result = UserUtil.GetMainData<MainDataTypes.AllTokenConfigs>();

        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());
            return false;
        }

        var allConfigs = result.AsOk();

        if (allConfigs.configs.TryGetValue(canisterId, out returnValue) == false)
        {
            Debug.LogError($"Could not find token configs for canister id: {canisterId}");
            return false;
        }

        return true;
    }
    public static bool TryGetTokenConfig(this DataTypes.Token token, out MainDataTypes.AllTokenConfigs.TokenConfig returnValue)
    {
        return TryGetTokenConfig(token.canisterId, out returnValue);
    }
    //NFTS
    public static UResult<LinkedList<MainDataTypes.AllNftCollectionConfig.NftConfig>, string> GetAllNftConfigs()
    {
        var result = UserUtil.GetMainData<MainDataTypes.AllNftCollectionConfig>();

        if (result.IsErr)
        {
            return new(result.AsErr());
        }

        var allConfigs = result.AsOk();

        LinkedList<MainDataTypes.AllNftCollectionConfig.NftConfig> configs = new();

        foreach (var tokenConfig in allConfigs.configs)
        {
            configs.AddLast(tokenConfig.Value);
        }

        return new(configs);
    }

    public static bool TryGetNftCollectionConfig(string canisterId, out MainDataTypes.AllNftCollectionConfig.NftConfig returnValue)
    {
        returnValue = default;

        var result = UserUtil.GetMainData<MainDataTypes.AllNftCollectionConfig>();

        if (result.IsErr)
        {
            Debug.LogError(result.AsErr());
            return false;
        }

        var allConfigs = result.AsOk();

        if (allConfigs.configs.TryGetValue(canisterId, out returnValue) == false)
        {
            Debug.LogError($"Could not find token configs for canister id: {canisterId}");
            return false;
        }

        return true;
    }

    public static bool TryGetNftCollectionConfig(this DataTypes.NftCollection collection, out MainDataTypes.AllNftCollectionConfig.NftConfig returnValue)
    {
        return TryGetNftCollectionConfig(collection.canisterId, out returnValue);
    }

}
