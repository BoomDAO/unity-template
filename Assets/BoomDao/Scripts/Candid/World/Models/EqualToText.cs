using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class EqualToText
	{
		[CandidName("equal")]
		public bool Equal { get; set; }

		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("value")]
		public string Value { get; set; }

		public EqualToText(bool equal, string fieldName, string value)
		{
			this.Equal = equal;
			this.FieldName = fieldName;
			this.Value = value;
		}

		public EqualToText()
		{
		}
	}
}