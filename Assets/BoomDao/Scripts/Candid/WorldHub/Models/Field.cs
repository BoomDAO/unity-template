using EdjCase.ICP.Candid.Mapping;

namespace Candid.WorldHub.Models
{
	public class Field
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("fieldValue")]
		public string FieldValue { get; set; }

		public Field(string fieldName, string fieldValue)
		{
			this.FieldName = fieldName;
			this.FieldValue = fieldValue;
		}

		public Field()
		{
		}
	}
}