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
using Candid.UserNode.Models;
using System.Collections.Generic;

namespace Candid.UserNode.Models
{
	public class ActionResponse
	{
		[CandidTag(0U)]
		public Action F0 { get; set; }

		[CandidTag(1U)]
		public List<Entity> F1 { get; set; }

		[CandidTag(2U)]
		public List<MintNft> F2 { get; set; }

		[CandidTag(3U)]
		public List<MintToken> F3 { get; set; }

		public ActionResponse(Action f0, List<Entity> f1, List<MintNft> f2, List<MintToken> f3)
		{
			this.F0 = f0;
			this.F1 = f1;
			this.F2 = f2;
			this.F3 = f3;
		}

		public ActionResponse()
		{
		}
	}
}