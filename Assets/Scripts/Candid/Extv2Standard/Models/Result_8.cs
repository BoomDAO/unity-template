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

namespace Candid.Extv2Standard.Models
{
	[Variant]
	public class Result_8
	{
		[VariantTagProperty()]
		public Result_8Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_8(Result_8Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_8()
		{
		}

		public static Result_8 Err(CommonError info)
		{
			return new Result_8(Result_8Tag.Err, info);
		}

		public static Result_8 Ok(EXTMetadata info)
		{
			return new Result_8(Result_8Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_8Tag.Err);
			return (CommonError)this.Value!;
		}

		public EXTMetadata AsOk()
		{
			this.ValidateTag(Result_8Tag.Ok);
			return (EXTMetadata)this.Value!;
		}

		private void ValidateTag(Result_8Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_8Tag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}