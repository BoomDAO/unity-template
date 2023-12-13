using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;
using EdjCase.ICP.Candid.Models.Values;

namespace Candid.Extv2Boom.Models
{
	[Variant]
	public class HttpStreamingStrategy
	{
		[VariantTagProperty]
		public HttpStreamingStrategyTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

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
		Callback
	}
}