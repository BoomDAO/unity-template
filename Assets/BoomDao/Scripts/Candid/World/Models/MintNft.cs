using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class MintNft
	{
		[CandidName("assetId")]
		public string AssetId { get; set; }

		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("metadata")]
		public string Metadata { get; set; }

		public MintNft(string assetId, string canister, string metadata)
		{
			this.AssetId = assetId;
			this.Canister = canister;
			this.Metadata = metadata;
		}

		public MintNft()
		{
		}
	}
}