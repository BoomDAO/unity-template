using System;
using System.Collections.Generic;
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


public struct ToggleActionWidgetState : IBroadcastState
{
    public bool enable;

    public ToggleActionWidgetState(bool value)
    {
        this.enable = value;
    }
}

public struct ConfigsState : IBroadcastState
{
    public bool ready;

    public ConfigsState(bool ready)
    {
        this.ready = ready;
    }
}
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
#region User

public struct FetchBalanceReqIcp : IBroadcast { }
public struct FetchckBalanceReqIcrc : IBroadcast { }

public struct UserLogin: IBroadcast
{
    public string principal;
    public string address;

    public UserLogin(string principal, string address)
    {
        this.principal = principal;
        this.address = address;
    }
}

public struct UserLogout : IBroadcast { }
public struct AnonLogin : IBroadcast
{
    public string principal;
    public string address;

    public AnonLogin(string principal, string address)
    {
        this.principal = principal;
        this.address = address;
    }
}


//public struct CoreUserDataState : IBroadcastState
//{
//    //From Core Canister
//    public string uid;
//    public string profileUserName;
//    public string profileAvatar;
//    public string profileLogoUrl;
//    public Dictionary<string, ItemData> items;
//    public HashSet<string> boughtOffers;
//}

//public struct GameUserDataState : IBroadcastState
//{
//    public Dictionary<string, ItemData> items;
//    public Dictionary<string, BuffItemData> buffs;
//    public Dictionary<string, Achievement> achievements;
//}
#endregion
