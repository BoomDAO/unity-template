using worldId = EdjCase.ICP.Candid.Models.OptionalValue<System.String>;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class MintToken__1
	{
		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("description")]
		public string Description { get; set; }

		[CandidName("imageUrl")]
		public string ImageUrl { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		public MintToken__1(string canister, string description, string imageUrl, string name)
		{
			this.Canister = canister;
			this.Description = description;
			this.ImageUrl = imageUrl;
			this.Name = name;
		}

		public MintToken__1()
		{
		}
	}
}