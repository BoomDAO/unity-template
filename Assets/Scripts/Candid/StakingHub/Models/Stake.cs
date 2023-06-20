using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.StakingHub.Models
{
	public class Stake
	{
		[CandidName("amount")]
		public UnboundedUInt Amount { get; set; }

		[CandidName("canister_id")]
		public string CanisterId { get; set; }

		[CandidName("dissolveAt")]
		public UnboundedInt DissolveAt { get; set; }

		[CandidName("index")]
		public OptionalValue<string> Index { get; set; }

		[CandidName("isDissolved")]
		public bool IsDissolved { get; set; }

		[CandidName("token_type")]
		public string TokenType { get; set; }

		public Stake(UnboundedUInt amount, string canisterId, UnboundedInt dissolveAt, OptionalValue<string> index, bool isDissolved, string tokenType)
		{
			this.Amount = amount;
			this.CanisterId = canisterId;
			this.DissolveAt = dissolveAt;
			this.Index = index;
			this.IsDissolved = isDissolved;
			this.TokenType = tokenType;
		}

		public Stake()
		{
		}
	}
}