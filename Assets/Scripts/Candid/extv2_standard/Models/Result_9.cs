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
	[Variant(typeof(Result_9Tag))]
	public class Result_9
	{
		[VariantTagProperty()]
		public Result_9Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_9(Result_9Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_9()
		{
		}

		public static Result_9 Err(CommonError info)
		{
			return new Result_9(Result_9Tag.Err, info);
		}

		public static Result_9 Ok(ValueTuple<AccountIdentifier__1, ulong> info)
		{
			return new Result_9(Result_9Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_9Tag.Err);
			return (CommonError)this.Value!;
		}

		public ValueTuple<AccountIdentifier__1, ulong> AsOk()
		{
			this.ValidateTag(Result_9Tag.Ok);
			return (ValueTuple<AccountIdentifier__1, ulong>)this.Value!;
		}

		private void ValidateTag(Result_9Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_9Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(ValueTuple<AccountIdentifier__1, ulong>))]
		Ok
	}
}