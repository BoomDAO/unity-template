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
using EdjCase.ICP.Candid.Models;

namespace Candid.ext_v2_standard.Models
{
	[Variant(typeof(MetadataTag))]
	public class Metadata
	{
		[VariantTagProperty()]
		public MetadataTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Metadata(MetadataTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Metadata()
		{
		}

		public static Metadata Fungible(Metadata.FungibleRecord info)
		{
			return new Metadata(MetadataTag.Fungible, info);
		}

		public static Metadata Nonfungible(Metadata.NonfungibleRecord info)
		{
			return new Metadata(MetadataTag.Nonfungible, info);
		}

		public Metadata.FungibleRecord AsFungible()
		{
			this.ValidateTag(MetadataTag.Fungible);
			return (Metadata.FungibleRecord)this.Value!;
		}

		public Metadata.NonfungibleRecord AsNonfungible()
		{
			this.ValidateTag(MetadataTag.Nonfungible);
			return (Metadata.NonfungibleRecord)this.Value!;
		}

		private void ValidateTag(MetadataTag tag)
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
			public OptionalValue<MetadataContainer> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("symbol")]
			public string Symbol { get; set; }

			public FungibleRecord(byte decimals, OptionalValue<MetadataContainer> metadata, string name, string symbol)
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
			[CandidName("asset")]
			public string Asset { get; set; }

			[CandidName("metaData")]
			public OptionalValue<MetadataContainer> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("thumbnail")]
			public string Thumbnail { get; set; }

			public NonfungibleRecord(string asset, OptionalValue<MetadataContainer> metadata, string name, string thumbnail)
			{
				this.Asset = asset;
				this.Metadata = metadata;
				this.Name = name;
				this.Thumbnail = thumbnail;
			}

			public NonfungibleRecord()
			{
			}
		}
	}

	public enum MetadataTag
	{
		[CandidName("fungible")]
		[VariantOptionType(typeof(Metadata.FungibleRecord))]
		Fungible,
		[CandidName("nonfungible")]
		[VariantOptionType(typeof(Metadata.NonfungibleRecord))]
		Nonfungible
	}
}