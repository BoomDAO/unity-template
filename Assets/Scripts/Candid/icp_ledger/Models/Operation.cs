using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using BlockIndex = System.UInt64;
using Memo = System.UInt64;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using TextAccountIdentifier = System.String;
using Icrc1BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Icrc1Timestamp = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using Candid.IcpLedger.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.IcpLedger.Models
{
	[Variant(typeof(OperationTag))]
	public class Operation
	{
		[VariantTagProperty()]
		public OperationTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Operation(OperationTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Operation()
		{
		}

		public static Operation Mint(Operation.MintInfo info)
		{
			return new Operation(OperationTag.Mint, info);
		}

		public static Operation Burn(Operation.BurnInfo info)
		{
			return new Operation(OperationTag.Burn, info);
		}

		public static Operation Transfer(Operation.TransferInfo info)
		{
			return new Operation(OperationTag.Transfer, info);
		}

		public static Operation Approve(Operation.ApproveInfo info)
		{
			return new Operation(OperationTag.Approve, info);
		}

		public static Operation TransferFrom(Operation.TransferFromInfo info)
		{
			return new Operation(OperationTag.TransferFrom, info);
		}

		public Operation.MintInfo AsMint()
		{
			this.ValidateTag(OperationTag.Mint);
			return (Operation.MintInfo)this.Value!;
		}

		public Operation.BurnInfo AsBurn()
		{
			this.ValidateTag(OperationTag.Burn);
			return (Operation.BurnInfo)this.Value!;
		}

		public Operation.TransferInfo AsTransfer()
		{
			this.ValidateTag(OperationTag.Transfer);
			return (Operation.TransferInfo)this.Value!;
		}

		public Operation.ApproveInfo AsApprove()
		{
			this.ValidateTag(OperationTag.Approve);
			return (Operation.ApproveInfo)this.Value!;
		}

		public Operation.TransferFromInfo AsTransferFrom()
		{
			this.ValidateTag(OperationTag.TransferFrom);
			return (Operation.TransferFromInfo)this.Value!;
		}

		private void ValidateTag(OperationTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class MintInfo
		{
			[CandidName("to")]
			public AccountIdentifier To { get; set; }

			[CandidName("amount")]
			public Tokens Amount { get; set; }

			public MintInfo(AccountIdentifier to, Tokens amount)
			{
				this.To = to;
				this.Amount = amount;
			}

			public MintInfo()
			{
			}
		}

		public class BurnInfo
		{
			[CandidName("from")]
			public AccountIdentifier From { get; set; }

			[CandidName("amount")]
			public Tokens Amount { get; set; }

			public BurnInfo(AccountIdentifier from, Tokens amount)
			{
				this.From = from;
				this.Amount = amount;
			}

			public BurnInfo()
			{
			}
		}

		public class TransferInfo
		{
			[CandidName("from")]
			public AccountIdentifier From { get; set; }

			[CandidName("to")]
			public AccountIdentifier To { get; set; }

			[CandidName("amount")]
			public Tokens Amount { get; set; }

			[CandidName("fee")]
			public Tokens Fee { get; set; }

			public TransferInfo(AccountIdentifier from, AccountIdentifier to, Tokens amount, Tokens fee)
			{
				this.From = from;
				this.To = to;
				this.Amount = amount;
				this.Fee = fee;
			}

			public TransferInfo()
			{
			}
		}

		public class ApproveInfo
		{
			[CandidName("from")]
			public AccountIdentifier From { get; set; }

			[CandidName("spender")]
			public AccountIdentifier Spender { get; set; }

			[CandidName("allowance_e8s")]
			public UnboundedInt AllowanceE8s { get; set; }

			[CandidName("fee")]
			public Tokens Fee { get; set; }

			[CandidName("expires_at")]
			public OptionalValue<TimeStamp> ExpiresAt { get; set; }

			public ApproveInfo(AccountIdentifier from, AccountIdentifier spender, UnboundedInt allowanceE8s, Tokens fee, OptionalValue<TimeStamp> expiresAt)
			{
				this.From = from;
				this.Spender = spender;
				this.AllowanceE8s = allowanceE8s;
				this.Fee = fee;
				this.ExpiresAt = expiresAt;
			}

			public ApproveInfo()
			{
			}
		}

		public class TransferFromInfo
		{
			[CandidName("from")]
			public AccountIdentifier From { get; set; }

			[CandidName("to")]
			public AccountIdentifier To { get; set; }

			[CandidName("spender")]
			public AccountIdentifier Spender { get; set; }

			[CandidName("amount")]
			public Tokens Amount { get; set; }

			[CandidName("fee")]
			public Tokens Fee { get; set; }

			public TransferFromInfo(AccountIdentifier from, AccountIdentifier to, AccountIdentifier spender, Tokens amount, Tokens fee)
			{
				this.From = from;
				this.To = to;
				this.Spender = spender;
				this.Amount = amount;
				this.Fee = fee;
			}

			public TransferFromInfo()
			{
			}
		}
	}

	public enum OperationTag
	{
		[VariantOptionType(typeof(Operation.MintInfo))]
		Mint,
		[VariantOptionType(typeof(Operation.BurnInfo))]
		Burn,
		[VariantOptionType(typeof(Operation.TransferInfo))]
		Transfer,
		[VariantOptionType(typeof(Operation.ApproveInfo))]
		Approve,
		[VariantOptionType(typeof(Operation.TransferFromInfo))]
		TransferFrom
	}
}