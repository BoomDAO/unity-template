using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Result3
	{
		[VariantTagProperty]
		public Result3Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result3(Result3Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result3()
		{
		}

		public static Result3 Err(CommonError info)
		{
			return new Result3(Result3Tag.Err, info);
		}

		public static Result3 Ok()
		{
			return new Result3(Result3Tag.Ok, null);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result3Tag.Err);
			return (CommonError)this.Value!;
		}

		private void ValidateTag(Result3Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result3Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}