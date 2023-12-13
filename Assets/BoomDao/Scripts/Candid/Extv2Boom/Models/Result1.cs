using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using System.Collections.Generic;
using TokenIndex = System.UInt32;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Result1
	{
		[VariantTagProperty]
		public Result1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result1(Result1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result1()
		{
		}

		public static Result1 Err(CommonError info)
		{
			return new Result1(Result1Tag.Err, info);
		}

		public static Result1 Ok(Result1.OkInfo info)
		{
			return new Result1(Result1Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result1Tag.Err);
			return (CommonError)this.Value!;
		}

		public Result1.OkInfo AsOk()
		{
			this.ValidateTag(Result1Tag.Ok);
			return (Result1.OkInfo)this.Value!;
		}

		private void ValidateTag(Result1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class OkInfo : List<TokenIndex>
		{
			public OkInfo()
			{
			}
		}
	}

	public enum Result1Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}