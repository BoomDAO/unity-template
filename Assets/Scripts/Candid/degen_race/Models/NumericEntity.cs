using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.degen_race.Models
{
	public class NumericEntity
	{
		[CandidName("avg")]
		public ulong Avg { get; set; }

		[CandidName("first")]
		public ulong First { get; set; }

		[CandidName("last")]
		public ulong Last { get; set; }

		[CandidName("max")]
		public ulong Max { get; set; }

		[CandidName("min")]
		public ulong Min { get; set; }

		public NumericEntity(ulong avg, ulong first, ulong last, ulong max, ulong min)
		{
			this.Avg = avg;
			this.First = first;
			this.Last = last;
			this.Max = max;
			this.Min = min;
		}

		public NumericEntity()
		{
		}
	}
}