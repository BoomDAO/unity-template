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

		public static Result_5 Ok(Result_5.OkInfo info)
		{
			return new Result_5(Result_5Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_5Tag.Err);
			return (string)this.Value!;
		}

		public Result_5.OkInfo AsOk()
		{
			this.ValidateTag(Result_5Tag.Ok);
			return (Result_5.OkInfo)this.Value!;
		}

		private void ValidateTag(Result_5Tag tag)
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
			public ulong F1 { get; set; }

			public OkInfo(AccountIdentifier__1 f0, ulong f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public OkInfo()
			{
			}
		}
	}

	public enum Result_5Tag
	{
		[CandidName("err")]
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}