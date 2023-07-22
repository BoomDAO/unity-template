using worldId = System.String;
using userId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using actionId = System.String;
using List_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.List_1Item>;
using List = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.ListItem>;
using Hash = System.UInt32;
using AssocList_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocList_1Item>;
using AssocList = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocListItem>;
using EdjCase.ICP.Candid.Mapping;
using Candid.UserNode.Models;
using System.Collections.Generic;

namespace Candid.UserNode.Models
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