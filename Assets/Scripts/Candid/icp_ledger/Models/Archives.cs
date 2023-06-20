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
using System.Collections.Generic;

namespace Candid.IcpLedger.Models
{
	public class Archives
	{
		[CandidName("archives")]
		public List<Archive> Archives_ { get; set; }

		public Archives(List<Archive> archives)
		{
			this.Archives_ = archives;
		}

		public Archives()
		{
		}
	}
}