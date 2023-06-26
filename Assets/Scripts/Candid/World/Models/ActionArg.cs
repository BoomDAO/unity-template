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
	[Variant(typeof(ActionArgTag))]
	public class ActionArg
	{
		[VariantTagProperty()]
		public ActionArgTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public ActionArg(ActionArgTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ActionArg()
		{
		}

		public static ActionArg BurnNft(ActionArg.BurnNftInfo info)
		{
			return new ActionArg(ActionArgTag.BurnNft, info);
		}

		public static ActionArg ClaimStakingReward(ActionArg.ClaimStakingRewardInfo info)
		{
			return new ActionArg(ActionArgTag.ClaimStakingReward, info);
		}

		public static ActionArg Default(ActionArg.DefaultInfo info)
		{
			return new ActionArg(ActionArgTag.Default, info);
		}

		public static ActionArg SpendTokens(ActionArg.SpendTokensInfo info)
		{
			return new ActionArg(ActionArgTag.SpendTokens, info);
		}

		public ActionArg.BurnNftInfo AsBurnNft()
		{
			this.ValidateTag(ActionArgTag.BurnNft);
			return (ActionArg.BurnNftInfo)this.Value!;
		}

		public ActionArg.ClaimStakingRewardInfo AsClaimStakingReward()
		{
			this.ValidateTag(ActionArgTag.ClaimStakingReward);
			return (ActionArg.ClaimStakingRewardInfo)this.Value!;
		}

		public ActionArg.DefaultInfo AsDefault()
		{
			this.ValidateTag(ActionArgTag.Default);
			return (ActionArg.DefaultInfo)this.Value!;
		}

		public ActionArg.SpendTokensInfo AsSpendTokens()
		{
			this.ValidateTag(ActionArgTag.SpendTokens);
			return (ActionArg.SpendTokensInfo)this.Value!;
		}

		private void ValidateTag(ActionArgTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class BurnNftInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			[CandidName("index")]
			public uint Index { get; set; }

			public BurnNftInfo(string actionId, uint index)
			{
				this.ActionId = actionId;
				this.Index = index;
			}

			public BurnNftInfo()
			{
			}
		}

		public class ClaimStakingRewardInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			public ClaimStakingRewardInfo(string actionId)
			{
				this.ActionId = actionId;
			}

			public ClaimStakingRewardInfo()
			{
			}
		}

		public class DefaultInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			public DefaultInfo(string actionId)
			{
				this.ActionId = actionId;
			}

			public DefaultInfo()
			{
			}
		}

		public class SpendTokensInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			[CandidName("hash")]
			public ulong Hash { get; set; }

			public SpendTokensInfo(string actionId, ulong hash)
			{
				this.ActionId = actionId;
				this.Hash = hash;
			}

			public SpendTokensInfo()
			{
			}
		}
	}

	public enum ActionArgTag
	{
		[CandidName("burnNft")]
		[VariantOptionType(typeof(ActionArg.BurnNftInfo))]
		BurnNft,
		[CandidName("claimStakingReward")]
		[VariantOptionType(typeof(ActionArg.ClaimStakingRewardInfo))]
		ClaimStakingReward,
		[CandidName("default")]
		[VariantOptionType(typeof(ActionArg.DefaultInfo))]
		Default,
		[CandidName("spendTokens")]
		[VariantOptionType(typeof(ActionArg.SpendTokensInfo))]
		SpendTokens
	}
}