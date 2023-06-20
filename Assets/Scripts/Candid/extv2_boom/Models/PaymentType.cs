using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.extv2_boom.Models.MetadataValue>;
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
using Candid.extv2_boom.Models;
using System.Collections.Generic;
using System;

namespace Candid.extv2_boom.Models
{
	[Variant(typeof(PaymentTypeTag))]
	public class PaymentType
	{
		[VariantTagProperty()]
		public PaymentTypeTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

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

		public static PaymentType Nfts(List<TokenIndex> info)
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

		public List<TokenIndex> AsNfts()
		{
			this.ValidateTag(PaymentTypeTag.Nfts);
			return (List<TokenIndex>)this.Value!;
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
	}

	public enum PaymentTypeTag
	{
		[CandidName("nft")]
		[VariantOptionType(typeof(TokenIndex))]
		Nft,
		[CandidName("nfts")]
		[VariantOptionType(typeof(List<TokenIndex>))]
		Nfts,
		[CandidName("sale")]
		[VariantOptionType(typeof(ulong))]
		Sale
	}
}