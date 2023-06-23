using BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using Timestamp = System.UInt64;
using Duration = System.UInt64;
using Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using Map = System.Collections.Generic.List<Candid.Icrc1Ledger.Models.MapItem>;
using Block = Candid.Icrc1Ledger.Models.Value;
using QueryBlockArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using EdjCase.ICP.Candid.Mapping;
using Candid.Icrc1Ledger.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.Icrc1Ledger.Models
{
	[Variant(typeof(ValueTag))]
	public class Value
	{
		[VariantTagProperty()]
		public ValueTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value_ { get; set; }

		public Value(ValueTag tag, object? value)
		{
			this.Tag = tag;
			this.Value_ = value;
		}

		protected Value()
		{
		}

		public static Value Blob(List<byte> info)
		{
			return new Value(ValueTag.Blob, info);
		}

		public static Value Text(string info)
		{
			return new Value(ValueTag.Text, info);
		}

		public static Value Nat(UnboundedUInt info)
		{
			return new Value(ValueTag.Nat, info);
		}

		public static Value Nat64(ulong info)
		{
			return new Value(ValueTag.Nat64, info);
		}

		public static Value Int(UnboundedInt info)
		{
			return new Value(ValueTag.Int, info);
		}

		public static Value Array(List<Value> info)
		{
			return new Value(ValueTag.Array, info);
		}

		public static Value Map(Map info)
		{
			return new Value(ValueTag.Map, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(ValueTag.Blob);
			return (List<byte>)this.Value_!;
		}

		public string AsText()
		{
			this.ValidateTag(ValueTag.Text);
			return (string)this.Value_!;
		}

		public UnboundedUInt AsNat()
		{
			this.ValidateTag(ValueTag.Nat);
			return (UnboundedUInt)this.Value_!;
		}

		public ulong AsNat64()
		{
			this.ValidateTag(ValueTag.Nat64);
			return (ulong)this.Value_!;
		}

		public UnboundedInt AsInt()
		{
			this.ValidateTag(ValueTag.Int);
			return (UnboundedInt)this.Value_!;
		}

		public List<Value> AsArray()
		{
			this.ValidateTag(ValueTag.Array);
			return (List<Value>)this.Value_!;
		}

		public Map AsMap()
		{
			this.ValidateTag(ValueTag.Map);
			return (Map)this.Value_!;
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
		[VariantOptionType(typeof(List<byte>))]
		Blob,
		[VariantOptionType(typeof(string))]
		Text,
		[VariantOptionType(typeof(UnboundedUInt))]
		Nat,
		[VariantOptionType(typeof(ulong))]
		Nat64,
		[VariantOptionType(typeof(UnboundedInt))]
		Int,
		[VariantOptionType(typeof(List<Value>))]
		Array,
		[VariantOptionType(typeof(Map))]
		Map
	}
}