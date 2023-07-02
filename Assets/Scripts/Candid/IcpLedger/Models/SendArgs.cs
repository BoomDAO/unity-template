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
using Candid.IcpLedger.Models;
using EdjCase.ICP.Candid.Models;

namespace Candid.IcpLedger.Models
{
	public class SendArgs
	{
		[CandidName("memo")]
		public Memo Memo { get; set; }

		[CandidName("amount")]
		public Tokens Amount { get; set; }

		[CandidName("fee")]
		public Tokens Fee { get; set; }

		[CandidName("from_subaccount")]
		public OptionalValue<SubAccount> FromSubaccount { get; set; }

		[CandidName("to")]
		public TextAccountIdentifier To { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<TimeStamp> CreatedAtTime { get; set; }

		public SendArgs(Memo memo, Tokens amount, Tokens fee, OptionalValue<SubAccount> fromSubaccount, TextAccountIdentifier to, OptionalValue<TimeStamp> createdAtTime)
		{
			this.Memo = memo;
			this.Amount = amount;
			this.Fee = fee;
			this.FromSubaccount = fromSubaccount;
			this.To = to;
			this.CreatedAtTime = createdAtTime;
		}

		public SendArgs()
		{
		}
	}
}