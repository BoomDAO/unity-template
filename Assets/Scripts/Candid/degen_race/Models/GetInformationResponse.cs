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
	public class GetInformationResponse
	{
		[CandidName("logs")]
		public OptionalValue<CanisterLogResponse> Logs { get; set; }

		[CandidName("metrics")]
		public OptionalValue<MetricsResponse> Metrics { get; set; }

		[CandidName("status")]
		public OptionalValue<StatusResponse> Status { get; set; }

		[CandidName("version")]
		public OptionalValue<UnboundedUInt> Version { get; set; }

		public GetInformationResponse(OptionalValue<CanisterLogResponse> logs, OptionalValue<MetricsResponse> metrics, OptionalValue<StatusResponse> status, OptionalValue<UnboundedUInt> version)
		{
			this.Logs = logs;
			this.Metrics = metrics;
			this.Status = status;
			this.Version = version;
		}

		public GetInformationResponse()
		{
		}
	}
}