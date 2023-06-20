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
	public class QueryBlocksResponse
	{
		[CandidName("chain_length")]
		public ulong ChainLength { get; set; }

		[CandidName("certificate")]
		public OptionalValue<List<byte>> Certificate { get; set; }

		[CandidName("blocks")]
		public List<Block> Blocks { get; set; }

		[CandidName("first_block_index")]
		public BlockIndex FirstBlockIndex { get; set; }

		[CandidName("archived_blocks")]
		public List<QueryBlocksResponse.ArchivedBlocksItem> ArchivedBlocks { get; set; }

		public QueryBlocksResponse(ulong chainLength, OptionalValue<List<byte>> certificate, List<Block> blocks, BlockIndex firstBlockIndex, List<QueryBlocksResponse.ArchivedBlocksItem> archivedBlocks)
		{
			this.ChainLength = chainLength;
			this.Certificate = certificate;
			this.Blocks = blocks;
			this.FirstBlockIndex = firstBlockIndex;
			this.ArchivedBlocks = archivedBlocks;
		}

		public QueryBlocksResponse()
		{
		}

		public class ArchivedBlocksItem
		{
			[CandidName("start")]
			public BlockIndex Start { get; set; }

			[CandidName("length")]
			public ulong Length { get; set; }

			[CandidName("callback")]
			public QueryArchiveFn Callback { get; set; }

			public ArchivedBlocksItem(BlockIndex start, ulong length, QueryArchiveFn callback)
			{
				this.Start = start;
				this.Length = length;
				this.Callback = callback;
			}

			public ArchivedBlocksItem()
			{
			}
		}
	}
}