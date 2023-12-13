using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using TokenIdentifier = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class CommonError
	{
		[VariantTagProperty]
		public CommonErrorTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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
		InvalidToken,
		Other
	}
}