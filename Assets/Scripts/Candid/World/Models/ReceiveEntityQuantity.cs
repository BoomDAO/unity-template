using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class ReceiveEntityQuantity
	{
		[CandidName("eid")]
		public entityId Eid { get; set; }

		[CandidName("gid")]
		public groupId Gid { get; set; }

		[CandidName("quantity")]
		public quantity Quantity { get; set; }

		[CandidName("wid")]
		public OptionalValue<worldId> Wid { get; set; }

		public ReceiveEntityQuantity(entityId eid, groupId gid, quantity quantity, OptionalValue<worldId> wid)
		{
			this.Eid = eid;
			this.Gid = gid;
			this.Quantity = quantity;
			this.Wid = wid;
		}

		public ReceiveEntityQuantity()
		{
		}
	}
}