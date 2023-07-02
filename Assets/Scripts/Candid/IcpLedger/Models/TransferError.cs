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

namespace Candid.IcpLedger.Models
{
	[Variant(typeof(TransferErrorTag))]
	public class TransferError
	{
		[VariantTagProperty()]
		public TransferErrorTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public TransferError(TransferErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferError()
		{
		}

		public static TransferError BadFee(TransferError.BadFeeInfo info)
		{
			return new TransferError(TransferErrorTag.BadFee, info);
		}

		public static TransferError InsufficientFunds(TransferError.InsufficientFundsInfo info)
		{
			return new TransferError(TransferErrorTag.InsufficientFunds, info);
		}

		public static TransferError TxTooOld(TransferError.TxTooOldInfo info)
		{
			return new TransferError(TransferErrorTag.TxTooOld, info);
		}

		public static TransferError TxCreatedInFuture()
		{
			return new TransferError(TransferErrorTag.TxCreatedInFuture, null);
		}

		public static TransferError TxDuplicate(TransferError.TxDuplicateInfo info)
		{
			return new TransferError(TransferErrorTag.TxDuplicate, info);
		}

		public TransferError.BadFeeInfo AsBadFee()
		{
			this.ValidateTag(TransferErrorTag.BadFee);
			return (TransferError.BadFeeInfo)this.Value!;
		}

		public TransferError.InsufficientFundsInfo AsInsufficientFunds()
		{
			this.ValidateTag(TransferErrorTag.InsufficientFunds);
			return (TransferError.InsufficientFundsInfo)this.Value!;
		}

		public TransferError.TxTooOldInfo AsTxTooOld()
		{
			this.ValidateTag(TransferErrorTag.TxTooOld);
			return (TransferError.TxTooOldInfo)this.Value!;
		}

		public TransferError.TxDuplicateInfo AsTxDuplicate()
		{
			this.ValidateTag(TransferErrorTag.TxDuplicate);
			return (TransferError.TxDuplicateInfo)this.Value!;
		}

		private void ValidateTag(TransferErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class BadFeeInfo
		{
			[CandidName("expected_fee")]
			public Tokens ExpectedFee { get; set; }

			public BadFeeInfo(Tokens expectedFee)
			{
				this.ExpectedFee = expectedFee;
			}

			public BadFeeInfo()
			{
			}
		}

		public class InsufficientFundsInfo
		{
			[CandidName("balance")]
			public Tokens Balance { get; set; }

			public InsufficientFundsInfo(Tokens balance)
			{
				this.Balance = balance;
			}

			public InsufficientFundsInfo()
			{
			}
		}

		public class TxTooOldInfo
		{
			[CandidName("allowed_window_nanos")]
			public ulong AllowedWindowNanos { get; set; }

			public TxTooOldInfo(ulong allowedWindowNanos)
			{
				this.AllowedWindowNanos = allowedWindowNanos;
			}

			public TxTooOldInfo()
			{
			}
		}

		public class TxDuplicateInfo
		{
			[CandidName("duplicate_of")]
			public BlockIndex DuplicateOf { get; set; }

			public TxDuplicateInfo(BlockIndex duplicateOf)
			{
				this.DuplicateOf = duplicateOf;
			}

			public TxDuplicateInfo()
			{
			}
		}
	}

	public enum TransferErrorTag
	{
		[VariantOptionType(typeof(TransferError.BadFeeInfo))]
		BadFee,
		[VariantOptionType(typeof(TransferError.InsufficientFundsInfo))]
		InsufficientFunds,
		[VariantOptionType(typeof(TransferError.TxTooOldInfo))]
		TxTooOld,
		TxCreatedInFuture,
		[VariantOptionType(typeof(TransferError.TxDuplicateInfo))]
		TxDuplicate
	}
}