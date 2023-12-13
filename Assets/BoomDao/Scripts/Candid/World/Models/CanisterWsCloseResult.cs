using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class CanisterWsCloseResult
	{
		[VariantTagProperty]
		public CanisterWsCloseResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public CanisterWsCloseResult(CanisterWsCloseResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterWsCloseResult()
		{
		}

		public static CanisterWsCloseResult Err(string info)
		{
			return new CanisterWsCloseResult(CanisterWsCloseResultTag.Err, info);
		}

		public static CanisterWsCloseResult Ok()
		{
			return new CanisterWsCloseResult(CanisterWsCloseResultTag.Ok, null);
		}

		public string AsErr()
		{
			this.ValidateTag(CanisterWsCloseResultTag.Err);
			return (string)this.Value!;
		}

		private void ValidateTag(CanisterWsCloseResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterWsCloseResultTag
	{
		Err,
		Ok
	}
}