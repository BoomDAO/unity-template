using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class SaleRemaining
	{
		[VariantTagProperty]
		public SaleRemainingTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public SaleRemaining(SaleRemainingTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected SaleRemaining()
		{
		}

		public static SaleRemaining Burn()
		{
			return new SaleRemaining(SaleRemainingTag.Burn, null);
		}

		public static SaleRemaining Retain()
		{
			return new SaleRemaining(SaleRemainingTag.Retain, null);
		}

		public static SaleRemaining Send(Accountidentifier1 info)
		{
			return new SaleRemaining(SaleRemainingTag.Send, info);
		}

		public Accountidentifier1 AsSend()
		{
			this.ValidateTag(SaleRemainingTag.Send);
			return (Accountidentifier1)this.Value!;
		}

		private void ValidateTag(SaleRemainingTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum SaleRemainingTag
	{
		[CandidName("burn")]
		Burn,
		[CandidName("retain")]
		Retain,
		[CandidName("send")]
		Send
	}
}