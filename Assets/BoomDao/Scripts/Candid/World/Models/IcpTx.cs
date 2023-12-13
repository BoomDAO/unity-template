using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class IcpTx
	{
		[CandidName("amount")]
		public double Amount { get; set; }

		[CandidName("toPrincipal")]
		public string ToPrincipal { get; set; }

		public IcpTx(double amount, string toPrincipal)
		{
			this.Amount = amount;
			this.ToPrincipal = toPrincipal;
		}

		public IcpTx()
		{
		}
	}
}