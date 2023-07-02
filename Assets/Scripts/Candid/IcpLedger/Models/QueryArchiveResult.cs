using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using BlockIndex = System.UInt64;
using Memo = System.UInt64;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using TextAccountIdentifier = System.String;
using Icrc1BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Icrc1Timestamp = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using Candid.IcpLedger.Models;
using System;

namespace Candid.IcpLedger.Models
{
	[Variant(typeof(QueryArchiveResultTag))]
	public class QueryArchiveResult
	{
		[VariantTagProperty()]
		public QueryArchiveResultTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public QueryArchiveResult(QueryArchiveResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected QueryArchiveResult()
		{
		}

		public static QueryArchiveResult Ok(BlockRange info)
		{
			return new QueryArchiveResult(QueryArchiveResultTag.Ok, info);
		}

		public static QueryArchiveResult Err(QueryArchiveError info)
		{
			return new QueryArchiveResult(QueryArchiveResultTag.Err, info);
		}

		public BlockRange AsOk()
		{
			this.ValidateTag(QueryArchiveResultTag.Ok);
			return (BlockRange)this.Value!;
		}

		public QueryArchiveError AsErr()
		{
			this.ValidateTag(QueryArchiveResultTag.Err);
			return (QueryArchiveError)this.Value!;
		}

		private void ValidateTag(QueryArchiveResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum QueryArchiveResultTag
	{
		[VariantOptionType(typeof(BlockRange))]
		Ok,
		[VariantOptionType(typeof(QueryArchiveError))]
		Err
	}
}