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
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Boom.Models
{
	[Variant(typeof(Result_12Tag))]
	public class Result_12
	{
		[VariantTagProperty()]
		public Result_12Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_12(Result_12Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_12()
		{
		}

		public static Result_12 Err(CommonError__1 info)
		{
			return new Result_12(Result_12Tag.Err, info);
		}

		public static Result_12 Ok(Result_12.OkInfo info)
		{
			return new Result_12(Result_12Tag.Ok, info);
		}

		public CommonError__1 AsErr()
		{
			this.ValidateTag(Result_12Tag.Err);
			return (CommonError__1)this.Value!;
		}

		public Result_12.OkInfo AsOk()
		{
			this.ValidateTag(Result_12Tag.Ok);
			return (Result_12.OkInfo)this.Value!;
		}

		private void ValidateTag(Result_12Tag tag)
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
			public OptionalValue<Listing> F1 { get; set; }

			public OkInfo(AccountIdentifier__2 f0, OptionalValue<Listing> f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public OkInfo()
			{
			}
		}
	}

	public enum Result_12Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError__1))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Result_12.OkInfo))]
		Ok
	}
}