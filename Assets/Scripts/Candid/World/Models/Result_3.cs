using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant(typeof(Result_3Tag))]
	public class Result_3
	{
		[VariantTagProperty()]
		public Result_3Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_3(Result_3Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_3()
		{
		}

		public static Result_3 Err(string info)
		{
			return new Result_3(Result_3Tag.Err, info);
		}

		public static Result_3 Ok(string info)
		{
			return new Result_3(Result_3Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_3Tag.Err);
			return (string)this.Value!;
		}

		public string AsOk()
		{
			this.ValidateTag(Result_3Tag.Ok);
			return (string)this.Value!;
		}

		private void ValidateTag(Result_3Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_3Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(string))]
		Ok
	}
}