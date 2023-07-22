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