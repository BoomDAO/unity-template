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
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using Candid.IcrcLedger;
using System.Threading.Tasks;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;

namespace Candid.IcrcLedger
{
	public class IcrcLedgerApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public IcrcLedgerApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async System.Threading.Tasks.Task<Models.TransferResult> Burn(Models.BurnArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "burn", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task DepositCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "deposit_cycles", arg);
		}

		public async System.Threading.Tasks.Task<OptionalValue<Models.Transaction__2>> GetTransaction(TxIndex__2 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "get_transaction", arg);
			return reply.ToObjects<OptionalValue<Models.Transaction__2>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Balance__2> Icrc1BalanceOf(Models.Account__2 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_balance_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance__2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<byte> Icrc1Decimals()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_decimals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<byte>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Balance__2> Icrc1Fee()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_fee", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance__2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.MetaDatum>> Icrc1Metadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.MetaDatum>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<Models.Account__2>> Icrc1MintingAccount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_minting_account", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Account__2>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<string> Icrc1Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.SupportedStandard>> Icrc1SupportedStandards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_supported_standards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SupportedStandard>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<string> Icrc1Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Balance__2> Icrc1TotalSupply()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_total_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance__2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.TransferResult> Icrc1Transfer(Models.TransferArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc1_transfer", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Allowance> Icrc2Allowance(Models.AllowanceArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc2_allowance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Allowance>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.ApproveResult> Icrc2Approve(Models.ApproveArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc2_approve", arg);
			return reply.ToObjects<Models.ApproveResult>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.TransferFromResult> Icrc2TransferFrom(Models.TransferFromArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc2_transfer_from", arg);
			return reply.ToObjects<Models.TransferFromResult>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.GetTransactionsResponse> Icrc3GetTransactions(Models.GetTransactionsRequest__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc3_get_transactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.GetTransactionsResponse>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.TransferResult> Mint(Models.Mint__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mint", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}
	}
}