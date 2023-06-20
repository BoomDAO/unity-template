using TokenIndex = System.UInt32;
using TokenIdentifier__1 = System.String;
using TokenIdentifier = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.extv2_boom.Models.MetadataValue>;
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
using Candid.extv2_boom.Models;

namespace Candid.extv2_boom.Models
{
	public class Payment
	{
		[CandidName("amount")]
		public ulong Amount { get; set; }

		[CandidName("expires")]
		public Time Expires { get; set; }

		[CandidName("payer")]
		public AccountIdentifier__1 Payer { get; set; }

		[CandidName("purchase")]
		public PaymentType Purchase { get; set; }

		[CandidName("subaccount")]
		public SubAccount__1 Subaccount { get; set; }

		public Payment(ulong amount, Time expires, AccountIdentifier__1 payer, PaymentType purchase, SubAccount__1 subaccount)
		{
			this.Amount = amount;
			this.Expires = expires;
			this.Payer = payer;
			this.Purchase = purchase;
			this.Subaccount = subaccount;
		}

		public Payment()
		{
		}
	}
}