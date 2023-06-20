using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.degen_race.Models
{
	public class Entry
	{
		[CandidName("score")]
		public UnboundedInt Score { get; set; }

		[CandidName("user_principal")]
		public string UserPrincipal { get; set; }

		public Entry(UnboundedInt score, string userPrincipal)
		{
			this.Score = score;
			this.UserPrincipal = userPrincipal;
		}

		public Entry()
		{
		}
	}
}