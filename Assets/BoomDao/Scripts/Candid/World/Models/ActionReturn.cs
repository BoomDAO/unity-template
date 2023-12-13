using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class ActionReturn
	{
		[CandidName("callerOutcomes")]
		public OptionalValue<List<ActionOutcomeOption>> CallerOutcomes { get; set; }

		[CandidName("callerPrincipalId")]
		public string CallerPrincipalId { get; set; }

		[CandidName("targetOutcomes")]
		public OptionalValue<List<ActionOutcomeOption>> TargetOutcomes { get; set; }

		[CandidName("targetPrincipalId")]
		public OptionalValue<string> TargetPrincipalId { get; set; }

		[CandidName("worldOutcomes")]
		public OptionalValue<List<ActionOutcomeOption>> WorldOutcomes { get; set; }

		[CandidName("worldPrincipalId")]
		public string WorldPrincipalId { get; set; }

		public ActionReturn(OptionalValue<List<ActionOutcomeOption>> callerOutcomes, string callerPrincipalId, OptionalValue<List<ActionOutcomeOption>> targetOutcomes, OptionalValue<string> targetPrincipalId, OptionalValue<List<ActionOutcomeOption>> worldOutcomes, string worldPrincipalId)
		{
			this.CallerOutcomes = callerOutcomes;
			this.CallerPrincipalId = callerPrincipalId;
			this.TargetOutcomes = targetOutcomes;
			this.TargetPrincipalId = targetPrincipalId;
			this.WorldOutcomes = worldOutcomes;
			this.WorldPrincipalId = worldPrincipalId;
		}

		public ActionReturn()
		{
		}
	}
}