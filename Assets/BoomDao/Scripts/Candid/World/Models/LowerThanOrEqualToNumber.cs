using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class LowerThanOrEqualToNumber
	{
		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("value")]
		public double Value { get; set; }

		public LowerThanOrEqualToNumber(string fieldName, double value)
		{
			this.FieldName = fieldName;
			this.Value = value;
		}

		public LowerThanOrEqualToNumber()
		{
		}
	}
}