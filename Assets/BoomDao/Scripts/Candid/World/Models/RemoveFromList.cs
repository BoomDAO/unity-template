using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class RemoveFromList
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("value")]
		public string Value { get; set; }

		public RemoveFromList(string fieldName, string value)
		{
			this.FieldName = fieldName;
			this.Value = value;
		}

		public RemoveFromList()
		{
		}
	}
}