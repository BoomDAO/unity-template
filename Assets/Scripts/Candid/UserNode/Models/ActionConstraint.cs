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
using EdjCase.ICP.Candid.Models;

namespace Candid.UserNode.Models
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