using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.World.Models;

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