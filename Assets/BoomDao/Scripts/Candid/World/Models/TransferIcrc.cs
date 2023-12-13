using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class TransferIcrc
	{
		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("quantity")]
		public double Quantity { get; set; }

		public TransferIcrc(string canister, double quantity)
		{
			this.Canister = canister;
			this.Quantity = quantity;
		}

		public TransferIcrc()
		{
		}
	}
}