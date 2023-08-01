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
	public class SpendEntityQuantity
	{
		[CandidName("eid")]
		public entityId Eid { get; set; }

		[CandidName("gid")]
		public groupId Gid { get; set; }

		[CandidName("quantity")]
		public quantity Quantity { get; set; }

		[CandidName("wid")]
		public OptionalValue<worldId> Wid { get; set; }

		public SpendEntityQuantity(entityId eid, groupId gid, quantity quantity, OptionalValue<worldId> wid)
		{
			this.Eid = eid;
			this.Gid = gid;
			this.Quantity = quantity;
			this.Wid = wid;
		}

		public SpendEntityQuantity()
		{
		}
	}
}