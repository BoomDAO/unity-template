using worldId = System.String;
using userId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using actionId = System.String;
using List_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.List_1Item>;
using List = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.ListItem>;
using Hash = System.UInt32;
using AssocList_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocList_1Item>;
using AssocList = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocListItem>;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.UserNode.Models
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