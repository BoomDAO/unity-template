using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class EntityConstraintType
	{
		[VariantTagProperty]
		public EntityConstraintTypeTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public EntityConstraintType(EntityConstraintTypeTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected EntityConstraintType()
		{
		}

		public static EntityConstraintType ContainsText(ContainsText info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.ContainsText, info);
		}

		public static EntityConstraintType EqualToNumber(EqualToNumber info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.EqualToNumber, info);
		}

		public static EntityConstraintType EqualToText(EqualToText info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.EqualToText, info);
		}

		public static EntityConstraintType Exist(Exist info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.Exist, info);
		}

		public static EntityConstraintType ExistField(ExistField info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.ExistField, info);
		}

		public static EntityConstraintType GreaterThanEqualToNumber(GreaterThanOrEqualToNumber info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.GreaterThanEqualToNumber, info);
		}

		public static EntityConstraintType GreaterThanNowTimestamp(GreaterThanNowTimestamp info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.GreaterThanNowTimestamp, info);
		}

		public static EntityConstraintType GreaterThanNumber(GreaterThanNumber info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.GreaterThanNumber, info);
		}

		public static EntityConstraintType LessThanEqualToNumber(LowerThanOrEqualToNumber info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.LessThanEqualToNumber, info);
		}

		public static EntityConstraintType LessThanNowTimestamp(LessThanNowTimestamp info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.LessThanNowTimestamp, info);
		}

		public static EntityConstraintType LessThanNumber(LessThanNumber info)
		{
			return new EntityConstraintType(EntityConstraintTypeTag.LessThanNumber, info);
		}

		public ContainsText AsContainsText()
		{
			this.ValidateTag(EntityConstraintTypeTag.ContainsText);
			return (ContainsText)this.Value!;
		}

		public EqualToNumber AsEqualToNumber()
		{
			this.ValidateTag(EntityConstraintTypeTag.EqualToNumber);
			return (EqualToNumber)this.Value!;
		}

		public EqualToText AsEqualToText()
		{
			this.ValidateTag(EntityConstraintTypeTag.EqualToText);
			return (EqualToText)this.Value!;
		}

		public Exist AsExist()
		{
			this.ValidateTag(EntityConstraintTypeTag.Exist);
			return (Exist)this.Value!;
		}

		public ExistField AsExistField()
		{
			this.ValidateTag(EntityConstraintTypeTag.ExistField);
			return (ExistField)this.Value!;
		}

		public GreaterThanOrEqualToNumber AsGreaterThanEqualToNumber()
		{
			this.ValidateTag(EntityConstraintTypeTag.GreaterThanEqualToNumber);
			return (GreaterThanOrEqualToNumber)this.Value!;
		}

		public GreaterThanNowTimestamp AsGreaterThanNowTimestamp()
		{
			this.ValidateTag(EntityConstraintTypeTag.GreaterThanNowTimestamp);
			return (GreaterThanNowTimestamp)this.Value!;
		}

		public GreaterThanNumber AsGreaterThanNumber()
		{
			this.ValidateTag(EntityConstraintTypeTag.GreaterThanNumber);
			return (GreaterThanNumber)this.Value!;
		}

		public LowerThanOrEqualToNumber AsLessThanEqualToNumber()
		{
			this.ValidateTag(EntityConstraintTypeTag.LessThanEqualToNumber);
			return (LowerThanOrEqualToNumber)this.Value!;
		}

		public LessThanNowTimestamp AsLessThanNowTimestamp()
		{
			this.ValidateTag(EntityConstraintTypeTag.LessThanNowTimestamp);
			return (LessThanNowTimestamp)this.Value!;
		}

		public LessThanNumber AsLessThanNumber()
		{
			this.ValidateTag(EntityConstraintTypeTag.LessThanNumber);
			return (LessThanNumber)this.Value!;
		}

		private void ValidateTag(EntityConstraintTypeTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum EntityConstraintTypeTag
	{
		[CandidName("containsText")]
		ContainsText,
		[CandidName("equalToNumber")]
		EqualToNumber,
		[CandidName("equalToText")]
		EqualToText,
		[CandidName("exist")]
		Exist,
		[CandidName("existField")]
		ExistField,
		[CandidName("greaterThanEqualToNumber")]
		GreaterThanEqualToNumber,
		[CandidName("greaterThanNowTimestamp")]
		GreaterThanNowTimestamp,
		[CandidName("greaterThanNumber")]
		GreaterThanNumber,
		[CandidName("lessThanEqualToNumber")]
		LessThanEqualToNumber,
		[CandidName("lessThanNowTimestamp")]
		LessThanNowTimestamp,
		[CandidName("lessThanNumber")]
		LessThanNumber
	}
}