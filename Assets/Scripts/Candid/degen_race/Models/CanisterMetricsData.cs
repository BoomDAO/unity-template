using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;
using System.Collections.Generic;
using System;

namespace Candid.degen_race.Models
{
	[Variant(typeof(CanisterMetricsDataTag))]
	public class CanisterMetricsData
	{
		[VariantTagProperty()]
		public CanisterMetricsDataTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public CanisterMetricsData(CanisterMetricsDataTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterMetricsData()
		{
		}

		public static CanisterMetricsData Daily(List<DailyMetricsData> info)
		{
			return new CanisterMetricsData(CanisterMetricsDataTag.Daily, info);
		}

		public static CanisterMetricsData Hourly(List<HourlyMetricsData> info)
		{
			return new CanisterMetricsData(CanisterMetricsDataTag.Hourly, info);
		}

		public List<DailyMetricsData> AsDaily()
		{
			this.ValidateTag(CanisterMetricsDataTag.Daily);
			return (List<DailyMetricsData>)this.Value!;
		}

		public List<HourlyMetricsData> AsHourly()
		{
			this.ValidateTag(CanisterMetricsDataTag.Hourly);
			return (List<HourlyMetricsData>)this.Value!;
		}

		private void ValidateTag(CanisterMetricsDataTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterMetricsDataTag
	{
		[CandidName("daily")]
		[VariantOptionType(typeof(List<DailyMetricsData>))]
		Daily,
		[CandidName("hourly")]
		[VariantOptionType(typeof(List<HourlyMetricsData>))]
		Hourly
	}
}