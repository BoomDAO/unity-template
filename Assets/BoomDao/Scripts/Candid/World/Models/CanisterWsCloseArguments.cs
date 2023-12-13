using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class CanisterWsCloseArguments
	{
		[CandidName("client_key")]
		public ClientKey ClientKey { get; set; }

		public CanisterWsCloseArguments(ClientKey clientKey)
		{
			this.ClientKey = clientKey;
		}

		public CanisterWsCloseArguments()
		{
		}
	}
}