using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.degen_race.Models
{
	public class RewardData
	{
		[CandidName("itemId")]
		public string ItemId { get; set; }

		[CandidName("quantity")]
		public double Quantity { get; set; }

		public RewardData(string itemId, double quantity)
		{
			this.ItemId = itemId;
			this.Quantity = quantity;
		}

		public RewardData()
		{
		}
	}
}