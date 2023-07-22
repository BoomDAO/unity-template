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
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using EdjCase.ICP.Agent.Responses;
using Candid.Extv2Standard;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.Extv2Standard
{
	public class Extv2StandardApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public Extv2StandardApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task AcceptCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "acceptCycles", arg);
		}

		public async Task AddAsset(AssetHandle arg0, uint arg1, string arg2, string arg3, string arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3), CandidTypedValue.FromObject(arg4));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addAsset", arg);
		}

		public async Task AddThumbnail(AssetHandle arg0, List<byte> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addThumbnail", arg);
		}

		public async Task AdminKillHeartbeat()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "adminKillHeartbeat", arg);
		}

		public async System.Threading.Tasks.Task<string> AdminRefund(string arg0, AccountIdentifier__1 arg1, AccountIdentifier__1 arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "adminRefund", arg);
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task AdminStartHeartbeat()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "adminStartHeartbeat", arg);
		}

		public async System.Threading.Tasks.Task<List<ValueTuple<Principal, List<SubAccount__1>>>> AllPayments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "allPayments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<ValueTuple<Principal, List<SubAccount__1>>>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.AllSettlementsArg0Item>> AllSettlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "allSettlements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.AllSettlementsArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> AvailableCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "availableCycles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.BalanceResponse> Balance(Models.BalanceRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "balance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.BalanceResponse>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_7> Bearer(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "bearer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_7>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> CheckAssetFiletype(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "checkAssetFiletype", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task ClearPayments(Principal arg0, List<SubAccount__1> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "clearPayments", arg);
		}

		public async Task CronCapEvents()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "cronCapEvents", arg);
		}

		public async Task CronDisbursements()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "cronDisbursements", arg);
		}

		public async Task CronSettlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "cronSettlements", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_9> Details(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "details", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_9>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_8> ExtMetadata(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_8>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.ExtMetadatagetarrayArg0Item>> ExtMetadatagetarray(UnboundedUInt arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_metadataGetArray", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.ExtMetadatagetarrayArg0Item>>(this.Converter);
		}

		public async Task ExtMetadatasetarray(List<Extv2StandardApiClient.ExtMetadatasetarrayArg0Item> arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAsync(this.CanisterId, "ext_metadataSetArray", arg);
		}

		public async Task ExtMetadatasetsingle(TokenIndex arg0, Models.EXTMetadata arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAsync(this.CanisterId, "ext_metadataSetSingle", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_8> ExtMetadatabyindex(TokenIndex arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_metadatabyindex", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_8>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extension>> Extensions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "extensions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extension>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.FailedSalesArg0Item>> FailedSales()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "failedSales", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.FailedSalesArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.GetAllAssetsArg0Item>> GetAllAssets()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllAssets", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.GetAllAssetsArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> GetAssetCount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAssetCount", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<AssetHandle>> GetAssetHandles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAssetHandles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<AssetHandle>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<List<Models.Asset>>> GetAssets(AssetHandle arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAssets", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Asset>>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<List<Models.Asset>>> GetAssetsFromHandle(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAssetsFromHandle", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Asset>>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.GetAssetsSliceArg0Item>> GetAssetsSlice(UnboundedUInt arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAssetsSlice", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.GetAssetsSliceArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Principal> GetMinter()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMinter", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Principal>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.GetRegistryArg0Item>> GetRegistry()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegistry", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.GetRegistryArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.GetRegistryHandlesArg0Item>> GetRegistryHandles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegistryHandles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.GetRegistryHandlesArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.GetTokensArg0Item>> GetTokens()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.GetTokensArg0Item>>(this.Converter);
		}

		public async Task HeartbeatExternal()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_external", arg);
		}

		public async System.Threading.Tasks.Task<List<ValueTuple<string, UnboundedUInt>>> HeartbeatPending()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "heartbeat_pending", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<ValueTuple<string, UnboundedUInt>>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> HistoricExport()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "historicExport", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.HttpResponse> HttpRequest(Models.HttpRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async Task InitCap()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "initCap", arg);
		}

		public async System.Threading.Tasks.Task<bool> IsHeartbeatRunning()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isHeartbeatRunning", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> List(Models.ListRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "list", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.ListingsArg0Item>> Listings()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "listings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.ListingsArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_7> Lock(TokenIdentifier__1 arg0, ulong arg1, AccountIdentifier__1 arg2, SubAccount__1 arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "lock", arg);
			return reply.ToObjects<Models.Result_7>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_6> Metadata(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_6>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<List<SubAccount__1>>> Payments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "payments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<SubAccount__1>>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_5> Reserve(ulong arg0, AssetHandle arg1, AccountIdentifier__1 arg2, SubAccount__1 arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "reserve", arg);
			return reply.ToObjects<Models.Result_5>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_4> Retreive(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "retreive", arg);
			return reply.ToObjects<Models.Result_4>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.SalesSettlementsArg0Item>> SalesSettlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "salesSettlements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.SalesSettlementsArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<(ulong Arg0, OptionalValue<byte> Arg1, Time Arg2, List<string> Arg3)> SalesStats(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "salesStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<ulong, OptionalValue<byte>, Time, List<string>>(this.Converter);
		}

		public async Task SetMinter(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "setMinter", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> Settle(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "settle", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2StandardApiClient.SettlementsArg0Item>> Settlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "settlements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2StandardApiClient.SettlementsArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<(ulong Arg0, ulong Arg1, ulong Arg2, ulong Arg3, UnboundedUInt Arg4, UnboundedUInt Arg5, UnboundedUInt Arg6)> Stats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "stats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<ulong, ulong, ulong, ulong, UnboundedUInt, UnboundedUInt, UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> Supply(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> Tokens(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "tokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> TokensExt(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "tokens_ext", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.Transaction>> Transactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "transactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Transaction>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.TransferResponse> Transfer(Models.TransferRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "transfer", arg);
			return reply.ToObjects<Models.TransferResponse>(this.Converter);
		}

		public class AllSettlementsArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.Settlement F1 { get; set; }

			public AllSettlementsArg0Item(TokenIndex f0, Models.Settlement f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public AllSettlementsArg0Item()
			{
			}
		}

		public class ExtMetadatagetarrayArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.EXTMetadata F1 { get; set; }

			public ExtMetadatagetarrayArg0Item(TokenIndex f0, Models.EXTMetadata f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public ExtMetadatagetarrayArg0Item()
			{
			}
		}

		public class ExtMetadatasetarrayArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.EXTMetadata F1 { get; set; }

			public ExtMetadatasetarrayArg0Item(TokenIndex f0, Models.EXTMetadata f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public ExtMetadatasetarrayArg0Item()
			{
			}
		}

		public class FailedSalesArg0Item
		{
			[CandidTag(0U)]
			public AccountIdentifier__1 F0 { get; set; }

			[CandidTag(1U)]
			public SubAccount__1 F1 { get; set; }

			public FailedSalesArg0Item(AccountIdentifier__1 f0, SubAccount__1 f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public FailedSalesArg0Item()
			{
			}
		}

		public class GetAllAssetsArg0Item
		{
			[CandidTag(0U)]
			public AssetHandle F0 { get; set; }

			[CandidTag(1U)]
			public List<Models.Asset> F1 { get; set; }

			public GetAllAssetsArg0Item(AssetHandle f0, List<Models.Asset> f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetAllAssetsArg0Item()
			{
			}
		}

		public class GetAssetsSliceArg0Item
		{
			[CandidTag(0U)]
			public AssetHandle F0 { get; set; }

			[CandidTag(1U)]
			public List<Models.Asset> F1 { get; set; }

			public GetAssetsSliceArg0Item(AssetHandle f0, List<Models.Asset> f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetAssetsSliceArg0Item()
			{
			}
		}

		public class GetRegistryArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public AccountIdentifier__1 F1 { get; set; }

			public GetRegistryArg0Item(TokenIndex f0, AccountIdentifier__1 f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetRegistryArg0Item()
			{
			}
		}

		public class GetRegistryHandlesArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public AssetHandle F1 { get; set; }

			public GetRegistryHandlesArg0Item(TokenIndex f0, AssetHandle f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetRegistryHandlesArg0Item()
			{
			}
		}

		public class GetTokensArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.Metadata F1 { get; set; }

			public GetTokensArg0Item(TokenIndex f0, Models.Metadata f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetTokensArg0Item()
			{
			}
		}

		public class ListingsArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.Listing F1 { get; set; }

			[CandidTag(2U)]
			public Models.Metadata F2 { get; set; }

			public ListingsArg0Item(TokenIndex f0, Models.Listing f1, Models.Metadata f2)
			{
				this.F0 = f0;
				this.F1 = f1;
				this.F2 = f2;
			}

			public ListingsArg0Item()
			{
			}
		}

		public class SalesSettlementsArg0Item
		{
			[CandidTag(0U)]
			public AccountIdentifier__1 F0 { get; set; }

			[CandidTag(1U)]
			public Models.Sale F1 { get; set; }

			public SalesSettlementsArg0Item(AccountIdentifier__1 f0, Models.Sale f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public SalesSettlementsArg0Item()
			{
			}
		}

		public class SettlementsArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public AccountIdentifier__1 F1 { get; set; }

			[CandidTag(2U)]
			public ulong F2 { get; set; }

			public SettlementsArg0Item(TokenIndex f0, AccountIdentifier__1 f1, ulong f2)
			{
				this.F0 = f0;
				this.F1 = f1;
				this.F2 = f2;
			}

			public SettlementsArg0Item()
			{
			}
		}
	}
}