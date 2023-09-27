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
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Standard.Models
{
	[Variant]
	public class EXTMetadata
	{
		[VariantTagProperty()]
		public EXTMetadataTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public EXTMetadata(EXTMetadataTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected EXTMetadata()
		{
		}

		public static EXTMetadata Fungible(EXTMetadata.FungibleInfo info)
		{
			return new EXTMetadata(EXTMetadataTag.Fungible, info);
		}

		public static EXTMetadata Nonfungible(EXTMetadata.NonfungibleInfo info)
		{
			return new EXTMetadata(EXTMetadataTag.Nonfungible, info);
		}

		public EXTMetadata.FungibleInfo AsFungible()
		{
			this.ValidateTag(EXTMetadataTag.Fungible);
			return (EXTMetadata.FungibleInfo)this.Value!;
		}

		public EXTMetadata.NonfungibleInfo AsNonfungible()
		{
			this.ValidateTag(EXTMetadataTag.Nonfungible);
			return (EXTMetadata.NonfungibleInfo)this.Value!;
		}

		private void ValidateTag(EXTMetadataTag tag)
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
			public OptionalValue<EXTMetadataContainer> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("symbol")]
			public string Symbol { get; set; }

			public FungibleInfo(byte decimals, OptionalValue<EXTMetadataContainer> metadata, string name, string symbol)
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
			[CandidName("asset")]
			public string Asset { get; set; }

			[CandidName("metadata")]
			public OptionalValue<EXTMetadataContainer> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("thumbnail")]
			public string Thumbnail { get; set; }

			public NonfungibleInfo(string asset, OptionalValue<EXTMetadataContainer> metadata, string name, string thumbnail)
			{
				this.Asset = asset;
				this.Metadata = metadata;
				this.Name = name;
				this.Thumbnail = thumbnail;
			}

			public NonfungibleInfo()
			{
			}
		}
	}

	public enum EXTMetadataTag
	{
		[CandidName("fungible")]
		
		Fungible,
		[CandidName("nonfungible")]
		
		Nonfungible
	}
}