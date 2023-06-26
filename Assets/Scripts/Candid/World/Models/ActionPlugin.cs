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
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	[Variant(typeof(ActionPluginTag))]
	public class ActionPlugin
	{
		[VariantTagProperty()]
		public ActionPluginTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public ActionPlugin(ActionPluginTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ActionPlugin()
		{
		}

		public static ActionPlugin BurnNft(ActionPlugin.BurnNftInfo info)
		{
			return new ActionPlugin(ActionPluginTag.BurnNft, info);
		}

		public static ActionPlugin ClaimStakingReward(ActionPlugin.ClaimStakingRewardInfo info)
		{
			return new ActionPlugin(ActionPluginTag.ClaimStakingReward, info);
		}

		public static ActionPlugin SpendTokens(ActionPlugin.SpendTokensInfo info)
		{
			return new ActionPlugin(ActionPluginTag.SpendTokens, info);
		}

		public ActionPlugin.BurnNftInfo AsBurnNft()
		{
			this.ValidateTag(ActionPluginTag.BurnNft);
			return (ActionPlugin.BurnNftInfo)this.Value!;
		}

		public ActionPlugin.ClaimStakingRewardInfo AsClaimStakingReward()
		{
			this.ValidateTag(ActionPluginTag.ClaimStakingReward);
			return (ActionPlugin.ClaimStakingRewardInfo)this.Value!;
		}

		public ActionPlugin.SpendTokensInfo AsSpendTokens()
		{
			this.ValidateTag(ActionPluginTag.SpendTokens);
			return (ActionPlugin.SpendTokensInfo)this.Value!;
		}

		private void ValidateTag(ActionPluginTag tag)
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
			[CandidName("baseZeroCount")]
			public UnboundedUInt BaseZeroCount { get; set; }

			[CandidName("requiredAmount")]
			public double RequiredAmount { get; set; }

			[CandidName("tokenCanister")]
			public string TokenCanister { get; set; }

			public ClaimStakingRewardInfo(UnboundedUInt baseZeroCount, double requiredAmount, string tokenCanister)
			{
				this.BaseZeroCount = baseZeroCount;
				this.RequiredAmount = requiredAmount;
				this.TokenCanister = tokenCanister;
			}

			public ClaimStakingRewardInfo()
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

	public enum ActionPluginTag
	{
		[CandidName("burnNft")]
		[VariantOptionType(typeof(ActionPlugin.BurnNftInfo))]
		BurnNft,
		[CandidName("claimStakingReward")]
		[VariantOptionType(typeof(ActionPlugin.ClaimStakingRewardInfo))]
		ClaimStakingReward,
		[CandidName("spendTokens")]
		[VariantOptionType(typeof(ActionPlugin.SpendTokensInfo))]
		SpendTokens
	}
}