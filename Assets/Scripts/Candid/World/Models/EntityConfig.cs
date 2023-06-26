using worldId = EdjCase.ICP.Candid.Models.OptionalValue<System.String>;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class EntityConfig
	{
		[CandidName("description")]
		public OptionalValue<string> Description { get; set; }

		[CandidName("duration")]
		public OptionalValue<UnboundedUInt> Duration { get; set; }

		[CandidName("eid")]
		public string Eid { get; set; }

		[CandidName("gid")]
		public string Gid { get; set; }

		[CandidName("imageUrl")]
		public OptionalValue<string> ImageUrl { get; set; }

		[CandidName("metadata")]
		public string Metadata { get; set; }

		[CandidName("name")]
		public OptionalValue<string> Name { get; set; }

		[CandidName("objectUrl")]
		public OptionalValue<string> ObjectUrl { get; set; }

		[CandidName("rarity")]
		public OptionalValue<string> Rarity { get; set; }

		[CandidName("tag")]
		public string Tag { get; set; }

		public EntityConfig(OptionalValue<string> description, OptionalValue<UnboundedUInt> duration, string eid, string gid, OptionalValue<string> imageUrl, string metadata, OptionalValue<string> name, OptionalValue<string> objectUrl, OptionalValue<string> rarity, string tag)
		{
			this.Description = description;
			this.Duration = duration;
			this.Eid = eid;
			this.Gid = gid;
			this.ImageUrl = imageUrl;
			this.Metadata = metadata;
			this.Name = name;
			this.ObjectUrl = objectUrl;
			this.Rarity = rarity;
			this.Tag = tag;
		}

		public EntityConfig()
		{
		}
	}
}