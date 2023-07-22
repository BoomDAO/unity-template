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
using System.Collections.Generic;

namespace Candid.Extv2Standard.Models
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