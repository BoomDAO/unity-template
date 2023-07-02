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
using EdjCase.ICP.Candid.Models;

namespace Candid.extv2_boom.Models
{
	[Variant(typeof(Result_10Tag))]
	public class Result_10
	{
		[VariantTagProperty()]
		public Result_10Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_10(Result_10Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_10()
		{
		}

		public static Result_10 Err(CommonError info)
		{
			return new Result_10(Result_10Tag.Err, info);
		}

		public static Result_10 Ok(Result_10.OkInfo info)
		{
			return new Result_10(Result_10Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_10Tag.Err);
			return (CommonError)this.Value!;
		}

		public Result_10.OkInfo AsOk()
		{
			this.ValidateTag(Result_10Tag.Ok);
			return (Result_10.OkInfo)this.Value!;
		}

		private void ValidateTag(Result_10Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class OkInfo
		{
			[CandidTag(0U)]
			public AccountIdentifier__1 F0 { get; set; }

			[CandidTag(1U)]
			public OptionalValue<Listing> F1 { get; set; }

			public OkInfo(AccountIdentifier__1 f0, OptionalValue<Listing> f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public OkInfo()
			{
			}
		}
	}

	public enum Result_10Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Result_10.OkInfo))]
		Ok
	}
}