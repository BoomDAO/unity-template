using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Candid.Extv2Boom.Models
{
	public class Sale
	{
		[CandidName("end")]
		public Time End { get; set; }

		[CandidName("groups")]
		public List<SalePricingGroup> Groups { get; set; }

		[CandidName("quantity")]
		public UnboundedUInt Quantity { get; set; }

		[CandidName("remaining")]
		public SaleRemaining Remaining { get; set; }

		[CandidName("start")]
		public Time Start { get; set; }

		public Sale(Time end, List<SalePricingGroup> groups, UnboundedUInt quantity, SaleRemaining remaining, Time start)
		{
			this.End = end;
			this.Groups = groups;
			this.Quantity = quantity;
			this.Remaining = remaining;
			this.Start = start;
		}

		public Sale()
		{
		}
	}
}