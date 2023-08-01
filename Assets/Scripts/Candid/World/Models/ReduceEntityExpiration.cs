using worldId = System.String;
using userId = System.String;
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
	public class ReduceEntityExpiration
	{
		[CandidName("duration")]
		public duration Duration { get; set; }

		[CandidName("eid")]
		public entityId Eid { get; set; }

		[CandidName("gid")]
		public groupId Gid { get; set; }

		[CandidName("wid")]
		public OptionalValue<worldId> Wid { get; set; }

		public ReduceEntityExpiration(duration duration, entityId eid, groupId gid, OptionalValue<worldId> wid)
		{
			this.Duration = duration;
			this.Eid = eid;
			this.Gid = gid;
			this.Wid = wid;
		}

		public ReduceEntityExpiration()
		{
		}
	}
}