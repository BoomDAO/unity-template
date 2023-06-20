using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class ActionConfig
	{
		[CandidName("actionConstraints")]
		public OptionalValue<List<ActionConstraint>> ActionConstraints { get; set; }

		[CandidName("actionDataType")]
		public ActionDataType ActionDataType { get; set; }

		[CandidName("actionResult")]
		public ActionResult ActionResult { get; set; }

		[CandidName("aid")]
		public string Aid { get; set; }

		[CandidName("description")]
		public OptionalValue<string> Description { get; set; }

		[CandidName("name")]
		public OptionalValue<string> Name { get; set; }

		public ActionConfig(OptionalValue<List<ActionConstraint>> actionConstraints, ActionDataType actionDataType, ActionResult actionResult, string aid, OptionalValue<string> description, OptionalValue<string> name)
		{
			this.ActionConstraints = actionConstraints;
			this.ActionDataType = actionDataType;
			this.ActionResult = actionResult;
			this.Aid = aid;
			this.Description = description;
			this.Name = name;
		}

		public ActionConfig()
		{
		}
	}
}