using TxIndex__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;
using Subaccount__1 = System.Collections.Generic.List<System.Byte>;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using Memo = System.Collections.Generic.List<System.Byte>;
using Balance__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace Candid.IcrcLedger.Models
{
	public class TokenInitArgs
	{
		[CandidName("advanced_settings")]
		public OptionalValue<AdvancedSettings> AdvancedSettings { get; set; }

		[CandidName("decimals")]
		public byte Decimals { get; set; }

		[CandidName("fee")]
		public Balance Fee { get; set; }

		[CandidName("initial_balances")]
		public List<TokenInitArgs.InitialBalancesItem> InitialBalances { get; set; }

		[CandidName("max_supply")]
		public Balance MaxSupply { get; set; }

		[CandidName("min_burn_amount")]
		public Balance MinBurnAmount { get; set; }

		[CandidName("minting_account")]
		public OptionalValue<Account> MintingAccount { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("symbol")]
		public string Symbol { get; set; }

		public TokenInitArgs(OptionalValue<AdvancedSettings> advancedSettings, byte decimals, Balance fee, List<TokenInitArgs.InitialBalancesItem> initialBalances, Balance maxSupply, Balance minBurnAmount, OptionalValue<Account> mintingAccount, string name, string symbol)
		{
			this.AdvancedSettings = advancedSettings;
			this.Decimals = decimals;
			this.Fee = fee;
			this.InitialBalances = initialBalances;
			this.MaxSupply = maxSupply;
			this.MinBurnAmount = minBurnAmount;
			this.MintingAccount = mintingAccount;
			this.Name = name;
			this.Symbol = symbol;
		}

		public TokenInitArgs()
		{
		}

		public class InitialBalancesItem
		{
			[CandidTag(0U)]
			public Account F0 { get; set; }

			[CandidTag(1U)]
			public Balance F1 { get; set; }

			public InitialBalancesItem(Account f0, Balance f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public InitialBalancesItem()
			{
			}
		}
	}
}