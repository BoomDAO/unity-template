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

		public static Result_7 Err(CommonError info)
		{
			return new Result_7(Result_7Tag.Err, info);
		}

		public static Result_7 Ok(AccountIdentifier__1 info)
		{
			return new Result_7(Result_7Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_7Tag.Err);
			return (CommonError)this.Value!;
		}

		public AccountIdentifier__1 AsOk()
		{
			this.ValidateTag(Result_7Tag.Ok);
			return (AccountIdentifier__1)this.Value!;
		}

		private void ValidateTag(Result_7Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_7Tag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}