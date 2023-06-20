using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;
using EdjCase.ICP.Candid.Models;

namespace Candid.degen_race.Models
{
	public class DailyMetricsData
	{
		[CandidName("canisterCycles")]
		public NumericEntity CanisterCycles { get; set; }

		[CandidName("canisterHeapMemorySize")]
		public NumericEntity CanisterHeapMemorySize { get; set; }

		[CandidName("canisterMemorySize")]
		public NumericEntity CanisterMemorySize { get; set; }

		[CandidName("timeMillis")]
		public UnboundedInt TimeMillis { get; set; }

		[CandidName("updateCalls")]
		public ulong UpdateCalls { get; set; }

		public DailyMetricsData(NumericEntity canisterCycles, NumericEntity canisterHeapMemorySize, NumericEntity canisterMemorySize, UnboundedInt timeMillis, ulong updateCalls)
		{
			this.CanisterCycles = canisterCycles;
			this.CanisterHeapMemorySize = canisterHeapMemorySize;
			this.CanisterMemorySize = canisterMemorySize;
			this.TimeMillis = timeMillis;
			this.UpdateCalls = updateCalls;
		}

		public DailyMetricsData()
		{
		}
	}
}