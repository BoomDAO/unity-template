using EdjCase.ICP.Candid.Mapping;
using GatewayPrincipal = EdjCase.ICP.Candid.Models.Principal;

namespace Candid.World.Models
{
	public class CanisterWsOpenArguments
	{
		[CandidName("client_nonce")]
		public ulong ClientNonce { get; set; }

		[CandidName("gateway_principal")]
		public GatewayPrincipal GatewayPrincipal { get; set; }

		public CanisterWsOpenArguments(ulong clientNonce, GatewayPrincipal gatewayPrincipal)
		{
			this.ClientNonce = clientNonce;
			this.GatewayPrincipal = gatewayPrincipal;
		}

		public CanisterWsOpenArguments()
		{
		}
	}
}