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
	[Variant(typeof(Result_4Tag))]
	public class Result_4
	{
		[VariantTagProperty()]
		public Result_4Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_4(Result_4Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_4()
		{
		}

		public static Result_4 Err(string info)
		{
			return new Result_4(Result_4Tag.Err, info);
		}

		public static Result_4 Ok()
		{
			return new Result_4(Result_4Tag.Ok, null);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_4Tag.Err);
			return (string)this.Value!;
		}

		private void ValidateTag(Result_4Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_4Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		Ok
	}
}