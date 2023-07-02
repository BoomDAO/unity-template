using Candid.extv2_boom;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;


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
        public string name;
        public ulong tokenizedAmount;
        public ulong decimalCount;
        public ulong baseUnitCount;
        public double Amount
        {
            get
            {
                if (baseUnitCount == 0) return tokenizedAmount;

                return tokenizedAmount / (double)baseUnitCount;
            }
        }

        public Token(string canisterId, string name, ulong amt, ulong decimalCount)
        {
            this.canisterId = canisterId;
            this.name = name;
            this.tokenizedAmount = amt;
            this.decimalCount = decimalCount;
            this.baseUnitCount = decimalCount == 0 ? 0 : (ulong)Mathf.Pow(10, decimalCount);

            $"{name} | Amt:{Amount}, RawAmt: {tokenizedAmount}, Decimals: {decimalCount}, baseUnitCount: {baseUnitCount}".Log(nameof(DataTypes));
        }

        public override string GetKey()
        {
            return canisterId;
        }
    }

    [Preserve]
    [Serializable]
    public class Item : Base
    {
        [Preserve] public string id;
        [Preserve] public double quantity = 1;

        public Item(string id, double quantity)
        {
            this.id = id;
            this.quantity = quantity;
        }
        public override string GetKey()
        {
            return id;
        }
    }

    [Preserve]
    [Serializable]
    public class Stat : Base
    {
        [Preserve] public string id;
        [Preserve] public double quantity = 1;
        [Preserve][HideInInspector] public ulong lastTs = 0;

        public Stat(string id, double quantity, ulong lastTs)
        {
            this.id = id;
            this.quantity = quantity;
            this.lastTs = lastTs;
        }
        public override string GetKey()
        {
            return id;
        }
    }

    [Preserve]
    [Serializable]
    public class NftCollection : Base
    {
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


        [Preserve] public string name;
        [Preserve] public string canisterId;
        [Preserve] public string standard;
        [Preserve] public string icon;
        [Preserve] public string description;
        [Preserve] public List<DabNftDetails> tokens = new List<DabNftDetails>();

        public override string GetKey()
        {
            return canisterId;
        }
    }

    [Preserve]
    [Serializable]
    public class BoomDaoNftCollection : Base
    {
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


        [Preserve] public string name;
        [Preserve] public string canisterId;
        [Preserve] public string standard;
        [Preserve] public string icon;
        [Preserve] public string description;
        [Preserve] public List<DabNftDetails> tokens = new List<DabNftDetails>();
        public override string GetKey()
        {
            return canisterId;
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
        public EntityConfig(Candid.World.Models.EntityConfig arg)
        {
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
        public OptionalValue<string> ImageUrl { get; set; }
        public string Metadata { get; set; }
        public OptionalValue<string> Name { get; set; }
        public OptionalValue<string> ObjectUrl { get; set; }
        public OptionalValue<string> Rarity { get; set; }
        public string Tag { get; set; }

        public override string GetKey()
        {
            return $"{Gid}{Eid}";
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
        }

        public OptionalValue<ActionConstraint> ActionConstraint { get; set; }
        public OptionalValue<ActionPlugin> ActionPlugin { get; set; }
        public ActionResult ActionResult { get; set; }
        public string Aid { get; set; }
        public OptionalValue<string> Description { get; set; }
        public OptionalValue<string> Name { get; set; }
        public OptionalValue<string> Tag { get; set; }

        public override string GetKey()
        {
            return $"{Aid}";
        }
    }

    [Preserve]
    [Serializable]
    public class Listing : Base
    {
        public Listing(string tokenIdentifier, Candid.extv2_boom.Extv2BoomApiClient.ListingsArg0Item arg)
        {
            this.tokenIdentifier = tokenIdentifier;
            F0 = arg.F0;
            F1 = arg.F1;
            F2 = arg.F2;
        }

        public string tokenIdentifier;
        public uint F0 { get; set; }
        public Candid.extv2_boom.Models.Listing F1 { get; set; }
        public Candid.extv2_boom.Models.MetadataLegacy F2 { get; set; }

        public override string GetKey()
        {
            return $"{F0}";
        }
    }
}