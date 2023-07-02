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

namespace Candid.ext_v2_standard.Models
{
	[Variant(typeof(SaleRemainingTag))]
	public class SaleRemaining
	{
		[VariantTagProperty()]
		public SaleRemainingTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public SaleRemaining(SaleRemainingTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected SaleRemaining()
		{
		}

		public static SaleRemaining Burn()
		{
			return new SaleRemaining(SaleRemainingTag.Burn, null);
		}

		public static SaleRemaining Retain()
		{
			return new SaleRemaining(SaleRemainingTag.Retain, null);
		}

		public static SaleRemaining Send(AccountIdentifier__1 info)
		{
			return new SaleRemaining(SaleRemainingTag.Send, info);
		}

		public AccountIdentifier__1 AsSend()
		{
			this.ValidateTag(SaleRemainingTag.Send);
			return (AccountIdentifier__1)this.Value!;
		}

		private void ValidateTag(SaleRemainingTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum SaleRemainingTag
	{
		[CandidName("burn")]
		Burn,
		[CandidName("retain")]
		Retain,
		[CandidName("send")]
		[VariantOptionType(typeof(AccountIdentifier__1))]
		Send
	}
}