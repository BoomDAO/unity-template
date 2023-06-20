using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.degen_race.Models;

namespace Candid.degen_race.Models
{
	public class GetMetricsParameters
	{
		[CandidName("dateFromMillis")]
		public UnboundedUInt DateFromMillis { get; set; }

		[CandidName("dateToMillis")]
		public UnboundedUInt DateToMillis { get; set; }

		[CandidName("granularity")]
		public MetricsGranularity Granularity { get; set; }

		public GetMetricsParameters(UnboundedUInt dateFromMillis, UnboundedUInt dateToMillis, MetricsGranularity granularity)
		{
			this.DateFromMillis = dateFromMillis;
			this.DateToMillis = dateToMillis;
			this.Granularity = granularity;
		}

		public GetMetricsParameters()
		{
		}
	}
}