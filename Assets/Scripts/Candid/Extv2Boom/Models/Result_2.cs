using TokenIndex__1 = System.UInt32;
using TokenIdentifier__2 = System.String;
using TokenIdentifier__1 = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.Extv2Boom.Models.MetadataValue>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField__1 = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle__1 = System.String;
using AccountIdentifier__2 = System.String;
using AccountIdentifier__1 = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System.Collections.Generic;
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Boom.Models
{
	[Variant]
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

		public static Result_2 Err(CommonError__1 info)
		{
			return new Result_2(Result_2Tag.Err, info);
		}

		public static Result_2 Ok(List<Result_2.OkItem> info)
		{
			return new Result_2(Result_2Tag.Ok, info);
		}

		public CommonError__1 AsErr()
		{
			this.ValidateTag(Result_2Tag.Err);
			return (CommonError__1)this.Value!;
		}

		public List<Result_2.OkItem> AsOk()
		{
			this.ValidateTag(Result_2Tag.Ok);
			return (List<Result_2.OkItem>)this.Value!;
		}

		private void ValidateTag(Result_2Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class OkItem
		{
			[CandidTag(0U)]
			public TokenIndex__1 F0 { get; set; }

			[CandidTag(1U)]
			public OptionalValue<Listing> F1 { get; set; }

			[CandidTag(2U)]
			public OptionalValue<List<byte>> F2 { get; set; }

			public OkItem(TokenIndex__1 f0, OptionalValue<Listing> f1, OptionalValue<List<byte>> f2)
			{
				this.F0 = f0;
				this.F1 = f1;
				this.F2 = f2;
			}

			public OkItem()
			{
			}
		}
	}

	public enum Result_2Tag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}