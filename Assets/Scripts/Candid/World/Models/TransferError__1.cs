using worldId = System.String;
using userId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant(typeof(TransferError__1Tag))]
	public class TransferError__1
	{
		[VariantTagProperty()]
		public TransferError__1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public TransferError__1(TransferError__1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferError__1()
		{
		}

		public static TransferError__1 BadFee(TransferError__1.BadFeeInfo info)
		{
			return new TransferError__1(TransferError__1Tag.BadFee, info);
		}

		public static TransferError__1 InsufficientFunds(TransferError__1.InsufficientFundsInfo info)
		{
			return new TransferError__1(TransferError__1Tag.InsufficientFunds, info);
		}

		public static TransferError__1 TxCreatedInFuture()
		{
			return new TransferError__1(TransferError__1Tag.TxCreatedInFuture, null);
		}

		public static TransferError__1 TxDuplicate(TransferError__1.TxDuplicateInfo info)
		{
			return new TransferError__1(TransferError__1Tag.TxDuplicate, info);
		}

		public static TransferError__1 TxTooOld(TransferError__1.TxTooOldInfo info)
		{
			return new TransferError__1(TransferError__1Tag.TxTooOld, info);
		}

		public TransferError__1.BadFeeInfo AsBadFee()
		{
			this.ValidateTag(TransferError__1Tag.BadFee);
			return (TransferError__1.BadFeeInfo)this.Value!;
		}

		public TransferError__1.InsufficientFundsInfo AsInsufficientFunds()
		{
			this.ValidateTag(TransferError__1Tag.InsufficientFunds);
			return (TransferError__1.InsufficientFundsInfo)this.Value!;
		}

		public TransferError__1.TxDuplicateInfo AsTxDuplicate()
		{
			this.ValidateTag(TransferError__1Tag.TxDuplicate);
			return (TransferError__1.TxDuplicateInfo)this.Value!;
		}

		public TransferError__1.TxTooOldInfo AsTxTooOld()
		{
			this.ValidateTag(TransferError__1Tag.TxTooOld);
			return (TransferError__1.TxTooOldInfo)this.Value!;
		}

		private void ValidateTag(TransferError__1Tag tag)
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
	}

	public enum TransferError__1Tag
	{
		[VariantOptionType(typeof(TransferError__1.BadFeeInfo))]
		BadFee,
		[VariantOptionType(typeof(TransferError__1.InsufficientFundsInfo))]
		InsufficientFunds,
		TxCreatedInFuture,
		[VariantOptionType(typeof(TransferError__1.TxDuplicateInfo))]
		TxDuplicate,
		[VariantOptionType(typeof(TransferError__1.TxTooOldInfo))]
		TxTooOld
	}
}