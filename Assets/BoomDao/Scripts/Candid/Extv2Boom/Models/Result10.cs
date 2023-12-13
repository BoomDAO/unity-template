using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using System;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Result10
	{
		[VariantTagProperty]
		public Result10Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result10(Result10Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result10()
		{
		}

		public static Result10 Err(CommonError info)
		{
			return new Result10(Result10Tag.Err, info);
		}

		public static Result10 Ok((Accountidentifier1, OptionalValue<Listing>) info)
		{
			return new Result10(Result10Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result10Tag.Err);
			return (CommonError)this.Value!;
		}

		public (Accountidentifier1, OptionalValue<Listing>) AsOk()
		{
			this.ValidateTag(Result10Tag.Ok);
			return ((Accountidentifier1, OptionalValue<Listing>))this.Value!;
		}

		private void ValidateTag(Result10Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result10Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}