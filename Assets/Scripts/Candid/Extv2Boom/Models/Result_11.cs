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
	[Variant]
	public class Result_11
	{
		[VariantTagProperty()]
		public Result_11Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_11(Result_11Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_11()
		{
		}

		public static Result_11 Err(CommonError__1 info)
		{
			return new Result_11(Result_11Tag.Err, info);
		}

		public static Result_11 Ok(Result_11.OkInfo info)
		{
			return new Result_11(Result_11Tag.Ok, info);
		}

		public CommonError__1 AsErr()
		{
			this.ValidateTag(Result_11Tag.Err);
			return (CommonError__1)this.Value!;
		}

		public Result_11.OkInfo AsOk()
		{
			this.ValidateTag(Result_11Tag.Ok);
			return (Result_11.OkInfo)this.Value!;
		}

		private void ValidateTag(Result_11Tag tag)
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

	public enum Result_11Tag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}