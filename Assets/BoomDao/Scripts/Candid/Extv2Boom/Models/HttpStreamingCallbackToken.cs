using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace Candid.Extv2Boom.Models
{
	public class HttpStreamingCallbackToken
	{
		[CandidName("content_encoding")]
		public string ContentEncoding { get; set; }

		[CandidName("index")]
		public UnboundedUInt Index { get; set; }

		[CandidName("key")]
		public string Key { get; set; }

		[CandidName("sha256")]
		public OptionalValue<List<byte>> Sha256 { get; set; }

		public HttpStreamingCallbackToken(string contentEncoding, UnboundedUInt index, string key, OptionalValue<List<byte>> sha256)
		{
			this.ContentEncoding = contentEncoding;
			this.Index = index;
			this.Key = key;
			this.Sha256 = sha256;
		}

		public HttpStreamingCallbackToken()
		{
		}
	}
}