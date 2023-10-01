using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;
using EdjCase.ICP.Candid.Models;

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

			public static ActionOutcomeOption.OptionInfo DecrementNumber(ActionOutcomeOption.OptionInfo.DecrementNumberInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.DecrementNumber, info);
			}

			public static ActionOutcomeOption.OptionInfo DeleteEntity(DeleteEntity info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.DeleteEntity, info);
			}

			public static ActionOutcomeOption.OptionInfo IncrementNumber(ActionOutcomeOption.OptionInfo.IncrementNumberInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.IncrementNumber, info);
			}

			public static ActionOutcomeOption.OptionInfo MintNft(MintNft info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.MintNft, info);
			}

			public static ActionOutcomeOption.OptionInfo MintToken(MintToken info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.MintToken, info);
			}

			public static ActionOutcomeOption.OptionInfo RenewTimestamp(ActionOutcomeOption.OptionInfo.RenewTimestampInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.RenewTimestamp, info);
			}

			public static ActionOutcomeOption.OptionInfo SetNumber(ActionOutcomeOption.OptionInfo.SetNumberInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.SetNumber, info);
			}

			public static ActionOutcomeOption.OptionInfo SetString(ActionOutcomeOption.OptionInfo.SetStringInfo info)
			{
				return new ActionOutcomeOption.OptionInfo(ActionOutcomeOption.OptionInfoTag.SetString, info);
			}

			public ActionOutcomeOption.OptionInfo.DecrementNumberInfo AsDecrementNumber()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.DecrementNumber);
				return (ActionOutcomeOption.OptionInfo.DecrementNumberInfo)this.Value!;
			}

			public DeleteEntity AsDeleteEntity()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.DeleteEntity);
				return (DeleteEntity)this.Value!;
			}

			public ActionOutcomeOption.OptionInfo.IncrementNumberInfo AsIncrementNumber()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.IncrementNumber);
				return (ActionOutcomeOption.OptionInfo.IncrementNumberInfo)this.Value!;
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

			public ActionOutcomeOption.OptionInfo.RenewTimestampInfo AsRenewTimestamp()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.RenewTimestamp);
				return (ActionOutcomeOption.OptionInfo.RenewTimestampInfo)this.Value!;
			}

			public ActionOutcomeOption.OptionInfo.SetNumberInfo AsSetNumber()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.SetNumber);
				return (ActionOutcomeOption.OptionInfo.SetNumberInfo)this.Value!;
			}

			public ActionOutcomeOption.OptionInfo.SetStringInfo AsSetString()
			{
				this.ValidateTag(ActionOutcomeOption.OptionInfoTag.SetString);
				return (ActionOutcomeOption.OptionInfo.SetStringInfo)this.Value!;
			}

			private void ValidateTag(ActionOutcomeOption.OptionInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}

			public class DecrementNumberInfo
			{
				[CandidName("eid")]
				public entityId Eid { get; set; }

				[CandidName("field")]
				public string Field { get; set; }

				[CandidName("gid")]
				public groupId Gid { get; set; }

				[CandidName("value")]
				public double Value { get; set; }

				[CandidName("wid")]
				public OptionalValue<worldId> Wid { get; set; }

				public DecrementNumberInfo(entityId eid, string field, groupId gid, double value, OptionalValue<worldId> wid)
				{
					this.Eid = eid;
					this.Field = field;
					this.Gid = gid;
					this.Value = value;
					this.Wid = wid;
				}

				public DecrementNumberInfo()
				{
				}
			}

			public class IncrementNumberInfo
			{
				[CandidName("eid")]
				public entityId Eid { get; set; }

				[CandidName("field")]
				public string Field { get; set; }

				[CandidName("gid")]
				public groupId Gid { get; set; }

				[CandidName("value")]
				public double Value { get; set; }

				[CandidName("wid")]
				public OptionalValue<worldId> Wid { get; set; }

				public IncrementNumberInfo(entityId eid, string field, groupId gid, double value, OptionalValue<worldId> wid)
				{
					this.Eid = eid;
					this.Field = field;
					this.Gid = gid;
					this.Value = value;
					this.Wid = wid;
				}

				public IncrementNumberInfo()
				{
				}
			}

			public class RenewTimestampInfo
			{
				[CandidName("eid")]
				public entityId Eid { get; set; }

				[CandidName("field")]
				public string Field { get; set; }

				[CandidName("gid")]
				public groupId Gid { get; set; }

				[CandidName("value")]
				public UnboundedUInt Value { get; set; }

				[CandidName("wid")]
				public OptionalValue<worldId> Wid { get; set; }

				public RenewTimestampInfo(entityId eid, string field, groupId gid, UnboundedUInt value, OptionalValue<worldId> wid)
				{
					this.Eid = eid;
					this.Field = field;
					this.Gid = gid;
					this.Value = value;
					this.Wid = wid;
				}

				public RenewTimestampInfo()
				{
				}
			}

			public class SetNumberInfo
			{
				[CandidName("eid")]
				public entityId Eid { get; set; }

				[CandidName("field")]
				public string Field { get; set; }

				[CandidName("gid")]
				public groupId Gid { get; set; }

				[CandidName("value")]
				public double Value { get; set; }

				[CandidName("wid")]
				public OptionalValue<worldId> Wid { get; set; }

				public SetNumberInfo(entityId eid, string field, groupId gid, double value, OptionalValue<worldId> wid)
				{
					this.Eid = eid;
					this.Field = field;
					this.Gid = gid;
					this.Value = value;
					this.Wid = wid;
				}

				public SetNumberInfo()
				{
				}
			}

			public class SetStringInfo
			{
				[CandidName("eid")]
				public entityId Eid { get; set; }

				[CandidName("field")]
				public string Field { get; set; }

				[CandidName("gid")]
				public groupId Gid { get; set; }

				[CandidName("value")]
				public string Value { get; set; }

				[CandidName("wid")]
				public OptionalValue<worldId> Wid { get; set; }

				public SetStringInfo(entityId eid, string field, groupId gid, string value, OptionalValue<worldId> wid)
				{
					this.Eid = eid;
					this.Field = field;
					this.Gid = gid;
					this.Value = value;
					this.Wid = wid;
				}

				public SetStringInfo()
				{
				}
			}
		}

		public enum OptionInfoTag
		{
			[CandidName("decrementNumber")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.DecrementNumberInfo))]
			DecrementNumber,
			[CandidName("deleteEntity")]
			[VariantOptionType(typeof(DeleteEntity))]
			DeleteEntity,
			[CandidName("incrementNumber")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.IncrementNumberInfo))]
			IncrementNumber,
			[CandidName("mintNft")]
			[VariantOptionType(typeof(MintNft))]
			MintNft,
			[CandidName("mintToken")]
			[VariantOptionType(typeof(MintToken))]
			MintToken,
			[CandidName("renewTimestamp")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.RenewTimestampInfo))]
			RenewTimestamp,
			[CandidName("setNumber")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.SetNumberInfo))]
			SetNumber,
			[CandidName("setString")]
			[VariantOptionType(typeof(ActionOutcomeOption.OptionInfo.SetStringInfo))]
			SetString
		}
	}
}