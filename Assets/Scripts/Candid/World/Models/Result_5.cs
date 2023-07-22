using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using System;

namespace Candid.World.Models
{
	[Variant(typeof(Result_5Tag))]
	public class Result_5
	{
		[VariantTagProperty()]
		public Result_5Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_5(Result_5Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_5()
		{
		}

		public static Result_5 Err(string info)
		{
			return new Result_5(Result_5Tag.Err, info);
		}

		public static Result_5 Ok(List<Action> info)
		{
			return new Result_5(Result_5Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_5Tag.Err);
			return (string)this.Value!;
		}

		public List<Action> AsOk()
		{
			this.ValidateTag(Result_5Tag.Ok);
			return (List<Action>)this.Value!;
		}

		private void ValidateTag(Result_5Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_5Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(List<Action>))]
		Ok
	}
}