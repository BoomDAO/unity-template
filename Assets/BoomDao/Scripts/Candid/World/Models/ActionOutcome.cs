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