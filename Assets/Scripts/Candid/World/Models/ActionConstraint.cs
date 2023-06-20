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
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	[Variant(typeof(ActionConstraintTag))]
	public class ActionConstraint
	{
		[VariantTagProperty()]
		public ActionConstraintTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public ActionConstraint(ActionConstraintTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ActionConstraint()
		{
		}

		public static ActionConstraint EntityConstraint(ActionConstraint.EntityConstraintInfo info)
		{
			return new ActionConstraint(ActionConstraintTag.EntityConstraint, info);
		}

		public static ActionConstraint TimeConstraint(ActionConstraint.TimeConstraintInfo info)
		{
			return new ActionConstraint(ActionConstraintTag.TimeConstraint, info);
		}

		public ActionConstraint.EntityConstraintInfo AsEntityConstraint()
		{
			this.ValidateTag(ActionConstraintTag.EntityConstraint);
			return (ActionConstraint.EntityConstraintInfo)this.Value!;
		}

		public ActionConstraint.TimeConstraintInfo AsTimeConstraint()
		{
			this.ValidateTag(ActionConstraintTag.TimeConstraint);
			return (ActionConstraint.TimeConstraintInfo)this.Value!;
		}

		private void ValidateTag(ActionConstraintTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class EntityConstraintInfo
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

			public EntityConstraintInfo(string entityId, OptionalValue<string> equalToAttribute, OptionalValue<double> greaterThanOrEqualQuantity, string groupId, OptionalValue<double> lessThanQuantity, OptionalValue<bool> notExpired, string worldId)
			{
				this.EntityId = entityId;
				this.EqualToAttribute = equalToAttribute;
				this.GreaterThanOrEqualQuantity = greaterThanOrEqualQuantity;
				this.GroupId = groupId;
				this.LessThanQuantity = lessThanQuantity;
				this.NotExpired = notExpired;
				this.WorldId = worldId;
			}

			public EntityConstraintInfo()
			{
			}
		}

		public class TimeConstraintInfo
		{
			[CandidName("actionsPerInterval")]
			public UnboundedUInt ActionsPerInterval { get; set; }

			[CandidName("intervalDuration")]
			public UnboundedUInt IntervalDuration { get; set; }

			public TimeConstraintInfo(UnboundedUInt actionsPerInterval, UnboundedUInt intervalDuration)
			{
				this.ActionsPerInterval = actionsPerInterval;
				this.IntervalDuration = intervalDuration;
			}

			public TimeConstraintInfo()
			{
			}
		}
	}

	public enum ActionConstraintTag
	{
		[CandidName("entityConstraint")]
		[VariantOptionType(typeof(ActionConstraint.EntityConstraintInfo))]
		EntityConstraint,
		[CandidName("timeConstraint")]
		[VariantOptionType(typeof(ActionConstraint.TimeConstraintInfo))]
		TimeConstraint
	}
}