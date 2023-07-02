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
	[Variant(typeof(Result_5Tag))]
	public class Result_5
	{
		[VariantTagProperty()]
		public Result_5Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_5(Result_5Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_5()
		{
		}

		public static Result_5 Err(string info)
		{
			return new Result_5(Result_5Tag.Err, info);
		}

		public static Result_5 Ok(ValueTuple<AccountIdentifier__1, ulong> info)
		{
			return new Result_5(Result_5Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_5Tag.Err);
			return (string)this.Value!;
		}

		public ValueTuple<AccountIdentifier__1, ulong> AsOk()
		{
			this.ValidateTag(Result_5Tag.Ok);
			return (ValueTuple<AccountIdentifier__1, ulong>)this.Value!;
		}

		private void ValidateTag(Result_5Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_5Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(ValueTuple<AccountIdentifier__1, ulong>))]
		Ok
	}
}