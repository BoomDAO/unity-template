using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.WorldHub.Models;
using WorldId = System.String;
using EntityId = System.String;

namespace Candid.WorldHub.Models
{
	public class StableEntity
	{
		[CandidName("eid")]
		public EntityId Eid { get; set; }

		[CandidName("fields")]
		public List<Field> Fields { get; set; }

		[CandidName("wid")]
		public WorldId Wid { get; set; }

		public StableEntity(EntityId eid, List<Field> fields, WorldId wid)
		{
			this.Eid = eid;
			this.Fields = fields;
			this.Wid = wid;
		}

		public StableEntity()
		{
		}
	}
}