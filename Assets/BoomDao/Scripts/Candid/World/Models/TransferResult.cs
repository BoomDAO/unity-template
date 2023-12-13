using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;
using BlockIndex = System.UInt64;

namespace Candid.World.Models
{
	[Variant]
	public class TransferResult
	{
		[VariantTagProperty]
		public TransferResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public TransferResult(TransferResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferResult()
		{
		}

		public static TransferResult Err(Transfererror1 info)
		{
			return new TransferResult(TransferResultTag.Err, info);
		}

		public static TransferResult Ok(BlockIndex info)
		{
			return new TransferResult(TransferResultTag.Ok, info);
		}

		public Transfererror1 AsErr()
		{
			this.ValidateTag(TransferResultTag.Err);
			return (Transfererror1)this.Value!;
		}

		public BlockIndex AsOk()
		{
			this.ValidateTag(TransferResultTag.Ok);
			return (BlockIndex)this.Value!;
		}

		private void ValidateTag(TransferResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum TransferResultTag
	{
		Err,
		Ok
	}
}