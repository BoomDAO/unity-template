using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public enum Result_2
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}