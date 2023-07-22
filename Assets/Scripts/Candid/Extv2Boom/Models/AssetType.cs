using TokenIndex__1 = System.UInt32;
using TokenIdentifier__2 = System.String;
using TokenIdentifier__1 = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.Extv2Boom.Models.MetadataValue>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField__1 = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle__1 = System.String;
using AccountIdentifier__2 = System.String;
using AccountIdentifier__1 = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System.Collections.Generic;
using System;

namespace Candid.Extv2Boom.Models
{
	[Variant(typeof(AssetTypeTag))]
	public class AssetType
	{
		[VariantTagProperty()]
		public AssetTypeTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public AssetType(AssetTypeTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected AssetType()
		{
		}

		public static AssetType Canister(AssetType.CanisterInfo info)
		{
			return new AssetType(AssetTypeTag.Canister, info);
		}

		public static AssetType Direct(List<ChunkId> info)
		{
			return new AssetType(AssetTypeTag.Direct, info);
		}

		public static AssetType Other(string info)
		{
			return new AssetType(AssetTypeTag.Other, info);
		}

		public AssetType.CanisterInfo AsCanister()
		{
			this.ValidateTag(AssetTypeTag.Canister);
			return (AssetType.CanisterInfo)this.Value!;
		}

		public List<ChunkId> AsDirect()
		{
			this.ValidateTag(AssetTypeTag.Direct);
			return (List<ChunkId>)this.Value!;
		}

		public string AsOther()
		{
			this.ValidateTag(AssetTypeTag.Other);
			return (string)this.Value!;
		}

		private void ValidateTag(AssetTypeTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class CanisterInfo
		{
			[CandidName("canister")]
			public string Canister { get; set; }

			[CandidName("id")]
			public AssetId Id { get; set; }

			public CanisterInfo(string canister, AssetId id)
			{
				this.Canister = canister;
				this.Id = id;
			}

			public CanisterInfo()
			{
			}
		}
	}

	public enum AssetTypeTag
	{
		[CandidName("canister")]
		[VariantOptionType(typeof(AssetType.CanisterInfo))]
		Canister,
		[CandidName("direct")]
		[VariantOptionType(typeof(List<ChunkId>))]
		Direct,
		[CandidName("other")]
		[VariantOptionType(typeof(string))]
		Other
	}
}