using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using BlockIndex = System.UInt64;
using Memo = System.UInt64;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using TextAccountIdentifier = System.String;
using Icrc1BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Icrc1Timestamp = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using Candid.IcpLedger.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Candid.IcpLedger.Models
{
	public class LedgerCanisterInitPayload
	{
		[CandidName("minting_account")]
		public TextAccountIdentifier MintingAccount { get; set; }

		[CandidName("initial_values")]
		public List<LedgerCanisterInitPayload.InitialValuesItem> InitialValues { get; set; }

		[CandidName("max_message_size_bytes")]
		public OptionalValue<ulong> MaxMessageSizeBytes { get; set; }

		[CandidName("transaction_window")]
		public OptionalValue<Duration> TransactionWindow { get; set; }

		[CandidName("archive_options")]
		public OptionalValue<ArchiveOptions> ArchiveOptions { get; set; }

		[CandidName("send_whitelist")]
		public List<Principal> SendWhitelist { get; set; }

		[CandidName("transfer_fee")]
		public OptionalValue<Tokens> TransferFee { get; set; }

		[CandidName("token_symbol")]
		public OptionalValue<string> TokenSymbol { get; set; }

		[CandidName("token_name")]
		public OptionalValue<string> TokenName { get; set; }

		public LedgerCanisterInitPayload(TextAccountIdentifier mintingAccount, List<LedgerCanisterInitPayload.InitialValuesItem> initialValues, OptionalValue<ulong> maxMessageSizeBytes, OptionalValue<Duration> transactionWindow, OptionalValue<ArchiveOptions> archiveOptions, List<Principal> sendWhitelist, OptionalValue<Tokens> transferFee, OptionalValue<string> tokenSymbol, OptionalValue<string> tokenName)
		{
			this.MintingAccount = mintingAccount;
			this.InitialValues = initialValues;
			this.MaxMessageSizeBytes = maxMessageSizeBytes;
			this.TransactionWindow = transactionWindow;
			this.ArchiveOptions = archiveOptions;
			this.SendWhitelist = sendWhitelist;
			this.TransferFee = transferFee;
			this.TokenSymbol = tokenSymbol;
			this.TokenName = tokenName;
		}

		public LedgerCanisterInitPayload()
		{
		}

		public class InitialValuesItem
		{
			[CandidTag(0U)]
			public TextAccountIdentifier F0 { get; set; }

			[CandidTag(1U)]
			public Tokens F1 { get; set; }

			public InitialValuesItem(TextAccountIdentifier f0, Tokens f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public InitialValuesItem()
			{
			}
		}
	}
}