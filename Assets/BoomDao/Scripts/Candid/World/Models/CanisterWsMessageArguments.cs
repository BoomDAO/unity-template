using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class CanisterWsMessageArguments
	{
		[CandidName("msg")]
		public WebsocketMessage Msg { get; set; }

		public CanisterWsMessageArguments(WebsocketMessage msg)
		{
			this.Msg = msg;
		}

		public CanisterWsMessageArguments()
		{
		}
	}
}