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
using Candid.Extv2Boom.Models;
using System;
using EdjCase.ICP.Candid.Models.Values;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class HttpStreamingStrategy__1
	{
		[VariantTagProperty()]
		public HttpStreamingStrategy__1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public HttpStreamingStrategy__1(HttpStreamingStrategy__1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected HttpStreamingStrategy__1()
		{
		}

		public static HttpStreamingStrategy__1 Callback(HttpStreamingStrategy__1.CallbackInfo info)
		{
			return new HttpStreamingStrategy__1(HttpStreamingStrategy__1Tag.Callback, info);
		}

		public HttpStreamingStrategy__1.CallbackInfo AsCallback()
		{
			this.ValidateTag(HttpStreamingStrategy__1Tag.Callback);
			return (HttpStreamingStrategy__1.CallbackInfo)this.Value!;
		}

		private void ValidateTag(HttpStreamingStrategy__1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class CallbackInfo
		{
			[CandidName("callback")]
			public CandidFunc Callback { get; set; }

			[CandidName("token")]
			public HttpStreamingCallbackToken__1 Token { get; set; }

			public CallbackInfo(CandidFunc callback, HttpStreamingCallbackToken__1 token)
			{
				this.Callback = callback;
				this.Token = token;
			}

			public CallbackInfo()
			{
			}
		}
	}

	public enum HttpStreamingStrategy__1Tag
	{
		
		Callback
	}
}