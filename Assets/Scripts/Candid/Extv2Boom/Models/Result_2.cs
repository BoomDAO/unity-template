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

namespace Candid.extv2_boom.Models
{
	[Variant(typeof(Result_2Tag))]
	public class Result_2
	{
		[VariantTagProperty()]
		public Result_2Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_2(Result_2Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_2()
		{
		}

		public static Result_2 Err(CommonError info)
		{
			return new Result_2(Result_2Tag.Err, info);
		}

		public static Result_2 Ok(Balance__1 info)
		{
			return new Result_2(Result_2Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result_2Tag.Err);
			return (CommonError)this.Value!;
		}

		public Balance__1 AsOk()
		{
			this.ValidateTag(Result_2Tag.Ok);
			return (Balance__1)this.Value!;
		}

		private void ValidateTag(Result_2Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_2Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Balance__1))]
		Ok
	}
}