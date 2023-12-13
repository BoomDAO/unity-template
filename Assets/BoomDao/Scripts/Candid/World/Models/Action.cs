using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.World.Models;

namespace Candid.World.Models
{
	public class Action
	{
		[CandidName("aid")]
		public string Aid { get; set; }

		[CandidName("callerAction")]
		public OptionalValue<SubAction> CallerAction { get; set; }

		[CandidName("targetAction")]
		public OptionalValue<SubAction> TargetAction { get; set; }

		[CandidName("worldAction")]
		public OptionalValue<SubAction> WorldAction { get; set; }

		public Action(string aid, OptionalValue<SubAction> callerAction, OptionalValue<SubAction> targetAction, OptionalValue<SubAction> worldAction)
		{
			this.Aid = aid;
			this.CallerAction = callerAction;
			this.TargetAction = targetAction;
			this.WorldAction = worldAction;
		}

		public Action()
		{
		}
	}
}