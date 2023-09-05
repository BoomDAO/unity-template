using worldId = System.String;
using userId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using actionId = System.String;
using List_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.List_1Item>;
using List = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.ListItem>;
using Hash = System.UInt32;
using AssocList_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocList_1Item>;
using AssocList = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocListItem>;
using EdjCase.ICP.Candid.Mapping;
using Candid.UserNode.Models;
using System.Collections.Generic;
using System;

namespace Candid.UserNode.Models
{
	[Variant(typeof(Result_1Tag))]
	public class Result_1
	{
		[VariantTagProperty()]
		public Result_1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_1(Result_1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_1()
		{
		}

		public static Result_1 Err(string info)
		{
			return new Result_1(Result_1Tag.Err, info);
		}

		public static Result_1 Ok(List<Entity> info)
		{
			return new Result_1(Result_1Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_1Tag.Err);
			return (string)this.Value!;
		}

		public List<Entity> AsOk()
		{
			this.ValidateTag(Result_1Tag.Ok);
			return (List<Entity>)this.Value!;
		}

		private void ValidateTag(Result_1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_1Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(List<Entity>))]
		Ok
	}
}