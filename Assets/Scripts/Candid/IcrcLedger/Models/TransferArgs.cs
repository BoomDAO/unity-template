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
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class TransferArgs
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("fee")]
		public OptionalValue<Balance> Fee { get; set; }

		[CandidName("from_subaccount")]
		public OptionalValue<Subaccount> FromSubaccount { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		public TransferArgs(Balance amount, OptionalValue<ulong> createdAtTime, OptionalValue<Balance> fee, OptionalValue<Subaccount> fromSubaccount, OptionalValue<List<byte>> memo, Account to)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.Fee = fee;
			this.FromSubaccount = fromSubaccount;
			this.Memo = memo;
			this.To = to;
		}

		public TransferArgs()
		{
		}
	}
}