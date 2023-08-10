using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant(typeof(Result_2Tag))]
	public class Result_2
	{
		[VariantTagProperty()]
		public Result_2Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_2(Result_2Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_2()
		{
		}

		public static Result_2 Err(string info)
		{
			return new Result_2(Result_2Tag.Err, info);
		}

		public static Result_2 Ok(string info)
		{
			return new Result_2(Result_2Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_2Tag.Err);
			return (string)this.Value!;
		}

		public string AsOk()
		{
			this.ValidateTag(Result_2Tag.Ok);
			return (string)this.Value!;
		}

		private void ValidateTag(Result_2Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_2Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(string))]
		Ok
	}
}