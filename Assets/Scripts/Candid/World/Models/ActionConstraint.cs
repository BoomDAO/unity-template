using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class ActionConstraint
	{
		[CandidName("entityConstraint")]
		public OptionalValue<List<ActionConstraint.EntityConstraintItemItem>> EntityConstraint { get; set; }

		[CandidName("timeConstraint")]
		public OptionalValue<ActionConstraint.TimeConstraintItem> TimeConstraint { get; set; }

		public ActionConstraint(OptionalValue<List<ActionConstraint.EntityConstraintItemItem>> entityConstraint, OptionalValue<ActionConstraint.TimeConstraintItem> timeConstraint)
		{
			this.EntityConstraint = entityConstraint;
			this.TimeConstraint = timeConstraint;
		}

		public ActionConstraint()
		{
		}

		public class EntityConstraintItemItem
		{
			[CandidName("eid")]
			public entityId Eid { get; set; }

			[CandidName("equalToAttribute")]
			public OptionalValue<string> EqualToAttribute { get; set; }

			[CandidName("gid")]
			public groupId Gid { get; set; }

			[CandidName("greaterThanOrEqualQuantity")]
			public OptionalValue<double> GreaterThanOrEqualQuantity { get; set; }

			[CandidName("lessThanQuantity")]
			public OptionalValue<double> LessThanQuantity { get; set; }

			[CandidName("notExpired")]
			public OptionalValue<bool> NotExpired { get; set; }

			[CandidName("wid")]
			public OptionalValue<worldId> Wid { get; set; }

			public EntityConstraintItemItem(entityId eid, OptionalValue<string> equalToAttribute, groupId gid, OptionalValue<double> greaterThanOrEqualQuantity, OptionalValue<double> lessThanQuantity, OptionalValue<bool> notExpired, OptionalValue<worldId> wid)
			{
				this.Eid = eid;
				this.EqualToAttribute = equalToAttribute;
				this.Gid = gid;
				this.GreaterThanOrEqualQuantity = greaterThanOrEqualQuantity;
				this.LessThanQuantity = lessThanQuantity;
				this.NotExpired = notExpired;
				this.Wid = wid;
			}

			public EntityConstraintItemItem()
			{
			}
		}

		public class TimeConstraintItem
		{
			[CandidName("actionsPerInterval")]
			public UnboundedUInt ActionsPerInterval { get; set; }

			[CandidName("intervalDuration")]
			public UnboundedUInt IntervalDuration { get; set; }

			public TimeConstraintItem(UnboundedUInt actionsPerInterval, UnboundedUInt intervalDuration)
			{
				this.ActionsPerInterval = actionsPerInterval;
				this.IntervalDuration = intervalDuration;
			}

			public TimeConstraintItem()
			{
			}
		}
	}
}