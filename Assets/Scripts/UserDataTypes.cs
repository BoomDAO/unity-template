using Candid.World.Models;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
[Serializable]
public class StandardEntity
{
    [Preserve] public string wid;
    [Preserve] public string eid;
    [Preserve] public UOption<double> quantity;
    [Preserve] public UOption<ulong> expiration;
}
[Preserve] [Serializable]
public class ItemData
{
    [Preserve] public string id;
    [Preserve] public double quantity = 1;

    public ItemData(string id, double quantity)
    {
        this.id = id;
        this.quantity = quantity;
    }
}
[Preserve]
[Serializable]
public class BuffItemData : ItemData
{
    [Preserve][HideInInspector] public ulong startTs = 0;

    public BuffItemData(string id, double quantity, ulong ts) : base(id, quantity)
    {
        this.startTs = ts;
    }
}

[Preserve]
[Serializable]
public class Stat : ItemData
{
    [Preserve][HideInInspector] public ulong lastTs = 0;

    public Stat(string id, double quantity, ulong lastTs) : base(id, quantity)
    {
        this.lastTs = lastTs;
    }
}
[Preserve]
[Serializable]
public class Raw
{
    [Preserve] public string id;
    [Preserve] public string type;
    [Preserve] public string raw;
}

[Preserve]
public class DabNftCollection
{
    [Preserve] public string name;
    [Preserve] public string canisterId;
    [Preserve] public string standard;
    [Preserve] public string icon;
    [Preserve] public string description;
    [Preserve] public List<DabNftDetails> tokens = new List<DabNftDetails>();
}

[Preserve]
public class DabNftDetails
{
    [Preserve] public long index;
    [Preserve] public string canister;
    [Preserve] public string tokenIdentifier;
    [Preserve] public string name;
    [Preserve] public string url;
    [Preserve] public string metadata; // can be anything?
    [Preserve] public string standard;
    [Preserve] public string collection;
}

[Preserve]
[Serializable]
public class Stake
{
    public uint Amount { get; set; }
    public string CanisterId { get; set; }
    public string? Index { get; set; }
    public string TokenType { get; set; }

    public Stake(uint amount,string canisterId,string? index, string tokenType)
    {
        this.Amount = amount;
        this.CanisterId = canisterId;
        this.Index = index;
        this.TokenType = tokenType;
    }

    public Stake()
    {
    }
}

[Preserve]
[Serializable]
public struct IcpData : IDataState
{
    public long amt;
}
[Preserve]
[Serializable]
public struct IcrcData : IDataState
{
    public string name;
    public long amt;
    public byte decimalCount;
}
[Preserve]
[Serializable]
public struct WorldConfigsData : IDataState
{
    public Dictionary<string, EntityConfig> entities;
    public Dictionary<string, ActionConfig> actions;

    public WorldConfigsData(List<EntityConfig> entityConfigs, List<ActionConfig> actionConfigs)
    {
        entities = new();
        actions = new();

        if(entityConfigs != null)
        {
            Debug.Log("Debug configs, element count: " +entityConfigs.Count);
            foreach (var item in entityConfigs)
            {
                //Debug.Log($"- Entity Config, ID : {item.Eid}, tag : {item.Tag}");

                entities.Add(item.Eid, item);
            }
        }
        if (actionConfigs != null)
        {
            Debug.Log("Debug configs, element count: " + actionConfigs.Count);
            foreach (var item in actionConfigs)
            {
                //Debug.Log($"- Action Config, ID : {item.Aid}, type : {item.ActionDataType.Tag}");

                actions.Add(item.Aid, item);
            }
        }
    }
}
[Preserve]
[Serializable]
public struct UserNodeData : IDataState
{

    public Dictionary<string, ItemData> itemEntities;
    public Dictionary<string, Stat> statEntities;

    public UserNodeData(List<Candid.World.Models.Entity> entities)
    {
        this.itemEntities = new();
        this.statEntities = new();

        if (entities != null)
        {
            Dictionary<string, Entity> _entities = new();

            foreach (var e in entities)
            {
                Debug.Log($"> Entity Instnace, ID : {e.Eid}, group {e.Gid}");
                if (UserUtil.TryGetEntityConfigData(e.Eid, out var config))
                {
                    if (config.Tag.Contains("item"))
                    {
                        itemEntities.Add(e.Eid, new ItemData(e.Eid, e.Quantity.ValueOrDefault));
                    }
                    else if (config.Tag.Contains("stat"))
                    {
                        e.Expiration.ValueOrDefault.TryToUInt64(out var lastTs);
                        itemEntities.Add(e.Eid, new Stat(e.Eid, e.Quantity.ValueOrDefault, lastTs));
                    }
                }
            }
        }
    }
}
[Preserve]
[Serializable]
public struct DabNftsData : IDataState
{
    public List<DabNftCollection> nonPlethoraNftCollections;
    public List<DabNftCollection> plethoraNftCollections;

    public DabNftsData(List<DabNftCollection> nonPlethoraNftCollections = null, List<DabNftCollection> plethoraNftCollections = null)
    {
        this.nonPlethoraNftCollections = nonPlethoraNftCollections ?? new();
        this.plethoraNftCollections = plethoraNftCollections ?? new();

        Debug.Log($"FETCHED NFTS: {JsonConvert.SerializeObject(this.nonPlethoraNftCollections)}");
        Debug.Log($"FETCHED PLETHORA NFTS: {JsonConvert.SerializeObject(this.plethoraNftCollections)}");
    }
}
[Preserve]
[Serializable]
public struct StakeData : IDataState
{
    public List<Stake> stakes;

    public StakeData(List<Stake> stakes = default)
    {
        this.stakes = stakes ?? new();
    }
}