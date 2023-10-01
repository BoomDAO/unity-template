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

namespace Candid.IcrcLedger.Models
{
	public class AllowanceArgs
	{
		[CandidName("account")]
		public Account__1 Account { get; set; }

		[CandidName("spender")]
		public Account__1 Spender { get; set; }

		public AllowanceArgs(Account__1 account, Account__1 spender)
		{
			this.Account = account;
			this.Spender = spender;
		}

		public AllowanceArgs()
		{
		}
	}
}