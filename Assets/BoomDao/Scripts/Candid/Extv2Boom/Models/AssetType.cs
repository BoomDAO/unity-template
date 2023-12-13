using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using System.Collections.Generic;
using ChunkId = System.UInt32;
using AssetId = System.UInt32;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class AssetType
	{
		[VariantTagProperty]
		public AssetTypeTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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

		public static AssetType Direct(AssetType.DirectInfo info)
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

		public AssetType.DirectInfo AsDirect()
		{
			this.ValidateTag(AssetTypeTag.Direct);
			return (AssetType.DirectInfo)this.Value!;
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

		public class DirectInfo : List<ChunkId>
		{
			public DirectInfo()
			{
			}
		}
	}

	public enum AssetTypeTag
	{
		[CandidName("canister")]
		Canister,
		[CandidName("direct")]
		Direct,
		[CandidName("other")]
		Other
	}
}