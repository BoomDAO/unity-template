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
	[Variant]
	public class TransferFromResult
	{
		[VariantTagProperty()]
		public TransferFromResultTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public TransferFromResult(TransferFromResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferFromResult()
		{
		}

		public static TransferFromResult Err(TransferFromError info)
		{
			return new TransferFromResult(TransferFromResultTag.Err, info);
		}

		public static TransferFromResult Ok(TxIndex__1 info)
		{
			return new TransferFromResult(TransferFromResultTag.Ok, info);
		}

		public TransferFromError AsErr()
		{
			this.ValidateTag(TransferFromResultTag.Err);
			return (TransferFromError)this.Value!;
		}

		public TxIndex__1 AsOk()
		{
			this.ValidateTag(TransferFromResultTag.Ok);
			return (TxIndex__1)this.Value!;
		}

		private void ValidateTag(TransferFromResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum TransferFromResultTag
	{
		
		Err,
		
		Ok
	}
}