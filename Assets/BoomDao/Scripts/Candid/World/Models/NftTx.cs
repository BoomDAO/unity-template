using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	public class NftTx
	{
		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("metadata")]
		public OptionalValue<string> Metadata { get; set; }

		[CandidName("nftConstraintType")]
		public NftTx.NftConstraintTypeInfo NftConstraintType { get; set; }

		public NftTx(string canister, OptionalValue<string> metadata, NftTx.NftConstraintTypeInfo nftConstraintType)
		{
			this.Canister = canister;
			this.Metadata = metadata;
			this.NftConstraintType = nftConstraintType;
		}

		public NftTx()
		{
		}

		[Variant]
		public class NftConstraintTypeInfo
		{
			[VariantTagProperty]
			public NftTx.NftConstraintTypeInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public NftConstraintTypeInfo(NftTx.NftConstraintTypeInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected NftConstraintTypeInfo()
			{
			}

			public static NftTx.NftConstraintTypeInfo Hold(NftTx.NftConstraintTypeInfo.HoldInfo info)
			{
				return new NftTx.NftConstraintTypeInfo(NftTx.NftConstraintTypeInfoTag.Hold, info);
			}

			public static NftTx.NftConstraintTypeInfo Transfer(NftTransfer info)
			{
				return new NftTx.NftConstraintTypeInfo(NftTx.NftConstraintTypeInfoTag.Transfer, info);
			}

			public NftTx.NftConstraintTypeInfo.HoldInfo AsHold()
			{
				this.ValidateTag(NftTx.NftConstraintTypeInfoTag.Hold);
				return (NftTx.NftConstraintTypeInfo.HoldInfo)this.Value!;
			}

			public NftTransfer AsTransfer()
			{
				this.ValidateTag(NftTx.NftConstraintTypeInfoTag.Transfer);
				return (NftTransfer)this.Value!;
			}

			private void ValidateTag(NftTx.NftConstraintTypeInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}

			public enum HoldInfo
			{
				[CandidName("boomEXT")]
				BoomEXT,
				[CandidName("originalEXT")]
				OriginalEXT
			}
		}

		public enum NftConstraintTypeInfoTag
		{
			[CandidName("hold")]
			Hold,
			[CandidName("transfer")]
			Transfer
		}
	}
}