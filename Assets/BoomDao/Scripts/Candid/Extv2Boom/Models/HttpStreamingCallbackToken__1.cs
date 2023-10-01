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
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace Candid.Extv2Boom.Models
{
	public class HttpStreamingCallbackToken__1
	{
		[CandidName("content_encoding")]
		public string ContentEncoding { get; set; }

		[CandidName("index")]
		public UnboundedUInt Index { get; set; }

		[CandidName("key")]
		public string Key { get; set; }

		[CandidName("sha256")]
		public OptionalValue<List<byte>> Sha256 { get; set; }

		public HttpStreamingCallbackToken__1(string contentEncoding, UnboundedUInt index, string key, OptionalValue<List<byte>> sha256)
		{
			this.ContentEncoding = contentEncoding;
			this.Index = index;
			this.Key = key;
			this.Sha256 = sha256;
		}

		public HttpStreamingCallbackToken__1()
		{
		}
	}
}