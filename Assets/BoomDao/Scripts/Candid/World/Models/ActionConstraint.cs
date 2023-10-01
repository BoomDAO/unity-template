using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;

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

			[CandidName("fieldName")]
			public string FieldName { get; set; }

			[CandidName("gid")]
			public groupId Gid { get; set; }

			[CandidName("validation")]
			public ActionConstraint.EntityConstraintItemItem.ValidationInfo Validation { get; set; }

			[CandidName("wid")]
			public OptionalValue<worldId> Wid { get; set; }

			public EntityConstraintItemItem(entityId eid, string fieldName, groupId gid, ActionConstraint.EntityConstraintItemItem.ValidationInfo validation, OptionalValue<worldId> wid)
			{
				this.Eid = eid;
				this.FieldName = fieldName;
				this.Gid = gid;
				this.Validation = validation;
				this.Wid = wid;
			}

			public EntityConstraintItemItem()
			{
			}

			[Variant(typeof(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag))]
			public class ValidationInfo
			{
				[VariantTagProperty()]
				public ActionConstraint.EntityConstraintItemItem.ValidationInfoTag Tag { get; set; }

				[VariantValueProperty()]
				public System.Object? Value { get; set; }

				public ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag tag, object? value)
				{
					this.Tag = tag;
					this.Value = value;
				}

				protected ValidationInfo()
				{
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo EqualToNumber(double info)
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.EqualToNumber, info);
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo EqualToString(string info)
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.EqualToString, info);
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo GreaterThanEqualToNumber(double info)
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanEqualToNumber, info);
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo GreaterThanNowTimestamp()
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanNowTimestamp, null);
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo GreaterThanNumber(double info)
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanNumber, info);
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo LessThanEqualToNumber(double info)
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanEqualToNumber, info);
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo LessThanNowTimestamp()
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanNowTimestamp, null);
				}

				public static ActionConstraint.EntityConstraintItemItem.ValidationInfo LessThanNumber(double info)
				{
					return new ActionConstraint.EntityConstraintItemItem.ValidationInfo(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanNumber, info);
				}

				public double AsEqualToNumber()
				{
					this.ValidateTag(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.EqualToNumber);
					return (double)this.Value!;
				}

				public string AsEqualToString()
				{
					this.ValidateTag(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.EqualToString);
					return (string)this.Value!;
				}

				public double AsGreaterThanEqualToNumber()
				{
					this.ValidateTag(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanEqualToNumber);
					return (double)this.Value!;
				}

				public double AsGreaterThanNumber()
				{
					this.ValidateTag(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.GreaterThanNumber);
					return (double)this.Value!;
				}

				public double AsLessThanEqualToNumber()
				{
					this.ValidateTag(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanEqualToNumber);
					return (double)this.Value!;
				}

				public double AsLessThanNumber()
				{
					this.ValidateTag(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag.LessThanNumber);
					return (double)this.Value!;
				}

				private void ValidateTag(ActionConstraint.EntityConstraintItemItem.ValidationInfoTag tag)
				{
					if (!this.Tag.Equals(tag))
					{
						throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
					}
				}
			}

			public enum ValidationInfoTag
			{
				[CandidName("equalToNumber")]
				[VariantOptionType(typeof(double))]
				EqualToNumber,
				[CandidName("equalToString")]
				[VariantOptionType(typeof(string))]
				EqualToString,
				[CandidName("greaterThanEqualToNumber")]
				[VariantOptionType(typeof(double))]
				GreaterThanEqualToNumber,
				[CandidName("greaterThanNowTimestamp")]
				GreaterThanNowTimestamp,
				[CandidName("greaterThanNumber")]
				[VariantOptionType(typeof(double))]
				GreaterThanNumber,
				[CandidName("lessThanEqualToNumber")]
				[VariantOptionType(typeof(double))]
				LessThanEqualToNumber,
				[CandidName("lessThanNowTimestamp")]
				LessThanNowTimestamp,
				[CandidName("lessThanNumber")]
				[VariantOptionType(typeof(double))]
				LessThanNumber
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