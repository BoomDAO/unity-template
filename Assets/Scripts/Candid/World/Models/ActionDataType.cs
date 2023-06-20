using worldId = System.String;
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
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	[Variant(typeof(ActionDataTypeTag))]
	public class ActionDataType
	{
		[VariantTagProperty()]
		public ActionDataTypeTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public ActionDataType(ActionDataTypeTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ActionDataType()
		{
		}

		public static ActionDataType BurnNft(ActionDataType.BurnNftInfo info)
		{
			return new ActionDataType(ActionDataTypeTag.BurnNft, info);
		}

		public static ActionDataType ClaimStakingReward(ActionDataType.ClaimStakingRewardInfo info)
		{
			return new ActionDataType(ActionDataTypeTag.ClaimStakingReward, info);
		}

		public static ActionDataType SpendEntities(ActionDataType.SpendEntitiesInfo info)
		{
			return new ActionDataType(ActionDataTypeTag.SpendEntities, info);
		}

		public static ActionDataType SpendTokens(ActionDataType.SpendTokensInfo info)
		{
			return new ActionDataType(ActionDataTypeTag.SpendTokens, info);
		}

		public ActionDataType.BurnNftInfo AsBurnNft()
		{
			this.ValidateTag(ActionDataTypeTag.BurnNft);
			return (ActionDataType.BurnNftInfo)this.Value!;
		}

		public ActionDataType.ClaimStakingRewardInfo AsClaimStakingReward()
		{
			this.ValidateTag(ActionDataTypeTag.ClaimStakingReward);
			return (ActionDataType.ClaimStakingRewardInfo)this.Value!;
		}

		public ActionDataType.SpendEntitiesInfo AsSpendEntities()
		{
			this.ValidateTag(ActionDataTypeTag.SpendEntities);
			return (ActionDataType.SpendEntitiesInfo)this.Value!;
		}

		public ActionDataType.SpendTokensInfo AsSpendTokens()
		{
			this.ValidateTag(ActionDataTypeTag.SpendTokens);
			return (ActionDataType.SpendTokensInfo)this.Value!;
		}

		private void ValidateTag(ActionDataTypeTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class BurnNftInfo
		{
			[CandidName("nftCanister")]
			public string NftCanister { get; set; }

			public BurnNftInfo(string nftCanister)
			{
				this.NftCanister = nftCanister;
			}

			public BurnNftInfo()
			{
			}
		}

		public class ClaimStakingRewardInfo
		{
			[CandidName("requiredAmount")]
			public UnboundedUInt RequiredAmount { get; set; }

			[CandidName("tokenCanister")]
			public string TokenCanister { get; set; }

			public ClaimStakingRewardInfo(UnboundedUInt requiredAmount, string tokenCanister)
			{
				this.RequiredAmount = requiredAmount;
				this.TokenCanister = tokenCanister;
			}

			public ClaimStakingRewardInfo()
			{
			}
		}

		public class SpendEntitiesInfo
		{
			public SpendEntitiesInfo()
			{
			}
		}

		public class SpendTokensInfo
		{
			[CandidName("amt")]
			public double Amt { get; set; }

			[CandidName("baseZeroCount")]
			public UnboundedUInt BaseZeroCount { get; set; }

			[CandidName("toPrincipal")]
			public string ToPrincipal { get; set; }

			[CandidName("tokenCanister")]
			public OptionalValue<string> TokenCanister { get; set; }

			public SpendTokensInfo(double amt, UnboundedUInt baseZeroCount, string toPrincipal, OptionalValue<string> tokenCanister)
			{
				this.Amt = amt;
				this.BaseZeroCount = baseZeroCount;
				this.ToPrincipal = toPrincipal;
				this.TokenCanister = tokenCanister;
			}

			public SpendTokensInfo()
			{
			}
		}
	}

	public enum ActionDataTypeTag
	{
		[CandidName("burnNft")]
		[VariantOptionType(typeof(ActionDataType.BurnNftInfo))]
		BurnNft,
		[CandidName("claimStakingReward")]
		[VariantOptionType(typeof(ActionDataType.ClaimStakingRewardInfo))]
		ClaimStakingReward,
		[CandidName("spendEntities")]
		[VariantOptionType(typeof(ActionDataType.SpendEntitiesInfo))]
		SpendEntities,
		[CandidName("spendTokens")]
		[VariantOptionType(typeof(ActionDataType.SpendTokensInfo))]
		SpendTokens
	}
}