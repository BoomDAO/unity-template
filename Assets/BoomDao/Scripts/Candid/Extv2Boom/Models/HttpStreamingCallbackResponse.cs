using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using Candid.Extv2Boom.Models;

namespace Candid.Extv2Boom.Models
{
	public class HttpStreamingCallbackResponse
	{
		[CandidName("body")]
		public List<byte> Body { get; set; }

		[CandidName("token")]
		public OptionalValue<HttpStreamingCallbackToken> Token { get; set; }

		public HttpStreamingCallbackResponse(List<byte> body, OptionalValue<HttpStreamingCallbackToken> token)
		{
			this.Body = body;
			this.Token = token;
		}

		public HttpStreamingCallbackResponse()
		{
		}
	}
}