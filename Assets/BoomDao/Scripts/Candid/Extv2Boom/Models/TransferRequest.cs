using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using TokenIdentifier = System.String;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.Collections.Generic.List<System.Byte>;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Candid.Extv2Boom.Models
{
	public class TransferRequest
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("from")]
		public User From { get; set; }

		[CandidName("memo")]
		public Memo Memo { get; set; }

		[CandidName("notify")]
		public bool Notify { get; set; }

		[CandidName("subaccount")]
		public OptionalValue<SubAccount> Subaccount { get; set; }

		[CandidName("to")]
		public User To { get; set; }

		[CandidName("token")]
		public TokenIdentifier Token { get; set; }

		public TransferRequest(Balance amount, User from, Memo memo, bool notify, OptionalValue<SubAccount> subaccount, User to, TokenIdentifier token)
		{
			this.Amount = amount;
			this.From = from;
			this.Memo = memo;
			this.Notify = notify;
			this.Subaccount = subaccount;
			this.To = to;
			this.Token = token;
		}

		public TransferRequest()
		{
		}
	}
}