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
using System;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.ext_v2_standard.Models
{
	public class SaleSettings
	{
		[CandidName("bulkPricing")]
		public List<ValueTuple<ulong, ulong>> BulkPricing { get; set; }

		[CandidName("price")]
		public ulong Price { get; set; }

		[CandidName("remaining")]
		public UnboundedUInt Remaining { get; set; }

		[CandidName("salePrice")]
		public ulong SalePrice { get; set; }

		[CandidName("sold")]
		public UnboundedUInt Sold { get; set; }

		[CandidName("startTime")]
		public Time StartTime { get; set; }

		[CandidName("totalToSell")]
		public UnboundedUInt TotalToSell { get; set; }

		[CandidName("whitelist")]
		public bool Whitelist { get; set; }

		[CandidName("whitelistTime")]
		public Time WhitelistTime { get; set; }

		public SaleSettings(List<ValueTuple<ulong, ulong>> bulkPricing, ulong price, UnboundedUInt remaining, ulong salePrice, UnboundedUInt sold, Time startTime, UnboundedUInt totalToSell, bool whitelist, Time whitelistTime)
		{
			this.BulkPricing = bulkPricing;
			this.Price = price;
			this.Remaining = remaining;
			this.SalePrice = salePrice;
			this.Sold = sold;
			this.StartTime = startTime;
			this.TotalToSell = totalToSell;
			this.Whitelist = whitelist;
			this.WhitelistTime = whitelistTime;
		}

		public SaleSettings()
		{
		}
	}
}