using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.ext_v2_standard.Models.MetadataValueItem>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.ext_v2_standard.Models;
using System.Collections.Generic;
using System;

namespace Candid.ext_v2_standard.Models
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

		public static AssetType Canister(AssetType.CanisterRecord info)
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

		public AssetType.CanisterRecord AsCanister()
		{
			this.ValidateTag(AssetTypeTag.Canister);
			return (AssetType.CanisterRecord)this.Value!;
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

		public class CanisterRecord
		{
			[CandidName("canister")]
			public string Canister { get; set; }

			[CandidName("id")]
			public AssetId Id { get; set; }

			public CanisterRecord(string canister, AssetId id)
			{
				this.Canister = canister;
				this.Id = id;
			}

			public CanisterRecord()
			{
			}
		}
	}

	public enum AssetTypeTag
	{
		[CandidName("canister")]
		[VariantOptionType(typeof(AssetType.CanisterRecord))]
		Canister,
		[CandidName("direct")]
		[VariantOptionType(typeof(List<ChunkId>))]
		Direct,
		[CandidName("other")]
		[VariantOptionType(typeof(string))]
		Other
	}
}