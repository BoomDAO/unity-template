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
using System.Collections.Generic;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Boom.Models
{
	public class HttpResponse__1
	{
		[CandidName("body")]
		public List<byte> Body { get; set; }

		[CandidName("headers")]
		public List<HeaderField__1> Headers { get; set; }

		[CandidName("status_code")]
		public ushort StatusCode { get; set; }

		[CandidName("streaming_strategy")]
		public OptionalValue<HttpStreamingStrategy__1> StreamingStrategy { get; set; }

		[CandidName("upgrade")]
		public bool Upgrade { get; set; }

		public HttpResponse__1(List<byte> body, List<HeaderField__1> headers, ushort statusCode, OptionalValue<HttpStreamingStrategy__1> streamingStrategy, bool upgrade)
		{
			this.Body = body;
			this.Headers = headers;
			this.StatusCode = statusCode;
			this.StreamingStrategy = streamingStrategy;
			this.Upgrade = upgrade;
		}

		public HttpResponse__1()
		{
		}
	}
}