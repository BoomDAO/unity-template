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
	[Variant(typeof(CanisterLogResponseTag))]
	public class CanisterLogResponse
	{
		[VariantTagProperty()]
		public CanisterLogResponseTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public CanisterLogResponse(CanisterLogResponseTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterLogResponse()
		{
		}

		public static CanisterLogResponse Messages(CanisterLogMessages info)
		{
			return new CanisterLogResponse(CanisterLogResponseTag.Messages, info);
		}

		public static CanisterLogResponse MessagesInfo(CanisterLogMessagesInfo info)
		{
			return new CanisterLogResponse(CanisterLogResponseTag.MessagesInfo, info);
		}

		public CanisterLogMessages AsMessages()
		{
			this.ValidateTag(CanisterLogResponseTag.Messages);
			return (CanisterLogMessages)this.Value!;
		}

		public CanisterLogMessagesInfo AsMessagesInfo()
		{
			this.ValidateTag(CanisterLogResponseTag.MessagesInfo);
			return (CanisterLogMessagesInfo)this.Value!;
		}

		private void ValidateTag(CanisterLogResponseTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterLogResponseTag
	{
		[CandidName("messages")]
		[VariantOptionType(typeof(CanisterLogMessages))]
		Messages,
		[CandidName("messagesInfo")]
		[VariantOptionType(typeof(CanisterLogMessagesInfo))]
		MessagesInfo
	}
}