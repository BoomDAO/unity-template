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
	[Variant(typeof(Icrc1TransferErrorTag))]
	public class Icrc1TransferError
	{
		[VariantTagProperty()]
		public Icrc1TransferErrorTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Icrc1TransferError(Icrc1TransferErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Icrc1TransferError()
		{
		}

		public static Icrc1TransferError BadFee(Icrc1TransferError.BadFeeInfo info)
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.BadFee, info);
		}

		public static Icrc1TransferError BadBurn(Icrc1TransferError.BadBurnInfo info)
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.BadBurn, info);
		}

		public static Icrc1TransferError InsufficientFunds(Icrc1TransferError.InsufficientFundsInfo info)
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.InsufficientFunds, info);
		}

		public static Icrc1TransferError TooOld()
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.TooOld, null);
		}

		public static Icrc1TransferError CreatedInFuture(Icrc1TransferError.CreatedInFutureInfo info)
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.CreatedInFuture, info);
		}

		public static Icrc1TransferError TemporarilyUnavailable()
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.TemporarilyUnavailable, null);
		}

		public static Icrc1TransferError Duplicate(Icrc1TransferError.DuplicateInfo info)
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.Duplicate, info);
		}

		public static Icrc1TransferError GenericError(Icrc1TransferError.GenericErrorInfo info)
		{
			return new Icrc1TransferError(Icrc1TransferErrorTag.GenericError, info);
		}

		public Icrc1TransferError.BadFeeInfo AsBadFee()
		{
			this.ValidateTag(Icrc1TransferErrorTag.BadFee);
			return (Icrc1TransferError.BadFeeInfo)this.Value!;
		}

		public Icrc1TransferError.BadBurnInfo AsBadBurn()
		{
			this.ValidateTag(Icrc1TransferErrorTag.BadBurn);
			return (Icrc1TransferError.BadBurnInfo)this.Value!;
		}

		public Icrc1TransferError.InsufficientFundsInfo AsInsufficientFunds()
		{
			this.ValidateTag(Icrc1TransferErrorTag.InsufficientFunds);
			return (Icrc1TransferError.InsufficientFundsInfo)this.Value!;
		}

		public Icrc1TransferError.CreatedInFutureInfo AsCreatedInFuture()
		{
			this.ValidateTag(Icrc1TransferErrorTag.CreatedInFuture);
			return (Icrc1TransferError.CreatedInFutureInfo)this.Value!;
		}

		public Icrc1TransferError.DuplicateInfo AsDuplicate()
		{
			this.ValidateTag(Icrc1TransferErrorTag.Duplicate);
			return (Icrc1TransferError.DuplicateInfo)this.Value!;
		}

		public Icrc1TransferError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(Icrc1TransferErrorTag.GenericError);
			return (Icrc1TransferError.GenericErrorInfo)this.Value!;
		}

		private void ValidateTag(Icrc1TransferErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class BadFeeInfo
		{
			[CandidName("expected_fee")]
			public Icrc1Tokens ExpectedFee { get; set; }

			public BadFeeInfo(Icrc1Tokens expectedFee)
			{
				this.ExpectedFee = expectedFee;
			}

			public BadFeeInfo()
			{
			}
		}

		public class BadBurnInfo
		{
			[CandidName("min_burn_amount")]
			public Icrc1Tokens MinBurnAmount { get; set; }

			public BadBurnInfo(Icrc1Tokens minBurnAmount)
			{
				this.MinBurnAmount = minBurnAmount;
			}

			public BadBurnInfo()
			{
			}
		}

		public class InsufficientFundsInfo
		{
			[CandidName("balance")]
			public Icrc1Tokens Balance { get; set; }

			public InsufficientFundsInfo(Icrc1Tokens balance)
			{
				this.Balance = balance;
			}

			public InsufficientFundsInfo()
			{
			}
		}

		public class CreatedInFutureInfo
		{
			[CandidName("ledger_time")]
			public ulong LedgerTime { get; set; }

			public CreatedInFutureInfo(ulong ledgerTime)
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
			public Icrc1BlockIndex DuplicateOf { get; set; }

			public DuplicateInfo(Icrc1BlockIndex duplicateOf)
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
	}

	public enum Icrc1TransferErrorTag
	{
		[VariantOptionType(typeof(Icrc1TransferError.BadFeeInfo))]
		BadFee,
		[VariantOptionType(typeof(Icrc1TransferError.BadBurnInfo))]
		BadBurn,
		[VariantOptionType(typeof(Icrc1TransferError.InsufficientFundsInfo))]
		InsufficientFunds,
		TooOld,
		[VariantOptionType(typeof(Icrc1TransferError.CreatedInFutureInfo))]
		CreatedInFuture,
		TemporarilyUnavailable,
		[VariantOptionType(typeof(Icrc1TransferError.DuplicateInfo))]
		Duplicate,
		[VariantOptionType(typeof(Icrc1TransferError.GenericErrorInfo))]
		GenericError
	}
}