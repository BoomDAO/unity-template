using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class ActionConstraint
	{
		[CandidName("entityConstraint")]
		public List<EntityConstraint> EntityConstraint { get; set; }

		[CandidName("icpConstraint")]
		public OptionalValue<IcpTx> IcpConstraint { get; set; }

		[CandidName("icrcConstraint")]
		public List<IcrcTx> IcrcConstraint { get; set; }

		[CandidName("nftConstraint")]
		public List<NftTx> NftConstraint { get; set; }

		[CandidName("timeConstraint")]
		public OptionalValue<ActionConstraint.TimeConstraintValue> TimeConstraint { get; set; }

		public ActionConstraint(List<EntityConstraint> entityConstraint, OptionalValue<IcpTx> icpConstraint, List<IcrcTx> icrcConstraint, List<NftTx> nftConstraint, OptionalValue<ActionConstraint.TimeConstraintValue> timeConstraint)
		{
			this.EntityConstraint = entityConstraint;
			this.IcpConstraint = icpConstraint;
			this.IcrcConstraint = icrcConstraint;
			this.NftConstraint = nftConstraint;
			this.TimeConstraint = timeConstraint;
		}

		public ActionConstraint()
		{
		}

		public class TimeConstraintValue
		{
			[CandidName("actionExpirationTimestamp")]
			public OptionalValue<UnboundedUInt> ActionExpirationTimestamp { get; set; }

			[CandidName("actionsPerInterval")]
			public UnboundedUInt ActionsPerInterval { get; set; }

			[CandidName("intervalDuration")]
			public UnboundedUInt IntervalDuration { get; set; }

			public TimeConstraintValue(OptionalValue<UnboundedUInt> actionExpirationTimestamp, UnboundedUInt actionsPerInterval, UnboundedUInt intervalDuration)
			{
				this.ActionExpirationTimestamp = actionExpirationTimestamp;
				this.ActionsPerInterval = actionsPerInterval;
				this.IntervalDuration = intervalDuration;
			}

			public TimeConstraintValue()
			{
			}
		}
	}
}