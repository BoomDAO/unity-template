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
	[Variant]
	public class CommonError__2
	{
		[VariantTagProperty()]
		public CommonError__2Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public CommonError__2(CommonError__2Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CommonError__2()
		{
		}

		public static CommonError__2 InvalidToken(TokenIdentifier__1 info)
		{
			return new CommonError__2(CommonError__2Tag.InvalidToken, info);
		}

		public static CommonError__2 Other(string info)
		{
			return new CommonError__2(CommonError__2Tag.Other, info);
		}

		public TokenIdentifier__1 AsInvalidToken()
		{
			this.ValidateTag(CommonError__2Tag.InvalidToken);
			return (TokenIdentifier__1)this.Value!;
		}

		public string AsOther()
		{
			this.ValidateTag(CommonError__2Tag.Other);
			return (string)this.Value!;
		}

		private void ValidateTag(CommonError__2Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CommonError__2Tag
	{
		
		InvalidToken,
		
		Other
	}
}