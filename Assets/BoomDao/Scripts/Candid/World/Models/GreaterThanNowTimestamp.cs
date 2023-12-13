using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class GreaterThanNowTimestamp
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		public GreaterThanNowTimestamp(string fieldName)
		{
			this.FieldName = fieldName;
		}

		public GreaterThanNowTimestamp()
		{
		}
	}
}