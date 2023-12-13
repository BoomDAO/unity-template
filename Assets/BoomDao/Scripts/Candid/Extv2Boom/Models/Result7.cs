using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Result7
	{
		[VariantTagProperty]
		public Result7Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result7(Result7Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result7()
		{
		}

		public static Result7 Err(CommonError info)
		{
			return new Result7(Result7Tag.Err, info);
		}

		public static Result7 Ok(Accountidentifier1 info)
		{
			return new Result7(Result7Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result7Tag.Err);
			return (CommonError)this.Value!;
		}

		public Accountidentifier1 AsOk()
		{
			this.ValidateTag(Result7Tag.Ok);
			return (Accountidentifier1)this.Value!;
		}

		private void ValidateTag(Result7Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result7Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}