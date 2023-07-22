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

namespace Candid.UserNode.Models
{
	public class List_1Item
	{
		[CandidTag(0U)]
		public List_1Item.F0Info F0 { get; set; }

		[CandidTag(1U)]
		public List_1 F1 { get; set; }

		public List_1Item(List_1Item.F0Info f0, List_1 f1)
		{
			this.F0 = f0;
			this.F1 = f1;
		}

		public List_1Item()
		{
		}

		public class F0Info
		{
			[CandidTag(0U)]
			public Key_1 F0 { get; set; }

			[CandidTag(1U)]
			public EntityPermission F1 { get; set; }

			public F0Info(Key_1 f0, EntityPermission f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public F0Info()
			{
			}
		}
	}
}