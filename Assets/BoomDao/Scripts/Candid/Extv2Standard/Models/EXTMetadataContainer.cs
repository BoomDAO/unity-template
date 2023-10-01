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
using System;

namespace Candid.Extv2Standard.Models
{
	[Variant(typeof(EXTMetadataContainerTag))]
	public class EXTMetadataContainer
	{
		[VariantTagProperty()]
		public EXTMetadataContainerTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public EXTMetadataContainer(EXTMetadataContainerTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected EXTMetadataContainer()
		{
		}

		public static EXTMetadataContainer Blob(List<byte> info)
		{
			return new EXTMetadataContainer(EXTMetadataContainerTag.Blob, info);
		}

		public static EXTMetadataContainer Data(List<EXTMetadataValue> info)
		{
			return new EXTMetadataContainer(EXTMetadataContainerTag.Data, info);
		}

		public static EXTMetadataContainer Json(string info)
		{
			return new EXTMetadataContainer(EXTMetadataContainerTag.Json, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(EXTMetadataContainerTag.Blob);
			return (List<byte>)this.Value!;
		}

		public List<EXTMetadataValue> AsData()
		{
			this.ValidateTag(EXTMetadataContainerTag.Data);
			return (List<EXTMetadataValue>)this.Value!;
		}

		public string AsJson()
		{
			this.ValidateTag(EXTMetadataContainerTag.Json);
			return (string)this.Value!;
		}

		private void ValidateTag(EXTMetadataContainerTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum EXTMetadataContainerTag
	{
		[CandidName("blob")]
		[VariantOptionType(typeof(List<byte>))]
		Blob,
		[CandidName("data")]
		[VariantOptionType(typeof(List<EXTMetadataValue>))]
		Data,
		[CandidName("json")]
		[VariantOptionType(typeof(string))]
		Json
	}
}