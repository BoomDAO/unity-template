using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.Extv2Boom.Models;
using System.Collections.Generic;
using TokenIndex = System.UInt32;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	public class SaleTransaction
	{
		[CandidName("buyer")]
		public Accountidentifier1 Buyer { get; set; }

		[CandidName("price")]
		public ulong Price { get; set; }

		[CandidName("seller")]
		public Principal Seller { get; set; }

		[CandidName("time")]
		public Time Time { get; set; }

		[CandidName("tokens")]
		public SaleTransaction.TokensInfo Tokens { get; set; }

		public SaleTransaction(Accountidentifier1 buyer, ulong price, Principal seller, Time time, SaleTransaction.TokensInfo tokens)
		{
			this.Buyer = buyer;
			this.Price = price;
			this.Seller = seller;
			this.Time = time;
			this.Tokens = tokens;
		}

		public SaleTransaction()
		{
		}

		public class TokensInfo : List<TokenIndex>
		{
			public TokensInfo()
			{
			}
		}
	}
}