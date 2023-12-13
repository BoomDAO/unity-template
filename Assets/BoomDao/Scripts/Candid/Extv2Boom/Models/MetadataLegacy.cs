using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class MetadataLegacy
	{
		[VariantTagProperty]
		public MetadataLegacyTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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
		Fungible,
		[CandidName("nonfungible")]
		Nonfungible
	}
}