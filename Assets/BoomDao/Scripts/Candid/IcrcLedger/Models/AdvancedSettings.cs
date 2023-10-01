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

namespace Candid.IcrcLedger.Models
{
	public class AdvancedSettings
	{
		[CandidName("burned_tokens")]
		public Balance BurnedTokens { get; set; }

		[CandidName("permitted_drift")]
		public Timestamp PermittedDrift { get; set; }

		[CandidName("transaction_window")]
		public Timestamp TransactionWindow { get; set; }

		public AdvancedSettings(Balance burnedTokens, Timestamp permittedDrift, Timestamp transactionWindow)
		{
			this.BurnedTokens = burnedTokens;
			this.PermittedDrift = permittedDrift;
			this.TransactionWindow = transactionWindow;
		}

		public AdvancedSettings()
		{
		}
	}
}