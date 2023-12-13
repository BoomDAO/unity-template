using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class EqualToNumber
	{
		[CandidName("equal")]
		public bool Equal { get; set; }

		[CandidName("fieldName")]
		public string FieldName { get; set; }

		[CandidName("value")]
		public double Value { get; set; }

		public EqualToNumber(bool equal, string fieldName, double value)
		{
			this.Equal = equal;
			this.FieldName = fieldName;
			this.Value = value;
		}

		public EqualToNumber()
		{
		}
	}
}