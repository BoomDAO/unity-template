using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using TokenIdentifier = System.String;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AccountIdentifier = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class TransferResponse
	{
		[VariantTagProperty]
		public TransferResponseTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public TransferResponse(TransferResponseTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferResponse()
		{
		}

		public static TransferResponse Err(TransferResponse.ErrInfo info)
		{
			return new TransferResponse(TransferResponseTag.Err, info);
		}

		public static TransferResponse Ok(Balance info)
		{
			return new TransferResponse(TransferResponseTag.Ok, info);
		}

		public TransferResponse.ErrInfo AsErr()
		{
			this.ValidateTag(TransferResponseTag.Err);
			return (TransferResponse.ErrInfo)this.Value!;
		}

		public Balance AsOk()
		{
			this.ValidateTag(TransferResponseTag.Ok);
			return (Balance)this.Value!;
		}

		private void ValidateTag(TransferResponseTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		[Variant]
		public class ErrInfo
		{
			[VariantTagProperty]
			public TransferResponse.ErrInfoTag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public ErrInfo(TransferResponse.ErrInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected ErrInfo()
			{
			}

			public static TransferResponse.ErrInfo CannotNotify(AccountIdentifier info)
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.CannotNotify, info);
			}

			public static TransferResponse.ErrInfo InsufficientBalance()
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.InsufficientBalance, null);
			}

			public static TransferResponse.ErrInfo InvalidToken(TokenIdentifier info)
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.InvalidToken, info);
			}

			public static TransferResponse.ErrInfo Other(string info)
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.Other, info);
			}

			public static TransferResponse.ErrInfo Rejected()
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.Rejected, null);
			}

			public static TransferResponse.ErrInfo Unauthorized(AccountIdentifier info)
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.Unauthorized, info);
			}

			public AccountIdentifier AsCannotNotify()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.CannotNotify);
				return (AccountIdentifier)this.Value!;
			}

			public TokenIdentifier AsInvalidToken()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.InvalidToken);
				return (TokenIdentifier)this.Value!;
			}

			public string AsOther()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.Other);
				return (string)this.Value!;
			}

			public AccountIdentifier AsUnauthorized()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.Unauthorized);
				return (AccountIdentifier)this.Value!;
			}

			private void ValidateTag(TransferResponse.ErrInfoTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}
		}

		public enum ErrInfoTag
		{
			CannotNotify,
			InsufficientBalance,
			InvalidToken,
			Other,
			Rejected,
			Unauthorized
		}
	}

	public enum TransferResponseTag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}