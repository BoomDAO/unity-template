using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class IcrcTx
	{
		[CandidName("amount")]
		public double Amount { get; set; }

		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("toPrincipal")]
		public string ToPrincipal { get; set; }

		public IcrcTx(double amount, string canister, string toPrincipal)
		{
			this.Amount = amount;
			this.Canister = canister;
			this.ToPrincipal = toPrincipal;
		}

		public IcrcTx()
		{
		}
	}
}