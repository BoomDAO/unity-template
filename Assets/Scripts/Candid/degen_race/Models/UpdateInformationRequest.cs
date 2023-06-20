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
	public class UpdateInformationRequest
	{
		[CandidName("metrics")]
		public OptionalValue<CollectMetricsRequestType> Metrics { get; set; }

		public UpdateInformationRequest(OptionalValue<CollectMetricsRequestType> metrics)
		{
			this.Metrics = metrics;
		}

		public UpdateInformationRequest()
		{
		}
	}
}