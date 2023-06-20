using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.degen_race.Models
{
	public class CanisterLogMessages
	{
		[CandidName("data")]
		public List<LogMessagesData> Data { get; set; }

		[CandidName("lastAnalyzedMessageTimeNanos")]
		public OptionalValue<Nanos> LastAnalyzedMessageTimeNanos { get; set; }

		public CanisterLogMessages(List<LogMessagesData> data, OptionalValue<Nanos> lastAnalyzedMessageTimeNanos)
		{
			this.Data = data;
			this.LastAnalyzedMessageTimeNanos = lastAnalyzedMessageTimeNanos;
		}

		public CanisterLogMessages()
		{
		}
	}
}