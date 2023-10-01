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
	public class Block
	{
		[CandidName("parent_hash")]
		public OptionalValue<List<byte>> ParentHash { get; set; }

		[CandidName("transaction")]
		public Transaction Transaction { get; set; }

		[CandidName("timestamp")]
		public TimeStamp Timestamp { get; set; }

		public Block(OptionalValue<List<byte>> parentHash, Transaction transaction, TimeStamp timestamp)
		{
			this.ParentHash = parentHash;
			this.Transaction = transaction;
			this.Timestamp = timestamp;
		}

		public Block()
		{
		}
	}
}