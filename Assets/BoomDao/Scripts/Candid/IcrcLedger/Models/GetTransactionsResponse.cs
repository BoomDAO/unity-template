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
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.IcrcLedger.Models
{
	public class GetTransactionsResponse
	{
		[CandidName("archived_transactions")]
		public List<ArchivedTransaction> ArchivedTransactions { get; set; }

		[CandidName("first_index")]
		public TxIndex FirstIndex { get; set; }

		[CandidName("log_length")]
		public UnboundedUInt LogLength { get; set; }

		[CandidName("transactions")]
		public List<Transaction> Transactions { get; set; }

		public GetTransactionsResponse(List<ArchivedTransaction> archivedTransactions, TxIndex firstIndex, UnboundedUInt logLength, List<Transaction> transactions)
		{
			this.ArchivedTransactions = archivedTransactions;
			this.FirstIndex = firstIndex;
			this.LogLength = logLength;
			this.Transactions = transactions;
		}

		public GetTransactionsResponse()
		{
		}
	}
}