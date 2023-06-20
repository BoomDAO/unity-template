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
using System;

namespace Candid.extv2_boom.Models
{
	[Variant(typeof(BalanceResponseTag))]
	public class BalanceResponse
	{
		[VariantTagProperty()]
		public BalanceResponseTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public BalanceResponse(BalanceResponseTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected BalanceResponse()
		{
		}

		public static BalanceResponse Err(CommonError__1 info)
		{
			return new BalanceResponse(BalanceResponseTag.Err, info);
		}

		public static BalanceResponse Ok(Balance info)
		{
			return new BalanceResponse(BalanceResponseTag.Ok, info);
		}

		public CommonError__1 AsErr()
		{
			this.ValidateTag(BalanceResponseTag.Err);
			return (CommonError__1)this.Value!;
		}

		public Balance AsOk()
		{
			this.ValidateTag(BalanceResponseTag.Ok);
			return (Balance)this.Value!;
		}

		private void ValidateTag(BalanceResponseTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum BalanceResponseTag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError__1))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Balance))]
		Ok
	}
}