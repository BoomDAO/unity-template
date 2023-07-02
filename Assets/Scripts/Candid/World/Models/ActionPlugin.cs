using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
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

		public static ActionPlugin ClaimStakingRewardIcp(ActionPlugin.ClaimStakingRewardIcpInfo info)
		{
			return new ActionPlugin(ActionPluginTag.ClaimStakingRewardIcp, info);
		}

		public static ActionPlugin ClaimStakingRewardIcrc(ActionPlugin.ClaimStakingRewardIcrcInfo info)
		{
			return new ActionPlugin(ActionPluginTag.ClaimStakingRewardIcrc, info);
		}

		public static ActionPlugin ClaimStakingRewardNft(ActionPlugin.ClaimStakingRewardNftInfo info)
		{
			return new ActionPlugin(ActionPluginTag.ClaimStakingRewardNft, info);
		}

		public static ActionPlugin VerifyTransferIcp(ActionPlugin.VerifyTransferIcpInfo info)
		{
			return new ActionPlugin(ActionPluginTag.VerifyTransferIcp, info);
		}

		public static ActionPlugin VerifyTransferIcrc(ActionPlugin.VerifyTransferIcrcInfo info)
		{
			return new ActionPlugin(ActionPluginTag.VerifyTransferIcrc, info);
		}

		public ActionPlugin.BurnNftInfo AsBurnNft()
		{
			this.ValidateTag(ActionPluginTag.BurnNft);
			return (ActionPlugin.BurnNftInfo)this.Value!;
		}

		public ActionPlugin.ClaimStakingRewardIcpInfo AsClaimStakingRewardIcp()
		{
			this.ValidateTag(ActionPluginTag.ClaimStakingRewardIcp);
			return (ActionPlugin.ClaimStakingRewardIcpInfo)this.Value!;
		}

		public ActionPlugin.ClaimStakingRewardIcrcInfo AsClaimStakingRewardIcrc()
		{
			this.ValidateTag(ActionPluginTag.ClaimStakingRewardIcrc);
			return (ActionPlugin.ClaimStakingRewardIcrcInfo)this.Value!;
		}

		public ActionPlugin.ClaimStakingRewardNftInfo AsClaimStakingRewardNft()
		{
			this.ValidateTag(ActionPluginTag.ClaimStakingRewardNft);
			return (ActionPlugin.ClaimStakingRewardNftInfo)this.Value!;
		}

		public ActionPlugin.VerifyTransferIcpInfo AsVerifyTransferIcp()
		{
			this.ValidateTag(ActionPluginTag.VerifyTransferIcp);
			return (ActionPlugin.VerifyTransferIcpInfo)this.Value!;
		}

		public ActionPlugin.VerifyTransferIcrcInfo AsVerifyTransferIcrc()
		{
			this.ValidateTag(ActionPluginTag.VerifyTransferIcrc);
			return (ActionPlugin.VerifyTransferIcrcInfo)this.Value!;
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
			[CandidName("canister")]
			public string Canister { get; set; }

			public BurnNftInfo(string canister)
			{
				this.Canister = canister;
			}

			public BurnNftInfo()
			{
			}
		}

		public class ClaimStakingRewardIcpInfo
		{
			[CandidName("requiredAmount")]
			public double RequiredAmount { get; set; }

			public ClaimStakingRewardIcpInfo(double requiredAmount)
			{
				this.RequiredAmount = requiredAmount;
			}

			public ClaimStakingRewardIcpInfo()
			{
			}
		}

		public class ClaimStakingRewardIcrcInfo
		{
			[CandidName("baseUnitCount")]
			public UnboundedUInt BaseUnitCount { get; set; }

			[CandidName("canister")]
			public string Canister { get; set; }

			[CandidName("requiredAmount")]
			public double RequiredAmount { get; set; }

			public ClaimStakingRewardIcrcInfo(UnboundedUInt baseUnitCount, string canister, double requiredAmount)
			{
				this.BaseUnitCount = baseUnitCount;
				this.Canister = canister;
				this.RequiredAmount = requiredAmount;
			}

			public ClaimStakingRewardIcrcInfo()
			{
			}
		}

		public class ClaimStakingRewardNftInfo
		{
			[CandidName("canister")]
			public string Canister { get; set; }

			[CandidName("requiredAmount")]
			public UnboundedUInt RequiredAmount { get; set; }

			public ClaimStakingRewardNftInfo(string canister, UnboundedUInt requiredAmount)
			{
				this.Canister = canister;
				this.RequiredAmount = requiredAmount;
			}

			public ClaimStakingRewardNftInfo()
			{
			}
		}

		public class VerifyTransferIcpInfo
		{
			[CandidName("amt")]
			public double Amt { get; set; }

			[CandidName("toPrincipal")]
			public string ToPrincipal { get; set; }

			public VerifyTransferIcpInfo(double amt, string toPrincipal)
			{
				this.Amt = amt;
				this.ToPrincipal = toPrincipal;
			}

			public VerifyTransferIcpInfo()
			{
			}
		}

		public class VerifyTransferIcrcInfo
		{
			[CandidName("amt")]
			public double Amt { get; set; }

			[CandidName("baseUnitCount")]
			public UnboundedUInt BaseUnitCount { get; set; }

			[CandidName("canister")]
			public string Canister { get; set; }

			[CandidName("toPrincipal")]
			public string ToPrincipal { get; set; }

			public VerifyTransferIcrcInfo(double amt, UnboundedUInt baseUnitCount, string canister, string toPrincipal)
			{
				this.Amt = amt;
				this.BaseUnitCount = baseUnitCount;
				this.Canister = canister;
				this.ToPrincipal = toPrincipal;
			}

			public VerifyTransferIcrcInfo()
			{
			}
		}
	}

	public enum ActionPluginTag
	{
		[CandidName("burnNft")]
		[VariantOptionType(typeof(ActionPlugin.BurnNftInfo))]
		BurnNft,
		[CandidName("claimStakingRewardIcp")]
		[VariantOptionType(typeof(ActionPlugin.ClaimStakingRewardIcpInfo))]
		ClaimStakingRewardIcp,
		[CandidName("claimStakingRewardIcrc")]
		[VariantOptionType(typeof(ActionPlugin.ClaimStakingRewardIcrcInfo))]
		ClaimStakingRewardIcrc,
		[CandidName("claimStakingRewardNft")]
		[VariantOptionType(typeof(ActionPlugin.ClaimStakingRewardNftInfo))]
		ClaimStakingRewardNft,
		[CandidName("verifyTransferIcp")]
		[VariantOptionType(typeof(ActionPlugin.VerifyTransferIcpInfo))]
		VerifyTransferIcp,
		[CandidName("verifyTransferIcrc")]
		[VariantOptionType(typeof(ActionPlugin.VerifyTransferIcrcInfo))]
		VerifyTransferIcrc
	}
}