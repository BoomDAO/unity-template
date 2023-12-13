using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class EntitySchema
	{
		[CandidName("eid")]
		public string Eid { get; set; }

		[CandidName("fields")]
		public List<Field> Fields { get; set; }

		[CandidName("uid")]
		public string Uid { get; set; }

		public EntitySchema(string eid, List<Field> fields, string uid)
		{
			this.Eid = eid;
			this.Fields = fields;
			this.Uid = uid;
		}

		public EntitySchema()
		{
		}
	}
}