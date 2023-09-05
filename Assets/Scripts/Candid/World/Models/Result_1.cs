using worldId = System.String;
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
	[Variant(typeof(Result_1Tag))]
	public class Result_1
	{
		[VariantTagProperty()]
		public Result_1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_1(Result_1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_1()
		{
		}

		public static Result_1 Err(Result_1.ErrInfo info)
		{
			return new Result_1(Result_1Tag.Err, info);
		}

		public static Result_1 Ok(TransferResult info)
		{
			return new Result_1(Result_1Tag.Ok, info);
		}

		public Result_1.ErrInfo AsErr()
		{
			this.ValidateTag(Result_1Tag.Err);
			return (Result_1.ErrInfo)this.Value!;
		}

		public TransferResult AsOk()
		{
			this.ValidateTag(Result_1Tag.Ok);
			return (TransferResult)this.Value!;
		}

		private void ValidateTag(Result_1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		[Variant(typeof(Result_1.ErrInfoTag))]
		public class ErrInfo
		{
			[VariantTagProperty()]
			public Result_1.ErrInfoTag Tag { get; set; }

			[VariantValueProperty()]
			public System.Object? Value { get; set; }

			public ErrInfo(Result_1.ErrInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected ErrInfo()
			{
			}

			public static Result_1.ErrInfo Err(string info)
			{
				return new Result_1.ErrInfo(Result_1.ErrInfoTag.Err, info);
			}

			public static Result_1.ErrInfo TxErr(TransferError__1 info)
			{
				return new Result_1.ErrInfo(Result_1.ErrInfoTag.TxErr, info);
			}

			public string AsErr()
			{
				this.ValidateTag(Result_1.ErrInfoTag.Err);
				return (string)this.Value!;
			}

			public TransferError__1 AsTxErr()
			{
				this.ValidateTag(Result_1.ErrInfoTag.TxErr);
				return (TransferError__1)this.Value!;
			}

			private void ValidateTag(Result_1.ErrInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}
		}

		public enum ErrInfoTag
		{
			[VariantOptionType(typeof(string))]
			Err,
			[VariantOptionType(typeof(TransferError__1))]
			TxErr
		}
	}

	public enum Result_1Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(Result_1.ErrInfo))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(TransferResult))]
		Ok
	}
}