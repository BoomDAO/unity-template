using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Metadata
	{
		[VariantTagProperty]
		public MetadataTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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
			public OptionalValue<MetadataContainer> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("symbol")]
			public string Symbol { get; set; }

			public FungibleInfo(byte decimals, OptionalValue<MetadataContainer> metadata, string name, string symbol)
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
			public OptionalValue<MetadataContainer> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("thumbnail")]
			public string Thumbnail { get; set; }

			public NonfungibleInfo(string asset, OptionalValue<MetadataContainer> metadata, string name, string thumbnail)
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

	public enum MetadataTag
	{
		[CandidName("fungible")]
		Fungible,
		[CandidName("nonfungible")]
		Nonfungible
	}
}