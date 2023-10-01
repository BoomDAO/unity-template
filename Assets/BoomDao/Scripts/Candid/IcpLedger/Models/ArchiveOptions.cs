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
using EdjCase.ICP.Candid.Models;

namespace Candid.IcpLedger.Models
{
	public class ArchiveOptions
	{
		[CandidName("trigger_threshold")]
		public ulong TriggerThreshold { get; set; }

		[CandidName("num_blocks_to_archive")]
		public ulong NumBlocksToArchive { get; set; }

		[CandidName("node_max_memory_size_bytes")]
		public OptionalValue<ulong> NodeMaxMemorySizeBytes { get; set; }

		[CandidName("max_message_size_bytes")]
		public OptionalValue<ulong> MaxMessageSizeBytes { get; set; }

		[CandidName("controller_id")]
		public Principal ControllerId { get; set; }

		[CandidName("cycles_for_archive_creation")]
		public OptionalValue<ulong> CyclesForArchiveCreation { get; set; }

		public ArchiveOptions(ulong triggerThreshold, ulong numBlocksToArchive, OptionalValue<ulong> nodeMaxMemorySizeBytes, OptionalValue<ulong> maxMessageSizeBytes, Principal controllerId, OptionalValue<ulong> cyclesForArchiveCreation)
		{
			this.TriggerThreshold = triggerThreshold;
			this.NumBlocksToArchive = numBlocksToArchive;
			this.NodeMaxMemorySizeBytes = nodeMaxMemorySizeBytes;
			this.MaxMessageSizeBytes = maxMessageSizeBytes;
			this.ControllerId = controllerId;
			this.CyclesForArchiveCreation = cyclesForArchiveCreation;
		}

		public ArchiveOptions()
		{
		}
	}
}