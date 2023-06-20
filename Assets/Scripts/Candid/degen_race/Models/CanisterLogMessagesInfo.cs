using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace Candid.degen_race.Models
{
	public class CanisterLogMessagesInfo
	{
		[CandidName("count")]
		public uint Count { get; set; }

		[CandidName("features")]
		public List<OptionalValue<CanisterLogFeature>> Features { get; set; }

		[CandidName("firstTimeNanos")]
		public OptionalValue<Nanos> FirstTimeNanos { get; set; }

		[CandidName("lastTimeNanos")]
		public OptionalValue<Nanos> LastTimeNanos { get; set; }

		public CanisterLogMessagesInfo(uint count, List<OptionalValue<CanisterLogFeature>> features, OptionalValue<Nanos> firstTimeNanos, OptionalValue<Nanos> lastTimeNanos)
		{
			this.Count = count;
			this.Features = features;
			this.FirstTimeNanos = firstTimeNanos;
			this.LastTimeNanos = lastTimeNanos;
		}

		public CanisterLogMessagesInfo()
		{
		}
	}
}