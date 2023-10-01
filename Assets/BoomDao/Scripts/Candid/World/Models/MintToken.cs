using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
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