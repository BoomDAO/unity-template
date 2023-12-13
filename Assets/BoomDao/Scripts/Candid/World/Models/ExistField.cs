using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class ExistField
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("value")]
		public bool Value { get; set; }

		public ExistField(string fieldName, bool value)
		{
			this.FieldName = fieldName;
			this.Value = value;
		}

		public ExistField()
		{
		}
	}
}