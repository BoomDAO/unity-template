using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class NftTransfer
	{
		[CandidName("toPrincipal")]
		public string ToPrincipal { get; set; }

		public NftTransfer(string toPrincipal)
		{
			this.ToPrincipal = toPrincipal;
		}

		public NftTransfer()
		{
		}
	}
}