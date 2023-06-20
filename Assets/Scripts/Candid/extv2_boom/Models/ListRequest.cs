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
using EdjCase.ICP.Candid.Models;

namespace Candid.extv2_boom.Models
{
	public class ListRequest
	{
		[CandidName("from_subaccount")]
		public OptionalValue<SubAccount__1> FromSubaccount { get; set; }

		[CandidName("price")]
		public OptionalValue<ulong> Price { get; set; }

		[CandidName("token")]
		public TokenIdentifier__1 Token { get; set; }

		public ListRequest(OptionalValue<SubAccount__1> fromSubaccount, OptionalValue<ulong> price, TokenIdentifier__1 token)
		{
			this.FromSubaccount = fromSubaccount;
			this.Price = price;
			this.Token = token;
		}

		public ListRequest()
		{
		}
	}
}