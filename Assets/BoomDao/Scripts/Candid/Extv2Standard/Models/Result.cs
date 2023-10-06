using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using EXTMetadataValue = System.ValueTuple<System.String, Candid.Extv2Standard.Models.EXTMetadataValue>;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Standard.Models;
using System.Collections.Generic;
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Standard.Models
{
	[Variant]
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

		public static Result Ok(List<Result.OkItem> info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (CommonError)this.Value!;
		}

		public List<Result.OkItem> AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (List<Result.OkItem>)this.Value!;
		}

		private void ValidateTag(ResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class OkItem
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public OptionalValue<Listing> F1 { get; set; }

			[CandidTag(2U)]
			public OptionalValue<List<byte>> F2 { get; set; }

			public OkItem(TokenIndex f0, OptionalValue<Listing> f1, OptionalValue<List<byte>> f2)
			{
				this.F0 = f0;
				this.F1 = f1;
				this.F2 = f2;
			}

			public OkItem()
			{
			}
		}
	}

	public enum ResultTag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}