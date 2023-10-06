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
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.IcrcLedger.Models
{
	[Variant]
	public class ApproveResult
	{
		[VariantTagProperty()]
		public ApproveResultTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public ApproveResult(ApproveResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ApproveResult()
		{
		}

		public static ApproveResult Err(ApproveError info)
		{
			return new ApproveResult(ApproveResultTag.Err, info);
		}

		public static ApproveResult Ok(UnboundedUInt info)
		{
			return new ApproveResult(ApproveResultTag.Ok, info);
		}

		public ApproveError AsErr()
		{
			this.ValidateTag(ApproveResultTag.Err);
			return (ApproveError)this.Value!;
		}

		public UnboundedUInt AsOk()
		{
			this.ValidateTag(ApproveResultTag.Ok);
			return (UnboundedUInt)this.Value!;
		}

		private void ValidateTag(ApproveResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ApproveResultTag
	{
		
		Err,
		
		Ok
	}
}