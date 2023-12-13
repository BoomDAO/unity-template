using EdjCase.ICP.Candid.Mapping;
using ClientPrincipal = EdjCase.ICP.Candid.Models.Principal;

namespace Candid.World.Models
{
	public class ClientKey
	{
		[CandidName("client_nonce")]
		public ulong ClientNonce { get; set; }

		[CandidName("client_principal")]
		public ClientPrincipal ClientPrincipal { get; set; }

		public ClientKey(ulong clientNonce, ClientPrincipal clientPrincipal)
		{
			this.ClientNonce = clientNonce;
			this.ClientPrincipal = clientPrincipal;
		}

		public ClientKey()
		{
		}
	}
}