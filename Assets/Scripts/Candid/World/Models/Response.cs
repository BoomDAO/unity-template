using worldId = EdjCase.ICP.Candid.Models.OptionalValue<System.String>;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;

namespace Candid.World.Models
{
	public class Response
	{
		[CandidTag(0U)]
		public Action F0 { get; set; }

		[CandidTag(1U)]
		public List<Entity> F1 { get; set; }

		[CandidTag(2U)]
		public List<MintNft__1> F2 { get; set; }

		[CandidTag(3U)]
		public List<MintToken__1> F3 { get; set; }

		public Response(Action f0, List<Entity> f1, List<MintNft__1> f2, List<MintToken__1> f3)
		{
			this.F0 = f0;
			this.F1 = f1;
			this.F2 = f2;
			this.F3 = f3;
		}

		public Response()
		{
		}
	}
}