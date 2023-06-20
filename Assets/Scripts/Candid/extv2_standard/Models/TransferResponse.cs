using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.ext_v2_standard.Models.MetadataValueItem>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.ext_v2_standard.Models;
using System;

namespace Candid.ext_v2_standard.Models
{
	[Variant(typeof(TransferResponseTag))]
	public class TransferResponse
	{
		[VariantTagProperty()]
		public TransferResponseTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public TransferResponse(TransferResponseTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferResponse()
		{
		}

		public static TransferResponse Err(TransferResponse.ErrVariant info)
		{
			return new TransferResponse(TransferResponseTag.Err, info);
		}

		public static TransferResponse Ok(Balance info)
		{
			return new TransferResponse(TransferResponseTag.Ok, info);
		}

		public TransferResponse.ErrVariant AsErr()
		{
			this.ValidateTag(TransferResponseTag.Err);
			return (TransferResponse.ErrVariant)this.Value!;
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

		[Variant(typeof(TransferResponse.ErrVariantTag))]
		public class ErrVariant
		{
			[VariantTagProperty()]
			public TransferResponse.ErrVariantTag Tag { get; set; }

			[VariantValueProperty()]
			public System.Object? Value { get; set; }

			public ErrVariant(TransferResponse.ErrVariantTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected ErrVariant()
			{
			}

			public static TransferResponse.ErrVariant CannotNotify(AccountIdentifier info)
			{
				return new TransferResponse.ErrVariant(TransferResponse.ErrVariantTag.CannotNotify, info);
			}

			public static TransferResponse.ErrVariant InsufficientBalance()
			{
				return new TransferResponse.ErrVariant(TransferResponse.ErrVariantTag.InsufficientBalance, null);
			}

			public static TransferResponse.ErrVariant InvalidToken(TokenIdentifier info)
			{
				return new TransferResponse.ErrVariant(TransferResponse.ErrVariantTag.InvalidToken, info);
			}

			public static TransferResponse.ErrVariant Other(string info)
			{
				return new TransferResponse.ErrVariant(TransferResponse.ErrVariantTag.Other, info);
			}

			public static TransferResponse.ErrVariant Rejected()
			{
				return new TransferResponse.ErrVariant(TransferResponse.ErrVariantTag.Rejected, null);
			}

			public static TransferResponse.ErrVariant Unauthorized(AccountIdentifier info)
			{
				return new TransferResponse.ErrVariant(TransferResponse.ErrVariantTag.Unauthorized, info);
			}

			public AccountIdentifier AsCannotNotify()
			{
				this.ValidateTag(TransferResponse.ErrVariantTag.CannotNotify);
				return (AccountIdentifier)this.Value!;
			}

			public TokenIdentifier AsInvalidToken()
			{
				this.ValidateTag(TransferResponse.ErrVariantTag.InvalidToken);
				return (TokenIdentifier)this.Value!;
			}

			public string AsOther()
			{
				this.ValidateTag(TransferResponse.ErrVariantTag.Other);
				return (string)this.Value!;
			}

			public AccountIdentifier AsUnauthorized()
			{
				this.ValidateTag(TransferResponse.ErrVariantTag.Unauthorized);
				return (AccountIdentifier)this.Value!;
			}

			private void ValidateTag(TransferResponse.ErrVariantTag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}
		}

		public enum ErrVariantTag
		{
			[VariantOptionType(typeof(AccountIdentifier))]
			CannotNotify,
			InsufficientBalance,
			[VariantOptionType(typeof(TokenIdentifier))]
			InvalidToken,
			[VariantOptionType(typeof(string))]
			Other,
			Rejected,
			[VariantOptionType(typeof(AccountIdentifier))]
			Unauthorized
		}
	}

	public enum TransferResponseTag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(TransferResponse.ErrVariant))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Balance))]
		Ok
	}
}