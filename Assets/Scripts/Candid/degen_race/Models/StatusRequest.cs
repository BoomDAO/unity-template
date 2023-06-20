using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.degen_race.Models
{
	public class StatusRequest
	{
		[CandidName("cycles")]
		public bool Cycles { get; set; }

		[CandidName("heap_memory_size")]
		public bool HeapMemorySize { get; set; }

		[CandidName("memory_size")]
		public bool MemorySize { get; set; }

		public StatusRequest(bool cycles, bool heapMemorySize, bool memorySize)
		{
			this.Cycles = cycles;
			this.HeapMemorySize = heapMemorySize;
			this.MemorySize = memorySize;
		}

		public StatusRequest()
		{
		}
	}
}