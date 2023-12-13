using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Result9
	{
		[VariantTagProperty]
		public Result9Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result9(Result9Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result9()
		{
		}

		public static Result9 Err(CommonError info)
		{
			return new Result9(Result9Tag.Err, info);
		}

		public static Result9 Ok((Accountidentifier1, ulong) info)
		{
			return new Result9(Result9Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result9Tag.Err);
			return (CommonError)this.Value!;
		}

		public (Accountidentifier1, ulong) AsOk()
		{
			this.ValidateTag(Result9Tag.Ok);
			return ((Accountidentifier1, ulong))this.Value!;
		}

		private void ValidateTag(Result9Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result9Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}