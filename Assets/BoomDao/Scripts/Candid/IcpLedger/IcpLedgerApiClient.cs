using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using BlockIndex = System.UInt64;
using Memo = System.UInt64;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using TextAccountIdentifier = System.String;
using Icrc1BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Icrc1Timestamp = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using Candid.IcpLedger;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.IcpLedger
{
	public class IcpLedgerApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public IcpLedgerApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async System.Threading.Tasks.Task<Models.TransferResult> Transfer(Models.TransferArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "transfer", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Tokens> AccountBalance(Models.AccountBalanceArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "account_balance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Tokens>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.TransferFee> TransferFee(Models.TransferFeeArg arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "transfer_fee", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.TransferFee>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.QueryBlocksResponse> QueryBlocks(Models.GetBlocksArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "query_blocks", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.QueryBlocksResponse>(this.Converter);
		}

		public async System.Threading.Tasks.Task<IcpLedgerApiClient.SymbolArg0> Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<IcpLedgerApiClient.SymbolArg0>(this.Converter);
		}

		public async System.Threading.Tasks.Task<IcpLedgerApiClient.NameArg0> Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<IcpLedgerApiClient.NameArg0>(this.Converter);
		}

		public async System.Threading.Tasks.Task<IcpLedgerApiClient.DecimalsArg0> Decimals()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "decimals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<IcpLedgerApiClient.DecimalsArg0>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Archives> Archives()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "archives", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Archives>(this.Converter);
		}

		public async System.Threading.Tasks.Task<BlockIndex> SendDfx(Models.SendArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "send_dfx", arg);
			return reply.ToObjects<BlockIndex>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Tokens> AccountBalanceDfx(Models.AccountBalanceArgsDfx arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "account_balance_dfx", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Tokens>(this.Converter);
		}

		public async System.Threading.Tasks.Task<string> Icrc1Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async System.Threading.Tasks.Task<string> Icrc1Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async System.Threading.Tasks.Task<byte> Icrc1Decimals()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_decimals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<byte>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<IcpLedgerApiClient.Icrc1MetadataArg0Item>> Icrc1Metadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<IcpLedgerApiClient.Icrc1MetadataArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Icrc1Tokens> Icrc1TotalSupply()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_total_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Icrc1Tokens>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Icrc1Tokens> Icrc1Fee()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_fee", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Icrc1Tokens>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<Models.Account>> Icrc1MintingAccount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_minting_account", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Account>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Icrc1Tokens> Icrc1BalanceOf(Models.Account arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_balance_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Icrc1Tokens>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Icrc1TransferResult> Icrc1Transfer(Models.TransferArg arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc1_transfer", arg);
			return reply.ToObjects<Models.Icrc1TransferResult>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<IcpLedgerApiClient.Icrc1SupportedStandardsArg0Item>> Icrc1SupportedStandards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_supported_standards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<IcpLedgerApiClient.Icrc1SupportedStandardsArg0Item>>(this.Converter);
		}

		public class SymbolArg0
		{
			[CandidName("symbol")]
			public string Symbol { get; set; }

			public SymbolArg0(string symbol)
			{
				this.Symbol = symbol;
			}

			public SymbolArg0()
			{
			}
		}

		public class NameArg0
		{
			[CandidName("name")]
			public string Name { get; set; }

			public NameArg0(string name)
			{
				this.Name = name;
			}

			public NameArg0()
			{
			}
		}

		public class DecimalsArg0
		{
			[CandidName("decimals")]
			public uint Decimals { get; set; }

			public DecimalsArg0(uint decimals)
			{
				this.Decimals = decimals;
			}

			public DecimalsArg0()
			{
			}
		}

		public class Icrc1MetadataArg0Item
		{
			[CandidTag(0U)]
			public string F0 { get; set; }

			[CandidTag(1U)]
			public Models.Value F1 { get; set; }

			public Icrc1MetadataArg0Item(string f0, Models.Value f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public Icrc1MetadataArg0Item()
			{
			}
		}

		public class Icrc1SupportedStandardsArg0Item
		{
			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("url")]
			public string Url { get; set; }

			public Icrc1SupportedStandardsArg0Item(string name, string url)
			{
				this.Name = name;
				this.Url = url;
			}

			public Icrc1SupportedStandardsArg0Item()
			{
			}
		}
	}
}