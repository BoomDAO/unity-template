using System;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Agents;
using Boom.Patterns.Broadcasts;
using UnityEngine;
using UnityEngine.Scripting;

public enum DataState
{
    None, Loading, Ready
}
public interface IDataState { }
public class DataState<T> : IBroadcastState where T : IDataState, new()
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
        State = DataState.None;
    }
    public bool IsLoading()
    {
        return State == DataState.Loading;
    }
    public bool IsReady()
    {
        return State == DataState.Ready;
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
    public void SetAsReady(T data)
    {
        LoadingMsg = "";
        this.data = data;
        State = DataState.Ready;
    }
}
public class Data<T> : IDataState
{
    public Dictionary<string, T> elements;
    public Data()
    {
        this.elements = new();
    }
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

        if (tokensUpdate == null) return;

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

public struct WaitingForResponse : IBroadcastState
{
    public bool value;
    public string waitingMessage;

    public WaitingForResponse(bool disable, string waitingMessage = "")
    {
        this.value = disable;
        this.waitingMessage = waitingMessage;
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
}
public struct LoginData: IDataState
{
    public IAgent agent;
    public string principal;
    public string accountIdentifier;
    public bool asAnon;
    public LoginData(IAgent agent, string principal, string accountIdentifier, bool asAnon)
    {
        this.agent = agent;
        this.principal = principal;
        this.accountIdentifier = accountIdentifier;
        this.asAnon = asAnon;
    }
}

public struct UserLogout : IBroadcast { }
#endregion
