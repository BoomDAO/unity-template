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
using EdjCase.ICP.Candid.Models;

namespace Candid.UserNode.Models
{
	public class Leaf_1
	{
		[CandidName("keyvals")]
		public AssocList_1 Keyvals { get; set; }

		[CandidName("size")]
		public UnboundedUInt Size { get; set; }

		public Leaf_1(AssocList_1 keyvals, UnboundedUInt size)
		{
			this.Keyvals = keyvals;
			this.Size = size;
		}

		public Leaf_1()
		{
		}
	}
}