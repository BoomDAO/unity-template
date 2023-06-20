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
	public class GetLogMessagesParameters
	{
		[CandidName("count")]
		public uint Count { get; set; }

		[CandidName("filter")]
		public OptionalValue<GetLogMessagesFilter> Filter { get; set; }

		[CandidName("fromTimeNanos")]
		public OptionalValue<Nanos> FromTimeNanos { get; set; }

		public GetLogMessagesParameters(uint count, OptionalValue<GetLogMessagesFilter> filter, OptionalValue<Nanos> fromTimeNanos)
		{
			this.Count = count;
			this.Filter = filter;
			this.FromTimeNanos = fromTimeNanos;
		}

		public GetLogMessagesParameters()
		{
		}
	}
}