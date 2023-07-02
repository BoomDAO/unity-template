using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.ext_v2_standard.Models.MetadataValueItem>;
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
using Candid.ext_v2_standard.Models;
using System;

namespace Candid.ext_v2_standard.Models
{
	[Variant(typeof(Result_3Tag))]
	public class Result_3
	{
		[VariantTagProperty()]
		public Result_3Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_3(Result_3Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_3()
		{
		}

		public static Result_3 Err(CommonError info)
		{
			return new Result_3(Result_3Tag.Err, info);
		}

		public static Result_3 Ok()
		{
			return new Result_3(Result_3Tag.Ok, null);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_3Tag.Err);
			return (CommonError)this.Value!;
		}

		private void ValidateTag(Result_3Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_3Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError))]
		Err,
		[CandidName("ok")]
		Ok
	}
}