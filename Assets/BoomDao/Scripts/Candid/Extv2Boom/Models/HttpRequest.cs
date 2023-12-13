using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using HeaderField = System.ValueTuple<System.String, System.String>;

namespace Candid.Extv2Boom.Models
{
	public class HttpRequest
	{
		[CandidName("body")]
		public List<byte> Body { get; set; }

		[CandidName("headers")]
		public List<HeaderField> Headers { get; set; }

		[CandidName("method")]
		public string Method { get; set; }

		[CandidName("url")]
		public string Url { get; set; }

		public HttpRequest(List<byte> body, List<HeaderField> headers, string method, string url)
		{
			this.Body = body;
			this.Headers = headers;
			this.Method = method;
			this.Url = url;
		}

		public HttpRequest()
		{
		}
	}
}