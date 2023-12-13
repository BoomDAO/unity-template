using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class SubAction
	{
		[CandidName("actionConstraint")]
		public OptionalValue<ActionConstraint> ActionConstraint { get; set; }

		[CandidName("actionResult")]
		public ActionResult ActionResult { get; set; }

		public SubAction(OptionalValue<ActionConstraint> actionConstraint, ActionResult actionResult)
		{
			this.ActionConstraint = actionConstraint;
			this.ActionResult = actionResult;
		}

		public SubAction()
		{
		}
	}
}