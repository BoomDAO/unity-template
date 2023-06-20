using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.degen_race.Models
{
	public class LogMessagesData
	{
		[CandidName("message")]
		public string Message { get; set; }

		[CandidName("timeNanos")]
		public Nanos TimeNanos { get; set; }

		public LogMessagesData(string message, Nanos timeNanos)
		{
			this.Message = message;
			this.TimeNanos = timeNanos;
		}

		public LogMessagesData()
		{
		}
	}
}