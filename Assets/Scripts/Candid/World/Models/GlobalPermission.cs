using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class GlobalPermission
	{
		[CandidName("wid")]
		public worldId Wid { get; set; }

		public GlobalPermission(worldId wid)
		{
			this.Wid = wid;
		}

		public GlobalPermission()
		{
		}
	}
}