using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class CanisterWsOpenResult
	{
		[VariantTagProperty]
		public CanisterWsOpenResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public CanisterWsOpenResult(CanisterWsOpenResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterWsOpenResult()
		{
		}

		public static CanisterWsOpenResult Err(string info)
		{
			return new CanisterWsOpenResult(CanisterWsOpenResultTag.Err, info);
		}

		public static CanisterWsOpenResult Ok()
		{
			return new CanisterWsOpenResult(CanisterWsOpenResultTag.Ok, null);
		}

		public string AsErr()
		{
			this.ValidateTag(CanisterWsOpenResultTag.Err);
			return (string)this.Value!;
		}

		private void ValidateTag(CanisterWsOpenResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterWsOpenResultTag
	{
		Err,
		Ok
	}
}