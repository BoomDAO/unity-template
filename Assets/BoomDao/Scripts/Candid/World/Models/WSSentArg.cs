using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;
using System;

namespace Candid.World.Models
{
	[Variant]
	public class WSSentArg
	{
		[VariantTagProperty]
		public WSSentArgTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public WSSentArg(WSSentArgTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected WSSentArg()
		{
		}

		public static WSSentArg ActionOutcomes(ActionReturn info)
		{
			return new WSSentArg(WSSentArgTag.ActionOutcomes, info);
		}

		public static WSSentArg UserIdsToFetchDataFrom(List<string> info)
		{
			return new WSSentArg(WSSentArgTag.UserIdsToFetchDataFrom, info);
		}

		public ActionReturn AsActionOutcomes()
		{
			this.ValidateTag(WSSentArgTag.ActionOutcomes);
			return (ActionReturn)this.Value!;
		}

		public List<string> AsUserIdsToFetchDataFrom()
		{
			this.ValidateTag(WSSentArgTag.UserIdsToFetchDataFrom);
			return (List<string>)this.Value!;
		}

		private void ValidateTag(WSSentArgTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum WSSentArgTag
	{
		[CandidName("actionOutcomes")]
		ActionOutcomes,
		[CandidName("userIdsToFetchDataFrom")]
		UserIdsToFetchDataFrom
	}
}