using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class MintToken
	{
		[CandidName("baseZeroCount")]
		public UnboundedUInt BaseZeroCount { get; set; }

		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("quantity")]
		public double Quantity { get; set; }

		public MintToken(UnboundedUInt baseZeroCount, string canister, double quantity)
		{
			this.BaseZeroCount = baseZeroCount;
			this.Canister = canister;
			this.Quantity = quantity;
		}

		public MintToken()
		{
		}
	}
}