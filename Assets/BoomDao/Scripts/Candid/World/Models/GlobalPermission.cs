using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
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