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

namespace Candid.Extv2Standard.Models
{
	public class Asset
	{
		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("ctype")]
		public string Ctype { get; set; }

		[CandidName("id")]
		public uint Id { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		public Asset(string canister, string ctype, uint id, string name)
		{
			this.Canister = canister;
			this.Ctype = ctype;
			this.Id = id;
			this.Name = name;
		}

		public Asset()
		{
		}
	}
}