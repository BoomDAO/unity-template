using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class LessThanNumber
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("value")]
		public double Value { get; set; }

		public LessThanNumber(string fieldName, double value)
		{
			this.FieldName = fieldName;
			this.Value = value;
		}

		public LessThanNumber()
		{
		}
	}
}