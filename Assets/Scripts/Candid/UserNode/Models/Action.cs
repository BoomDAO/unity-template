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
	public class Action
	{
		[CandidName("actionCount")]
		public UnboundedUInt ActionCount { get; set; }

		[CandidName("actionId")]
		public string ActionId { get; set; }

		[CandidName("intervalStartTs")]
		public UnboundedUInt IntervalStartTs { get; set; }

		public Action(UnboundedUInt actionCount, string actionId, UnboundedUInt intervalStartTs)
		{
			this.ActionCount = actionCount;
			this.ActionId = actionId;
			this.IntervalStartTs = intervalStartTs;
		}

		public Action()
		{
		}
	}
}