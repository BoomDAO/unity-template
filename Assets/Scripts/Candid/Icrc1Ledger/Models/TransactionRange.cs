using BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using Timestamp = System.UInt64;
using Duration = System.UInt64;
using Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using Map = System.Collections.Generic.List<Candid.Icrc1Ledger.Models.MapItem>;
using Block = Candid.Icrc1Ledger.Models.Value;
using QueryBlockArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using EdjCase.ICP.Candid.Mapping;
using Candid.Icrc1Ledger.Models;
using System.Collections.Generic;

namespace Candid.Icrc1Ledger.Models
{
	public class TransactionRange
	{
		[CandidName("transactions")]
		public List<Transaction> Transactions { get; set; }

		public TransactionRange(List<Transaction> transactions)
		{
			this.Transactions = transactions;
		}

		public TransactionRange()
		{
		}
	}
}