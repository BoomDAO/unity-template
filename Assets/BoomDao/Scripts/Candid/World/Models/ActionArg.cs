using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class ActionArg
	{
		[CandidName("actionId")]
		public string ActionId { get; set; }

		[CandidName("fields")]
		public List<Field> Fields { get; set; }

		public ActionArg(string actionId, List<Field> fields)
		{
			this.ActionId = actionId;
			this.Fields = fields;
		}

		public ActionArg()
		{
		}
	}
}