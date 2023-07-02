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
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using System;

namespace Candid.ext_v2_standard.Models
{
	[Variant(typeof(ResultTag))]
	public class Result
	{
		[VariantTagProperty()]
		public ResultTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result(ResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result()
		{
		}

		public static Result Err(CommonError info)
		{
			return new Result(ResultTag.Err, info);
		}

		public static Result Ok(List<ValueTuple<TokenIndex, OptionalValue<Listing>, OptionalValue<List<byte>>>> info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (CommonError)this.Value!;
		}

		public List<ValueTuple<TokenIndex, OptionalValue<Listing>, OptionalValue<List<byte>>>> AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (List<ValueTuple<TokenIndex, OptionalValue<Listing>, OptionalValue<List<byte>>>>)this.Value!;
		}

		private void ValidateTag(ResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ResultTag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(List<ValueTuple<TokenIndex, OptionalValue<Listing>, OptionalValue<List<byte>>>>))]
		Ok
	}
}