using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using Tokenidentifier1 = System.String;
using Subaccount1 = System.Collections.Generic.List<System.Byte>;

namespace Candid.Extv2Boom.Models
{
	public class ListRequest
	{
		[CandidName("from_subaccount")]
		public ListRequest.FromSubaccountInfo FromSubaccount { get; set; }

		[CandidName("price")]
		public OptionalValue<ulong> Price { get; set; }

		[CandidName("token")]
		public Tokenidentifier1 Token { get; set; }

		public ListRequest(ListRequest.FromSubaccountInfo fromSubaccount, OptionalValue<ulong> price, Tokenidentifier1 token)
		{
			this.FromSubaccount = fromSubaccount;
			this.Price = price;
			this.Token = token;
		}

		public ListRequest()
		{
		}

		public class FromSubaccountInfo : OptionalValue<Subaccount1>
		{
			public FromSubaccountInfo()
			{
			}

			public FromSubaccountInfo(Subaccount1 value) : base(value)
			{
			}
		}
	}
}