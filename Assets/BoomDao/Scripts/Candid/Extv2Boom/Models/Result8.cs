using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Result8
	{
		[VariantTagProperty]
		public Result8Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result8(Result8Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result8()
		{
		}

		public static Result8 Err(CommonError info)
		{
			return new Result8(Result8Tag.Err, info);
		}

		public static Result8 Ok(Metadata info)
		{
			return new Result8(Result8Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result8Tag.Err);
			return (CommonError)this.Value!;
		}

		public Metadata AsOk()
		{
			this.ValidateTag(Result8Tag.Ok);
			return (Metadata)this.Value!;
		}

		private void ValidateTag(Result8Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result8Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}