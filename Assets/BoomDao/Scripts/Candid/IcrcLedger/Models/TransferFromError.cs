using TxIndex__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;
using Subaccount__1 = System.Collections.Generic.List<System.Byte>;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using Memo = System.Collections.Generic.List<System.Byte>;
using Balance__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.IcrcLedger.Models
{
	[Variant]
	public class TransferFromError
	{
		[VariantTagProperty()]
		public TransferFromErrorTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public TransferFromError(TransferFromErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferFromError()
		{
		}

		public static TransferFromError BadBurn(TransferFromError.BadBurnInfo info)
		{
			return new TransferFromError(TransferFromErrorTag.BadBurn, info);
		}

		public static TransferFromError BadFee(TransferFromError.BadFeeInfo info)
		{
			return new TransferFromError(TransferFromErrorTag.BadFee, info);
		}

		public static TransferFromError CreatedInFuture(TransferFromError.CreatedInFutureInfo info)
		{
			return new TransferFromError(TransferFromErrorTag.CreatedInFuture, info);
		}

		public static TransferFromError Duplicate(TransferFromError.DuplicateInfo info)
		{
			return new TransferFromError(TransferFromErrorTag.Duplicate, info);
		}

		public static TransferFromError GenericError(TransferFromError.GenericErrorInfo info)
		{
			return new TransferFromError(TransferFromErrorTag.GenericError, info);
		}

		public static TransferFromError InsufficientAllowance(TransferFromError.InsufficientAllowanceInfo info)
		{
			return new TransferFromError(TransferFromErrorTag.InsufficientAllowance, info);
		}

		public static TransferFromError InsufficientFunds(TransferFromError.InsufficientFundsInfo info)
		{
			return new TransferFromError(TransferFromErrorTag.InsufficientFunds, info);
		}

		public static TransferFromError TemporarilyUnavailable()
		{
			return new TransferFromError(TransferFromErrorTag.TemporarilyUnavailable, null);
		}

		public static TransferFromError TooOld()
		{
			return new TransferFromError(TransferFromErrorTag.TooOld, null);
		}

		public TransferFromError.BadBurnInfo AsBadBurn()
		{
			this.ValidateTag(TransferFromErrorTag.BadBurn);
			return (TransferFromError.BadBurnInfo)this.Value!;
		}

		public TransferFromError.BadFeeInfo AsBadFee()
		{
			this.ValidateTag(TransferFromErrorTag.BadFee);
			return (TransferFromError.BadFeeInfo)this.Value!;
		}

		public TransferFromError.CreatedInFutureInfo AsCreatedInFuture()
		{
			this.ValidateTag(TransferFromErrorTag.CreatedInFuture);
			return (TransferFromError.CreatedInFutureInfo)this.Value!;
		}

		public TransferFromError.DuplicateInfo AsDuplicate()
		{
			this.ValidateTag(TransferFromErrorTag.Duplicate);
			return (TransferFromError.DuplicateInfo)this.Value!;
		}

		public TransferFromError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(TransferFromErrorTag.GenericError);
			return (TransferFromError.GenericErrorInfo)this.Value!;
		}

		public TransferFromError.InsufficientAllowanceInfo AsInsufficientAllowance()
		{
			this.ValidateTag(TransferFromErrorTag.InsufficientAllowance);
			return (TransferFromError.InsufficientAllowanceInfo)this.Value!;
		}

		public TransferFromError.InsufficientFundsInfo AsInsufficientFunds()
		{
			this.ValidateTag(TransferFromErrorTag.InsufficientFunds);
			return (TransferFromError.InsufficientFundsInfo)this.Value!;
		}

		private void ValidateTag(TransferFromErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class BadBurnInfo
		{
			[CandidName("min_burn_amount")]
			public Balance MinBurnAmount { get; set; }

			public BadBurnInfo(Balance minBurnAmount)
			{
				this.MinBurnAmount = minBurnAmount;
			}

			public BadBurnInfo()
			{
			}
		}

		public class BadFeeInfo
		{
			[CandidName("expected_fee")]
			public Balance ExpectedFee { get; set; }

			public BadFeeInfo(Balance expectedFee)
			{
				this.ExpectedFee = expectedFee;
			}

			public BadFeeInfo()
			{
			}
		}

		public class CreatedInFutureInfo
		{
			[CandidName("ledger_time")]
			public Timestamp LedgerTime { get; set; }

			public CreatedInFutureInfo(Timestamp ledgerTime)
			{
				this.LedgerTime = ledgerTime;
			}

			public CreatedInFutureInfo()
			{
			}
		}

		public class DuplicateInfo
		{
			[CandidName("duplicate_of")]
			public TxIndex DuplicateOf { get; set; }

			public DuplicateInfo(TxIndex duplicateOf)
			{
				this.DuplicateOf = duplicateOf;
			}

			public DuplicateInfo()
			{
			}
		}

		public class GenericErrorInfo
		{
			[CandidName("error_code")]
			public UnboundedUInt ErrorCode { get; set; }

			[CandidName("message")]
			public string Message { get; set; }

			public GenericErrorInfo(UnboundedUInt errorCode, string message)
			{
				this.ErrorCode = errorCode;
				this.Message = message;
			}

			public GenericErrorInfo()
			{
			}
		}

		public class InsufficientAllowanceInfo
		{
			[CandidName("allowance")]
			public UnboundedUInt Allowance { get; set; }

			public InsufficientAllowanceInfo(UnboundedUInt allowance)
			{
				this.Allowance = allowance;
			}

			public InsufficientAllowanceInfo()
			{
			}
		}

		public class InsufficientFundsInfo
		{
			[CandidName("balance")]
			public Balance Balance { get; set; }

			public InsufficientFundsInfo(Balance balance)
			{
				this.Balance = balance;
			}

			public InsufficientFundsInfo()
			{
			}
		}
	}

	public enum TransferFromErrorTag
	{
		
		BadBurn,
		
		BadFee,
		
		CreatedInFuture,
		
		Duplicate,
		
		GenericError,
		
		InsufficientAllowance,
		
		InsufficientFunds,
		TemporarilyUnavailable,
		TooOld
	}
}