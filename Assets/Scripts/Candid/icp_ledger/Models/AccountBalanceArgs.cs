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

namespace Candid.IcpLedger.Models
{
	public class AccountBalanceArgs
	{
		[CandidName("account")]
		public AccountIdentifier Account { get; set; }

		public AccountBalanceArgs(AccountIdentifier account)
		{
			this.Account = account;
		}

		public AccountBalanceArgs()
		{
		}
	}
}