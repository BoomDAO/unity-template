using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using TokenIndex = System.UInt32;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Result
	{
		[VariantTagProperty]
		public ResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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

		public static Result Ok(Result.OkInfo info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (CommonError)this.Value!;
		}

		public Result.OkInfo AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (Result.OkInfo)this.Value!;
		}

		private void ValidateTag(ResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class OkInfo : List<Result.OkInfo.OkInfoElement>
		{
			public OkInfo()
			{
			}

			public class OkInfoElement
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public OptionalValue<Listing> F1 { get; set; }

				[CandidTag(2U)]
				public OptionalValue<List<byte>> F2 { get; set; }

				public OkInfoElement(TokenIndex f0, OptionalValue<Listing> f1, OptionalValue<List<byte>> f2)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
				}

				public OkInfoElement()
				{
				}
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