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
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.Extv2Boom.Models
{
	[Variant(typeof(MetadataValueTag))]
	public class MetadataValue
	{
		[VariantTagProperty()]
		public MetadataValueTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public MetadataValue(MetadataValueTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MetadataValue()
		{
		}

		public static MetadataValue Blob(List<byte> info)
		{
			return new MetadataValue(MetadataValueTag.Blob, info);
		}

		public static MetadataValue Nat(UnboundedUInt info)
		{
			return new MetadataValue(MetadataValueTag.Nat, info);
		}

		public static MetadataValue Nat8(byte info)
		{
			return new MetadataValue(MetadataValueTag.Nat8, info);
		}

		public static MetadataValue Text(string info)
		{
			return new MetadataValue(MetadataValueTag.Text, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(MetadataValueTag.Blob);
			return (List<byte>)this.Value!;
		}

		public UnboundedUInt AsNat()
		{
			this.ValidateTag(MetadataValueTag.Nat);
			return (UnboundedUInt)this.Value!;
		}

		public byte AsNat8()
		{
			this.ValidateTag(MetadataValueTag.Nat8);
			return (byte)this.Value!;
		}

		public string AsText()
		{
			this.ValidateTag(MetadataValueTag.Text);
			return (string)this.Value!;
		}

		private void ValidateTag(MetadataValueTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum MetadataValueTag
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