using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.StakingHub.Models
{
	public class Stake
	{
		[CandidName("amount")]
		public UnboundedUInt Amount { get; set; }

		[CandidName("blockIndex")]
		public OptionalValue<string> BlockIndex { get; set; }

		[CandidName("canister_id")]
		public string CanisterId { get; set; }

		[CandidName("dissolveAt")]
		public UnboundedInt DissolveAt { get; set; }

		[CandidName("isDissolved")]
		public bool IsDissolved { get; set; }

		[CandidName("token_type")]
		public string TokenType { get; set; }

		public Stake(UnboundedUInt amount, OptionalValue<string> blockIndex, string canisterId, UnboundedInt dissolveAt, bool isDissolved, string tokenType)
		{
			this.Amount = amount;
			this.BlockIndex = blockIndex;
			this.CanisterId = canisterId;
			this.DissolveAt = dissolveAt;
			this.IsDissolved = isDissolved;
			this.TokenType = tokenType;
		}

		public Stake()
		{
		}
	}
}