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
	public class GetInformationRequest
	{
		[CandidName("logs")]
		public OptionalValue<CanisterLogRequest> Logs { get; set; }

		[CandidName("metrics")]
		public OptionalValue<MetricsRequest> Metrics { get; set; }

		[CandidName("status")]
		public OptionalValue<StatusRequest> Status { get; set; }

		[CandidName("version")]
		public bool Version { get; set; }

		public GetInformationRequest(OptionalValue<CanisterLogRequest> logs, OptionalValue<MetricsRequest> metrics, OptionalValue<StatusRequest> status, bool version)
		{
			this.Logs = logs;
			this.Metrics = metrics;
			this.Status = status;
			this.Version = version;
		}

		public GetInformationRequest()
		{
		}
	}
}