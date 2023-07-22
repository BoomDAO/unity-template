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

namespace Candid.IcrcLedger.Models
{
	public class Transaction
	{
		[CandidName("burn")]
		public OptionalValue<Burn> Burn { get; set; }

		[CandidName("index")]
		public TxIndex Index { get; set; }

		[CandidName("kind")]
		public string Kind { get; set; }

		[CandidName("mint")]
		public OptionalValue<Mint> Mint { get; set; }

		[CandidName("timestamp")]
		public Timestamp Timestamp { get; set; }

		[CandidName("transfer")]
		public OptionalValue<Transfer> Transfer { get; set; }

		public Transaction(OptionalValue<Burn> burn, TxIndex index, string kind, OptionalValue<Mint> mint, Timestamp timestamp, OptionalValue<Transfer> transfer)
		{
			this.Burn = burn;
			this.Index = index;
			this.Kind = kind;
			this.Mint = mint;
			this.Timestamp = timestamp;
			this.Transfer = transfer;
		}

		public Transaction()
		{
		}
	}
}