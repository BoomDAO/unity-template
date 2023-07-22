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

		public static Result_3 Err(string info)
		{
			return new Result_3(Result_3Tag.Err, info);
		}

		public static Result_3 Ok(List<Action> info)
		{
			return new Result_3(Result_3Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_3Tag.Err);
			return (string)this.Value!;
		}

		public List<Action> AsOk()
		{
			this.ValidateTag(Result_3Tag.Ok);
			return (List<Action>)this.Value!;
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
		[VariantOptionType(typeof(string))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(List<Action>))]
		Ok
	}
}