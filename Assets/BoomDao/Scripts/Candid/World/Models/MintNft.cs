using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class MintNft
	{
		[CandidName("assetId")]
		public string AssetId { get; set; }

		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("index")]
		public OptionalValue<uint> Index { get; set; }

		[CandidName("metadata")]
		public string Metadata { get; set; }

		public MintNft(string assetId, string canister, OptionalValue<uint> index, string metadata)
		{
			this.AssetId = assetId;
			this.Canister = canister;
			this.Index = index;
			this.Metadata = metadata;
		}

		public MintNft()
		{
		}
	}
}