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
using EdjCase.ICP.Candid.Models;

namespace Candid.UserNode.Models
{
	public class Branch_1
	{
		[CandidName("left")]
		public Trie_1 Left { get; set; }

		[CandidName("right")]
		public Trie_1 Right { get; set; }

		[CandidName("size")]
		public UnboundedUInt Size { get; set; }

		public Branch_1(Trie_1 left, Trie_1 right, UnboundedUInt size)
		{
			this.Left = left;
			this.Right = right;
			this.Size = size;
		}

		public Branch_1()
		{
		}
	}
}