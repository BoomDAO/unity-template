using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
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

		public static Result3 Err(string info)
		{
			return new Result3(Result3Tag.Err, info);
		}

		public static Result3 Ok(ActionReturn info)
		{
			return new Result3(Result3Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result3Tag.Err);
			return (string)this.Value!;
		}

		public ActionReturn AsOk()
		{
			this.ValidateTag(Result3Tag.Ok);
			return (ActionReturn)this.Value!;
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