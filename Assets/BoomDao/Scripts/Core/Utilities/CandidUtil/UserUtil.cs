// Ignore Spelling: Util

using Boom;
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
    public enum LoginType { User, Anon, None }

    public static void SetAsLoginIn()
    {
        UserUtil.UpdateMainData(new MainDataTypes.LoginData() { state = MainDataTypes.LoginData.State.LoginRequested});
    }

    public static bool IsLoginRequestedPending() 
    {
        var loginDataResult = GetMainData<MainDataTypes.LoginData>();

        if(loginDataResult.IsErr) return false;

        var loginDataAsOk = loginDataResult.AsOk();


        return loginDataAsOk.state == MainDataTypes.LoginData.State.LoginRequested;
    }

    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the User/Anon LoginData
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the Data State Object of LoginData or an error message</returns>
    public static UResult<MainDataTypes.LoginData, string> GetLogInData()
    {
        var loginDataResult = GetMainData<MainDataTypes.LoginData>();


        if (loginDataResult.IsErr)
        {
            return new(loginDataResult.AsErr());
        }


        return new(loginDataResult.AsOk());
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the User/Anon Principal
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the user Principal or an error message</returns>
    public static UResult<string> GetPrincipal()
    {
        var result = GetLogInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(new UResult<string>.ERR<string>(result.AsErr()));
        }

        return new(new UResult<string>.OK<string>(result.AsOk().principal));
    }
    /// <summary>
    /// If LoginData is ever initialized this function will return a result as an Ok, being this the User/Anon AccountIdentifier
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns>Could be either the user AccountIdentifier or an error message</returns>
    public static UResult<string> GetAccountIdentifier()
    {
        var result = GetLogInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(new UResult<string>.ERR<string>(result.AsErr()));
        }

        return new(new UResult<string>.OK<string>(result.AsOk().accountIdentifier));
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
    /// If LoginData is ever initialized this function will return a result as an Ok, being this true if Login Data is from an User
    /// Otherwise it will return a result as an Err, being this an error message
    /// </summary>
    /// <returns></returns>
    public static bool IsLoggedIn(out LoginType loginType)
    {
        loginType = LoginType.None;

        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return false;
        }

        var asOk = getLogInDataResult.AsOk();

        if (asOk.state == MainDataTypes.LoginData.State.LoggedIn)
        {
            loginType = LoginType.User;
            return true;
        }
        else if (asOk.state == MainDataTypes.LoginData.State.LoggedInAsAnon)
        {
            loginType = LoginType.Anon;
            return true;
        }

        return false;
    }
    public static LoginType GetLoginType()
    {
        IsLoggedIn(out LoginType loginType);
        return loginType;
    }
    public static bool IsUserLoggedIn(out MainDataTypes.LoginData loginData)
    {
        loginData = default;


        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return false;
        }

        var asOk = getLogInDataResult.AsOk();

        if (asOk.state == MainDataTypes.LoginData.State.LoggedIn)
        {
            loginData = getLogInDataResult.AsOk();
            return true;
        }

        return false;
    }
    public static bool IsUserLoggedIn()
    {
        return IsLoggedIn(out LoginType loginType) && loginType == LoginType.User;
    }
    public static bool IsAnonLoggedIn(out MainDataTypes.LoginData loginData)
    {
        loginData = default;


        var getLogInDataResult = UserUtil.GetLogInData();

        if (getLogInDataResult.Tag == UResultTag.Err)
        {
            return false;
        }

        var asOk = getLogInDataResult.AsOk();

        if (asOk.state == MainDataTypes.LoginData.State.LoggedInAsAnon)
        {
            loginData = getLogInDataResult.AsOk();
            return true;
        }

        return false;
    }
    public static bool IsAnonLoggedIn()
    {
        return IsLoggedIn(out LoginType loginType) && loginType == LoginType.Anon;
    }

    #endregion

    #region DataTypes

    static readonly Dictionary<string, HashSet<string>> loadingData = new();// worldId/userId -> loading data type

    //SUBSCRIPTIONS
    public static void AddListenerRequestData<T>(this System.Action<FetchDataReq<T>> action) where T : DataTypeRequestArgs.Base
    {
        Broadcast.Register<FetchDataReq<T>>(action);
    }
    public static void RemoveListenerRequestData<T>(this System.Action<FetchDataReq<T>> action) where T : DataTypeRequestArgs.Base
    {
        Broadcast.Unregister<FetchDataReq<T>>(action);
    }

    public static void AddListenerDataChange<T>(this System.Action<Data<T>> action, bool invokeOnRegistration = false, params string[] uids) where T : DataTypes.Base
    {
        if (IsUserLoggedIn(out var loginData))
        {
            foreach (var uid in uids)
            {
                string _uid = uid == loginData.principal ? "self" : uid;

                BroadcastState.Register<Data<T>>(action, invokeOnRegistration, _uid);
            }

            return;
        }

        foreach (var uid in uids) 
            BroadcastState.Register<Data<T>>(action, invokeOnRegistration, uid);
    }
    public static void RemoveListenerDataChange<T>(this System.Action<Data<T>> action, params string[] uids) where T : DataTypes.Base
    {
        if (IsUserLoggedIn(out var loginData))
        {
            foreach (var uid in uids)
            {
                string _uid = uid == loginData.principal ? "self" : uid;

                BroadcastState.Unregister<Data<T>>(action, _uid);
            }

            return;
        }

        foreach (var uid in uids)
            BroadcastState.Unregister<Data<T>>(action, uid);
    }

    public static void AddListenerDataChangeSelf<T>(this System.Action<Data<T>> action, bool invokeOnRegistration = false) where T : DataTypes.Base
    {
        AddListenerDataChange<T>(action, invokeOnRegistration, "self");
    }
    public static void RemoveListenerDataChangeSelf<T>(this System.Action<Data<T>> action) where T : DataTypes.Base
    {
        RemoveListenerDataChange<T>(action, "self");
    }

    //CLEAR

    public abstract class ClearMode
    {
        public abstract class Base { };
        public class All : Base { }
        public class AllBut : Base
        {
            public string[] blacklistIds;

            public AllBut(params string[] blacklistIds)
            {
                this.blacklistIds = blacklistIds;
            }
        }
        public class Targets : Base
        {
            public string[] targetIds;

            public Targets(params string[] targetIds)
            {
                this.targetIds = targetIds;
            }
        }
    }
    /// <summary>
    /// Clean up user data handled by type
    /// </summary>
    //
    public static void ClearData<T>(ClearMode.Base cleanUpType = default) where T : DataTypes.Base
    {
        switch (cleanUpType)
        {
            case ClearMode.AllBut e:
                BroadcastState.TryDisposeAll<Data<T>>(e.blacklistIds);
                break;
            case ClearMode.Targets e:

                e.targetIds.Iterate(k =>
                {
                    BroadcastState.ForceInvoke<Data<T>>(j =>
                    {
                        j.Clear();
                        return j;
                    }, k);
                });
                break;
            default:
                BroadcastState.TryDisposeAll<Data<T>>();
                break;

        }
    }

    // REQUEST
    /// <summary>
    /// Request Data from Canisters,
    /// This will trigger listeners from "CandidApiManager" which handle fetching the data
    /// After Fetching the data "UpdateData<T>" will be called
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optional">you can set it to any argument as required, for example DataTypes.Token requires an canisterId as argument so that it knows what token canister to fetch data from</param>
    /// <param name="loadingMessage">This is the message you want to display when waiting for the data to be fetched</param>
    public static void RequestData(DataTypeRequestArgs.Base arg)
    {
        if (IsUserLoggedIn(out var loginData) == false)
        {
            Debug.LogError("Issue getting loginData!");

            return;
        }

        if (loginData.state != MainDataTypes.LoginData.State.LoggedIn)
        {
            Debug.LogError("You cannot fetch shared data as an anon user!");

            return;
        }

        var loadingDataType = "";
        if (arg.uids.Length == 0)
        {
            arg.uids = new string[1] { loginData.principal };
        }

        switch (arg)
        {
            case DataTypeRequestArgs.Entity e:

                loadingDataType = nameof(DataTypes.Entity);

                QueueLoadingType(loginData.principal, loadingDataType, arg.uids);

                Broadcast.Invoke<FetchDataReq<DataTypeRequestArgs.Entity>>(new FetchDataReq<DataTypeRequestArgs.Entity>(e));
                break;

            case DataTypeRequestArgs.ActionState e:

                loadingDataType = nameof(DataTypes.ActionState);

                QueueLoadingType(loginData.principal, loadingDataType, arg.uids);

                Broadcast.Invoke<FetchDataReq<DataTypeRequestArgs.ActionState>>(new FetchDataReq<DataTypeRequestArgs.ActionState>(e));
                break;

            case DataTypeRequestArgs.Token e:

                loadingDataType = nameof(DataTypes.Token);

                QueueLoadingType(loginData.principal, loadingDataType, arg.uids);

                Broadcast.Invoke<FetchDataReq<DataTypeRequestArgs.Token>>(new FetchDataReq<DataTypeRequestArgs.Token>(e));
                break;

            case DataTypeRequestArgs.NftCollection e:

                loadingDataType = nameof(DataTypes.NftCollection);

                QueueLoadingType(loginData.principal, loadingDataType, arg.uids);

                Broadcast.Invoke<FetchDataReq<DataTypeRequestArgs.NftCollection>>(new FetchDataReq<DataTypeRequestArgs.NftCollection>(e));
                break;
        }

        static void QueueLoadingType(string selfPrincipal, string loadingDataType, string[] uids)
        {
            foreach (var uid in uids)
            {
                string _uid = uid != selfPrincipal? uid : "self";
                //Debug.Log($"*** Start Loading   |   uid: {_uid.SimplifyAddress()}   |   type: {loadingDataType}");

                if (!loadingData.TryGetValue(_uid, out HashSet<string> loadingDataTypes))
                {
                    loadingDataTypes = new HashSet<string>();
                    loadingData.Add(_uid, loadingDataTypes);
                }

                loadingDataTypes.Add(loadingDataType);
            }
        }
    }

    public static void RequestDataSelf<T>() where T : DataTypeRequestArgs.Base, new()
    {
        T arg = new()
        {
            uids = new string[0]
        };

        RequestData(arg);
    }

    //UPDATE

    /// <summary>
    /// Use it to update the data of a given DataType, this will add or override entries, but will not remove them,
    /// the entries are only removed by calling CleanUpUserData()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newVals"></param>

    public static void UpdateData<T>(string uid, params T[] newVals) where T : DataTypes.Base
    {
        //#if UNITY_EDITOR
        if (newVals != null) $">>> DATA of type {typeof(T).Name}, key: {uid}, has been fetched.\nKeys to store:\n{newVals.Reduce(e => $"* {e.GetKey()}, value: {JsonConvert.SerializeObject(e)}", ",\n")}".Log(nameof(UserUtil));
        //#endif

        if (IsUserLoggedIn(out var loginData) == false)
        {
            Debug.LogError("Issue getting loginData!");

            return;
        }

        if (loginData.state != MainDataTypes.LoginData.State.LoggedIn)
        {
            Debug.LogError("You cannot update shared data as an anon user!");

            return;
        }

        if (uid == loginData.principal) uid = "self";


        BroadcastState.ForceInvoke<Data<T>>(e =>
        {
            ////#if UNITY_EDITOR
            //            $"Before Update of: {typeof(T).Name}\nCurrent Keys:\n{e.data.elements.Reduce(k => $"* {k.Key}, value: {JsonConvert.SerializeObject(k.Value)}", ",\n")}".Log(nameof(UserUtil));
            ////#endif
            ///

            var loadingDataType = "";

            switch (newVals)
            {

                case DataTypes.Entity[]:

                    loadingDataType = nameof(DataTypes.Entity);
                    break;

                case DataTypes.ActionState[]:

                    loadingDataType = nameof(DataTypes.ActionState);
                    break;

                case DataTypes.Token[]:

                    loadingDataType = nameof(DataTypes.Token);
                    break;

                case DataTypes.NftCollection[]:

                    loadingDataType = nameof(DataTypes.NftCollection);
                    break;
            }

            //Debug.Log($"*** End Loading   |   uid: {uid.SimplifyAddress()}   |    type: {loadingDataType}");

            if (loadingData.TryGetValue(uid, out var dependencies))
            {
                if (dependencies.Contains(loadingDataType)) dependencies.Remove(loadingDataType);
                if (dependencies.Count == 0) loadingData.Remove(uid);
            }


            return new(uid, e, k => k.GetKey(), newVals);

        }, uid);
    }
    public static void UpdateDataSelf<T>(params T[] newVals) where T : DataTypes.Base
    {
        UpdateData("self", newVals);
    }

    //GET


    public static bool IsDataValid<T>(params string[] uids) where T : DataTypes.Base
    {
        if (IsUserLoggedIn(out var loginData) == false)
        {
            "Issue getting loginData!".Error();

            return false;
        }

        if (loginData.state != MainDataTypes.LoginData.State.LoggedIn)
        {
            "You cannot check shared data as an anon user!".Error();

            return false;
        }

        for (int i = 0; i < uids.Length; i++)
        {
            var uid = uids[i];
            if (uids[i] == loginData.principal) uid = "self";

            if (BroadcastState.TryRead<Data<T>>(out var val, uid) == false)
            {
                //$"Could not find data of type {typeof(T)} of id: {uid.SimplifyAddress()}".Error();
                return false;
            }
        }

        return true;
    }
    public static bool IsDataValidSelf<T>() where T : DataTypes.Base
    {
        return IsDataValid<T>("self");
    }

    public static bool IsDataLoading<T>(params string[] uids) where T : DataTypes.Base
    {
        if (IsUserLoggedIn(out var loginData) == false)
        {
            Debug.LogError("Issue getting loginData!");

            return false;
        }

        if (loginData.state != MainDataTypes.LoginData.State.LoggedIn)
        {
            Debug.LogError("You cannot check shared data as an anon user!");

            return false;
        }

        var loadingDataType = typeof(T).Name;

        if (uids.Length == 0)
        {
            foreach (var loadingTypes in loadingData.Values)
            {
                if (loadingTypes.Contains(loadingDataType)) return true;
            }
        }
        else
        {
            for (int i = 0; i < uids.Length; i++)
            {
                var uid = uids[i];
                if (uids[i] == loginData.principal) uid = "self";

                if (loadingData.TryGetValue(uid, out var loadingTypes))
                {
                    if (loadingTypes.Contains(loadingDataType)) return true;
                }
            }
        }

        return false;
    }

    public static bool IsDataLoadingSelf<T>() where T : DataTypes.Base
    {
        return IsDataLoading<T>("self");
    }

    //

    public static UResult<Data<T>, string> GetData<T>(string uid) where T : DataTypes.Base
    {
        if (IsUserLoggedIn(out var loginData) == false)
        {
            return new("Issue getting loginData!");
        }

        if (loginData.state != MainDataTypes.LoginData.State.LoggedIn)
        {
            return new("You cannot get shared data as an anon user!");
        }

        if (uid == loginData.principal) uid = "self";

        if (BroadcastState.TryRead<Data<T>>(out var val, uid) == false)
        {
            return new($"Data could not be found for DataType: {typeof(T).Name}");
        }

        return new(val);
    }

    public static UResult<Data<T>, string> GetDataSelf<T>() where T : DataTypes.Base
    {
        return GetData<T>("self");
    }

    //

        /// <summary>
        /// Try get an element from collection of "T" Type that derive from Datatypes.Base
        /// </summary>
        /// <typeparam name="T">Datatypes.Base</typeparam>
        /// <returns></returns>
    public static UResult<T, string> GetElementOfType<T>(string uid, string elementId) where T : DataTypes.Base
    {
        var result = GetData<T>(uid);
        if (result.IsErr)
        {
            return new(result.AsErr());
        }

        var asOk = result.AsOk();

        if (asOk.elements.TryGetValue(elementId, out var element) == false) return new($"Data Type: {typeof(T).Name} does not contain element of id: {elementId}.\n\nValid Ids:\n\n{asOk.elements.Reduce(e => $"-{e.Key}", "\n\n")}\nend.");
        return new(element);
    }

    public static UResult<T, string> GetElementOfTypeSelf<T>(string elementId) where T : DataTypes.Base
    {
        return GetElementOfType<T>("self", elementId);
    }

    /// <summary>
    /// Get a collection of "T" Type that derive from Datatypes.Base
    /// </summary>
    /// <typeparam name="T">Datatypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<LinkedList<T>, string> GetElementsOfType<T>(string uid) where T : DataTypes.Base
    {
        var result = GetData<T>(uid);
        if (result.IsErr)
        {
            return new(result.AsErr());
        }

        LinkedList<T> elements = new();
        result.AsOk().elements.Iterate(e =>
        {
            elements.AddLast(e.Value);
        });

        return new(elements);
    }
    public static UResult<LinkedList<T>, string> GetElementsOfTypeSelf<T>() where T : DataTypes.Base
    {
        return GetElementsOfType<T>("self");
    }

    /// <summary>
    /// Try get a property value from an element from collection of "T" Type that derive from Datatypes.Base
    /// </summary>
    /// <typeparam name="T">Datatypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<PropertyType, string> GetPropertyFromType<T, PropertyType>(string uid, string elementId, Func<T, PropertyType> getter) where T : DataTypes.Base
    {
        var restult = GetElementOfType<T>(uid, elementId);

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
    public static UResult<PropertyType, string> GetPropertyFromTypeSelf<T, PropertyType>(string elementId, Func<T, PropertyType> getter) where T : DataTypes.Base
    {
        return GetPropertyFromType<T, PropertyType>("self", elementId, getter);
    }

    public static PropertyType GetPropertyFromType<T, PropertyType>(string uid, string elementId, Func<T, PropertyType> getter, PropertyType defaultVal = default) where T : DataTypes.Base
    {
        var restult = GetElementOfType<T>(uid, elementId);

        if (restult.Tag == UResultTag.Err)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Failure to find property of element: " + elementId);
#endif

            return defaultVal;
        }

        PropertyType propertyType = getter(restult.AsOk());

        return propertyType;
    }
    public static PropertyType GetPropertyFromTypeSelf<T, PropertyType>(string elementId, Func<T, PropertyType> getter, PropertyType defaultVal = default) where T : DataTypes.Base
    {
        return GetPropertyFromType<T, PropertyType>("self", elementId, getter, defaultVal); 
    }

#endregion

    #region DataTypes Main

    public static void AddListenerMainDataChange<T>(this System.Action<T> action, bool invokeOnRegistration = false) where T : MainDataTypes.Base, new()
    {
        BroadcastState.Register<T>(action, invokeOnRegistration);
    }
    public static void RemoveListenerMainDataChange<T>(this System.Action<T> action) where T : MainDataTypes.Base, new()
    {
        BroadcastState.Unregister<T>(action);
    }


    //CLEAR

    /// <summary>
    /// Clean up user data handled by type
    /// </summary>
    //
    public static void ClearMainData<T>() where T : MainDataTypes.Base, new()
    {
        BroadcastState.TryDispose<T>(out var disposedValue);
    }

    //UPDATE

    public static void UpdateMainData<T>(T newVal) where T : MainDataTypes.Base, new()
    {
        //#if UNITY_EDITOR
        //if (newVal != null) $"DATA of type {typeof(T).Name}, has been fetched, value: {JsonConvert.SerializeObject(newVal)}".Log(nameof(UserUtil));
        //#endif

        BroadcastState.ForceInvoke<T>(newVal);
    }

    //GET
    public static bool IsMainDataValid<T>() where T : MainDataTypes.Base, new()
    {
        if (BroadcastState.TryRead<T>(out var val) == false)
        {
            return false;
        }

        return true;
    }

    public static UResult<T, string> GetMainData<T>() where T : MainDataTypes.Base, new()
    {
        if (BroadcastState.TryRead<T>(out var val) == false)
        {
            return new($"Data could not be found for DataType: {typeof(T).Name}");
        }

        return new(val);
    }

    #endregion

}
