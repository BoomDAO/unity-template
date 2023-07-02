// Ignore Spelling: Util

using Candid.World.Models;
using EdjCase.ICP.Agent.Agents;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using System.Collections.Generic;
using UnityEngine;

public static class UserUtil
{
    #region Login
    public enum SigningType { user, anon }
    public static void RegisterToLoginDataChange(this System.Action<DataState<SignInData>> action, bool invokeOnRegistration = false)
    {
        BroadcastState.Register<DataState<SignInData>>(action, invokeOnRegistration);
    }
    public static void UnregisterToLoginDataChange(this System.Action<DataState<SignInData>> action)
    {
        BroadcastState.Unregister<DataState<SignInData>>(action);
    }
    public static void StartLogin(string json, bool useLocalHost, string loadingMessage = "Loading...")
    {
        Broadcast.Invoke<StartLogin>(new StartLogin(json, useLocalHost));

        BroadcastState.ForceInvoke<DataState<SignInData>>(e =>
        {
            e.SetAsLoading(loadingMessage);

            return e;
        });
    }
    public static void UpdateLoginData(IAgent agent, string principal, string accountIdentifier, bool asAnon)
    {
        BroadcastState.ForceInvoke<DataState<SignInData>>(e =>
        {
            e.data = new SignInData(agent, principal, accountIdentifier, asAnon);
            e.SetAsReady();
            return e;
        });
    }

    public static UResult<SignInData, string> GetSignInData()
    {
        if (BroadcastState.TryRead<DataState<SignInData>>(out var state) == false)
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
    public static UResult<Wrapper<string>, string> GetPrincipal()
    {
        var result = GetSignInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(result.AsErr());
        }

        return new(new Wrapper<string>(result.AsOk().principal));
    }
    public static UResult<Wrapper<string>, string> GetAccountIdentifier()
    {
        var result = GetSignInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(result.AsErr());
        }

        return new(new Wrapper<string>(result.AsOk().accountIdentifier));
    }
    public static UResult<IAgent, string> GetAgent()
    {
        var result = GetSignInData();
        if (result.Tag == UResultTag.Err)
        {
            return new(result.AsErr());
        }

        return new(result.AsOk().agent);
    }

    public static UResult<SigningType, string> GetSignInType()
    {
        var getSignInDataResult = UserUtil.GetSignInData();

        if (getSignInDataResult.Tag == UResultTag.Err)
        {
            return new(getSignInDataResult.AsErr());
        }

        return getSignInDataResult.AsOk().asAnon ? new(SigningType.anon) : new(SigningType.user);
    }

    public static UResult<bool, string> IsUserSignedIn()
    {
        var getSignInDataResult = UserUtil.GetSignInData();

        if (getSignInDataResult.Tag == UResultTag.Err)
        {
            return new(getSignInDataResult.AsErr());
        }

        return new(getSignInDataResult.AsOk().asAnon == false) ;
    }
    public static UResult<bool, string> IsAnonSignedIn()
    {
        var getSignInDataResult = UserUtil.GetSignInData();

        if (getSignInDataResult.Tag == UResultTag.Err)
        {
            return new(getSignInDataResult.AsErr());
        }

        return new(getSignInDataResult.AsOk().asAnon);
    }
    #endregion

    #region User Data Management
    public static (DataTypes.Item[] items, DataTypes.Stat[] stats) ProcessEntities(this List<Entity> entities)
    {
        List<DataTypes.Item> items = new();
        List<DataTypes.Stat> stats = new();

        if (entities != null)
        {
            Dictionary<string, Entity> _entities = new();

            foreach (var e in entities)
            {
                var entityConfigResponse = UserUtil.GetDataElementOfType<DataTypes.EntityConfig>($"{e.Gid}{e.Eid}");
                if (entityConfigResponse.Tag == UResultTag.Err)
                {
                    continue;
                }

                var config = entityConfigResponse.AsOk();

                if (config.Tag.Contains("item"))
                {
                    items.Add(new DataTypes.Item(e.Eid, e.Quantity.ValueOrDefault));
                }
                else if (config.Tag.Contains("stat"))
                {
                    e.Expiration.ValueOrDefault.TryToUInt64(out var lastTs);
                    stats.Add(new DataTypes.Stat(e.Eid, e.Quantity.ValueOrDefault, lastTs));
                }
            }
        }

        return (items.ToArray(), stats.ToArray());
    }

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
    /// Request Data from some Canisters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optional"></param>
    /// <param name="loadingMessage"></param>
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
    /// Get the data state of some "T" Type that derive from DataTypes.Base
    /// </summary>
    /// <typeparam name="T">DataTypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<DataState<Data<T>>, string> GetDataStateOfType<T>() where T : DataTypes.Base
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
    /// Get an element from collection of "T" Type that derive from Datatypes.Base
    /// </summary>
    /// <typeparam name="T">Datatypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<T, string> GetDataElementOfType<T>(string id) where T : DataTypes.Base
    {
        if (BroadcastState.TryRead<DataState<Data<T>>>(out var val) == false)
        {
            return new($"Data could not be found for DataType: {typeof(T).Name}");
        }

        if (val.IsReady() == false) return new($"Data of type {typeof(T).Name} is not yet ready");

        if (val.data.elements.TryGetValue(id, out var token) == false) return new($"Data does not contain element of id: {id}.\n\nValid Action Ids:\n\n{val.data.elements.Reduce(e=>$"-{e.Key}","\n\n")}\nend.");
        return new(token);
    }
    /// <summary>
    /// Get a collection of "T" Type that derive from Datatypes.Base
    /// </summary>
    /// <typeparam name="T">Datatypes.Base</typeparam>
    /// <returns></returns>
    public static UResult<List<T>, string> GetDataElementsOfType<T>() where T : DataTypes.Base
    {
        if (BroadcastState.TryRead<DataState<Data<T>>>(out var val) == false)
        {
            return new($"Data could not be found for DataType: {typeof(T).Name}");
        }

        if (val.IsReady() == false) return new("Data is not yet ready");

        List<T> tokens = new();
        val.data.elements.Iterate(e =>
        {
            tokens.Add(e.Value);
        });

        return new(tokens);
    }

    public static void CleanUpData()
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Token>>>(e =>
        {
            e.Clear();
            return e;
        });
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Item>>>(e =>
        {
            e.Clear();
            return e;
        });
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Stat>>>(e =>
        {
            e.Clear();
            return e;
        });
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.NftCollection>>>(e =>
        {
            e.Clear();
            return e;
        });
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.BoomDaoNftCollection>>>(e =>
        {
            e.Clear();
            return e;
        });
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Stake>>>(e =>
        {
            e.Clear();
            return e;
        });
    }

    #region Data Update
    public static void UpdateData(params DataTypes.Token[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Token>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }
    public static void UpdateData(params DataTypes.Item[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Item>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }
    public static void UpdateData(params DataTypes.Stat[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Stat>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }
    public static void UpdateData(params DataTypes.NftCollection[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.NftCollection>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }
    public static void UpdateData(params DataTypes.BoomDaoNftCollection[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.BoomDaoNftCollection>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }
    public static void UpdateData(params DataTypes.Stake[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Stake>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }

    public static void UpdateData(params DataTypes.ActionConfig[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.ActionConfig>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }
    public static void UpdateData(params DataTypes.EntityConfig[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.EntityConfig>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }

    public static void UpdateData(params DataTypes.Listing[] newVals)
    {
        BroadcastState.ForceInvoke<DataState<Data<DataTypes.Listing>>>(e =>
        {
            e.data = new(
                //Pass PrevData
                e.data,
                //Pass Key Getter
                e => e.GetKey(),
                //Pass Tokens to Override or Add
                newVals
                );
            //Set as Ready
            e.SetAsReady();
            return e;
        });
    }
    #endregion ^^

    #endregion ^
}
