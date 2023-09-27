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

		public static Result_2 Err(string info)
		{
			return new Result_2(Result_2Tag.Err, info);
		}

		public static Result_2 Ok(List<Action> info)
		{
			return new Result_2(Result_2Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result_2Tag.Err);
			return (string)this.Value!;
		}

		public List<Action> AsOk()
		{
			this.ValidateTag(Result_2Tag.Ok);
			return (List<Action>)this.Value!;
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
		
		Err,
		[CandidName("ok")]
		
		Ok
	}
}