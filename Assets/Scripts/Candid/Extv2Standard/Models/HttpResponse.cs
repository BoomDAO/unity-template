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
using System.Collections.Generic;
using Candid.ext_v2_standard.Models;
using EdjCase.ICP.Candid.Models;

namespace Candid.ext_v2_standard.Models
{
	public class HttpResponse
	{
		[CandidName("body")]
		public List<byte> Body { get; set; }

		[CandidName("headers")]
		public List<HeaderField> Headers { get; set; }

		[CandidName("status_code")]
		public ushort StatusCode { get; set; }

		[CandidName("streaming_strategy")]
		public OptionalValue<HttpStreamingStrategy> StreamingStrategy { get; set; }

		[CandidName("upgrade")]
		public bool Upgrade { get; set; }

		public HttpResponse(List<byte> body, List<HeaderField> headers, ushort statusCode, OptionalValue<HttpStreamingStrategy> streamingStrategy, bool upgrade)
		{
			this.Body = body;
			this.Headers = headers;
			this.StatusCode = statusCode;
			this.StreamingStrategy = streamingStrategy;
			this.Upgrade = upgrade;
		}

		public HttpResponse()
		{
		}
	}
}