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
using EdjCase.ICP.Candid.Models;

namespace Candid.extv2_boom.Models
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