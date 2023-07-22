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
using EdjCase.ICP.Candid.Models;

namespace Candid.UserNode.Models
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

		[CandidName("imageUrl")]
		public OptionalValue<string> ImageUrl { get; set; }

		[CandidName("name")]
		public OptionalValue<string> Name { get; set; }

		[CandidName("tag")]
		public OptionalValue<string> Tag { get; set; }

		public ActionConfig(OptionalValue<ActionConstraint> actionConstraint, OptionalValue<ActionPlugin> actionPlugin, ActionResult actionResult, string aid, OptionalValue<string> description, OptionalValue<string> imageUrl, OptionalValue<string> name, OptionalValue<string> tag)
		{
			this.ActionConstraint = actionConstraint;
			this.ActionPlugin = actionPlugin;
			this.ActionResult = actionResult;
			this.Aid = aid;
			this.Description = description;
			this.ImageUrl = imageUrl;
			this.Name = name;
			this.Tag = tag;
		}

		public ActionConfig()
		{
		}
	}
}