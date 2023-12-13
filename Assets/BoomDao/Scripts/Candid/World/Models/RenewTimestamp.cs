using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	public class RenewTimestamp
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("fieldValue")]
		public RenewTimestamp.FieldValueInfo FieldValue { get; set; }

		public RenewTimestamp(string fieldName, RenewTimestamp.FieldValueInfo fieldValue)
		{
			this.FieldName = fieldName;
			this.FieldValue = fieldValue;
		}

		public RenewTimestamp()
		{
		}

		[Variant]
		public class FieldValueInfo
		{
			[VariantTagProperty]
			public RenewTimestamp.FieldValueInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public FieldValueInfo(RenewTimestamp.FieldValueInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected FieldValueInfo()
			{
			}

			public static RenewTimestamp.FieldValueInfo Formula(string info)
			{
				return new RenewTimestamp.FieldValueInfo(RenewTimestamp.FieldValueInfoTag.Formula, info);
			}

			public static RenewTimestamp.FieldValueInfo Number(double info)
			{
				return new RenewTimestamp.FieldValueInfo(RenewTimestamp.FieldValueInfoTag.Number, info);
			}

			public string AsFormula()
			{
				this.ValidateTag(RenewTimestamp.FieldValueInfoTag.Formula);
				return (string)this.Value!;
			}

			public double AsNumber()
			{
				this.ValidateTag(RenewTimestamp.FieldValueInfoTag.Number);
				return (double)this.Value!;
			}

			private void ValidateTag(RenewTimestamp.FieldValueInfoTag tag)
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