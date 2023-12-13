using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.World.Models;
using ConfigId = System.String;

namespace Candid.World.Models
{
	public class StableConfig
	{
		[CandidName("cid")]
		public ConfigId Cid { get; set; }

		[CandidName("fields")]
		public List<Field> Fields { get; set; }

		public StableConfig(ConfigId cid, List<Field> fields)
		{
			this.Cid = cid;
			this.Fields = fields;
		}

		public StableConfig()
		{
		}
	}
}