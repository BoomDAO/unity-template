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

		[Variant]
		public class OptionInfo
		{
			[VariantTagProperty]
			public ActionOutcomeOption.OptionInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public OptionInfo(ActionOutcomeOption.OptionInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected OptionInfo()
			{
			}

			public static ActionOutcomeOption.OptionInfo MintNft(MintNft info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.MintNft, info);
			}

			public static ActionOutcomeOption.OptionInfo TransferIcrc(TransferIcrc info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.TransferIcrc, info);
			}

			public static ActionOutcomeOption.OptionInfo UpdateEntity(UpdateEntity info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.UpdateEntity, info);
			}

			public MintNft AsMintNft()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.MintNft);
				return (MintNft)this.Value!;
			}

			public TransferIcrc AsTransferIcrc()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.TransferIcrc);
				return (TransferIcrc)this.Value!;
			}

			public UpdateEntity AsUpdateEntity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.UpdateEntity);
				return (UpdateEntity)this.Value!;
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
			[CandidName("mintNft")]
			MintNft,
			[CandidName("transferIcrc")]
			TransferIcrc,
			[CandidName("updateEntity")]
			UpdateEntity
		}
	}
}