using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class MintNft
	{
		[CandidName("assetId")]
		public string AssetId { get; set; }

		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("collection")]
		public string Collection { get; set; }

		[CandidName("metadata")]
		public string Metadata { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		public MintNft(string assetId, string canister, string collection, string metadata, string name)
		{
			this.AssetId = assetId;
			this.Canister = canister;
			this.Collection = collection;
			this.Metadata = metadata;
			this.Name = name;
		}

		public MintNft()
		{
		}
	}
}