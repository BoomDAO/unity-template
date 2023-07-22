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