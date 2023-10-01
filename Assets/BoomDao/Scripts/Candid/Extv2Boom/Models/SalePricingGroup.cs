using TokenIndex__1 = System.UInt32;
using TokenIdentifier__2 = System.String;
using TokenIdentifier__1 = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.Extv2Boom.Models.MetadataValue>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField__1 = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle__1 = System.String;
using AccountIdentifier__2 = System.String;
using AccountIdentifier__1 = System.String;
using EdjCase.ICP.Candid.Mapping;
using System;
using System.Collections.Generic;

namespace Candid.Extv2Boom.Models
{
	public class SalePricingGroup
	{
		[CandidName("end")]
		public Time End { get; set; }

		[CandidName("limit")]
		public ValueTuple<ulong, ulong> Limit { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("participants")]
		public List<AccountIdentifier__2> Participants { get; set; }

		[CandidName("pricing")]
		public List<ValueTuple<ulong, ulong>> Pricing { get; set; }

		[CandidName("start")]
		public Time Start { get; set; }

		public SalePricingGroup(Time end, ValueTuple<ulong, ulong> limit, string name, List<AccountIdentifier__2> participants, List<ValueTuple<ulong, ulong>> pricing, Time start)
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