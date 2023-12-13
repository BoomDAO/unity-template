using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class BalanceResponse
	{
		[VariantTagProperty]
		public BalanceResponseTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public BalanceResponse(BalanceResponseTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected BalanceResponse()
		{
		}

		public static BalanceResponse Err(Commonerror1 info)
		{
			return new BalanceResponse(BalanceResponseTag.Err, info);
		}

		public static BalanceResponse Ok(Balance info)
		{
			return new BalanceResponse(BalanceResponseTag.Ok, info);
		}

		public Commonerror1 AsErr()
		{
			this.ValidateTag(BalanceResponseTag.Err);
			return (Commonerror1)this.Value!;
		}

		public Balance AsOk()
		{
			this.ValidateTag(BalanceResponseTag.Ok);
			return (Balance)this.Value!;
		}

		private void ValidateTag(BalanceResponseTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum BalanceResponseTag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}