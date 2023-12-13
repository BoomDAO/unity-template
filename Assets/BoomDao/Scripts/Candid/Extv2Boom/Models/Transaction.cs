using EdjCase.ICP.Candid.Mapping;
using TokenIndex = System.UInt32;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	public class Transaction
	{
		[CandidName("buyer")]
		public Accountidentifier1 Buyer { get; set; }

		[CandidName("price")]
		public ulong Price { get; set; }

		[CandidName("seller")]
		public Accountidentifier1 Seller { get; set; }

		[CandidName("time")]
		public Time Time { get; set; }

		[CandidName("token")]
		public TokenIndex Token { get; set; }

		public Transaction(Accountidentifier1 buyer, ulong price, Accountidentifier1 seller, Time time, TokenIndex token)
		{
			this.Buyer = buyer;
			this.Price = price;
			this.Seller = seller;
			this.Time = time;
			this.Token = token;
		}

		public Transaction()
		{
		}
	}
}