using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class Result__1
	{
		[VariantTagProperty()]
		public Result__1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result__1(Result__1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result__1()
		{
		}

		public static Result__1 Err(TransferError info)
		{
			return new Result__1(Result__1Tag.Err, info);
		}

		public static Result__1 Ok(UnboundedUInt info)
		{
			return new Result__1(Result__1Tag.Ok, info);
		}

		public TransferError AsErr()
		{
			this.ValidateTag(Result__1Tag.Err);
			return (TransferError)this.Value!;
		}

		public UnboundedUInt AsOk()
		{
			this.ValidateTag(Result__1Tag.Ok);
			return (UnboundedUInt)this.Value!;
		}

		private void ValidateTag(Result__1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result__1Tag
	{
		
		Err,
		
		Ok
	}
}