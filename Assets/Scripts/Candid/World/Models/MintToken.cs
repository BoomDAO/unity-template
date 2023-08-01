using worldId = System.String;
using userId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class MintToken
	{
		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("quantity")]
		public double Quantity { get; set; }

		public MintToken(string canister, double quantity)
		{
			this.Canister = canister;
			this.Quantity = quantity;
		}

		public MintToken()
		{
		}
	}
}