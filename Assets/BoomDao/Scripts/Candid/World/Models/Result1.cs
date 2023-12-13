using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class Result1
	{
		[VariantTagProperty]
		public Result1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result1(Result1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result1()
		{
		}

		public static Result1 Err(Result1.ErrInfo info)
		{
			return new Result1(Result1Tag.Err, info);
		}

		public static Result1 Ok(TransferResult info)
		{
			return new Result1(Result1Tag.Ok, info);
		}

		public Result1.ErrInfo AsErr()
		{
			this.ValidateTag(Result1Tag.Err);
			return (Result1.ErrInfo)this.Value!;
		}

		public TransferResult AsOk()
		{
			this.ValidateTag(Result1Tag.Ok);
			return (TransferResult)this.Value!;
		}

		private void ValidateTag(Result1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		[Variant]
		public class ErrInfo
		{
			[VariantTagProperty]
			public Result1.ErrInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public ErrInfo(Result1.ErrInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected ErrInfo()
			{
			}

			public static Result1.ErrInfo Err(string info)
			{
				return new Result1.ErrInfo(Result1.ErrInfoTag.Err, info);
			}

			public static Result1.ErrInfo TxErr(Transfererror1 info)
			{
				return new Result1.ErrInfo(Result1.ErrInfoTag.TxErr, info);
			}

			public string AsErr()
			{
				this.ValidateTag(Result1.ErrInfoTag.Err);
				return (string)this.Value!;
			}

			public Transfererror1 AsTxErr()
			{
				this.ValidateTag(Result1.ErrInfoTag.TxErr);
				return (Transfererror1)this.Value!;
			}

			private void ValidateTag(Result1.ErrInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}
		}

		public enum ErrInfoTag
		{
			Err,
			TxErr
		}
	}

	public enum Result1Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}