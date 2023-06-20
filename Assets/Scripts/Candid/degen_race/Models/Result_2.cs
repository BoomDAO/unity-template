using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.degen_race.Models
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

		public static Result_2 Ok(UnboundedUInt info)
		{
			return new Result_2(Result_2Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_2Tag.Err);
			return (string)this.Value!;
		}

		public UnboundedUInt AsOk()
		{
			this.ValidateTag(Result_2Tag.Ok);
			return (UnboundedUInt)this.Value!;
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
		[VariantOptionType(typeof(UnboundedUInt))]
		Ok
	}
}