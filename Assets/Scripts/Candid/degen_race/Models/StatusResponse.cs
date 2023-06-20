using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.degen_race.Models
{
	public class StatusResponse
	{
		[CandidName("cycles")]
		public OptionalValue<ulong> Cycles { get; set; }

		[CandidName("heap_memory_size")]
		public OptionalValue<ulong> HeapMemorySize { get; set; }

		[CandidName("memory_size")]
		public OptionalValue<ulong> MemorySize { get; set; }

		public StatusResponse(OptionalValue<ulong> cycles, OptionalValue<ulong> heapMemorySize, OptionalValue<ulong> memorySize)
		{
			this.Cycles = cycles;
			this.HeapMemorySize = heapMemorySize;
			this.MemorySize = memorySize;
		}

		public StatusResponse()
		{
		}
	}
}