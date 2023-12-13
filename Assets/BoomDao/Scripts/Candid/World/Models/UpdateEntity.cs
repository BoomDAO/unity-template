using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using WorldId = System.String;
using EntityId = System.String;

namespace Candid.World.Models
{
	public class UpdateEntity
	{
		[CandidName("eid")]
		public EntityId Eid { get; set; }

		[CandidName("updates")]
		public List<UpdateEntityType> Updates { get; set; }

		[CandidName("wid")]
		public UpdateEntity.WidInfo Wid { get; set; }

		public UpdateEntity(EntityId eid, List<UpdateEntityType> updates, UpdateEntity.WidInfo wid)
		{
			this.Eid = eid;
			this.Updates = updates;
			this.Wid = wid;
		}

		public UpdateEntity()
		{
		}

		public class WidInfo : OptionalValue<WorldId>
		{
			public WidInfo()
			{
			}

			public WidInfo(WorldId value) : base(value)
			{
			}
		}
	}
}