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
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.Extv2Standard.Models
{
	[Variant]
	public class EXTMetadataValue
	{
		[VariantTagProperty()]
		public EXTMetadataValueTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public EXTMetadataValue(EXTMetadataValueTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected EXTMetadataValue()
		{
		}

		public static EXTMetadataValue Blob(List<byte> info)
		{
			return new EXTMetadataValue(EXTMetadataValueTag.Blob, info);
		}

		public static EXTMetadataValue Nat(UnboundedUInt info)
		{
			return new EXTMetadataValue(EXTMetadataValueTag.Nat, info);
		}

		public static EXTMetadataValue Nat8(byte info)
		{
			return new EXTMetadataValue(EXTMetadataValueTag.Nat8, info);
		}

		public static EXTMetadataValue Text(string info)
		{
			return new EXTMetadataValue(EXTMetadataValueTag.Text, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(EXTMetadataValueTag.Blob);
			return (List<byte>)this.Value!;
		}

		public UnboundedUInt AsNat()
		{
			this.ValidateTag(EXTMetadataValueTag.Nat);
			return (UnboundedUInt)this.Value!;
		}

		public byte AsNat8()
		{
			this.ValidateTag(EXTMetadataValueTag.Nat8);
			return (byte)this.Value!;
		}

		public string AsText()
		{
			this.ValidateTag(EXTMetadataValueTag.Text);
			return (string)this.Value!;
		}

		private void ValidateTag(EXTMetadataValueTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum EXTMetadataValueTag
	{
		[CandidName("blob")]
		
		Blob,
		[CandidName("nat")]
		
		Nat,
		[CandidName("nat8")]
		
		Nat8,
		[CandidName("text")]
		
		Text
	}
}