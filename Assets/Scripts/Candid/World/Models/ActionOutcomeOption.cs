using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	public class ActionOutcomeOption
	{
		[CandidName("option")]
		public ActionOutcomeOption.OptionInfo Option { get; set; }

		[CandidName("weight")]
		public double Weight { get; set; }

		public ActionOutcomeOption(ActionOutcomeOption.OptionInfo option, double weight)
		{
			this.Option = option;
			this.Weight = weight;
		}

		public ActionOutcomeOption()
		{
		}

		[Variant(typeof(ActionOutcomeOption.OptionInfoTag))]
		public class OptionInfo
		{
			[VariantTagProperty()]
			public ActionOutcomeOption.OptionInfoTag Tag { get; set; }

			[VariantValueProperty()]
			public System.Object? Value { get; set; }

			public OptionInfo(ActionOutcomeOption.OptionInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected OptionInfo()
			{
			}

			public static ActionOutcomeOption.OptionInfo DeleteEntity(DeleteEntity info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.DeleteEntity, info);
			}

			public static ActionOutcomeOption.OptionInfo MintNft(MintNft info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.MintNft, info);
			}

			public static ActionOutcomeOption.OptionInfo MintToken(MintToken info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.MintToken, info);
			}

			public static ActionOutcomeOption.OptionInfo ReceiveEntityQuantity(ReceiveEntityQuantity info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity, info);
			}

			public static ActionOutcomeOption.OptionInfo ReduceEntityExpiration(ReduceEntityExpiration info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.ReduceEntityExpiration, info);
			}

			public static ActionOutcomeOption.OptionInfo RenewEntityExpiration(RenewEntityExpiration info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.RenewEntityExpiration, info);
			}

			public static ActionOutcomeOption.OptionInfo SetEntityAttribute(SetEntityAttribute info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.SetEntityAttribute, info);
			}

			public static ActionOutcomeOption.OptionInfo SpendEntityQuantity(SpendEntityQuantity info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.SpendEntityQuantity, info);
			}

			public DeleteEntity AsDeleteEntity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.DeleteEntity);
				return (DeleteEntity)this.Value!;
			}

			public MintNft AsMintNft()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.MintNft);
				return (MintNft)this.Value!;
			}

			public MintToken AsMintToken()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.MintToken);
				return (MintToken)this.Value!;
			}

			public ReceiveEntityQuantity AsReceiveEntityQuantity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity);
				return (ReceiveEntityQuantity)this.Value!;
			}

			public ReduceEntityExpiration AsReduceEntityExpiration()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.ReduceEntityExpiration);
				return (ReduceEntityExpiration)this.Value!;
			}

			public RenewEntityExpiration AsRenewEntityExpiration()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.RenewEntityExpiration);
				return (RenewEntityExpiration)this.Value!;
			}

			public SetEntityAttribute AsSetEntityAttribute()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.SetEntityAttribute);
				return (SetEntityAttribute)this.Value!;
			}

			public SpendEntityQuantity AsSpendEntityQuantity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.SpendEntityQuantity);
				return (SpendEntityQuantity)this.Value!;
			}

			private void ValidateTag(ActionOutcomeOption.OptionInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}
		}

		public enum OptionInfoTag
		{
			[CandidName("deleteEntity")]
			[VariantOptionType(typeof(DeleteEntity))]
			DeleteEntity,
			[CandidName("mintNft")]
			[VariantOptionType(typeof(MintNft))]
			MintNft,
			[CandidName("mintToken")]
			[VariantOptionType(typeof(MintToken))]
			MintToken,
			[CandidName("receiveEntityQuantity")]
			[VariantOptionType(typeof(ReceiveEntityQuantity))]
			ReceiveEntityQuantity,
			[CandidName("reduceEntityExpiration")]
			[VariantOptionType(typeof(ReduceEntityExpiration))]
			ReduceEntityExpiration,
			[CandidName("renewEntityExpiration")]
			[VariantOptionType(typeof(RenewEntityExpiration))]
			RenewEntityExpiration,
			[CandidName("setEntityAttribute")]
			[VariantOptionType(typeof(SetEntityAttribute))]
			SetEntityAttribute,
			[CandidName("spendEntityQuantity")]
			[VariantOptionType(typeof(SpendEntityQuantity))]
			SpendEntityQuantity
		}
	}
}