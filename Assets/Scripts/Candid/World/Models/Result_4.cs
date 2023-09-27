using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class Result_4
	{
		[VariantTagProperty()]
		public Result_4Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_4(Result_4Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_4()
		{
		}

		public static Result_4 Err(string info)
		{
			return new Result_4(Result_4Tag.Err, info);
		}

		public static Result_4 Ok(List<ActionOutcomeOption> info)
		{
			return new Result_4(Result_4Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_4Tag.Err);
			return (string)this.Value!;
		}

		public List<ActionOutcomeOption> AsOk()
		{
			this.ValidateTag(Result_4Tag.Ok);
			return (List<ActionOutcomeOption>)this.Value!;
		}

		private void ValidateTag(Result_4Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_4Tag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}