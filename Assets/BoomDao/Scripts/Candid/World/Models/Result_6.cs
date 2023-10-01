using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using System;

namespace Candid.World.Models
{
	[Variant(typeof(Result_6Tag))]
	public class Result_6
	{
		[VariantTagProperty()]
		public Result_6Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_6(Result_6Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_6()
		{
		}

		public static Result_6 Err(string info)
		{
			return new Result_6(Result_6Tag.Err, info);
		}

		public static Result_6 Ok(List<ActionState> info)
		{
			return new Result_6(Result_6Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_6Tag.Err);
			return (string)this.Value!;
		}

		public List<ActionState> AsOk()
		{
			this.ValidateTag(Result_6Tag.Ok);
			return (List<ActionState>)this.Value!;
		}

		private void ValidateTag(Result_6Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_6Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(List<ActionState>))]
		Ok
	}
}