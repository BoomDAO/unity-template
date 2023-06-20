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

namespace Candid.IcpLedger.Models
{
	public class TransferFee
	{
		[CandidName("transfer_fee")]
		public Tokens TransferFee_ { get; set; }

		public TransferFee(Tokens transferFee)
		{
			this.TransferFee_ = transferFee;
		}

		public TransferFee()
		{
		}
	}
}