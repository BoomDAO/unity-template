using EdjCase.ICP.Candid.Mapping;
using WorldId = System.String;
using EntityId = System.String;

namespace Candid.World.Models
{
	public class EntityPermission
	{
		[CandidName("eid")]
		public EntityId Eid { get; set; }

		[CandidName("wid")]
		public WorldId Wid { get; set; }

		public EntityPermission(EntityId eid, WorldId wid)
		{
			this.Eid = eid;
			this.Wid = wid;
		}

		public EntityPermission()
		{
		}
	}
}