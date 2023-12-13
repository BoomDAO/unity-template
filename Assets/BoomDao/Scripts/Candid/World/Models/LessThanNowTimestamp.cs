using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class LessThanNowTimestamp
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		public LessThanNowTimestamp(string fieldName)
		{
			this.FieldName = fieldName;
		}

		public LessThanNowTimestamp()
		{
		}
	}
}