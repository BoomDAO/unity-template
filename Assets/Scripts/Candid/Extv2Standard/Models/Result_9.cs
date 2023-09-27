using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using EXTMetadataValue = System.ValueTuple<System.String, Candid.Extv2Standard.Models.EXTMetadataValue>;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Standard.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Standard.Models
{
	[Variant]
	public class Result_9
	{
		[VariantTagProperty()]
		public Result_9Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_9(Result_9Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_9()
		{
		}

		public static Result_9 Err(CommonError info)
		{
			return new Result_9(Result_9Tag.Err, info);
		}

		public static Result_9 Ok(Result_9.OkInfo info)
		{
			return new Result_9(Result_9Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_9Tag.Err);
			return (CommonError)this.Value!;
		}

		public Result_9.OkInfo AsOk()
		{
			this.ValidateTag(Result_9Tag.Ok);
			return (Result_9.OkInfo)this.Value!;
		}

		private void ValidateTag(Result_9Tag tag)
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

	public enum Result_9Tag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}