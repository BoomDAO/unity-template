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

namespace Candid.Extv2Boom.Models
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

		public static SaleRemaining Send(AccountIdentifier__2 info)
		{
			return new SaleRemaining(SaleRemainingTag.Send, info);
		}

		public AccountIdentifier__2 AsSend()
		{
			this.ValidateTag(SaleRemainingTag.Send);
			return (AccountIdentifier__2)this.Value!;
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
		[VariantOptionType(typeof(AccountIdentifier__2))]
		Send
	}
}