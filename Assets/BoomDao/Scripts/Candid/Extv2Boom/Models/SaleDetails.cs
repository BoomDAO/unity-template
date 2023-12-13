using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Candid.Extv2Boom.Models
{
	public class SaleDetails
	{
		[CandidName("end")]
		public Time End { get; set; }

		[CandidName("groups")]
		public List<SaleDetailGroup> Groups { get; set; }

		[CandidName("quantity")]
		public UnboundedUInt Quantity { get; set; }

		[CandidName("remaining")]
		public UnboundedUInt Remaining { get; set; }

		[CandidName("start")]
		public Time Start { get; set; }

		public SaleDetails(Time end, List<SaleDetailGroup> groups, UnboundedUInt quantity, UnboundedUInt remaining, Time start)
		{
			this.End = end;
			this.Groups = groups;
			this.Quantity = quantity;
			this.Remaining = remaining;
			this.Start = start;
		}

		public SaleDetails()
		{
		}
	}
}