using TokenIndex__1 = System.UInt32;
using TokenIdentifier__2 = System.String;
using TokenIdentifier__1 = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.Extv2Boom.Models.MetadataValue>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField__1 = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle__1 = System.String;
using AccountIdentifier__2 = System.String;
using AccountIdentifier__1 = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;

namespace Candid.Extv2Boom.Models
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

		[Variant(typeof(TransferResponse.ErrInfoTag))]
		public class ErrInfo
		{
			[VariantTagProperty()]
			public TransferResponse.ErrInfoTag Tag { get; set; }

			[VariantValueProperty()]
			public System.Object? Value { get; set; }

			public ErrInfo(TransferResponse.ErrInfoTag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected ErrInfo()
			{
			}

			public static TransferResponse.ErrInfo CannotNotify(AccountIdentifier__1 info)
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.CannotNotify, info);
			}

			public static TransferResponse.ErrInfo InsufficientBalance()
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.InsufficientBalance, null);
			}

			public static TransferResponse.ErrInfo InvalidToken(TokenIdentifier__1 info)
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

			public static TransferResponse.ErrInfo Unauthorized(AccountIdentifier__1 info)
			{
				return new TransferResponse.ErrInfo(TransferResponse.ErrInfoTag.Unauthorized, info);
			}

			public AccountIdentifier__1 AsCannotNotify()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.CannotNotify);
				return (AccountIdentifier__1)this.Value!;
			}

			public TokenIdentifier__1 AsInvalidToken()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.InvalidToken);
				return (TokenIdentifier__1)this.Value!;
			}

			public string AsOther()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.Other);
				return (string)this.Value!;
			}

			public AccountIdentifier__1 AsUnauthorized()
			{
				this.ValidateTag(TransferResponse.ErrInfoTag.Unauthorized);
				return (AccountIdentifier__1)this.Value!;
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
			[VariantOptionType(typeof(AccountIdentifier__1))]
			CannotNotify,
			InsufficientBalance,
			[VariantOptionType(typeof(TokenIdentifier__1))]
			InvalidToken,
			[VariantOptionType(typeof(string))]
			Other,
			Rejected,
			[VariantOptionType(typeof(AccountIdentifier__1))]
			Unauthorized
		}
	}

	public enum TransferResponseTag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(TransferResponse.ErrInfo))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Balance))]
		Ok
	}
}