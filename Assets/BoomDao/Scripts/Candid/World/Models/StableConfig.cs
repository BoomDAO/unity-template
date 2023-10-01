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
	public class StableConfig
	{
		[CandidName("cid")]
		public configId Cid { get; set; }

		[CandidName("fields")]
		public List<ValueTuple<string, string>> Fields { get; set; }

		public StableConfig(configId cid, List<ValueTuple<string, string>> fields)
		{
			this.Cid = cid;
			this.Fields = fields;
		}

		public StableConfig()
		{
		}
	}
}