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

		public static BalanceResponse Err(CommonError__2 info)
		{
			return new BalanceResponse(BalanceResponseTag.Err, info);
		}

		public static BalanceResponse Ok(Balance info)
		{
			return new BalanceResponse(BalanceResponseTag.Ok, info);
		}

		public CommonError__2 AsErr()
		{
			this.ValidateTag(BalanceResponseTag.Err);
			return (CommonError__2)this.Value!;
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
		[VariantOptionType(typeof(CommonError__2))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Balance))]
		Ok
	}
}