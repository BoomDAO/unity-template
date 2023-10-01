using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;

namespace Candid.World.Models
{
	public class ActionResult
	{
		[CandidName("outcomes")]
		public List<ActionOutcome> Outcomes { get; set; }

		public ActionResult(List<ActionOutcome> outcomes)
		{
			this.Outcomes = outcomes;
		}

		public ActionResult()
		{
		}
	}
}