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
	public class MetaDatum
	{
		[CandidTag(0U)]
		public string F0 { get; set; }

		[CandidTag(1U)]
		public Value F1 { get; set; }

		public MetaDatum(string f0, Value f1)
		{
			this.F0 = f0;
			this.F1 = f1;
		}

		public MetaDatum()
		{
		}
	}
}