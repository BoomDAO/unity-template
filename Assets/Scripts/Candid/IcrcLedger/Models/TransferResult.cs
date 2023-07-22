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

namespace Candid.IcrcLedger.Models
{
	[Variant(typeof(TransferResultTag))]
	public class TransferResult
	{
		[VariantTagProperty()]
		public TransferResultTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public TransferResult(TransferResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferResult()
		{
		}

		public static TransferResult Err(TransferError info)
		{
			return new TransferResult(TransferResultTag.Err, info);
		}

		public static TransferResult Ok(TxIndex info)
		{
			return new TransferResult(TransferResultTag.Ok, info);
		}

		public TransferError AsErr()
		{
			this.ValidateTag(TransferResultTag.Err);
			return (TransferError)this.Value!;
		}

		public TxIndex AsOk()
		{
			this.ValidateTag(TransferResultTag.Ok);
			return (TxIndex)this.Value!;
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
		[VariantOptionType(typeof(TransferError))]
		Err,
		[VariantOptionType(typeof(TxIndex))]
		Ok
	}
}