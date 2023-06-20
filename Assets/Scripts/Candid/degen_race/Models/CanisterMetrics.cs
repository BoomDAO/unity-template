using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;

namespace Candid.degen_race.Models
{
	public class CanisterMetrics
	{
		[CandidName("data")]
		public CanisterMetricsData Data { get; set; }

		public CanisterMetrics(CanisterMetricsData data)
		{
			this.Data = data;
		}

		public CanisterMetrics()
		{
		}
	}
}