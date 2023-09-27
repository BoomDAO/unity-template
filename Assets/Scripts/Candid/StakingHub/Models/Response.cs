using EdjCase.ICP.Candid.Mapping;
using Candid.StakingHub.Models;
using System;

namespace Candid.StakingHub.Models
{
	[Variant]
	public class Response
	{
		[VariantTagProperty()]
		public ResponseTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Response(ResponseTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Response()
		{
		}

		public static Response Err(string info)
		{
			return new Response(ResponseTag.Err, info);
		}

		public static Response Success(string info)
		{
			return new Response(ResponseTag.Success, info);
		}

		public string AsErr()
		{
			this.ValidateTag(ResponseTag.Err);
			return (string)this.Value!;
		}

		public string AsSuccess()
		{
			this.ValidateTag(ResponseTag.Success);
			return (string)this.Value!;
		}

		private void ValidateTag(ResponseTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ResponseTag
	{
		
		Err,
		
		Success
	}
}