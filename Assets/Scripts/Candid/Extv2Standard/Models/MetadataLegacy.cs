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
using System;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.ext_v2_standard.Models
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

		public static MetadataLegacy Fungible(MetadataLegacy.FungibleRecord info)
		{
			return new MetadataLegacy(MetadataLegacyTag.Fungible, info);
		}

		public static MetadataLegacy Nonfungible(MetadataLegacy.NonfungibleRecord info)
		{
			return new MetadataLegacy(MetadataLegacyTag.Nonfungible, info);
		}

		public MetadataLegacy.FungibleRecord AsFungible()
		{
			this.ValidateTag(MetadataLegacyTag.Fungible);
			return (MetadataLegacy.FungibleRecord)this.Value!;
		}

		public MetadataLegacy.NonfungibleRecord AsNonfungible()
		{
			this.ValidateTag(MetadataLegacyTag.Nonfungible);
			return (MetadataLegacy.NonfungibleRecord)this.Value!;
		}

		private void ValidateTag(MetadataLegacyTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class FungibleRecord
		{
			[CandidName("decimals")]
			public byte Decimals { get; set; }

			[CandidName("metaData")]
			public OptionalValue<List<byte>> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("symbol")]
			public string Symbol { get; set; }

			public FungibleRecord(byte decimals, OptionalValue<List<byte>> metadata, string name, string symbol)
			{
				this.Decimals = decimals;
				this.Metadata = metadata;
				this.Name = name;
				this.Symbol = symbol;
			}

			public FungibleRecord()
			{
			}
		}

		public class NonfungibleRecord
		{
			[CandidName("metaData")]
			public OptionalValue<List<byte>> Metadata { get; set; }

			public NonfungibleRecord(OptionalValue<List<byte>> metadata)
			{
				this.Metadata = metadata;
			}

			public NonfungibleRecord()
			{
			}
		}
	}

	public enum MetadataLegacyTag
	{
		[CandidName("fungible")]
		[VariantOptionType(typeof(MetadataLegacy.FungibleRecord))]
		Fungible,
		[CandidName("nonfungible")]
		[VariantOptionType(typeof(MetadataLegacy.NonfungibleRecord))]
		Nonfungible
	}
}