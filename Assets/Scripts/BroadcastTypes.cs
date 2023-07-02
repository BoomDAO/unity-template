using System;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Agents;
using ItsJackAnton.Patterns.Broadcasts;
using UnityEngine;
using UnityEngine.Scripting;

public enum DataState
{
    None, Loading, Ready
}
public interface IDataState { }
public class DataState<T> : IBroadcastState where T : struct, IDataState
{
    public DataState State { get; private set; } = DataState.None;
    public string LoadingMsg { get; private set; } = "";
    public T data;

    public DataState()
    {
        State = DataState.None;
        this.data = new();
    }
    public DataState(T data)
    {
        State = DataState.Ready;
        this.data = data;
    }

    public void Clear()
    {
        LoadingMsg = "";
        data = new();
        //SetAsReady();
        State = DataState.None;
    }
    public bool IsLoading()
    {
        return State == DataState.Loading;
    }
    public bool IsReady()
    {
        return State == DataState.Ready && IsLoading() == false;
    }
    public bool IsNull()
    {
        return State == DataState.None;
    }

    public void SetAsLoading(string loadingMsg = "Loading...")
    {
        this.LoadingMsg = loadingMsg;

        State = DataState.Loading;
    }
    public void SetAsReady()
    {
        LoadingMsg = "";

        State = DataState.Ready;
    }
}
public struct Data<T> : IDataState
{
    public Dictionary<string, T> elements;

    public Data(List<T> elements, Func<T, string> getKey)
    {
        elements ??= new();
        this.elements = new();

        foreach (var item in elements)
        {
            this.elements.Add(getKey(item), item);
        }
    }
    public Data(Data<T> tokenData, Func<T, string> getKey, params T[] tokensUpdate)
    {
        tokenData.elements ??= new();
        elements = tokenData.elements;

        foreach (var item in tokensUpdate)
        {
            string key = getKey(item);

            if (elements.ContainsKey(key))
            {
                elements[key] = item;
            }
            else elements.Add(key, item);
        }
    }
}

public struct DisableButtonInteraction : IBroadcastState
{
    public bool disable;

    public DisableButtonInteraction(bool disable)
    {
        this.disable = disable;
    }
}

#region MarketPlace
    public struct ListingNftState : IBroadcastState
    {
        public bool isListing;

        public ListingNftState(bool isListing)
        {
            this.isListing = isListing;
        }
    }
    public struct PurchasingNftState : IBroadcastState
    {
        public bool isPurchasing;

        public PurchasingNftState(bool isPurchasing)
        {
            this.isPurchasing = isPurchasing;
        }
    }
#endregion

#region Fetch Data Request
public struct FetchDataReq<T> : IBroadcast where T : DataTypes.Base
{
    public object optional;

    public FetchDataReq(object optional)
    {
        this.optional = optional;
    }
}

#endregion

#region User
public struct StartLogin: IBroadcast
{
    public string json;
    public bool useLocalHost;

    public StartLogin(string json, bool useLocalHost)
    {
        this.json = json;
        this.useLocalHost = useLocalHost;
    }
}
public struct SignInData: IDataState
{
    public IAgent agent;
    public string principal;
    public string accountIdentifier;
    public bool asAnon;
    public SignInData(IAgent agent, string principal, string accountIdentifier, bool asAnon)
    {
        this.agent = agent;
        this.principal = principal;
        this.accountIdentifier = accountIdentifier;
        this.asAnon = asAnon;
    }
}

public struct UserLogout : IBroadcast { }
#endregion
