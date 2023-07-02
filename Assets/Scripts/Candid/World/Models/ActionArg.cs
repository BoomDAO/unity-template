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

		public static ActionArg ClaimStakingRewardIcp(ActionArg.ClaimStakingRewardIcpInfo info)
		{
			return new ActionArg(ActionArgTag.ClaimStakingRewardIcp, info);
		}

		public static ActionArg ClaimStakingRewardIcrc(ActionArg.ClaimStakingRewardIcrcInfo info)
		{
			return new ActionArg(ActionArgTag.ClaimStakingRewardIcrc, info);
		}

		public static ActionArg ClaimStakingRewardNft(ActionArg.ClaimStakingRewardNftInfo info)
		{
			return new ActionArg(ActionArgTag.ClaimStakingRewardNft, info);
		}

		public static ActionArg Default(ActionArg.DefaultInfo info)
		{
			return new ActionArg(ActionArgTag.Default, info);
		}

		public static ActionArg VerifyTransferIcp(ActionArg.VerifyTransferIcpInfo info)
		{
			return new ActionArg(ActionArgTag.VerifyTransferIcp, info);
		}

		public static ActionArg VerifyTransferIcrc(ActionArg.VerifyTransferIcrcInfo info)
		{
			return new ActionArg(ActionArgTag.VerifyTransferIcrc, info);
		}

		public ActionArg.BurnNftInfo AsBurnNft()
		{
			this.ValidateTag(ActionArgTag.BurnNft);
			return (ActionArg.BurnNftInfo)this.Value!;
		}

		public ActionArg.ClaimStakingRewardIcpInfo AsClaimStakingRewardIcp()
		{
			this.ValidateTag(ActionArgTag.ClaimStakingRewardIcp);
			return (ActionArg.ClaimStakingRewardIcpInfo)this.Value!;
		}

		public ActionArg.ClaimStakingRewardIcrcInfo AsClaimStakingRewardIcrc()
		{
			this.ValidateTag(ActionArgTag.ClaimStakingRewardIcrc);
			return (ActionArg.ClaimStakingRewardIcrcInfo)this.Value!;
		}

		public ActionArg.ClaimStakingRewardNftInfo AsClaimStakingRewardNft()
		{
			this.ValidateTag(ActionArgTag.ClaimStakingRewardNft);
			return (ActionArg.ClaimStakingRewardNftInfo)this.Value!;
		}

		public ActionArg.DefaultInfo AsDefault()
		{
			this.ValidateTag(ActionArgTag.Default);
			return (ActionArg.DefaultInfo)this.Value!;
		}

		public ActionArg.VerifyTransferIcpInfo AsVerifyTransferIcp()
		{
			this.ValidateTag(ActionArgTag.VerifyTransferIcp);
			return (ActionArg.VerifyTransferIcpInfo)this.Value!;
		}

		public ActionArg.VerifyTransferIcrcInfo AsVerifyTransferIcrc()
		{
			this.ValidateTag(ActionArgTag.VerifyTransferIcrc);
			return (ActionArg.VerifyTransferIcrcInfo)this.Value!;
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

		public class ClaimStakingRewardIcpInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			public ClaimStakingRewardIcpInfo(string actionId)
			{
				this.ActionId = actionId;
			}

			public ClaimStakingRewardIcpInfo()
			{
			}
		}

		public class ClaimStakingRewardIcrcInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			public ClaimStakingRewardIcrcInfo(string actionId)
			{
				this.ActionId = actionId;
			}

			public ClaimStakingRewardIcrcInfo()
			{
			}
		}

		public class ClaimStakingRewardNftInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			public ClaimStakingRewardNftInfo(string actionId)
			{
				this.ActionId = actionId;
			}

			public ClaimStakingRewardNftInfo()
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

		public class VerifyTransferIcpInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			[CandidName("blockIndex")]
			public ulong BlockIndex { get; set; }

			public VerifyTransferIcpInfo(string actionId, ulong blockIndex)
			{
				this.ActionId = actionId;
				this.BlockIndex = blockIndex;
			}

			public VerifyTransferIcpInfo()
			{
			}
		}

		public class VerifyTransferIcrcInfo
		{
			[CandidName("actionId")]
			public string ActionId { get; set; }

			[CandidName("blockIndex")]
			public UnboundedUInt BlockIndex { get; set; }

			public VerifyTransferIcrcInfo(string actionId, UnboundedUInt blockIndex)
			{
				this.ActionId = actionId;
				this.BlockIndex = blockIndex;
			}

			public VerifyTransferIcrcInfo()
			{
			}
		}
	}

	public enum ActionArgTag
	{
		[CandidName("burnNft")]
		[VariantOptionType(typeof(ActionArg.BurnNftInfo))]
		BurnNft,
		[CandidName("claimStakingRewardIcp")]
		[VariantOptionType(typeof(ActionArg.ClaimStakingRewardIcpInfo))]
		ClaimStakingRewardIcp,
		[CandidName("claimStakingRewardIcrc")]
		[VariantOptionType(typeof(ActionArg.ClaimStakingRewardIcrcInfo))]
		ClaimStakingRewardIcrc,
		[CandidName("claimStakingRewardNft")]
		[VariantOptionType(typeof(ActionArg.ClaimStakingRewardNftInfo))]
		ClaimStakingRewardNft,
		[CandidName("default")]
		[VariantOptionType(typeof(ActionArg.DefaultInfo))]
		Default,
		[CandidName("verifyTransferIcp")]
		[VariantOptionType(typeof(ActionArg.VerifyTransferIcpInfo))]
		VerifyTransferIcp,
		[CandidName("verifyTransferIcrc")]
		[VariantOptionType(typeof(ActionArg.VerifyTransferIcrcInfo))]
		VerifyTransferIcrc
	}
}