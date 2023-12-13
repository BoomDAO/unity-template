using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class Result5
	{
		[VariantTagProperty]
		public Result5Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result5(Result5Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result5()
		{
		}

		public static Result5 Err(string info)
		{
			return new Result5(Result5Tag.Err, info);
		}

		public static Result5 Ok(List<StableEntity> info)
		{
			return new Result5(Result5Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result5Tag.Err);
			return (string)this.Value!;
		}

		public List<StableEntity> AsOk()
		{
			this.ValidateTag(Result5Tag.Ok);
			return (List<StableEntity>)this.Value!;
		}

		private void ValidateTag(Result5Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result5Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}