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
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class TransferFromArgs
	{
		[CandidName("amount")]
		public Balance__1 Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("fee")]
		public OptionalValue<Balance__1> Fee { get; set; }

		[CandidName("from")]
		public Account__1 From { get; set; }

		[CandidName("memo")]
		public OptionalValue<Memo> Memo { get; set; }

		[CandidName("spender_subaccount")]
		public OptionalValue<Subaccount__1> SpenderSubaccount { get; set; }

		[CandidName("to")]
		public Account__1 To { get; set; }

		public TransferFromArgs(Balance__1 amount, OptionalValue<ulong> createdAtTime, OptionalValue<Balance__1> fee, Account__1 from, OptionalValue<Memo> memo, OptionalValue<Subaccount__1> spenderSubaccount, Account__1 to)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.Fee = fee;
			this.From = from;
			this.Memo = memo;
			this.SpenderSubaccount = spenderSubaccount;
			this.To = to;
		}

		public TransferFromArgs()
		{
		}
	}
}