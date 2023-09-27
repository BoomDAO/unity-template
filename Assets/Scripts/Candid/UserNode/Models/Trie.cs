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
	public class Trie
	{
		[VariantTagProperty()]
		public TrieTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Trie(TrieTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Trie()
		{
		}

		public static Trie Branch(Branch info)
		{
			return new Trie(TrieTag.Branch, info);
		}

		public static Trie Empty()
		{
			return new Trie(TrieTag.Empty, null);
		}

		public static Trie Leaf(Leaf info)
		{
			return new Trie(TrieTag.Leaf, info);
		}

		public Branch AsBranch()
		{
			this.ValidateTag(TrieTag.Branch);
			return (Branch)this.Value!;
		}

		public Leaf AsLeaf()
		{
			this.ValidateTag(TrieTag.Leaf);
			return (Leaf)this.Value!;
		}

		private void ValidateTag(TrieTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum TrieTag
	{
		[CandidName("branch")]
		
		Branch,
		[CandidName("empty")]
		Empty,
		[CandidName("leaf")]
		
		Leaf
	}
}