using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using WorldId = System.String;
using EntityId = System.String;

namespace Candid.World.Models
{
	public class EntityConstraint
	{
		[CandidName("eid")]
		public EntityId Eid { get; set; }

		[CandidName("entityConstraintType")]
		public EntityConstraintType EntityConstraintType { get; set; }

		[CandidName("wid")]
		public EntityConstraint.WidInfo Wid { get; set; }

		public EntityConstraint(EntityId eid, EntityConstraintType entityConstraintType, EntityConstraint.WidInfo wid)
		{
			this.Eid = eid;
			this.EntityConstraintType = entityConstraintType;
			this.Wid = wid;
		}

		public EntityConstraint()
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