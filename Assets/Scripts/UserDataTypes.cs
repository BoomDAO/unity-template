using Candid.extv2_boom;
using Candid.IcpLedger.Models;
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
public struct Token
{
    public string canisterId;
    public string name;
    public ulong rawAmt;
    public ulong decimalCount;
    public ulong baseZeroCount;
    public readonly double Amount
    {
        get
        {
            if (baseZeroCount == 0) return rawAmt;

            return rawAmt / (double)baseZeroCount;
        }
    }

    public Token(string canisterId, string name, ulong amt, ulong decimalCount)
    {
        this.canisterId = canisterId;
        this.name = name;
        this.rawAmt = amt;
        this.decimalCount = decimalCount;
        this.baseZeroCount = decimalCount == 0? 0 : (ulong)Mathf.Pow(10, decimalCount);

        Debug.Log($"{name} | Amt:{Amount}, RawAmt: {rawAmt}, Decimals: {decimalCount}, baseZeroCount: {baseZeroCount}");
    }
}

[Preserve]
[Serializable]
public struct TokensData : IDataState
{
    public Dictionary<string,Token> tokens;

    public TokensData(List<Token> tokens)
    {
        tokens ??= new();
        this.tokens = new();

        foreach (var item in tokens)
        {
            this.tokens.Add(item.canisterId, item);
        }
    }
    public TokensData(TokensData tokenData, params Token[] tokensUpdate)
    {
        tokens = new();

        tokenData.tokens ??= new();

        foreach (var item in tokenData.tokens)
        {
            tokens.Add(item.Key, item.Value);
        }

        foreach (var item in tokensUpdate)
        {
            Update(item);
        }
    }

    private void Update(Token tokenUpdate)
    {
        if (tokens.ContainsKey(tokenUpdate.canisterId))
        {
            tokens[tokenUpdate.canisterId] = tokenUpdate;
        }
        else tokens.Add(tokenUpdate.canisterId, tokenUpdate);
    }
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
            //Debug.Log("Debug configs, element count: " +entityConfigs.Count);
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
                Debug.Log($"- Action Config, ID : {item.Aid}, tag : {item.Tag}, content : {JsonConvert.SerializeObject(item)}");

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
public struct ListingData : IDataState
{
    public Dictionary<string, Extv2BoomApiClient.ListingsArg0Item> listing;

    public ListingData(Dictionary<string, Extv2BoomApiClient.ListingsArg0Item> listing = null)
    {
        this.listing = listing ?? new();

        Debug.Log($"FETCHED Listing: {JsonConvert.SerializeObject(this.listing)}");
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