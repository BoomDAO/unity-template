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
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using Candid.extv2_boom;
using EdjCase.ICP.Agent.Responses;
using System;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.extv2_boom
{
	public class Extv2BoomApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public Extv2BoomApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		//public async Task AcceptCycles()
		//{
		//	CandidArg arg = CandidArg.FromCandid();
		//	await this.Agent.CallAndWaitAsync(this.CanisterId, "acceptCycles", arg);
		//}

		//public async System.Threading.Tasks.Task<UnboundedUInt> AcceptCycles()
		//{
		//	CandidArg arg = CandidArg.FromCandid();
		//	CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "accept_cycles", arg);
		//	return reply.ToObjects<UnboundedUInt>(this.Converter);
		//}

		public async Task AddAsset(AssetHandle arg0, uint arg1, string arg2, string arg3, string arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3), CandidTypedValue.FromObject(arg4));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addAsset", arg);
		}

		public async Task AddMinter(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addMinter", arg);
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

		public async Task AdminStartHeartbeat()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "adminStartHeartbeat", arg);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.AllSettlementsArg0Item>> AllSettlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "allSettlements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.AllSettlementsArg0Item>>(this.Converter);
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

		public async System.Threading.Tasks.Task<UnboundedUInt> CycleBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "cycleBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_10> Details(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "details", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_10>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<Models.Metadata>> ExtGetTokenMetadata(TokenIndex arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "extGetTokenMetadata", arg);
			return reply.ToObjects<OptionalValue<Models.Metadata>>(this.Converter);
		}

		public async Task ExtAddadmin(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_addAdmin", arg);
		}

		public async Task ExtAddassetcanister()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_addAssetCanister", arg);
		}

		public async System.Threading.Tasks.Task<List<Principal>> ExtAdmin()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_admin", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Principal>>(this.Converter);
		}

		public async Task ExtAssetadd(AssetHandle arg0, string arg1, string arg2, Models.AssetType arg3, UnboundedUInt arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3), CandidTypedValue.FromObject(arg4));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_assetAdd", arg);
		}

		public async System.Threading.Tasks.Task<bool> ExtAssetexists(AssetHandle arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_assetExists", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> ExtAssetfits(bool arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_assetFits", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> ExtAssetstream(AssetHandle arg0, List<byte> arg1, bool arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_assetStream", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.BalanceResponse> ExtBalance(Models.BalanceRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_balance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.BalanceResponse>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_7> ExtBearer(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_bearer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_7>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> ExtBurn(TokenIdentifier__1 arg0, AccountIdentifier__1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_burn", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async Task ExtCapinit()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_capInit", arg);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.ExtExpiredArg0Item>> ExtExpired()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_expired", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.ExtExpiredArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extension>> ExtExtensions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_extensions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extension>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<(string Arg0, string Arg1)> ExtGetcollectionmetadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_getCollectionMetadata", arg);
			return reply.ToObjects<string, string>(this.Converter);
		}

		public async Task ExtInternalBulkBurn(uint arg0, uint arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_internal_bulk_burn", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> ExtInternalBurn(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_internal_burn", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> ExtMarketplacelist(Models.ListRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_marketplaceList", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.ExtMarketplacelistingsArg0Item>> ExtMarketplacelistings()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_marketplaceListings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.ExtMarketplacelistingsArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_9> ExtMarketplacepurchase(TokenIdentifier__1 arg0, ulong arg1, AccountIdentifier__1 arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_marketplacePurchase", arg);
			return reply.ToObjects<Models.Result_9>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> ExtMarketplacesettle(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_marketplaceSettle", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<(ulong Arg0, ulong Arg1, ulong Arg2, ulong Arg3, UnboundedUInt Arg4, UnboundedUInt Arg5, UnboundedUInt Arg6)> ExtMarketplacestats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_marketplaceStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<ulong, ulong, ulong, ulong, UnboundedUInt, UnboundedUInt, UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.Transaction>> ExtMarketplacetransactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_marketplaceTransactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Transaction>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_8> ExtMetadata(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_8>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<TokenIndex>> ExtMint(List<Extv2BoomApiClient.ExtMintArg0Item> arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_mint", arg);
			return reply.ToObjects<List<TokenIndex>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.ExtPaymentsArg0Item>> ExtPayments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_payments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.ExtPaymentsArg0Item>>(this.Converter);
		}

		public async Task ExtRemoveadmin(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_removeAdmin", arg);
		}

		public async System.Threading.Tasks.Task<bool> ExtSaleclose()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleClose", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<Models.Sale>> ExtSalecurrent()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_saleCurrent", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Sale>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> ExtSaleend()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleEnd", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> ExtSaleopen(List<Models.SalePricingGroup> arg0, Models.SaleRemaining arg1, List<AccountIdentifier__1> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleOpen", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> ExtSalepause()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_salePause", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_5> ExtSalepurchase(UnboundedUInt arg0, ulong arg1, ulong arg2, AccountIdentifier__1 arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_salePurchase", arg);
			return reply.ToObjects<Models.Result_5>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> ExtSaleresume()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleResume", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<OptionalValue<Models.SaleDetails>> ExtSalesettings(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_saleSettings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.SaleDetails>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_4> ExtSalesettle(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleSettle", arg);
			return reply.ToObjects<Models.Result_4>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.SaleTransaction>> ExtSaletransactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_saleTransactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SaleTransaction>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<bool> ExtSaleupdate(OptionalValue<List<Models.SalePricingGroup>> arg0, OptionalValue<Models.SaleRemaining> arg1, OptionalValue<List<AccountIdentifier__1>> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleUpdate", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task ExtSetcollectionmetadata(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setCollectionMetadata", arg);
		}

		public async Task ExtSetmarketplaceopen(Time arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setMarketplaceOpen", arg);
		}

		public async Task ExtSetowner(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setOwner", arg);
		}

		public async Task ExtSetroyalty(List<Extv2BoomApiClient.ExtSetroyaltyArg0Item> arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setRoyalty", arg);
		}

		public async Task ExtSetsaleroyalty(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAsync(this.CanisterId, "ext_setSaleRoyalty", arg);
		}

		public async System.Threading.Tasks.Task<Models.TransferResponse> ExtTransfer(Models.TransferRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_transfer", arg);
			return reply.ToObjects<Models.TransferResponse>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> ExtdataSupply(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "extdata_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extension>> Extensions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "extensions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extension>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.FailedSalesArg0Item>> FailedSales()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "failedSales", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.FailedSalesArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.GetMetadataArg0Item>> GetMetadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMetadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.GetMetadataArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Principal>> GetMinter()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMinter", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Principal>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.GetRegistryArg0Item>> GetRegistry()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegistry", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.GetRegistryArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> GetSize()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getSize", arg);
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.GetTokensArg0Item>> GetTokens()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.GetTokensArg0Item>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> GetTotalTokens()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTotalTokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<AssetHandle>> GetAllAssethandles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_all_assetHandles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<AssetHandle>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<TokenIndex> GetAssethandle(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_assetHandle", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<TokenIndex>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.GetPagedRegistryArg0Item>> GetPagedRegistry(uint arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_paged_registry", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.GetPagedRegistryArg0Item>>(this.Converter);
		}

		public async Task HeartbeatAssetcanisters()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_assetCanisters", arg);
		}

		public async Task HeartbeatCapevents()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_capEvents", arg);
		}

		public async Task HeartbeatDisbursements()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_disbursements", arg);
		}

		public async Task HeartbeatExternal()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_external", arg);
		}

		public async System.Threading.Tasks.Task<bool> HeartbeatIsrunning()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "heartbeat_isRunning", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task HeartbeatPaymentsettlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_paymentSettlements", arg);
		}

		public async System.Threading.Tasks.Task<List<ValueTuple<string, UnboundedUInt>>> HeartbeatPending()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "heartbeat_pending", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<ValueTuple<string, UnboundedUInt>>>(this.Converter);
		}

		public async Task HeartbeatStart()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_start", arg);
		}

		public async Task HeartbeatStop()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "heartbeat_stop", arg);
		}

		public async System.Threading.Tasks.Task<Models.HttpResponse> HttpRequest(Models.HttpRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.HttpStreamingCallbackResponse> HttpRequestStreamingCallback(Models.HttpStreamingCallbackToken arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request_streaming_callback", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpStreamingCallbackResponse>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.HttpResponse> HttpRequestUpdate(Models.HttpRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "http_request_update", arg);
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async Task InternalExtAddadmin()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "internal_ext_addAdmin", arg);
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

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.ListingsArg0Item>> Listings()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "listings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.ListingsArg0Item>>(this.Converter);
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

		public async System.Threading.Tasks.Task<Models.Result_5> Reserve(ulong arg0, ulong arg1, AccountIdentifier__1 arg2, SubAccount__1 arg3)
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

		public async System.Threading.Tasks.Task<List<Models.SaleTransaction>> SaleTransactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "saleTransactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SaleTransaction>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.SaleSettings> SalesSettings(AccountIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "salesSettings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.SaleSettings>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> Settle(TokenIdentifier__1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "settle", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Extv2BoomApiClient.SettlementsArg0Item>> Settlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "settlements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Extv2BoomApiClient.SettlementsArg0Item>>(this.Converter);
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
			public Extv2BoomApiClient.AllSettlementsArg0Item.F1Info F1 { get; set; }

			public AllSettlementsArg0Item(TokenIndex f0, Extv2BoomApiClient.AllSettlementsArg0Item.F1Info f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public AllSettlementsArg0Item()
			{
			}

			public class F1Info
			{
				[CandidName("buyer")]
				public AccountIdentifier__1 Buyer { get; set; }

				[CandidName("price")]
				public ulong Price { get; set; }

				[CandidName("seller")]
				public Principal Seller { get; set; }

				[CandidName("subaccount")]
				public SubAccount__1 Subaccount { get; set; }

				public F1Info(AccountIdentifier__1 buyer, ulong price, Principal seller, SubAccount__1 subaccount)
				{
					this.Buyer = buyer;
					this.Price = price;
					this.Seller = seller;
					this.Subaccount = subaccount;
				}

				public F1Info()
				{
				}
			}
		}

		public class ExtExpiredArg0Item
		{
			[CandidTag(0U)]
			public AccountIdentifier__1 F0 { get; set; }

			[CandidTag(1U)]
			public SubAccount__1 F1 { get; set; }

			public ExtExpiredArg0Item(AccountIdentifier__1 f0, SubAccount__1 f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public ExtExpiredArg0Item()
			{
			}
		}

		public class ExtMarketplacelistingsArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.Listing F1 { get; set; }

			[CandidTag(2U)]
			public Models.Metadata F2 { get; set; }

			public ExtMarketplacelistingsArg0Item(TokenIndex f0, Models.Listing f1, Models.Metadata f2)
			{
				this.F0 = f0;
				this.F1 = f1;
				this.F2 = f2;
			}

			public ExtMarketplacelistingsArg0Item()
			{
			}
		}

		public class ExtMintArg0Item
		{
			[CandidTag(0U)]
			public AccountIdentifier__1 F0 { get; set; }

			[CandidTag(1U)]
			public Models.Metadata F1 { get; set; }

			public ExtMintArg0Item(AccountIdentifier__1 f0, Models.Metadata f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public ExtMintArg0Item()
			{
			}
		}

		public class ExtPaymentsArg0Item
		{
			[CandidTag(0U)]
			public AccountIdentifier__1 F0 { get; set; }

			[CandidTag(1U)]
			public Models.Payment F1 { get; set; }

			public ExtPaymentsArg0Item(AccountIdentifier__1 f0, Models.Payment f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public ExtPaymentsArg0Item()
			{
			}
		}

		public class ExtSetroyaltyArg0Item
		{
			[CandidTag(0U)]
			public AccountIdentifier__1 F0 { get; set; }

			[CandidTag(1U)]
			public ulong F1 { get; set; }

			public ExtSetroyaltyArg0Item(AccountIdentifier__1 f0, ulong f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public ExtSetroyaltyArg0Item()
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

		public class GetMetadataArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.MetadataLegacy F1 { get; set; }

			public GetMetadataArg0Item(TokenIndex f0, Models.MetadataLegacy f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetMetadataArg0Item()
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

		public class GetTokensArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public Models.MetadataLegacy F1 { get; set; }

			public GetTokensArg0Item(TokenIndex f0, Models.MetadataLegacy f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetTokensArg0Item()
			{
			}
		}

		public class GetPagedRegistryArg0Item
		{
			[CandidTag(0U)]
			public TokenIndex F0 { get; set; }

			[CandidTag(1U)]
			public AccountIdentifier__1 F1 { get; set; }

			public GetPagedRegistryArg0Item(TokenIndex f0, AccountIdentifier__1 f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetPagedRegistryArg0Item()
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
			public Models.MetadataLegacy F2 { get; set; }

			public ListingsArg0Item(TokenIndex f0, Models.Listing f1, Models.MetadataLegacy f2)
			{
				this.F0 = f0;
				this.F1 = f1;
				this.F2 = f2;
			}

			public ListingsArg0Item()
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