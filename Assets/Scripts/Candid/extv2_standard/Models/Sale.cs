using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.ext_v2_standard.Models.MetadataValueItem>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.ext_v2_standard.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.ext_v2_standard.Models
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