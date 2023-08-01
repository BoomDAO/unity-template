// Ignore Spelling: metadata eid gid wid

using Candid.IcpLedger.Models;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public uint amount;
        public string canisterId;
        public string? blockIndex;
        public string tokenType;

        public Stake(uint amount, string canisterId, string? blockIndex, string tokenType)
        {
            this.amount = amount;
            this.canisterId = canisterId;
            this.blockIndex = blockIndex;
            this.tokenType = tokenType;
        }

        public Stake()
        {
        }

        public override string GetKey()
        {
            return $"{canisterId}{blockIndex}";
        }
    }

    [Preserve]
    [Serializable]
    public class EntityConfig : Base
    {
        public EntityConfig(string wid, Candid.World.Models.EntityConfig arg)
        {
            this.wid = wid;
            description = arg.Description.HasValue ? arg.Description.ValueOrDefault : null;
            duration = null;
            if (arg.Duration.HasValue)
            {
                arg.Duration.ValueOrDefault.TryToUInt64(out ulong _duration);

                duration = _duration;
            }
            eid = arg.Eid;
            gid = arg.Gid;
            imageUrl = arg.ImageUrl.HasValue? arg.ImageUrl.ValueOrDefault : null;
            metadata = arg.Metadata.HasValue ? arg.Metadata.ValueOrDefault : null;
            name = arg.Name.HasValue ? arg.Name.ValueOrDefault : null;
            objectUrl = arg.ObjectUrl.HasValue ? arg.ObjectUrl.ValueOrDefault : null;
            rarity = arg.Rarity.HasValue ? arg.Rarity.ValueOrDefault : null;
            tag = arg.Tag.HasValue ? arg.Tag.ValueOrDefault : null;
        }

        public string description;
        public ulong? duration;
        public string eid;
        public string gid;
        public string wid;
        public string imageUrl;
        public string metadata;
        public string name;
        public string objectUrl;
        public string rarity;
        public string tag;

        public override string GetKey()
        {
            return $"{wid}{gid}{eid}";
        }
    }

    [Preserve]
    [Serializable]
    public class ActionConfig : Base
    {
        public ActionConfig(Candid.World.Models.ActionConfig arg)
        {
            if (arg.ActionConstraint.HasValue)
            {
                timeConstraint = arg.ActionConstraint.ValueOrDefault.TimeConstraint.ValueOrDefault;
                entityConstraints = arg.ActionConstraint.ValueOrDefault.EntityConstraint.ValueOrDefault;
            }

            actionPlugin = arg.ActionPlugin.HasValue ? arg.ActionPlugin.ValueOrDefault : null;
            actionResult = arg.ActionResult;
            aid = arg.Aid;
            description = arg.Description.HasValue ? arg.Description.ValueOrDefault : null;
            name = arg.Name.HasValue ? arg.Name.ValueOrDefault : null;
            tag = arg.Tag.HasValue ? arg.Tag.ValueOrDefault : null;
            imageUrl = arg.ImageUrl.HasValue ? arg.ImageUrl.ValueOrDefault : null;
        }

        public ActionConstraint.TimeConstraintItem timeConstraint;
        public List<ActionConstraint.EntityConstraintItemItem> entityConstraints;
        public ActionPlugin actionPlugin;
        public ActionResult actionResult;
        public string aid;
        public string description;
        public string name;
        public string tag;
        public string imageUrl;

        public override string GetKey()
        {
            return $"{aid}";
        }
    }

    [Preserve]
    [Serializable]
    public class Listing : Base
    {
        public Listing(string tokenIdentifier, Candid.Extv2Boom.Extv2BoomApiClient.ListingsArg0Item arg)
        {
            this.tokenIdentifier = tokenIdentifier;
            index = arg.F0;
            details = arg.F1;
            metadataLegacy = arg.F2;
        }

        public string tokenIdentifier;
        public uint index;
        public Candid.Extv2Boom.Models.Listing details;
        public Candid.Extv2Boom.Models.MetadataLegacy metadataLegacy;

        public override string GetKey()
        {
            return $"{index}";
        }
    }
}