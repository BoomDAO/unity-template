using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class ContainsText
	{
		[CandidName("contains")]
		public bool Contains { get; set; }

		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("value")]
		public string Value { get; set; }

		public ContainsText(bool contains, string fieldName, string value)
		{
			this.Contains = contains;
			this.FieldName = fieldName;
			this.Value = value;
		}

		public ContainsText()
		{
		}
	}
}