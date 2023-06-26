using worldId = EdjCase.ICP.Candid.Models.OptionalValue<System.String>;
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

			public static ActionOutcomeOption.OptionInfo DeleteEntity(ActionOutcomeOption.OptionInfo.DeleteEntityInfo info)
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

			public static ActionOutcomeOption.OptionInfo ReceiveEntityQuantity(ActionOutcomeOption.OptionInfo.ReceiveEntityQuantityInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity, info);
			}

			public static ActionOutcomeOption.OptionInfo ReduceEntityExpiration(ActionOutcomeOption.OptionInfo.ReduceEntityExpirationInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.ReduceEntityExpiration, info);
			}

			public static ActionOutcomeOption.OptionInfo RenewEntityExpiration(ActionOutcomeOption.OptionInfo.RenewEntityExpirationInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.RenewEntityExpiration, info);
			}

			public static ActionOutcomeOption.OptionInfo SetEntityAttribute(ActionOutcomeOption.OptionInfo.SetEntityAttributeInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.SetEntityAttribute, info);
			}

			public static ActionOutcomeOption.OptionInfo SpendEntityQuantity(ActionOutcomeOption.OptionInfo.SpendEntityQuantityInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.SpendEntityQuantity, info);
			}

			public ActionOutcomeOption.OptionInfo.DeleteEntityInfo AsDeleteEntity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.DeleteEntity);
				return (ActionOutcomeOption.OptionInfo.DeleteEntityInfo)this.Value!;
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

			public ActionOutcomeOption.OptionInfo.ReceiveEntityQuantityInfo AsReceiveEntityQuantity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.ReceiveEntityQuantity);
				return (ActionOutcomeOption.OptionInfo.ReceiveEntityQuantityInfo)this.Value!;
			}

			public ActionOutcomeOption.OptionInfo.ReduceEntityExpirationInfo AsReduceEntityExpiration()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.ReduceEntityExpiration);
				return (ActionOutcomeOption.OptionInfo.ReduceEntityExpirationInfo)this.Value!;
			}

			public ActionOutcomeOption.OptionInfo.RenewEntityExpirationInfo AsRenewEntityExpiration()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.RenewEntityExpiration);
				return (ActionOutcomeOption.OptionInfo.RenewEntityExpirationInfo)this.Value!;
			}

			public ActionOutcomeOption.OptionInfo.SetEntityAttributeInfo AsSetEntityAttribute()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.SetEntityAttribute);
				return (ActionOutcomeOption.OptionInfo.SetEntityAttributeInfo)this.Value!;
			}

			public ActionOutcomeOption.OptionInfo.SpendEntityQuantityInfo AsSpendEntityQuantity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.SpendEntityQuantity);
				return (ActionOutcomeOption.OptionInfo.SpendEntityQuantityInfo)this.Value!;
			}

			private void ValidateTag(ActionOutcomeOption.OptionInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}

			public class DeleteEntityInfo
			{
				[CandidTag(0U)]
				public worldId F0 { get; set; }

				[CandidTag(1U)]
				public groupId F1 { get; set; }

				[CandidTag(2U)]
				public entityId F2 { get; set; }

				public DeleteEntityInfo(worldId f0, groupId f1, entityId f2)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
				}

				public DeleteEntityInfo()
				{
				}
			}

			public class ReceiveEntityQuantityInfo
			{
				[CandidTag(0U)]
				public worldId F0 { get; set; }

				[CandidTag(1U)]
				public groupId F1 { get; set; }

				[CandidTag(2U)]
				public entityId F2 { get; set; }

				[CandidTag(3U)]
				public quantity F3 { get; set; }

				public ReceiveEntityQuantityInfo(worldId f0, groupId f1, entityId f2, quantity f3)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
					this.F3 = f3;
				}

				public ReceiveEntityQuantityInfo()
				{
				}
			}

			public class ReduceEntityExpirationInfo
			{
				[CandidTag(0U)]
				public worldId F0 { get; set; }

				[CandidTag(1U)]
				public groupId F1 { get; set; }

				[CandidTag(2U)]
				public entityId F2 { get; set; }

				[CandidTag(3U)]
				public duration F3 { get; set; }

				public ReduceEntityExpirationInfo(worldId f0, groupId f1, entityId f2, duration f3)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
					this.F3 = f3;
				}

				public ReduceEntityExpirationInfo()
				{
				}
			}

			public class RenewEntityExpirationInfo
			{
				[CandidTag(0U)]
				public worldId F0 { get; set; }

				[CandidTag(1U)]
				public groupId F1 { get; set; }

				[CandidTag(2U)]
				public entityId F2 { get; set; }

				[CandidTag(3U)]
				public duration F3 { get; set; }

				public RenewEntityExpirationInfo(worldId f0, groupId f1, entityId f2, duration f3)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
					this.F3 = f3;
				}

				public RenewEntityExpirationInfo()
				{
				}
			}

			public class SetEntityAttributeInfo
			{
				[CandidTag(0U)]
				public worldId F0 { get; set; }

				[CandidTag(1U)]
				public groupId F1 { get; set; }

				[CandidTag(2U)]
				public entityId F2 { get; set; }

				[CandidTag(3U)]
				public attribute F3 { get; set; }

				public SetEntityAttributeInfo(worldId f0, groupId f1, entityId f2, attribute f3)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
					this.F3 = f3;
				}

				public SetEntityAttributeInfo()
				{
				}
			}

			public class SpendEntityQuantityInfo
			{
				[CandidTag(0U)]
				public worldId F0 { get; set; }

				[CandidTag(1U)]
				public groupId F1 { get; set; }

				[CandidTag(2U)]
				public entityId F2 { get; set; }

				[CandidTag(3U)]
				public quantity F3 { get; set; }

				public SpendEntityQuantityInfo(worldId f0, groupId f1, entityId f2, quantity f3)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
					this.F3 = f3;
				}

				public SpendEntityQuantityInfo()
				{
				}
			}
		}

		public enum OptionInfoTag
		{
			[CandidName("deleteEntity")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.DeleteEntityInfo))]
			DeleteEntity,
			[CandidName("mintNft")]
			[VariantOptionType(typeof(MintNft))]
			MintNft,
			[CandidName("mintToken")]
			[VariantOptionType(typeof(MintToken))]
			MintToken,
			[CandidName("receiveEntityQuantity")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.ReceiveEntityQuantityInfo))]
			ReceiveEntityQuantity,
			[CandidName("reduceEntityExpiration")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.ReduceEntityExpirationInfo))]
			ReduceEntityExpiration,
			[CandidName("renewEntityExpiration")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.RenewEntityExpirationInfo))]
			RenewEntityExpiration,
			[CandidName("setEntityAttribute")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.SetEntityAttributeInfo))]
			SetEntityAttribute,
			[CandidName("spendEntityQuantity")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.SpendEntityQuantityInfo))]
			SpendEntityQuantity
		}
	}
}