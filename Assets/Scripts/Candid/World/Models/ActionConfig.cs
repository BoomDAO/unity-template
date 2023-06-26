using worldId = EdjCase.ICP.Candid.Models.OptionalValue<System.String>;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class ActionConfig
	{
		[CandidName("actionConstraint")]
		public OptionalValue<ActionConstraint> ActionConstraint { get; set; }

		[CandidName("actionPlugin")]
		public OptionalValue<ActionPlugin> ActionPlugin { get; set; }

		[CandidName("actionResult")]
		public ActionResult ActionResult { get; set; }

		[CandidName("aid")]
		public string Aid { get; set; }

		[CandidName("description")]
		public OptionalValue<string> Description { get; set; }

		[CandidName("name")]
		public OptionalValue<string> Name { get; set; }

		[CandidName("tag")]
		public OptionalValue<string> Tag { get; set; }

		public ActionConfig(OptionalValue<ActionConstraint> actionConstraint, OptionalValue<ActionPlugin> actionPlugin, ActionResult actionResult, string aid, OptionalValue<string> description, OptionalValue<string> name, OptionalValue<string> tag)
		{
			this.ActionConstraint = actionConstraint;
			this.ActionPlugin = actionPlugin;
			this.ActionResult = actionResult;
			this.Aid = aid;
			this.Description = description;
			this.Name = name;
			this.Tag = tag;
		}

		public ActionConfig()
		{
		}
	}
}