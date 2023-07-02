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

namespace Candid.ext_v2_standard.Models
{
	public class Transaction
	{
		[CandidName("buyer")]
		public AccountIdentifier__1 Buyer { get; set; }

		[CandidName("price")]
		public ulong Price { get; set; }

		[CandidName("seller")]
		public AccountIdentifier__1 Seller { get; set; }

		[CandidName("time")]
		public Time Time { get; set; }

		[CandidName("token")]
		public TokenIndex Token { get; set; }

		public Transaction(AccountIdentifier__1 buyer, ulong price, AccountIdentifier__1 seller, Time time, TokenIndex token)
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