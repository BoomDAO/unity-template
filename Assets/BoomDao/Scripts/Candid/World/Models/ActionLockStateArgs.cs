using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class ActionLockStateArgs
	{
		[CandidName("aid")]
		public string Aid { get; set; }

		[CandidName("uid")]
		public string Uid { get; set; }

		public ActionLockStateArgs(string aid, string uid)
		{
			this.Aid = aid;
			this.Uid = uid;
		}

		public ActionLockStateArgs()
		{
		}
	}
}