using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public enum Result_3
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}