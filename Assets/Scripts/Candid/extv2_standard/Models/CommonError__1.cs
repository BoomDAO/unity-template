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
	[Variant(typeof(CommonError__1Tag))]
	public class CommonError__1
	{
		[VariantTagProperty()]
		public CommonError__1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public CommonError__1(CommonError__1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CommonError__1()
		{
		}

		public static CommonError__1 InvalidToken(TokenIdentifier info)
		{
			return new CommonError__1(CommonError__1Tag.InvalidToken, info);
		}

		public static CommonError__1 Other(string info)
		{
			return new CommonError__1(CommonError__1Tag.Other, info);
		}

		public TokenIdentifier AsInvalidToken()
		{
			this.ValidateTag(CommonError__1Tag.InvalidToken);
			return (TokenIdentifier)this.Value!;
		}

		public string AsOther()
		{
			this.ValidateTag(CommonError__1Tag.Other);
			return (string)this.Value!;
		}

		private void ValidateTag(CommonError__1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CommonError__1Tag
	{
		[VariantOptionType(typeof(TokenIdentifier))]
		InvalidToken,
		[VariantOptionType(typeof(string))]
		Other
	}
}