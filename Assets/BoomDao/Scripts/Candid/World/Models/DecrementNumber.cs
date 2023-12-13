using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	public class DecrementNumber
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("fieldValue")]
		public DecrementNumber.FieldValueInfo FieldValue { get; set; }

		public DecrementNumber(string fieldName, DecrementNumber.FieldValueInfo fieldValue)
		{
			this.FieldName = fieldName;
			this.FieldValue = fieldValue;
		}

		public DecrementNumber()
		{
		}

		[Variant]
		public class FieldValueInfo
		{
			[VariantTagProperty]
			public DecrementNumber.FieldValueInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public FieldValueInfo(DecrementNumber.FieldValueInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected FieldValueInfo()
			{
			}

			public static DecrementNumber.FieldValueInfo Formula(string info)
			{
				return new DecrementNumber.FieldValueInfo(DecrementNumber.FieldValueInfoTag.Formula, info);
			}

			public static DecrementNumber.FieldValueInfo Number(double info)
			{
				return new DecrementNumber.FieldValueInfo(DecrementNumber.FieldValueInfoTag.Number, info);
			}

			public string AsFormula()
			{
				this.ValidateTag(DecrementNumber.FieldValueInfoTag.Formula);
				return (string)this.Value!;
			}

			public double AsNumber()
			{
				this.ValidateTag(DecrementNumber.FieldValueInfoTag.Number);
				return (double)this.Value!;
			}

			private void ValidateTag(DecrementNumber.FieldValueInfoTag tag)
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