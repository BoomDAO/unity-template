using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class MetadataValueValue_1
	{
		[VariantTagProperty]
		public MetadataValueValue_1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public MetadataValueValue_1(MetadataValueValue_1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MetadataValueValue_1()
		{
		}

		public static MetadataValueValue_1 Blob(List<byte> info)
		{
			return new MetadataValueValue_1(MetadataValueValue_1Tag.Blob, info);
		}

		public static MetadataValueValue_1 Nat(UnboundedUInt info)
		{
			return new MetadataValueValue_1(MetadataValueValue_1Tag.Nat, info);
		}

		public static MetadataValueValue_1 Nat8(byte info)
		{
			return new MetadataValueValue_1(MetadataValueValue_1Tag.Nat8, info);
		}

		public static MetadataValueValue_1 Text(string info)
		{
			return new MetadataValueValue_1(MetadataValueValue_1Tag.Text, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(MetadataValueValue_1Tag.Blob);
			return (List<byte>)this.Value!;
		}

		public UnboundedUInt AsNat()
		{
			this.ValidateTag(MetadataValueValue_1Tag.Nat);
			return (UnboundedUInt)this.Value!;
		}

		public byte AsNat8()
		{
			this.ValidateTag(MetadataValueValue_1Tag.Nat8);
			return (byte)this.Value!;
		}

		public string AsText()
		{
			this.ValidateTag(MetadataValueValue_1Tag.Text);
			return (string)this.Value!;
		}

		private void ValidateTag(MetadataValueValue_1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum MetadataValueValue_1Tag
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