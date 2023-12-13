using EdjCase.ICP.Candid.Mapping;
using WorldId = System.String;

namespace Candid.World.Models
{
	public class GlobalPermission
	{
		[CandidName("wid")]
		public WorldId Wid { get; set; }

		public GlobalPermission(WorldId wid)
		{
			this.Wid = wid;
		}

		public GlobalPermission()
		{
		}
	}
}