using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant(typeof(Result_1Tag))]
	public class Result_1
	{
		[VariantTagProperty()]
		public Result_1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_1(Result_1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_1()
		{
		}

		public static Result_1 Err(string info)
		{
			return new Result_1(Result_1Tag.Err, info);
		}

		public static Result_1 Ok(string info)
		{
			return new Result_1(Result_1Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_1Tag.Err);
			return (string)this.Value!;
		}

		public string AsOk()
		{
			this.ValidateTag(Result_1Tag.Ok);
			return (string)this.Value!;
		}

		private void ValidateTag(Result_1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_1Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(string))]
		Ok
	}
}