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
using System;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Boom.Models
{
	[Variant(typeof(MetadataLegacyTag))]
	public class MetadataLegacy
	{
		[VariantTagProperty()]
		public MetadataLegacyTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public MetadataLegacy(MetadataLegacyTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MetadataLegacy()
		{
		}

		public static MetadataLegacy Fungible(MetadataLegacy.FungibleInfo info)
		{
			return new MetadataLegacy(MetadataLegacyTag.Fungible, info);
		}

		public static MetadataLegacy Nonfungible(MetadataLegacy.NonfungibleInfo info)
		{
			return new MetadataLegacy(MetadataLegacyTag.Nonfungible, info);
		}

		public MetadataLegacy.FungibleInfo AsFungible()
		{
			this.ValidateTag(MetadataLegacyTag.Fungible);
			return (MetadataLegacy.FungibleInfo)this.Value!;
		}

		public MetadataLegacy.NonfungibleInfo AsNonfungible()
		{
			this.ValidateTag(MetadataLegacyTag.Nonfungible);
			return (MetadataLegacy.NonfungibleInfo)this.Value!;
		}

		private void ValidateTag(MetadataLegacyTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class FungibleInfo
		{
			[CandidName("decimals")]
			public byte Decimals { get; set; }

			[CandidName("metadata")]
			public OptionalValue<List<byte>> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("symbol")]
			public string Symbol { get; set; }

			public FungibleInfo(byte decimals, OptionalValue<List<byte>> metadata, string name, string symbol)
			{
				this.Decimals = decimals;
				this.Metadata = metadata;
				this.Name = name;
				this.Symbol = symbol;
			}

			public FungibleInfo()
			{
			}
		}

		public class NonfungibleInfo
		{
			[CandidName("metadata")]
			public OptionalValue<List<byte>> Metadata { get; set; }

			public NonfungibleInfo(OptionalValue<List<byte>> metadata)
			{
				this.Metadata = metadata;
			}

			public NonfungibleInfo()
			{
			}
		}
	}

	public enum MetadataLegacyTag
	{
		[CandidName("fungible")]
		[VariantOptionType(typeof(MetadataLegacy.FungibleInfo))]
		Fungible,
		[CandidName("nonfungible")]
		[VariantOptionType(typeof(MetadataLegacy.NonfungibleInfo))]
		Nonfungible
	}
}