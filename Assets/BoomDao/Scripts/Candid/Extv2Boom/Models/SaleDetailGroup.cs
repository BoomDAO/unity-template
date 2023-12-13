using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Candid.Extv2Boom.Models
{
	public class SaleDetailGroup
	{
		[CandidName("available")]
		public bool Available { get; set; }

		[CandidName("end")]
		public Time End { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("pricing")]
		public Dictionary<ulong, ulong> Pricing { get; set; }

		[CandidName("start")]
		public Time Start { get; set; }

		public SaleDetailGroup(bool available, Time end, UnboundedUInt id, string name, Dictionary<ulong, ulong> pricing, Time start)
		{
			this.Available = available;
			this.End = end;
			this.Id = id;
			this.Name = name;
			this.Pricing = pricing;
			this.Start = start;
		}

		public SaleDetailGroup()
		{
		}
	}
}