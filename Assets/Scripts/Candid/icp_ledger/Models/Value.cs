using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using BlockIndex = System.UInt64;
using Memo = System.UInt64;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using TextAccountIdentifier = System.String;
using Icrc1BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Icrc1Timestamp = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using Candid.IcpLedger.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using System;

namespace Candid.IcpLedger.Models
{
	[Variant(typeof(ValueTag))]
	public class Value
	{
		[VariantTagProperty()]
		public ValueTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? value { get; set; }

		public Value(ValueTag tag, object? value)
		{
			this.Tag = tag;
			this.value = value;
		}

		protected Value()
		{
		}

		public static Value Nat(UnboundedUInt info)
		{
			return new Value(ValueTag.Nat, info);
		}

		public static Value Int(UnboundedInt info)
		{
			return new Value(ValueTag.Int, info);
		}

		public static Value Text(string info)
		{
			return new Value(ValueTag.Text, info);
		}

		public static Value Blob(List<byte> info)
		{
			return new Value(ValueTag.Blob, info);
		}

		public UnboundedUInt AsNat()
		{
			this.ValidateTag(ValueTag.Nat);
			return (UnboundedUInt)this.value!;
		}

		public UnboundedInt AsInt()
		{
			this.ValidateTag(ValueTag.Int);
			return (UnboundedInt)this.value!;
		}

		public string AsText()
		{
			this.ValidateTag(ValueTag.Text);
			return (string)this.value!;
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(ValueTag.Blob);
			return (List<byte>)this.value!;
		}

		private void ValidateTag(ValueTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ValueTag
	{
		[VariantOptionType(typeof(UnboundedUInt))]
		Nat,
		[VariantOptionType(typeof(UnboundedInt))]
		Int,
		[VariantOptionType(typeof(string))]
		Text,
		[VariantOptionType(typeof(List<byte>))]
		Blob
	}
}