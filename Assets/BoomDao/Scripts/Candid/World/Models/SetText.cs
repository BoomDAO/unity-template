using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class SetText
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("fieldValue")]
		public string FieldValue { get; set; }

		public SetText(string fieldName, string fieldValue)
		{
			this.FieldName = fieldName;
			this.FieldValue = fieldValue;
		}

		public SetText()
		{
		}
	}
}