using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using EXTMetadataValue = System.ValueTuple<System.String, Candid.Extv2Standard.Models.EXTMetadataValue>;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Standard.Models;
using System;

namespace Candid.Extv2Standard.Models
{
	[Variant(typeof(CommonErrorTag))]
	public class CommonError
	{
		[VariantTagProperty()]
		public CommonErrorTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public CommonError(CommonErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CommonError()
		{
		}

		public static CommonError InvalidToken(TokenIdentifier info)
		{
			return new CommonError(CommonErrorTag.InvalidToken, info);
		}

		public static CommonError Other(string info)
		{
			return new CommonError(CommonErrorTag.Other, info);
		}

		public TokenIdentifier AsInvalidToken()
		{
			this.ValidateTag(CommonErrorTag.InvalidToken);
			return (TokenIdentifier)this.Value!;
		}

		public string AsOther()
		{
			this.ValidateTag(CommonErrorTag.Other);
			return (string)this.Value!;
		}

		private void ValidateTag(CommonErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CommonErrorTag
	{
		[VariantOptionType(typeof(TokenIdentifier))]
		InvalidToken,
		[VariantOptionType(typeof(string))]
		Other
	}
}