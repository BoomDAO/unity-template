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
	public class ApproveError
	{
		[VariantTagProperty()]
		public ApproveErrorTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public ApproveError(ApproveErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ApproveError()
		{
		}

		public static ApproveError AllowanceChanged(ApproveError.AllowanceChangedInfo info)
		{
			return new ApproveError(ApproveErrorTag.AllowanceChanged, info);
		}

		public static ApproveError BadFee(ApproveError.BadFeeInfo info)
		{
			return new ApproveError(ApproveErrorTag.BadFee, info);
		}

		public static ApproveError CreatedInFuture(ApproveError.CreatedInFutureInfo info)
		{
			return new ApproveError(ApproveErrorTag.CreatedInFuture, info);
		}

		public static ApproveError Duplicate(ApproveError.DuplicateInfo info)
		{
			return new ApproveError(ApproveErrorTag.Duplicate, info);
		}

		public static ApproveError Expired(ApproveError.ExpiredInfo info)
		{
			return new ApproveError(ApproveErrorTag.Expired, info);
		}

		public static ApproveError GenericError(ApproveError.GenericErrorInfo info)
		{
			return new ApproveError(ApproveErrorTag.GenericError, info);
		}

		public static ApproveError InsufficientFunds(ApproveError.InsufficientFundsInfo info)
		{
			return new ApproveError(ApproveErrorTag.InsufficientFunds, info);
		}

		public static ApproveError TemporarilyUnavailable()
		{
			return new ApproveError(ApproveErrorTag.TemporarilyUnavailable, null);
		}

		public static ApproveError TooOld()
		{
			return new ApproveError(ApproveErrorTag.TooOld, null);
		}

		public ApproveError.AllowanceChangedInfo AsAllowanceChanged()
		{
			this.ValidateTag(ApproveErrorTag.AllowanceChanged);
			return (ApproveError.AllowanceChangedInfo)this.Value!;
		}

		public ApproveError.BadFeeInfo AsBadFee()
		{
			this.ValidateTag(ApproveErrorTag.BadFee);
			return (ApproveError.BadFeeInfo)this.Value!;
		}

		public ApproveError.CreatedInFutureInfo AsCreatedInFuture()
		{
			this.ValidateTag(ApproveErrorTag.CreatedInFuture);
			return (ApproveError.CreatedInFutureInfo)this.Value!;
		}

		public ApproveError.DuplicateInfo AsDuplicate()
		{
			this.ValidateTag(ApproveErrorTag.Duplicate);
			return (ApproveError.DuplicateInfo)this.Value!;
		}

		public ApproveError.ExpiredInfo AsExpired()
		{
			this.ValidateTag(ApproveErrorTag.Expired);
			return (ApproveError.ExpiredInfo)this.Value!;
		}

		public ApproveError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(ApproveErrorTag.GenericError);
			return (ApproveError.GenericErrorInfo)this.Value!;
		}

		public ApproveError.InsufficientFundsInfo AsInsufficientFunds()
		{
			this.ValidateTag(ApproveErrorTag.InsufficientFunds);
			return (ApproveError.InsufficientFundsInfo)this.Value!;
		}

		private void ValidateTag(ApproveErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class AllowanceChangedInfo
		{
			[CandidName("current_allowance")]
			public UnboundedUInt CurrentAllowance { get; set; }

			public AllowanceChangedInfo(UnboundedUInt currentAllowance)
			{
				this.CurrentAllowance = currentAllowance;
			}

			public AllowanceChangedInfo()
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

		public class ExpiredInfo
		{
			[CandidName("ledger_time")]
			public ulong LedgerTime { get; set; }

			public ExpiredInfo(ulong ledgerTime)
			{
				this.LedgerTime = ledgerTime;
			}

			public ExpiredInfo()
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

	public enum ApproveErrorTag
	{
		
		AllowanceChanged,
		
		BadFee,
		
		CreatedInFuture,
		
		Duplicate,
		
		Expired,
		
		GenericError,
		
		InsufficientFunds,
		TemporarilyUnavailable,
		TooOld
	}
}