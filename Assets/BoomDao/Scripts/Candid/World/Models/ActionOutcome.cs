using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class ActionOutcome
	{
		[CandidName("possibleOutcomes")]
		public List<ActionOutcomeOption> PossibleOutcomes { get; set; }

		public ActionOutcome(List<ActionOutcomeOption> possibleOutcomes)
		{
			this.PossibleOutcomes = possibleOutcomes;
		}

		public ActionOutcome()
		{
		}
	}
}