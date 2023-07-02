using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.extv2_boom.Models.MetadataValue>;
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
using Candid.extv2_boom.Models;
using System;
using EdjCase.ICP.Candid.Models.Values;

namespace Candid.extv2_boom.Models
{
	[Variant(typeof(HttpStreamingStrategyTag))]
	public class HttpStreamingStrategy
	{
		[VariantTagProperty()]
		public HttpStreamingStrategyTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public HttpStreamingStrategy(HttpStreamingStrategyTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected HttpStreamingStrategy()
		{
		}

		public static HttpStreamingStrategy Callback(HttpStreamingStrategy.CallbackInfo info)
		{
			return new HttpStreamingStrategy(HttpStreamingStrategyTag.Callback, info);
		}

		public HttpStreamingStrategy.CallbackInfo AsCallback()
		{
			this.ValidateTag(HttpStreamingStrategyTag.Callback);
			return (HttpStreamingStrategy.CallbackInfo)this.Value!;
		}

		private void ValidateTag(HttpStreamingStrategyTag tag)
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
			public HttpStreamingCallbackToken Token { get; set; }

			public CallbackInfo(CandidFunc callback, HttpStreamingCallbackToken token)
			{
				this.Callback = callback;
				this.Token = token;
			}

			public CallbackInfo()
			{
			}
		}
	}

	public enum HttpStreamingStrategyTag
	{
		[VariantOptionType(typeof(HttpStreamingStrategy.CallbackInfo))]
		Callback
	}
}