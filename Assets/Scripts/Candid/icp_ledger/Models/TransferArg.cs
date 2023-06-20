using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using BlockIndex = System.UInt64;
using Memo = System.UInt64;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using TextAccountIdentifier = System.String;
using Icrc1BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Icrc1Timestamp = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.IcpLedger.Models;
using System.Collections.Generic;

namespace Candid.IcpLedger.Models
{
	public class TransferArg
	{
		[CandidName("from_subaccount")]
		public OptionalValue<SubAccount> FromSubaccount { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		[CandidName("amount")]
		public Icrc1Tokens Amount { get; set; }

		[CandidName("fee")]
		public OptionalValue<Icrc1Tokens> Fee { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<Icrc1Timestamp> CreatedAtTime { get; set; }

		public TransferArg(OptionalValue<SubAccount> fromSubaccount, Account to, Icrc1Tokens amount, OptionalValue<Icrc1Tokens> fee, OptionalValue<List<byte>> memo, OptionalValue<Icrc1Timestamp> createdAtTime)
		{
			this.FromSubaccount = fromSubaccount;
			this.To = to;
			this.Amount = amount;
			this.Fee = fee;
			this.Memo = memo;
			this.CreatedAtTime = createdAtTime;
		}

		public TransferArg()
		{
		}
	}
}