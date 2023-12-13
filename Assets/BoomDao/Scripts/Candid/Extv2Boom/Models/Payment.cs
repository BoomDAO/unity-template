using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using Subaccount1 = System.Collections.Generic.List<System.Byte>;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	public class Payment
	{
		[CandidName("amount")]
		public ulong Amount { get; set; }

		[CandidName("expires")]
		public Time Expires { get; set; }

		[CandidName("payer")]
		public Accountidentifier1 Payer { get; set; }

		[CandidName("purchase")]
		public PaymentType Purchase { get; set; }

		[CandidName("subaccount")]
		public Subaccount1 Subaccount { get; set; }

		public Payment(ulong amount, Time expires, Accountidentifier1 payer, PaymentType purchase, Subaccount1 subaccount)
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