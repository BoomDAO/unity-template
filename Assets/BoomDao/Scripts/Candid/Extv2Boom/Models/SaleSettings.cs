using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Candid.Extv2Boom.Models
{
	public class SaleSettings
	{
		[CandidName("bulkPricing")]
		public Dictionary<ulong, ulong> BulkPricing { get; set; }

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

		public SaleSettings(Dictionary<ulong, ulong> bulkPricing, ulong price, UnboundedUInt remaining, ulong salePrice, UnboundedUInt sold, Time startTime, UnboundedUInt totalToSell, bool whitelist, Time whitelistTime)
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