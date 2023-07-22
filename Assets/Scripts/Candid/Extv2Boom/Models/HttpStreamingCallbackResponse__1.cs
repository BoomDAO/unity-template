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
	public class HttpStreamingCallbackResponse__1
	{
		[CandidName("body")]
		public List<byte> Body { get; set; }

		[CandidName("token")]
		public OptionalValue<HttpStreamingCallbackToken__1> Token { get; set; }

		public HttpStreamingCallbackResponse__1(List<byte> body, OptionalValue<HttpStreamingCallbackToken__1> token)
		{
			this.Body = body;
			this.Token = token;
		}

		public HttpStreamingCallbackResponse__1()
		{
		}
	}
}