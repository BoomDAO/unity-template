using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class Exist
	{
		[CandidName("value")]
		public bool Value { get; set; }

		public Exist(bool value)
		{
			this.Value = value;
		}

		public Exist()
		{
		}
	}
}