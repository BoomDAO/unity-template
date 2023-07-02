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
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using Candid.Icrc1Ledger.Models;

namespace Candid.Icrc1Ledger.Models
{
	public class GetBlocksResponse
	{
		[CandidName("first_index")]
		public BlockIndex FirstIndex { get; set; }

		[CandidName("chain_length")]
		public ulong ChainLength { get; set; }

		[CandidName("certificate")]
		public OptionalValue<List<byte>> Certificate { get; set; }

		[CandidName("blocks")]
		public List<Block> Blocks { get; set; }

		[CandidName("archived_blocks")]
		public List<GetBlocksResponse.ArchivedBlocksItem> ArchivedBlocks { get; set; }

		public GetBlocksResponse(BlockIndex firstIndex, ulong chainLength, OptionalValue<List<byte>> certificate, List<Block> blocks, List<GetBlocksResponse.ArchivedBlocksItem> archivedBlocks)
		{
			this.FirstIndex = firstIndex;
			this.ChainLength = chainLength;
			this.Certificate = certificate;
			this.Blocks = blocks;
			this.ArchivedBlocks = archivedBlocks;
		}

		public GetBlocksResponse()
		{
		}

		public class ArchivedBlocksItem
		{
			[CandidName("start")]
			public BlockIndex Start { get; set; }

			[CandidName("length")]
			public UnboundedUInt Length { get; set; }

			[CandidName("callback")]
			public QueryBlockArchiveFn Callback { get; set; }

			public ArchivedBlocksItem(BlockIndex start, UnboundedUInt length, QueryBlockArchiveFn callback)
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