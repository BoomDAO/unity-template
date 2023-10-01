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

namespace Candid.Extv2Boom.Models
{
	[Variant(typeof(Result_7Tag))]
	public class Result_7
	{
		[VariantTagProperty()]
		public Result_7Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_7(Result_7Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_7()
		{
		}

		public static Result_7 Err(string info)
		{
			return new Result_7(Result_7Tag.Err, info);
		}

		public static Result_7 Ok(Result_7.OkInfo info)
		{
			return new Result_7(Result_7Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_7Tag.Err);
			return (string)this.Value!;
		}

		public Result_7.OkInfo AsOk()
		{
			this.ValidateTag(Result_7Tag.Ok);
			return (Result_7.OkInfo)this.Value!;
		}

		private void ValidateTag(Result_7Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class OkInfo
		{
			[CandidTag(0U)]
			public AccountIdentifier__2 F0 { get; set; }

			[CandidTag(1U)]
			public ulong F1 { get; set; }

			public OkInfo(AccountIdentifier__2 f0, ulong f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public OkInfo()
			{
			}
		}
	}

	public enum Result_7Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Result_7.OkInfo))]
		Ok
	}
}