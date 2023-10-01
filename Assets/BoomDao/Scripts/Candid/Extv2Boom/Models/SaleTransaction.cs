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
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace Candid.Extv2Boom.Models
{
	public class SaleTransaction
	{
		[CandidName("buyer")]
		public AccountIdentifier__2 Buyer { get; set; }

		[CandidName("price")]
		public ulong Price { get; set; }

		[CandidName("seller")]
		public Principal Seller { get; set; }

		[CandidName("time")]
		public Time Time { get; set; }

		[CandidName("tokens")]
		public List<TokenIndex__1> Tokens { get; set; }

		public SaleTransaction(AccountIdentifier__2 buyer, ulong price, Principal seller, Time time, List<TokenIndex__1> tokens)
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
	}
}