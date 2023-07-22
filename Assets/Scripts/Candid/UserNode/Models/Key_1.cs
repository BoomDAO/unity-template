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

namespace Candid.UserNode.Models
{
	public class Key_1
	{
		[CandidName("hash")]
		public Hash Hash { get; set; }

		[CandidName("key")]
		public string Key { get; set; }

		public Key_1(Hash hash, string key)
		{
			this.Hash = hash;
			this.Key = key;
		}

		public Key_1()
		{
		}
	}
}