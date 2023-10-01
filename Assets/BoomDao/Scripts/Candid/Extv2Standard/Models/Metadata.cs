using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using EXTMetadataValue = System.ValueTuple<System.String, Candid.Extv2Standard.Models.EXTMetadataValue>;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Standard.Models;
using System;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Standard.Models
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

		public static Metadata Fungible(Metadata.FungibleInfo info)
		{
			return new Metadata(MetadataTag.Fungible, info);
		}

		public static Metadata Nonfungible(Metadata.NonfungibleInfo info)
		{
			return new Metadata(MetadataTag.Nonfungible, info);
		}

		public Metadata.FungibleInfo AsFungible()
		{
			this.ValidateTag(MetadataTag.Fungible);
			return (Metadata.FungibleInfo)this.Value!;
		}

		public Metadata.NonfungibleInfo AsNonfungible()
		{
			this.ValidateTag(MetadataTag.Nonfungible);
			return (Metadata.NonfungibleInfo)this.Value!;
		}

		private void ValidateTag(MetadataTag tag)
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

	public enum MetadataTag
	{
		[CandidName("fungible")]
		[VariantOptionType(typeof(Metadata.FungibleInfo))]
		Fungible,
		[CandidName("nonfungible")]
		[VariantOptionType(typeof(Metadata.NonfungibleInfo))]
		Nonfungible
	}
}