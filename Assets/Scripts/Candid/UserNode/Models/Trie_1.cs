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
using System;

namespace Candid.UserNode.Models
{
	[Variant]
	public class Trie_1
	{
		[VariantTagProperty()]
		public Trie_1Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Trie_1(Trie_1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Trie_1()
		{
		}

		public static Trie_1 Branch(Branch_1 info)
		{
			return new Trie_1(Trie_1Tag.Branch, info);
		}

		public static Trie_1 Empty()
		{
			return new Trie_1(Trie_1Tag.Empty, null);
		}

		public static Trie_1 Leaf(Leaf_1 info)
		{
			return new Trie_1(Trie_1Tag.Leaf, info);
		}

		public Branch_1 AsBranch()
		{
			this.ValidateTag(Trie_1Tag.Branch);
			return (Branch_1)this.Value!;
		}

		public Leaf_1 AsLeaf()
		{
			this.ValidateTag(Trie_1Tag.Leaf);
			return (Leaf_1)this.Value!;
		}

		private void ValidateTag(Trie_1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Trie_1Tag
	{
		[CandidName("branch")]
		
		Branch,
		[CandidName("empty")]
		Empty,
		[CandidName("leaf")]
		
		Leaf
	}
}