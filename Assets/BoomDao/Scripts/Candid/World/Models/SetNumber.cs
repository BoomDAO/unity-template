using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	public class SetNumber
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("fieldValue")]
		public SetNumber.FieldValueInfo FieldValue { get; set; }

		public SetNumber(string fieldName, SetNumber.FieldValueInfo fieldValue)
		{
			this.FieldName = fieldName;
			this.FieldValue = fieldValue;
		}

		public SetNumber()
		{
		}

		[Variant]
		public class FieldValueInfo
		{
			[VariantTagProperty]
			public SetNumber.FieldValueInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public FieldValueInfo(SetNumber.FieldValueInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected FieldValueInfo()
			{
			}

			public static SetNumber.FieldValueInfo Formula(string info)
			{
				return new SetNumber.FieldValueInfo(SetNumber.FieldValueInfoTag.Formula, info);
			}

			public static SetNumber.FieldValueInfo Number(double info)
			{
				return new SetNumber.FieldValueInfo(SetNumber.FieldValueInfoTag.Number, info);
			}

			public string AsFormula()
			{
				this.ValidateTag(SetNumber.FieldValueInfoTag.Formula);
				return (string)this.Value!;
			}

			public double AsNumber()
			{
				this.ValidateTag(SetNumber.FieldValueInfoTag.Number);
				return (double)this.Value!;
			}

			private void ValidateTag(SetNumber.FieldValueInfoTag tag)
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