using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	public class IncrementNumber
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("fieldValue")]
		public IncrementNumber.FieldValueInfo FieldValue { get; set; }

		public IncrementNumber(string fieldName, IncrementNumber.FieldValueInfo fieldValue)
		{
			this.FieldName = fieldName;
			this.FieldValue = fieldValue;
		}

		public IncrementNumber()
		{
		}

		[Variant]
		public class FieldValueInfo
		{
			[VariantTagProperty]
			public IncrementNumber.FieldValueInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public FieldValueInfo(IncrementNumber.FieldValueInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected FieldValueInfo()
			{
			}

			public static IncrementNumber.FieldValueInfo Formula(string info)
			{
				return new IncrementNumber.FieldValueInfo(IncrementNumber.FieldValueInfoTag.Formula, info);
			}

			public static IncrementNumber.FieldValueInfo Number(double info)
			{
				return new IncrementNumber.FieldValueInfo(IncrementNumber.FieldValueInfoTag.Number, info);
			}

			public string AsFormula()
			{
				this.ValidateTag(IncrementNumber.FieldValueInfoTag.Formula);
				return (string)this.Value!;
			}

			public double AsNumber()
			{
				this.ValidateTag(IncrementNumber.FieldValueInfoTag.Number);
				return (double)this.Value!;
			}

			private void ValidateTag(IncrementNumber.FieldValueInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}
		}

		public enum FieldValueInfoTag
		{
			[CandidName("formula")]
			Formula,
			[CandidName("number")]
			Number
		}
	}
}