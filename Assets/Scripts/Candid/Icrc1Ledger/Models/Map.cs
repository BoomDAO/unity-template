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

namespace Candid.Icrc1Ledger.Models
{
	public class MapItem
	{
		[CandidTag(0U)]
		public string F0 { get; set; }

		[CandidTag(1U)]
		public Value F1 { get; set; }

		public MapItem(string f0, Value f1)
		{
			this.F0 = f0;
			this.F1 = f1;
		}

		public MapItem()
		{
		}
	}
}