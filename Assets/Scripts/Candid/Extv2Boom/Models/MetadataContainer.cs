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
using System;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class MetadataContainer
	{
		[VariantTagProperty()]
		public MetadataContainerTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public MetadataContainer(MetadataContainerTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MetadataContainer()
		{
		}

		public static MetadataContainer Blob(List<byte> info)
		{
			return new MetadataContainer(MetadataContainerTag.Blob, info);
		}

		public static MetadataContainer Data(List<MetadataValue> info)
		{
			return new MetadataContainer(MetadataContainerTag.Data, info);
		}

		public static MetadataContainer Json(string info)
		{
			return new MetadataContainer(MetadataContainerTag.Json, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(MetadataContainerTag.Blob);
			return (List<byte>)this.Value!;
		}

		public List<MetadataValue> AsData()
		{
			this.ValidateTag(MetadataContainerTag.Data);
			return (List<MetadataValue>)this.Value!;
		}

		public string AsJson()
		{
			this.ValidateTag(MetadataContainerTag.Json);
			return (string)this.Value!;
		}

		private void ValidateTag(MetadataContainerTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum MetadataContainerTag
	{
		[CandidName("blob")]
		
		Blob,
		[CandidName("data")]
		
		Data,
		[CandidName("json")]
		
		Json
	}
}