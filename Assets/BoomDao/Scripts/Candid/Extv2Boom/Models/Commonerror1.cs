using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using TokenIdentifier = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class Commonerror1
	{
		[VariantTagProperty]
		public Commonerror1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Commonerror1(Commonerror1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Commonerror1()
		{
		}

		public static Commonerror1 InvalidToken(TokenIdentifier info)
		{
			return new Commonerror1(Commonerror1Tag.InvalidToken, info);
		}

		public static Commonerror1 Other(string info)
		{
			return new Commonerror1(Commonerror1Tag.Other, info);
		}

		public TokenIdentifier AsInvalidToken()
		{
			this.ValidateTag(Commonerror1Tag.InvalidToken);
			return (TokenIdentifier)this.Value!;
		}

		public string AsOther()
		{
			this.ValidateTag(Commonerror1Tag.Other);
			return (string)this.Value!;
		}

		private void ValidateTag(Commonerror1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Commonerror1Tag
	{
		InvalidToken,
		Other
	}
}