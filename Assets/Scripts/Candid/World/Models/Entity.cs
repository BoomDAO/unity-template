using worldId = System.String;
using userId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class Entity
	{
		[CandidName("attribute")]
		public OptionalValue<string> Attribute { get; set; }

		[CandidName("eid")]
		public string Eid { get; set; }

		[CandidName("expiration")]
		public OptionalValue<UnboundedUInt> Expiration { get; set; }

		[CandidName("gid")]
		public string Gid { get; set; }

		[CandidName("quantity")]
		public OptionalValue<double> Quantity { get; set; }

		[CandidName("wid")]
		public string Wid { get; set; }

		public Entity(OptionalValue<string> attribute, string eid, OptionalValue<UnboundedUInt> expiration, string gid, OptionalValue<double> quantity, string wid)
		{
			this.Attribute = attribute;
			this.Eid = eid;
			this.Expiration = expiration;
			this.Gid = gid;
			this.Quantity = quantity;
			this.Wid = wid;
		}

		public Entity()
		{
		}
	}
}