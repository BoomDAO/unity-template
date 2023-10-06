using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class Result
	{
		[VariantTagProperty()]
		public ResultTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result(ResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result()
		{
		}

		public static Result Err(Result.ErrInfo info)
		{
			return new Result(ResultTag.Err, info);
		}

		public static Result Ok(Result__1 info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public Result.ErrInfo AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (Result.ErrInfo)this.Value!;
		}

		public Result__1 AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (Result__1)this.Value!;
		}

		private void ValidateTag(ResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		[Variant]
		public class ErrInfo
		{
			[VariantTagProperty()]
			public Result.ErrInfoTag Tag { get; set; }

			[VariantValueProperty()]
			public System.Object? Value { get; set; }

			public ErrInfo(Result.ErrInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected ErrInfo()
			{
			}

			public static Result.ErrInfo Err(string info)
			{
				return new Result.ErrInfo(Result.ErrInfoTag.Err, info);
			}

			public static Result.ErrInfo TxErr(TransferError info)
			{
				return new Result.ErrInfo(Result.ErrInfoTag.TxErr, info);
			}

			public string AsErr()
			{
				this.ValidateTag(Result.ErrInfoTag.Err);
				return (string)this.Value!;
			}

			public TransferError AsTxErr()
			{
				this.ValidateTag(Result.ErrInfoTag.TxErr);
				return (TransferError)this.Value!;
			}

			private void ValidateTag(Result.ErrInfoTag tag)
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

	public enum ResultTag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}