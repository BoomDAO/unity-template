using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	public class SalePricingGroup
	{
		[CandidName("end")]
		public Time End { get; set; }

		[CandidName("limit")]
		public (ulong, ulong) Limit { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("participants")]
		public List<Accountidentifier1> Participants { get; set; }

		[CandidName("pricing")]
		public Dictionary<ulong, ulong> Pricing { get; set; }

		[CandidName("start")]
		public Time Start { get; set; }

		public SalePricingGroup(Time end, (ulong, ulong) limit, string name, List<Accountidentifier1> participants, Dictionary<ulong, ulong> pricing, Time start)
		{
			this.End = end;
			this.Limit = limit;
			this.Name = name;
			this.Participants = participants;
			this.Pricing = pricing;
			this.Start = start;
		}

		public SalePricingGroup()
		{
		}
	}
}