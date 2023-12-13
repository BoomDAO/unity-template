using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class CanisterWsGetMessagesResult
	{
		[VariantTagProperty]
		public CanisterWsGetMessagesResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public CanisterWsGetMessagesResult(CanisterWsGetMessagesResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterWsGetMessagesResult()
		{
		}

		public static CanisterWsGetMessagesResult Err(string info)
		{
			return new CanisterWsGetMessagesResult(CanisterWsGetMessagesResultTag.Err, info);
		}

		public static CanisterWsGetMessagesResult Ok(CanisterOutputCertifiedMessages info)
		{
			return new CanisterWsGetMessagesResult(CanisterWsGetMessagesResultTag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(CanisterWsGetMessagesResultTag.Err);
			return (string)this.Value!;
		}

		public CanisterOutputCertifiedMessages AsOk()
		{
			this.ValidateTag(CanisterWsGetMessagesResultTag.Ok);
			return (CanisterOutputCertifiedMessages)this.Value!;
		}

		private void ValidateTag(CanisterWsGetMessagesResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterWsGetMessagesResultTag
	{
		Err,
		Ok
	}
}