using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class Result6
	{
		[VariantTagProperty]
		public Result6Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result6(Result6Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result6()
		{
		}

		public static Result6 Err(string info)
		{
			return new Result6(Result6Tag.Err, info);
		}

		public static Result6 Ok(List<ActionState> info)
		{
			return new Result6(Result6Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result6Tag.Err);
			return (string)this.Value!;
		}

		public List<ActionState> AsOk()
		{
			this.ValidateTag(Result6Tag.Ok);
			return (List<ActionState>)this.Value!;
		}

		private void ValidateTag(Result6Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result6Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}