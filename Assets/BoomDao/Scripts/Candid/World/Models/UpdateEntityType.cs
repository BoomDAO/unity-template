using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class UpdateEntityType
	{
		[VariantTagProperty]
		public UpdateEntityTypeTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public UpdateEntityType(UpdateEntityTypeTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected UpdateEntityType()
		{
		}

		public static UpdateEntityType AddToList(AddToList info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.AddToList, info);
		}

		public static UpdateEntityType DecrementNumber(DecrementNumber info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.DecrementNumber, info);
		}

		public static UpdateEntityType DeleteEntity(DeleteEntity info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.DeleteEntity, info);
		}

		public static UpdateEntityType DeleteField(DeleteField info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.DeleteField, info);
		}

		public static UpdateEntityType IncrementNumber(IncrementNumber info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.IncrementNumber, info);
		}

		public static UpdateEntityType RemoveFromList(RemoveFromList info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.RemoveFromList, info);
		}

		public static UpdateEntityType RenewTimestamp(RenewTimestamp info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.RenewTimestamp, info);
		}

		public static UpdateEntityType SetNumber(SetNumber info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.SetNumber, info);
		}

		public static UpdateEntityType SetText(SetText info)
		{
			return new UpdateEntityType(UpdateEntityTypeTag.SetText, info);
		}

		public AddToList AsAddToList()
		{
			this.ValidateTag(UpdateEntityTypeTag.AddToList);
			return (AddToList)this.Value!;
		}

		public DecrementNumber AsDecrementNumber()
		{
			this.ValidateTag(UpdateEntityTypeTag.DecrementNumber);
			return (DecrementNumber)this.Value!;
		}

		public DeleteEntity AsDeleteEntity()
		{
			this.ValidateTag(UpdateEntityTypeTag.DeleteEntity);
			return (DeleteEntity)this.Value!;
		}

		public DeleteField AsDeleteField()
		{
			this.ValidateTag(UpdateEntityTypeTag.DeleteField);
			return (DeleteField)this.Value!;
		}

		public IncrementNumber AsIncrementNumber()
		{
			this.ValidateTag(UpdateEntityTypeTag.IncrementNumber);
			return (IncrementNumber)this.Value!;
		}

		public RemoveFromList AsRemoveFromList()
		{
			this.ValidateTag(UpdateEntityTypeTag.RemoveFromList);
			return (RemoveFromList)this.Value!;
		}

		public RenewTimestamp AsRenewTimestamp()
		{
			this.ValidateTag(UpdateEntityTypeTag.RenewTimestamp);
			return (RenewTimestamp)this.Value!;
		}

		public SetNumber AsSetNumber()
		{
			this.ValidateTag(UpdateEntityTypeTag.SetNumber);
			return (SetNumber)this.Value!;
		}

		public SetText AsSetText()
		{
			this.ValidateTag(UpdateEntityTypeTag.SetText);
			return (SetText)this.Value!;
		}

		private void ValidateTag(UpdateEntityTypeTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum UpdateEntityTypeTag
	{
		[CandidName("addToList")]
		AddToList,
		[CandidName("decrementNumber")]
		DecrementNumber,
		[CandidName("deleteEntity")]
		DeleteEntity,
		[CandidName("deleteField")]
		DeleteField,
		[CandidName("incrementNumber")]
		IncrementNumber,
		[CandidName("removeFromList")]
		RemoveFromList,
		[CandidName("renewTimestamp")]
		RenewTimestamp,
		[CandidName("setNumber")]
		SetNumber,
		[CandidName("setText")]
		SetText
	}
}