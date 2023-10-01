using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using EXTMetadataValue = System.ValueTuple<System.String, Candid.Extv2Standard.Models.EXTMetadataValue>;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetHandle = System.String;
using AccountIdentifier__1 = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.Extv2Standard.Models
{
	public class Settlement
	{
		[CandidName("buyer")]
		public AccountIdentifier__1 Buyer { get; set; }

		[CandidName("price")]
		public ulong Price { get; set; }

		[CandidName("seller")]
		public Principal Seller { get; set; }

		[CandidName("subaccount")]
		public SubAccount__1 Subaccount { get; set; }

		public Settlement(AccountIdentifier__1 buyer, ulong price, Principal seller, SubAccount__1 subaccount)
		{
			this.Buyer = buyer;
			this.Price = price;
			this.Seller = seller;
			this.Subaccount = subaccount;
		}

		public Settlement()
		{
		}
	}
}