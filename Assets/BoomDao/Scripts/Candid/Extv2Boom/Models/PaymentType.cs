using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using System.Collections.Generic;
using TokenIndex = System.UInt32;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class PaymentType
	{
		[VariantTagProperty]
		public PaymentTypeTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public PaymentType(PaymentTypeTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected PaymentType()
		{
		}

		public static PaymentType Nft(TokenIndex info)
		{
			return new PaymentType(PaymentTypeTag.Nft, info);
		}

		public static PaymentType Nfts(PaymentType.NftsInfo info)
		{
			return new PaymentType(PaymentTypeTag.Nfts, info);
		}

		public static PaymentType Sale(ulong info)
		{
			return new PaymentType(PaymentTypeTag.Sale, info);
		}

		public TokenIndex AsNft()
		{
			this.ValidateTag(PaymentTypeTag.Nft);
			return (TokenIndex)this.Value!;
		}

		public PaymentType.NftsInfo AsNfts()
		{
			this.ValidateTag(PaymentTypeTag.Nfts);
			return (PaymentType.NftsInfo)this.Value!;
		}

		public ulong AsSale()
		{
			this.ValidateTag(PaymentTypeTag.Sale);
			return (ulong)this.Value!;
		}

		private void ValidateTag(PaymentTypeTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class NftsInfo : List<TokenIndex>
		{
			public NftsInfo()
			{
			}
		}
	}

	public enum PaymentTypeTag
	{
		[CandidName("nft")]
		Nft,
		[CandidName("nfts")]
		Nfts,
		[CandidName("sale")]
		Sale
	}
}