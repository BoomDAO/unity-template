// Ignore Spelling: Util

using Candid.World.Models;
using EdjCase.ICP.Agent.Agents;
using Boom.Patterns.Broadcasts;
using Boom.Utility;
using Boom.Values;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class UserUtil
{
    #region Login
    public enum LoginType { User, Anon }
    public static void RegisterToLoginDataChange(this Action<DataState<LoginData>> action, bool invokeOnRegistration = false)
    {
        BroadcastState.Register<DataState<LoginData>>(action, invokeOnRegistration);
    }
    public static void UnregisterToLoginDataChange(this Action<DataState<LoginData>> action)
    {
        BroadcastState.Unregister<DataState<LoginData>>(action);
    }

    public static void StartLogin(string loadingMessage = "Loading...")
    {
        Broadcast.Invoke<StartLogin>();

        BroadcastState.ForceInvoke<DataState<LoginData>>(e =>
        {
            e.SetAsLoading(loadingMessage);

            return e;
        });
    }
    public static void UpdateLoginData(IAgent agent, string principal, string accountIdentifier, bool asAnon)
    {
        BroadcastState.ForceInvoke<DataState<LoginData>>(e =>
        {
            Debug.Log($"Principal: {principal}");
            Debug.Log($"Address: {accountIdentifier}");
            e.SetAsReady(new LoginData(agent, principal, accountIdentifier, asAnon));
            return e;
        });
    }

    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the DataState Object of the User/Anon LoginData
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the Data State Object of LoginData or an error message</returns>
    public static UResult<DataState<LoginData>, string> GetLogInDataState()
    {
        if (BroadcastState.TryRead<DataState<LoginData>>(out var state) == false)
        {
            return new("Data could not be found");
        }

        if (!state.IsReady())
        {
            if (state.IsLoading())
            {
                return new(state.LoadingMsg);
            }
            else
            {
                return new("StartLogin process has not started");
            }
        }

        return new(state);
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the User/Anon LoginData
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the Data State Object of LoginData or an error message</returns>
    public static UResult<LoginData, string> GetLogInData()
    {
        if (BroadcastState.TryRead<DataState<LoginData>>(out var state) == false)
        {
            return new("Data could not be found");
        }

        if (!state.IsReady())
        {
            if (state.IsLoading())
            {
                return new(state.LoadingMsg);
            }
            else
            {
                return new("StartLogin process has not started");
            }
        }

        return new(state.data);
    }
    public static bool IsLoginDataValid()
    {
        if (BroadcastState.TryRead<DataState<LoginData>>(out var state) == false)
        {
            return false;
        }

        if (!state.IsReady())
        {
            if (state.IsLoading())
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the User/Anon Principal
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the user Principal or an error message</returns>
    public static UResult<Wrapper<string>, string> GetPrincipal()
    {
        var result = GetLogInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(result.AsErr());
        }

        return new(new Wrapper<string>(result.AsOk().principal));
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the User/Anon AccountIdentifier
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the user AccountIdentifier or an error message</returns>
    public static UResult<Wrapper<string>, string> GetAccountIdentifier()
    {
        var result = GetLogInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(result.AsErr());
        }

        return new(new Wrapper<string>(result.AsOk().accountIdentifier));
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the User/Anon Agent
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the user Agent or an error message</returns>
    public static UResult<IAgent, string> GetAgent()
    {
        var result = GetLogInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(result.AsErr());
        }

        return new(result.AsOk().agent);
    }

    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the LoginType which could be User or Anon
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns></returns>
    public static UResult<LoginType, string> GetLogInType()
    {
        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return new(getLogInDataResult.AsErr());
        }

        return getLogInDataResult.AsOk().asAnon ? new(LoginType.Anon) : new(LoginType.User);
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this true if Login Data is from an User
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns></returns>
    public static bool IsUserLoggedIn()
    {
        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return false;
        }

        return !getLogInDataResult.AsOk().asAnon;
    }
    public static bool IsUserLoggedIn(out LoginData loginData)
    {
        loginData = default;

        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return false;
        }
        loginData = getLogInDataResult.AsOk();
        return !loginData.asAnon;
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this true if Login Data is from Anon
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns></returns>
    public static bool IsAnonLoggedIn()
    {
        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return false;
        }

        return getLogInDataResult.AsOk().asAnon;
    }
    public static bool IsAnonLoggedIn(out LoginData loginData)
    {
        loginData = default;

        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return false;
        }
        loginData = getLogInDataResult.AsOk();
        return loginData.asAnon;
    }
    #endregion

    #region User Data Management
    public static void RegisterToRequestData<T>(this System.Action<FetchDataReq<T>> action) where T : DataTypes.Base
    {
        Broadcast.Register<FetchDataReq<T>>(action);
    }
    public static void UnregisterToRequestData<T>(this System.Action<FetchDataReq<T>> action) where T : DataTypes.Base
    {
        Broadcast.Unregister<FetchDataReq<T>>(action);
    }

    public static void RegisterToDataChange<T>(this System.Action<DataState<Data<T>>> action, bool invokeOnRegistration = false) where T : DataTypes.Base
    {
        BroadcastState.Register<DataState<Data<T>>>(action, invokeOnRegistration);
    }
    public static void UnregisterToDataChange<T>(this System.Action<DataState<Data<T>>> action) where T : DataTypes.Base
    {
        BroadcastState.Unregister<DataState<Data<T>>>(action);
    }

    /// <summary>
    /// Request Data from Canisters,
    /// This will trigger listeners from "CandidApiManager" which handle fetching the data
    /// After Fetching the data "UpdateData<T>" will be called
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optional">you can set it to any argument as required, for example DataTypes.Token requires an canisterId as argument so that it knows what token canister to fetch data from</param>
    /// <param name="loadingMessage">This is the message you want to display when waiting for the data to be fetched</param>
    public static void RequestData<T>(object optional = null, string loadingMessage = "Loading...") where T : DataTypes.Base
    {
        BroadcastState.ForceInvoke<DataState<Data<T>>>(e =>
        {
            e.SetAsLoading(loadingMessage);
            return e;
        });

        Broadcast.Invoke<FetchDataReq<T>>(new FetchDataReq<T>(optional));
    }

    /// <summary>
    /// Data will be valid or ready after getting loaded into the game, it will not be ready if not initialized or whenever it is in a loading state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsDataValid<T>() where T : DataTypes.Base
    {
        if (BroadcastState.TryRead<DataState<Data<T>>>(out var val) == false)
        {
            return false;
        }

        $"Checking if data of type {typeof(T).Name} is valid\nIsReady: {val.IsReady()}  current state: {val.State}".Log();
        return val.IsReady();
    }

    public static bool IsDataValid<T>(params string[] expectedEntires) where T : DataTypes.Base
    {
        if (BroadcastState.TryRead<DataState<Data<T>>>(out var val) == false)
        {
            return false;
        }

        if (!val.IsReady()) return false;
        var hasAllEntries = expectedEntires.TrueForAll(e => val.data.elements.Has(k => k.Key == e));
        return hasAllEntries;
    }

    #region Get
    /// <summary>
    /// Get the data state of some "T" Type that derive from DataTypes.Base
    /// </summary>
    /// <typeparam name="T">DataTypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<DataState<Data<T>>, string> GetDataOfType<T>() where T : DataTypes.Base
    {
        if (BroadcastState.TryRead<DataState<Data<T>>>(out var val) == false)
        {
            return new($"Data could not be found for DataType: {typeof(T).Name}");
        }

        if (val.IsReady() == false && val.IsLoading()) return new(string.IsNullOrEmpty(val.LoadingMsg)? $"{typeof(T).Name} Loading..." : val.LoadingMsg);

        if (val.IsReady() == false) return new($"Data of type {typeof(T).Name} is not ready");

        return new(val);
    }
    /// <summary>
    /// Try get an element from collection of "T" Type that derive from Datatypes.Base
    /// </summary>
    /// <typeparam name="T">Datatypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<T, string> GetElementOfType<T>(string id) where T : DataTypes.Base
    {
        if (BroadcastState.TryRead<DataState<Data<T>>>(out var val) == false)
        {
            return new($"Data could not be found for DataType: {typeof(T).Name}");
        }

        if (val.IsReady() == false) return new($"Data of type {typeof(T).Name} is not yet ready");

        if (val.data.elements.TryGetValue(id, out var element) == false) return new($"Data Type: {typeof(T).Name} does not contain element of id: {id}.\n\nValid Ids:\n\n{val.data.elements.Reduce(e=>$"-{e.Key}","\n\n")}\nend.");
        return new(element);
    }
    /// <summary>
    /// Try get a property value from an element from collection of "T" Type that derive from Datatypes.Base
    /// </summary>
    /// <typeparam name="T">Datatypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<PropertyType, string> GetPropertyFromType<T, PropertyType>(string id, Func<T, PropertyType> getter) where T : DataTypes.Base
    {
        var restult = GetElementOfType<T>(id);

        if (restult.Tag == UResultTag.Err)
        {
            return new(restult.AsErr());
        }

        try
        {
            PropertyType propertyType = getter(restult.AsOk());

            return new(propertyType);
        }
        catch (Exception e)
        {
            return new(e.Message);
        }
    }
    public static PropertyType GetPropertyFromType<T, PropertyType>(string id, Func<T, PropertyType> getter, PropertyType defaultVal = default) where T : DataTypes.Base
    {
        var restult = GetElementOfType<T>(id);

        if (restult.Tag == UResultTag.Err)
        {
            return defaultVal;
        }

        PropertyType propertyType = getter(restult.AsOk());

        return propertyType;
    }

    /// <summary>
    /// Get a collection of "T" Type that derive from Datatypes.Base
    /// </summary>
    /// <typeparam name="T">Datatypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<List<T>, string> GetElementsOfType<T>() where T : DataTypes.Base
    {
        if (BroadcastState.TryRead<DataState<Data<T>>>(out var val) == false)
        {
            return new($"Data could not be found for DataType: {typeof(T).Name}");
        }

        if (val.IsReady() == false) return new("Data is not yet ready");

        List<T> elements = new();
        val.data.elements.Iterate(e =>
        {
            elements.Add(e.Value);
        });

        return new(elements);
    }
    #endregion

    /// <summary>
    /// Clean up user data handled by type
    /// </summary>
    public static void Clean<T>() where T : DataTypes.Base
    {
        BroadcastState.ForceInvoke<DataState<Data<T>>>(e =>
        {
            e.Clear();
            return e;
        });
    }

    /// <summary>
    /// Use it to update the data of a given DataType, this will add or override entries, but will not remove them,
    /// the entries are only removed by calling CleanUpUserData()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newVals"></param>
    public static void UpdateData<T>(params T[] newVals) where T : DataTypes.Base
    {
//#if UNITY_EDITOR
        if(newVals != null) $"DATA of type {typeof(T).Name} has been fetched.\nKeys to store:\n{newVals.Reduce(e=>$"* {e.GetKey()}, value: {JsonConvert.SerializeObject(e)}", ",\n")}".Log(nameof(UserUtil));
//#endif

        BroadcastState.ForceInvoke<DataState<Data<T>>>(e =>
        {
//#if UNITY_EDITOR
            $"Before Update of: {typeof(T).Name}\nCurrent Keys:\n{e.data.elements.Reduce(k => $"* {k.Key}, value: {JsonConvert.SerializeObject(k.Value)}", ",\n")}".Log(nameof(UserUtil));
//#endif

            //Set as Ready
            e.SetAsReady(new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                ));
            return e;
        });
    }

    #endregion ^

    public static ulong GetTokenBaseUnitAmount(string canisterId)
    {
        var tokenResult = GetElementOfType<DataTypes.Token>(canisterId);

        if (tokenResult.IsErr) return 0;

        return tokenResult.AsOk().baseUnitAmount;
    }

    /// <summary>
    /// This is for lassy people who wants to fetch a Token along with its Config by its tokenCanisterId
    /// </summary>
    /// <param name="canisterId">Canister Id of the Token</param>
    /// <returns></returns>
    public static UResult<(DataTypes.Token token, DataTypes.TokenConfig configs), string> GetTokenAndConfigs(string canisterId)
    {
        var tokenResult = GetElementOfType<DataTypes.Token>(canisterId);

        if (tokenResult.IsErr) return new(tokenResult.AsErr());

        var configsResult = GetElementOfType<DataTypes.TokenConfig>(canisterId);

        if (configsResult.IsErr) return new(configsResult.AsErr());

        return new((tokenResult.AsOk(), configsResult.AsOk()));
    }
}
