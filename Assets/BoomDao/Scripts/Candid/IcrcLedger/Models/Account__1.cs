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
using EdjCase.ICP.Candid.Models;

namespace Candid.IcrcLedger.Models
{
	public class Account__1
	{
		[CandidName("owner")]
		public Principal Owner { get; set; }

		[CandidName("subaccount")]
		public OptionalValue<Subaccount> Subaccount { get; set; }

		public Account__1(Principal owner, OptionalValue<Subaccount> subaccount)
		{
			this.Owner = owner;
			this.Subaccount = subaccount;
		}

		public Account__1()
		{
		}
	}
}