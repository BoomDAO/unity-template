using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;

namespace Candid.degen_race.Models
{
	public class MetricsRequest
	{
		[CandidName("parameters")]
		public GetMetricsParameters Parameters { get; set; }

		public MetricsRequest(GetMetricsParameters parameters)
		{
			this.Parameters = parameters;
		}

		public MetricsRequest()
		{
		}
	}
}