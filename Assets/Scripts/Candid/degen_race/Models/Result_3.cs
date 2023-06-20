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

		public static Result_3 Ok(UnboundedUInt info)
		{
			return new Result_3(Result_3Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_3Tag.Err);
			return (string)this.Value!;
		}

		public UnboundedUInt AsOk()
		{
			this.ValidateTag(Result_3Tag.Ok);
			return (UnboundedUInt)this.Value!;
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
		[VariantOptionType(typeof(UnboundedUInt))]
		Ok
	}
}