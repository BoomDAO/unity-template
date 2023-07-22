using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

public class NftCollectionToFetch
{
    public string collectionId;
    public string collectionName;
    public bool isStandard;

    public NftCollectionToFetch(string collectionId, string collectionName, bool isStandard)
    {
        this.collectionId = collectionId;
        this.collectionName = collectionName;
        this.isStandard = isStandard;
    }
}

public static class DataTypes
{
    public abstract class Base
    {
        public abstract string GetKey();
    }

    [Preserve]
    [Serializable]
    public class Token : Base
    {
        public string canisterId;
        public ulong baseUnitAmount;
        public Token(string canisterId, ulong baseUnitAmount)
        {
            this.canisterId = canisterId;
            this.baseUnitAmount = baseUnitAmount;
        }

        public override string GetKey()
        {
            return canisterId;
        }
    }

    [Preserve]
    [Serializable]
    public class TokenConfig : Base
    {
        public string canisterId;
        public string name;
        public string symbol;
        public byte decimals;
        public ulong fee;

        public TokenConfig(string canisterId, string name, string symbol, byte decimals, ulong fee)
        {
            this.canisterId = canisterId;
            this.name = name;
            this.symbol = symbol;
            this.decimals = decimals;
            this.fee = fee;
        }

        public override string GetKey()
        {
            return canisterId;
        }
    }

    [Preserve]
    [Serializable]
    public class Entity : Base
    {
        [Preserve] public string wid;
        [Preserve] public string gid;
        [Preserve] public string eid;
        [Preserve] public double? quantity = 0;
        [Preserve] public string? attribute = "";
        [Preserve] public ulong? lastTs = 0;

        public Entity(string wid, string gid, string eid, double? quantity, string? attribute, ulong? lastTs)
        {
            this.wid = wid;
            this.gid = gid;
            this.eid = eid;
            this.quantity = quantity;
            this.attribute = attribute;
            this.lastTs = lastTs;
        }
        public override string GetKey()
        {
            return $"{wid}{gid}{eid}";
        }
    }

    [Preserve]
    [Serializable]
    public class Action : Base
    {
        public string actionId;
        public ulong actionCount;
        public ulong intervalStartTs;

        public Action(string actionId, ulong actionCount, ulong intervalStartTs)
        {
            this.actionId = actionId;
            this.actionCount = actionCount;
            this.intervalStartTs = intervalStartTs;
        }

        public override string GetKey()
        {
            return actionId;
        }
    }

    [Preserve]
    [Serializable]
    public class NftCollection : Base
    {
        [Preserve]
        [Serializable]
        public class Nft
        {
            [Preserve] public string canister;
            [Preserve] public uint index;
            [Preserve] public string tokenIdentifier;
            [Preserve] public string url;
            [Preserve] public string metadata;

            public Nft(string canister, uint index, string tokenIdentifier, string url, string metadata)
            {
                this.canister = canister;
                this.index = index;
                this.tokenIdentifier = tokenIdentifier;
                this.url = url;
                this.metadata = metadata;
            }
        }

        [Preserve] public string collectionName;
        [Preserve] public string canister;
        [Preserve] public List<Nft> tokens = new();
        public override string GetKey()
        {
            return canister;
        }
    }

    [Preserve]
    [Serializable]
    public class Stake : Base
    {
        public uint Amount { get; set; }
        public string CanisterId { get; set; }
        public string? BlockIndex { get; set; }
        public string TokenType { get; set; }

        public Stake(uint amount, string canisterId, string? blockIndex, string tokenType)
        {
            this.Amount = amount;
            this.CanisterId = canisterId;
            this.BlockIndex = blockIndex;
            this.TokenType = tokenType;
        }

        public Stake()
        {
        }

        public override string GetKey()
        {
            return $"{CanisterId}{BlockIndex}";
        }
    }

    [Preserve]
    [Serializable]
    public class EntityConfig : Base
    {
        public EntityConfig(string wid, Candid.World.Models.EntityConfig arg)
        {
            Wid = wid;
            Description = arg.Description;
            Duration = arg.Duration;
            Eid = arg.Eid;
            Gid = arg.Gid;
            ImageUrl = arg.ImageUrl;
            Metadata = arg.Metadata;
            Name = arg.Name;
            ObjectUrl = arg.ObjectUrl;
            Rarity = arg.Rarity;
            Tag = arg.Tag;
        }

        public OptionalValue<string> Description { get; set; }
        public OptionalValue<UnboundedUInt> Duration { get; set; }
        public string Eid { get; set; }
        public string Gid { get; set; }
        public string Wid { get; set; }
        public OptionalValue<string> ImageUrl { get; set; }
        public string Metadata { get; set; }
        public OptionalValue<string> Name { get; set; }
        public OptionalValue<string> ObjectUrl { get; set; }
        public OptionalValue<string> Rarity { get; set; }
        public string Tag { get; set; }

        public override string GetKey()
        {
            return $"{Wid}{Gid}{Eid}";
        }
    }

    [Preserve]
    [Serializable]
    public class ActionConfig : Base
    {
        public ActionConfig(Candid.World.Models.ActionConfig arg)
        {
            ActionConstraint = arg.ActionConstraint;
            ActionPlugin = arg.ActionPlugin;
            ActionResult = arg.ActionResult;
            Aid = arg.Aid;
            Description = arg.Description;
            Name = arg.Name;
            Tag = arg.Tag;
            ImageUrl = arg.ImageUrl;
        }

        public OptionalValue<ActionConstraint> ActionConstraint { get; set; }
        public OptionalValue<ActionPlugin> ActionPlugin { get; set; }
        public ActionResult ActionResult { get; set; }
        public string Aid { get; set; }
        public OptionalValue<string> Description { get; set; }
        public OptionalValue<string> Name { get; set; }
        public OptionalValue<string> Tag { get; set; }
        public OptionalValue<string> ImageUrl { get; set; }

        public override string GetKey()
        {
            return $"{Aid}";
        }
    }

    [Preserve]
    [Serializable]
    public class Listing : Base
    {
        public Listing(string tokenIdentifier, Candid.Extv2Boom.Extv2BoomApiClient.ListingsArg0Item arg)
        {
            this.tokenIdentifier = tokenIdentifier;
            Index = arg.F0;
            Details = arg.F1;
            MetadataLegacy = arg.F2;
        }

        public string tokenIdentifier;
        public uint Index { get; set; }
        public string TokenIdentifier { get; set; }
        public Candid.Extv2Boom.Models.Listing Details { get; set; }
        public Candid.Extv2Boom.Models.MetadataLegacy MetadataLegacy { get; set; }

        public override string GetKey()
        {
            return $"{Index}";
        }
    }
}