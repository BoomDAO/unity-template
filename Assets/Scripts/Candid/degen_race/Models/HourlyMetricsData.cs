using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.degen_race.Models
{
	public class HourlyMetricsData
	{
		[CandidName("canisterCycles")]
		public CanisterCyclesAggregatedData CanisterCycles { get; set; }

		[CandidName("canisterHeapMemorySize")]
		public CanisterHeapMemoryAggregatedData CanisterHeapMemorySize { get; set; }

		[CandidName("canisterMemorySize")]
		public CanisterMemoryAggregatedData CanisterMemorySize { get; set; }

		[CandidName("timeMillis")]
		public UnboundedInt TimeMillis { get; set; }

		[CandidName("updateCalls")]
		public UpdateCallsAggregatedData UpdateCalls { get; set; }

		public HourlyMetricsData(CanisterCyclesAggregatedData canisterCycles, CanisterHeapMemoryAggregatedData canisterHeapMemorySize, CanisterMemoryAggregatedData canisterMemorySize, UnboundedInt timeMillis, UpdateCallsAggregatedData updateCalls)
		{
			this.CanisterCycles = canisterCycles;
			this.CanisterHeapMemorySize = canisterHeapMemorySize;
			this.CanisterMemorySize = canisterMemorySize;
			this.TimeMillis = timeMillis;
			this.UpdateCalls = updateCalls;
		}

		public HourlyMetricsData()
		{
		}
	}
}