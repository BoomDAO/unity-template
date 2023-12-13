using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public enum Result2
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}