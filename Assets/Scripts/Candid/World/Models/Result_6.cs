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

		public static Result_6 Ok(List<Action> info)
		{
			return new Result_6(Result_6Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_6Tag.Err);
			return (string)this.Value!;
		}

		public List<Action> AsOk()
		{
			this.ValidateTag(Result_6Tag.Ok);
			return (List<Action>)this.Value!;
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
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}