using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;
using BlockIndex = System.UInt64;

namespace Candid.World.Models
{
	[Variant]
	public class Transfererror1
	{
		[VariantTagProperty]
		public Transfererror1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Transfererror1(Transfererror1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Transfererror1()
		{
		}

		public static Transfererror1 BadFee(Transfererror1.BadFeeInfo info)
		{
			return new Transfererror1(Transfererror1Tag.BadFee, info);
		}

		public static Transfererror1 InsufficientFunds(Transfererror1.InsufficientFundsInfo info)
		{
			return new Transfererror1(Transfererror1Tag.InsufficientFunds, info);
		}

		public static Transfererror1 TxCreatedInFuture()
		{
			return new Transfererror1(Transfererror1Tag.TxCreatedInFuture, null);
		}

		public static Transfererror1 TxDuplicate(Transfererror1.TxDuplicateInfo info)
		{
			return new Transfererror1(Transfererror1Tag.TxDuplicate, info);
		}

		public static Transfererror1 TxTooOld(Transfererror1.TxTooOldInfo info)
		{
			return new Transfererror1(Transfererror1Tag.TxTooOld, info);
		}

		public Transfererror1.BadFeeInfo AsBadFee()
		{
			this.ValidateTag(Transfererror1Tag.BadFee);
			return (Transfererror1.BadFeeInfo)this.Value!;
		}

		public Transfererror1.InsufficientFundsInfo AsInsufficientFunds()
		{
			this.ValidateTag(Transfererror1Tag.InsufficientFunds);
			return (Transfererror1.InsufficientFundsInfo)this.Value!;
		}

		public Transfererror1.TxDuplicateInfo AsTxDuplicate()
		{
			this.ValidateTag(Transfererror1Tag.TxDuplicate);
			return (Transfererror1.TxDuplicateInfo)this.Value!;
		}

		public Transfererror1.TxTooOldInfo AsTxTooOld()
		{
			this.ValidateTag(Transfererror1Tag.TxTooOld);
			return (Transfererror1.TxTooOldInfo)this.Value!;
		}

		private void ValidateTag(Transfererror1Tag tag)
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

	public enum Transfererror1Tag
	{
		BadFee,
		InsufficientFunds,
		TxCreatedInFuture,
		TxDuplicate,
		TxTooOld
	}
}