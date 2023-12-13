using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System.Collections.Generic;
using System;
using MetadataValue = System.ValueTuple<System.String, Candid.Extv2Boom.Models.MetadataValueValue_1>;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class MetadataContainer
	{
		[VariantTagProperty]
		public MetadataContainerTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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

		public static MetadataContainer Data(MetadataContainer.DataInfo info)
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

		public MetadataContainer.DataInfo AsData()
		{
			this.ValidateTag(MetadataContainerTag.Data);
			return (MetadataContainer.DataInfo)this.Value!;
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

		public class DataInfo : List<MetadataValue>
		{
			public DataInfo()
			{
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