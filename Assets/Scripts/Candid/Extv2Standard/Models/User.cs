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
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.Extv2Standard.Models
{
	[Variant]
	public class User
	{
		[VariantTagProperty()]
		public UserTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public User(UserTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected User()
		{
		}

		public static User Address(AccountIdentifier info)
		{
			return new User(UserTag.Address, info);
		}

		public static User Principal(Principal info)
		{
			return new User(UserTag.Principal, info);
		}

		public AccountIdentifier AsAddress()
		{
			this.ValidateTag(UserTag.Address);
			return (AccountIdentifier)this.Value!;
		}

		public Principal AsPrincipal()
		{
			this.ValidateTag(UserTag.Principal);
			return (Principal)this.Value!;
		}

		private void ValidateTag(UserTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum UserTag
	{
		[CandidName("address")]
		
		Address,
		[CandidName("principal")]
		
		Principal
	}
}