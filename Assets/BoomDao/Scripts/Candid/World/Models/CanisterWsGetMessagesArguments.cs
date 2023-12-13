using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class CanisterWsGetMessagesArguments
	{
		[CandidName("nonce")]
		public ulong Nonce { get; set; }

		public CanisterWsGetMessagesArguments(ulong nonce)
		{
			this.Nonce = nonce;
		}

		public CanisterWsGetMessagesArguments()
		{
		}
	}
}