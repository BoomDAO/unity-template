using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using System;
using System.Collections.Generic;

namespace Candid.World.Models
{
	public class StableEntity
	{
		[CandidName("eid")]
		public entityId Eid { get; set; }

		[CandidName("fields")]
		public List<ValueTuple<string, string>> Fields { get; set; }

		[CandidName("gid")]
		public groupId Gid { get; set; }

		[CandidName("wid")]
		public worldId Wid { get; set; }

		public StableEntity(entityId eid, List<ValueTuple<string, string>> fields, groupId gid, worldId wid)
		{
			this.Eid = eid;
			this.Fields = fields;
			this.Gid = gid;
			this.Wid = wid;
		}

		public StableEntity()
		{
		}
	}
}