using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using Candid.Extv2Boom.Models;
using HeaderField = System.ValueTuple<System.String, System.String>;

namespace Candid.Extv2Boom.Models
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