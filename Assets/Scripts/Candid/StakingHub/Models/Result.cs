using EdjCase.ICP.Candid.Mapping;
using Candid.StakingHub.Models;
using System;

namespace Candid.StakingHub.Models
{
	[Variant(typeof(ResultTag))]
	public class Result
	{
		[VariantTagProperty()]
		public ResultTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result(ResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result()
		{
		}

		public static Result Err(string info)
		{
			return new Result(ResultTag.Err, info);
		}

		public static Result Ok(string info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (string)this.Value!;
		}

		public string AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (string)this.Value!;
		}

		private void ValidateTag(ResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ResultTag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(string))]
		Ok
	}
}