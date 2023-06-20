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
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using Candid.IcpLedger.Models;

namespace Candid.IcpLedger.Models
{
	public class Transaction
	{
		[CandidName("memo")]
		public Memo Memo { get; set; }

		[CandidName("icrc1_memo")]
		public OptionalValue<List<byte>> Icrc1Memo { get; set; }

		[CandidName("operation")]
		public OptionalValue<Operation> Operation { get; set; }

		[CandidName("created_at_time")]
		public TimeStamp CreatedAtTime { get; set; }

		public Transaction(Memo memo, OptionalValue<List<byte>> icrc1Memo, OptionalValue<Operation> operation, TimeStamp createdAtTime)
		{
			this.Memo = memo;
			this.Icrc1Memo = icrc1Memo;
			this.Operation = operation;
			this.CreatedAtTime = createdAtTime;
		}

		public Transaction()
		{
		}
	}
}