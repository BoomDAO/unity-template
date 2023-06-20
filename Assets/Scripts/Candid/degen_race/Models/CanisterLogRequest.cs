using UpdateCallsAggregatedData = System.Collections.Generic.List<System.UInt64>;
using Nanos = System.UInt64;
using CanisterMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterHeapMemoryAggregatedData = System.Collections.Generic.List<System.UInt64>;
using CanisterCyclesAggregatedData = System.Collections.Generic.List<System.UInt64>;
using EdjCase.ICP.Candid.Mapping;
using Candid.degen_race.Models;
using System;

namespace Candid.degen_race.Models
{
	[Variant(typeof(CanisterLogRequestTag))]
	public class CanisterLogRequest
	{
		[VariantTagProperty()]
		public CanisterLogRequestTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public CanisterLogRequest(CanisterLogRequestTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterLogRequest()
		{
		}

		public static CanisterLogRequest GetLatestMessages(GetLatestLogMessagesParameters info)
		{
			return new CanisterLogRequest(CanisterLogRequestTag.GetLatestMessages, info);
		}

		public static CanisterLogRequest GetMessages(GetLogMessagesParameters info)
		{
			return new CanisterLogRequest(CanisterLogRequestTag.GetMessages, info);
		}

		public static CanisterLogRequest GetMessagesInfo()
		{
			return new CanisterLogRequest(CanisterLogRequestTag.GetMessagesInfo, null);
		}

		public GetLatestLogMessagesParameters AsGetLatestMessages()
		{
			this.ValidateTag(CanisterLogRequestTag.GetLatestMessages);
			return (GetLatestLogMessagesParameters)this.Value!;
		}

		public GetLogMessagesParameters AsGetMessages()
		{
			this.ValidateTag(CanisterLogRequestTag.GetMessages);
			return (GetLogMessagesParameters)this.Value!;
		}

		private void ValidateTag(CanisterLogRequestTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterLogRequestTag
	{
		[CandidName("getLatestMessages")]
		[VariantOptionType(typeof(GetLatestLogMessagesParameters))]
		GetLatestMessages,
		[CandidName("getMessages")]
		[VariantOptionType(typeof(GetLogMessagesParameters))]
		GetMessages,
		[CandidName("getMessagesInfo")]
		GetMessagesInfo
	}
}