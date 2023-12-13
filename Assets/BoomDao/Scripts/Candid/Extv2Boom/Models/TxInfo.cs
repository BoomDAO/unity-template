using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using TokenIndex = System.UInt32;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom.Models
{
	public class TxInfo
	{
		[CandidName("current_holder")]
		public Accountidentifier1 CurrentHolder { get; set; }

		[CandidName("index")]
		public TokenIndex Index { get; set; }

		[CandidName("kind")]
		public TxInfo.KindInfo Kind { get; set; }

		[CandidName("metadata")]
		public OptionalValue<string> Metadata { get; set; }

		[CandidName("previous_holder")]
		public Accountidentifier1 PreviousHolder { get; set; }

		[CandidName("txid")]
		public string Txid { get; set; }

		public TxInfo(Accountidentifier1 currentHolder, TokenIndex index, TxInfo.KindInfo kind, OptionalValue<string> metadata, Accountidentifier1 previousHolder, string txid)
		{
			this.CurrentHolder = currentHolder;
			this.Index = index;
			this.Kind = kind;
			this.Metadata = metadata;
			this.PreviousHolder = previousHolder;
			this.Txid = txid;
		}

		public TxInfo()
		{
		}

		public enum KindInfo
		{
			[CandidName("hold")]
			Hold,
			[CandidName("transfer")]
			Transfer
		}
	}
}