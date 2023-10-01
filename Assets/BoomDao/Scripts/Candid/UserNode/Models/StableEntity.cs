using worldId = System.String;
using userId = System.String;
using groupId = System.String;
using entityId = System.String;
using actionId = System.String;
using List_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.List_1Item>;
using List = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.ListItem>;
using Hash = System.UInt32;
using AssocList_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocList_1Item>;
using AssocList = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocListItem>;
using EdjCase.ICP.Candid.Mapping;
using System;
using System.Collections.Generic;

namespace Candid.UserNode.Models
{
	public class StableEntity
	{
		[CandidName("eid")]
		public entityId Eid { get; set; }

		[CandidName("fields")]
		public List<ValueTuple<string, string>> Fields { get; set; }

		[CandidName("gid")]
		public groupId Gid { get; set; }

		[CandidName("wid")]
		public worldId Wid { get; set; }

		public StableEntity(entityId eid, List<ValueTuple<string, string>> fields, groupId gid, worldId wid)
		{
			this.Eid = eid;
			this.Fields = fields;
			this.Gid = gid;
			this.Wid = wid;
		}

		public StableEntity()
		{
		}
	}
}