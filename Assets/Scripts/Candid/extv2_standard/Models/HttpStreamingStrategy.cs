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
using Candid.ext_v2_standard.Models;
using System;
using EdjCase.ICP.Candid.Models.Values;

namespace Candid.ext_v2_standard.Models
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

		public static HttpStreamingStrategy Callback(HttpStreamingStrategy.CallbackRecord info)
		{
			return new HttpStreamingStrategy(HttpStreamingStrategyTag.Callback, info);
		}

		public HttpStreamingStrategy.CallbackRecord AsCallback()
		{
			this.ValidateTag(HttpStreamingStrategyTag.Callback);
			return (HttpStreamingStrategy.CallbackRecord)this.Value!;
		}

		private void ValidateTag(HttpStreamingStrategyTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class CallbackRecord
		{
			[CandidName("callback")]
			public CandidFunc Callback { get; set; }

			[CandidName("token")]
			public HttpStreamingCallbackToken Token { get; set; }

			public CallbackRecord(CandidFunc callback, HttpStreamingCallbackToken token)
			{
				this.Callback = callback;
				this.Token = token;
			}

			public CallbackRecord()
			{
			}
		}
	}

	public enum HttpStreamingStrategyTag
	{
		[VariantOptionType(typeof(HttpStreamingStrategy.CallbackRecord))]
		Callback
	}
}