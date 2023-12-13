using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using System;
using AccountIdentifier = System.String;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class User
	{
		[VariantTagProperty]
		public UserTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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