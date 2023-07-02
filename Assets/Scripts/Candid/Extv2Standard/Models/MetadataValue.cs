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
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.ext_v2_standard.Models
{
	[Variant(typeof(MetadataValueItemTag))]
	public class MetadataValueItem
	{
		[VariantTagProperty()]
		public MetadataValueItemTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public MetadataValueItem(MetadataValueItemTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MetadataValueItem()
		{
		}

		public static MetadataValueItem Blob(List<byte> info)
		{
			return new MetadataValueItem(MetadataValueItemTag.Blob, info);
		}

		public static MetadataValueItem Nat(UnboundedUInt info)
		{
			return new MetadataValueItem(MetadataValueItemTag.Nat, info);
		}

		public static MetadataValueItem Nat8(byte info)
		{
			return new MetadataValueItem(MetadataValueItemTag.Nat8, info);
		}

		public static MetadataValueItem Text(string info)
		{
			return new MetadataValueItem(MetadataValueItemTag.Text, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(MetadataValueItemTag.Blob);
			return (List<byte>)this.Value!;
		}

		public UnboundedUInt AsNat()
		{
			this.ValidateTag(MetadataValueItemTag.Nat);
			return (UnboundedUInt)this.Value!;
		}

		public byte AsNat8()
		{
			this.ValidateTag(MetadataValueItemTag.Nat8);
			return (byte)this.Value!;
		}

		public string AsText()
		{
			this.ValidateTag(MetadataValueItemTag.Text);
			return (string)this.Value!;
		}

		private void ValidateTag(MetadataValueItemTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum MetadataValueItemTag
	{
		[CandidName("blob")]
		[VariantOptionType(typeof(List<byte>))]
		Blob,
		[CandidName("nat")]
		[VariantOptionType(typeof(UnboundedUInt))]
		Nat,
		[CandidName("nat8")]
		[VariantOptionType(typeof(byte))]
		Nat8,
		[CandidName("text")]
		[VariantOptionType(typeof(string))]
		Text
	}
}