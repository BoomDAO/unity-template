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
			[CandidName("entityId")]
			public string EntityId { get; set; }

			[CandidName("equalToAttribute")]
			public OptionalValue<string> EqualToAttribute { get; set; }

			[CandidName("greaterThanOrEqualQuantity")]
			public OptionalValue<double> GreaterThanOrEqualQuantity { get; set; }

			[CandidName("groupId")]
			public string GroupId { get; set; }

			[CandidName("lessThanQuantity")]
			public OptionalValue<double> LessThanQuantity { get; set; }

			[CandidName("notExpired")]
			public OptionalValue<bool> NotExpired { get; set; }

			[CandidName("worldId")]
			public string WorldId { get; set; }

			public EntityConstraintItemItem(string entityId, OptionalValue<string> equalToAttribute, OptionalValue<double> greaterThanOrEqualQuantity, string groupId, OptionalValue<double> lessThanQuantity, OptionalValue<bool> notExpired, string worldId)
			{
				this.EntityId = entityId;
				this.EqualToAttribute = equalToAttribute;
				this.GreaterThanOrEqualQuantity = greaterThanOrEqualQuantity;
				this.GroupId = groupId;
				this.LessThanQuantity = lessThanQuantity;
				this.NotExpired = notExpired;
				this.WorldId = worldId;
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