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
	[Variant(typeof(Result_6Tag))]
	public class Result_6
	{
		[VariantTagProperty()]
		public Result_6Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_6(Result_6Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_6()
		{
		}

		public static Result_6 Err(CommonError info)
		{
			return new Result_6(Result_6Tag.Err, info);
		}

		public static Result_6 Ok(Metadata info)
		{
			return new Result_6(Result_6Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_6Tag.Err);
			return (CommonError)this.Value!;
		}

		public Metadata AsOk()
		{
			this.ValidateTag(Result_6Tag.Ok);
			return (Metadata)this.Value!;
		}

		private void ValidateTag(Result_6Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_6Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Metadata))]
		Ok
	}
}