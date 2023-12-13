using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using Candid.Extv2Boom;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;
using TokenIndex = System.UInt32;
using Tokenidentifier1 = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using Subaccount1 = System.Collections.Generic.List<System.Byte>;
using Extension = System.String;
using AssetHandle = System.String;
using Accountidentifier1 = System.String;

namespace Candid.Extv2Boom
{
	public class Extv2BoomApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public Extv2BoomApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
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
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addAsset", arg);
		}

		public async Task AddMinter(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addMinter", arg);
		}

		public async Task AddThumbnail(AssetHandle arg0, List<byte> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
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

		public async Task<Extv2BoomApiClient.AllSettlementsReturnArg0> AllSettlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "allSettlements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.AllSettlementsReturnArg0>(this.Converter);
		}

		public async Task<UnboundedUInt> AvailableCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "availableCycles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<Models.BalanceResponse> Balance(Models.BalanceRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "balance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.BalanceResponse>(this.Converter);
		}

		public async Task<Models.Result7> Bearer(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "bearer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result7>(this.Converter);
		}

		public async Task<Models.Result10> Details(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "details", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result10>(this.Converter);
		}

		public async Task<OptionalValue<Models.Metadata>> ExtGetTokenMetadata(TokenIndex arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "extGetTokenMetadata", arg);
			return reply.ToObjects<OptionalValue<Models.Metadata>>(this.Converter);
		}

		public async Task ExtAddadmin(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_addAdmin", arg);
		}

		public async Task ExtAddassetcanister()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_addAssetCanister", arg);
		}

		public async Task<List<Principal>> ExtAdmin()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_admin", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Principal>>(this.Converter);
		}

		public async Task ExtAssetadd(AssetHandle arg0, string arg1, string arg2, Models.AssetType arg3, UnboundedUInt arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_assetAdd", arg);
		}

		public async Task<bool> ExtAssetexists(AssetHandle arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_assetExists", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ExtAssetfits(bool arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_assetFits", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ExtAssetstream(AssetHandle arg0, List<byte> arg1, bool arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_assetStream", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Models.BalanceResponse> ExtBalance(Models.BalanceRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_balance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.BalanceResponse>(this.Converter);
		}

		public async Task<Models.Result7> ExtBearer(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_bearer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result7>(this.Converter);
		}

		public async Task<Models.Result3> ExtBurn(Tokenidentifier1 arg0, Accountidentifier1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_burn", arg);
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task ExtCapinit()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_capInit", arg);
		}

		public async Task<Extv2BoomApiClient.ExtExpiredReturnArg0> ExtExpired()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_expired", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.ExtExpiredReturnArg0>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.ExtExtensionsReturnArg0> ExtExtensions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_extensions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.ExtExtensionsReturnArg0>(this.Converter);
		}

		public async Task<(string ReturnArg0, string ReturnArg1)> ExtGetcollectionmetadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_getCollectionMetadata", arg);
			return reply.ToObjects<string, string>(this.Converter);
		}

		public async Task ExtInternalBulkBurn(uint arg0, uint arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_internal_bulk_burn", arg);
		}

		public async Task<Models.Result3> ExtInternalBurn(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_internal_burn", arg);
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task<Models.Result3> ExtMarketplacelist(Models.ListRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_marketplaceList", arg);
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.ExtMarketplacelistingsReturnArg0> ExtMarketplacelistings()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_marketplaceListings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.ExtMarketplacelistingsReturnArg0>(this.Converter);
		}

		public async Task<Models.Result9> ExtMarketplacepurchase(Tokenidentifier1 arg0, ulong arg1, Accountidentifier1 arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_marketplacePurchase", arg);
			return reply.ToObjects<Models.Result9>(this.Converter);
		}

		public async Task<Models.Result3> ExtMarketplacesettle(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_marketplaceSettle", arg);
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task<(ulong ReturnArg0, ulong ReturnArg1, ulong ReturnArg2, ulong ReturnArg3, UnboundedUInt ReturnArg4, UnboundedUInt ReturnArg5, UnboundedUInt ReturnArg6)> ExtMarketplacestats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_marketplaceStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<ulong, ulong, ulong, ulong, UnboundedUInt, UnboundedUInt, UnboundedUInt>(this.Converter);
		}

		public async Task<List<Models.Transaction>> ExtMarketplacetransactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_marketplaceTransactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Transaction>>(this.Converter);
		}

		public async Task<Models.Result8> ExtMetadata(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result8>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.ExtMintReturnArg0> ExtMint(Extv2BoomApiClient.ExtMintArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_mint", arg);
			return reply.ToObjects<Extv2BoomApiClient.ExtMintReturnArg0>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.ExtPaymentsReturnArg0> ExtPayments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_payments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.ExtPaymentsReturnArg0>(this.Converter);
		}

		public async Task ExtRemoveadmin(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_removeAdmin", arg);
		}

		public async Task<bool> ExtSaleclose()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleClose", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<OptionalValue<Models.Sale>> ExtSalecurrent()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_saleCurrent", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Sale>>(this.Converter);
		}

		public async Task<bool> ExtSaleend()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleEnd", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ExtSaleopen(List<Models.SalePricingGroup> arg0, Models.SaleRemaining arg1, Extv2BoomApiClient.ExtSaleopenArg2 arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleOpen", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ExtSalepause()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_salePause", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Models.Result5> ExtSalepurchase(UnboundedUInt arg0, ulong arg1, ulong arg2, Accountidentifier1 arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_salePurchase", arg);
			return reply.ToObjects<Models.Result5>(this.Converter);
		}

		public async Task<bool> ExtSaleresume()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleResume", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<OptionalValue<Models.SaleDetails>> ExtSalesettings(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_saleSettings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.SaleDetails>>(this.Converter);
		}

		public async Task<Models.Result4> ExtSalesettle(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleSettle", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<List<Models.SaleTransaction>> ExtSaletransactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ext_saleTransactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SaleTransaction>>(this.Converter);
		}

		public async Task<bool> ExtSaleupdate(OptionalValue<List<Models.SalePricingGroup>> arg0, OptionalValue<Models.SaleRemaining> arg1, Extv2BoomApiClient.ExtSaleupdateArg2 arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_saleUpdate", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task ExtSetcollectionmetadata(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setCollectionMetadata", arg);
		}

		public async Task ExtSetmarketplaceopen(Time arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setMarketplaceOpen", arg);
		}

		public async Task ExtSetowner(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setOwner", arg);
		}

		public async Task ExtSetroyalty(Extv2BoomApiClient.ExtSetroyaltyArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_setRoyalty", arg);
		}

		public async Task ExtSetsaleroyalty(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAsync(this.CanisterId, "ext_setSaleRoyalty", arg);
		}

		public async Task<Models.TransferResponse> ExtTransfer(Models.TransferRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ext_transfer", arg);
			return reply.ToObjects<Models.TransferResponse>(this.Converter);
		}

		public async Task<Models.Result2> ExtdataSupply(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "extdata_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result2>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.ExtensionsReturnArg0> Extensions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "extensions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.ExtensionsReturnArg0>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.FailedSalesReturnArg0> FailedSales()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "failedSales", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.FailedSalesReturnArg0>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.GetMetadataReturnArg0> GetMetadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMetadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.GetMetadataReturnArg0>(this.Converter);
		}

		public async Task<List<Principal>> GetMinter()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMinter", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Principal>>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.GetRegistryReturnArg0> GetRegistry()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegistry", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.GetRegistryReturnArg0>(this.Converter);
		}

		public async Task<UnboundedUInt> GetSize()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getSize", arg);
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<OptionalValue<Models.Metadata>> GetTokenMetadata(TokenIndex arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTokenMetadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Metadata>>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.GetTokensReturnArg0> GetTokens()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.GetTokensReturnArg0>(this.Converter);
		}

		public async Task<UnboundedUInt> GetTotalTokens()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTotalTokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<List<Models.TxInfo>> GetUserNftTx(string arg0, Extv2BoomApiClient.GetUserNftTxArg1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserNftTx", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.TxInfo>>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.GetAllAssethandlesReturnArg0> GetAllAssethandles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_all_assetHandles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.GetAllAssethandlesReturnArg0>(this.Converter);
		}

		public async Task<List<byte>> GetAssetEncoding(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_asset_encoding", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<byte>>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.GetPagedRegistryReturnArg0> GetPagedRegistry(uint arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_paged_registry", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.GetPagedRegistryReturnArg0>(this.Converter);
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

		public async Task<bool> HeartbeatIsrunning()
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

		public async Task<Dictionary<string, UnboundedUInt>> HeartbeatPending()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "heartbeat_pending", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Dictionary<string, UnboundedUInt>>(this.Converter);
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

		public async Task<Models.HttpResponse> HttpRequest(Models.HttpRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async Task<Models.HttpStreamingCallbackResponse> HttpRequestStreamingCallback(Models.HttpStreamingCallbackToken arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request_streaming_callback", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpStreamingCallbackResponse>(this.Converter);
		}

		public async Task<Models.HttpResponse> HttpRequestUpdate(Models.HttpRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "http_request_update", arg);
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async Task InternalExtAddadmin()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "internal_ext_addAdmin", arg);
		}

		public async Task<bool> IsHeartbeatRunning()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isHeartbeatRunning", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Models.Result3> List(Models.ListRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "list", arg);
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.ListingsReturnArg0> Listings()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "listings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.ListingsReturnArg0>(this.Converter);
		}

		public async Task<Models.Result7> Lock(Tokenidentifier1 arg0, ulong arg1, Accountidentifier1 arg2, Subaccount1 arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "lock", arg);
			return reply.ToObjects<Models.Result7>(this.Converter);
		}

		public async Task<Models.Result6> Metadata(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result6>(this.Converter);
		}

		public async Task<Models.Result5> Reserve(ulong arg0, ulong arg1, Accountidentifier1 arg2, Subaccount1 arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "reserve", arg);
			return reply.ToObjects<Models.Result5>(this.Converter);
		}

		public async Task<Models.Result4> Retreive(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "retreive", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<List<Models.SaleTransaction>> SaleTransactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "saleTransactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SaleTransaction>>(this.Converter);
		}

		public async Task<Models.SaleSettings> SalesSettings(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "salesSettings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.SaleSettings>(this.Converter);
		}

		public async Task<Models.Result3> Settle(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "settle", arg);
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task<Extv2BoomApiClient.SettlementsReturnArg0> Settlements()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "settlements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Extv2BoomApiClient.SettlementsReturnArg0>(this.Converter);
		}

		public async Task<(ulong ReturnArg0, ulong ReturnArg1, ulong ReturnArg2, ulong ReturnArg3, UnboundedUInt ReturnArg4, UnboundedUInt ReturnArg5, UnboundedUInt ReturnArg6)> Stats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "stats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<ulong, ulong, ulong, ulong, UnboundedUInt, UnboundedUInt, UnboundedUInt>(this.Converter);
		}

		public async Task<Models.Result2> Supply(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result2>(this.Converter);
		}

		public async Task<Models.Result1> Tokens(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "tokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result1>(this.Converter);
		}

		public async Task<Models.Result> TokensExt(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "tokens_ext", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<List<Models.Transaction>> Transactions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "transactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Transaction>>(this.Converter);
		}

		public async Task<Models.TransferResponse> Transfer(Models.TransferRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "transfer", arg);
			return reply.ToObjects<Models.TransferResponse>(this.Converter);
		}

		public class AllSettlementsReturnArg0 : List<Extv2BoomApiClient.AllSettlementsReturnArg0.AllSettlementsReturnArg0Element>
		{
			public AllSettlementsReturnArg0()
			{
			}

			public class AllSettlementsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Extv2BoomApiClient.AllSettlementsReturnArg0.AllSettlementsReturnArg0Element.F1Info F1 { get; set; }

				public AllSettlementsReturnArg0Element(TokenIndex f0, Extv2BoomApiClient.AllSettlementsReturnArg0.AllSettlementsReturnArg0Element.F1Info f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public AllSettlementsReturnArg0Element()
				{
				}

				public class F1Info
				{
					[CandidName("buyer")]
					public Accountidentifier1 Buyer { get; set; }

					[CandidName("price")]
					public ulong Price { get; set; }

					[CandidName("seller")]
					public Principal Seller { get; set; }

					[CandidName("subaccount")]
					public Subaccount1 Subaccount { get; set; }

					public F1Info(Accountidentifier1 buyer, ulong price, Principal seller, Subaccount1 subaccount)
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
		}

		public class ExtExpiredReturnArg0 : List<Extv2BoomApiClient.ExtExpiredReturnArg0.ExtExpiredReturnArg0Element>
		{
			public ExtExpiredReturnArg0()
			{
			}

			public class ExtExpiredReturnArg0Element
			{
				[CandidTag(0U)]
				public Accountidentifier1 F0 { get; set; }

				[CandidTag(1U)]
				public Subaccount1 F1 { get; set; }

				public ExtExpiredReturnArg0Element(Accountidentifier1 f0, Subaccount1 f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public ExtExpiredReturnArg0Element()
				{
				}
			}
		}

		public class ExtExtensionsReturnArg0 : List<Extension>
		{
			public ExtExtensionsReturnArg0()
			{
			}
		}

		public class ExtMarketplacelistingsReturnArg0 : List<Extv2BoomApiClient.ExtMarketplacelistingsReturnArg0.ExtMarketplacelistingsReturnArg0Element>
		{
			public ExtMarketplacelistingsReturnArg0()
			{
			}

			public class ExtMarketplacelistingsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Models.Listing F1 { get; set; }

				[CandidTag(2U)]
				public Models.Metadata F2 { get; set; }

				public ExtMarketplacelistingsReturnArg0Element(TokenIndex f0, Models.Listing f1, Models.Metadata f2)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
				}

				public ExtMarketplacelistingsReturnArg0Element()
				{
				}
			}
		}

		public class ExtMintArg0 : List<Extv2BoomApiClient.ExtMintArg0.ExtMintArg0Element>
		{
			public ExtMintArg0()
			{
			}

			public class ExtMintArg0Element
			{
				[CandidTag(0U)]
				public Accountidentifier1 F0 { get; set; }

				[CandidTag(1U)]
				public Models.Metadata F1 { get; set; }

				public ExtMintArg0Element(Accountidentifier1 f0, Models.Metadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public ExtMintArg0Element()
				{
				}
			}
		}

		public class ExtMintReturnArg0 : List<TokenIndex>
		{
			public ExtMintReturnArg0()
			{
			}
		}

		public class ExtPaymentsReturnArg0 : List<Extv2BoomApiClient.ExtPaymentsReturnArg0.ExtPaymentsReturnArg0Element>
		{
			public ExtPaymentsReturnArg0()
			{
			}

			public class ExtPaymentsReturnArg0Element
			{
				[CandidTag(0U)]
				public Accountidentifier1 F0 { get; set; }

				[CandidTag(1U)]
				public Models.Payment F1 { get; set; }

				public ExtPaymentsReturnArg0Element(Accountidentifier1 f0, Models.Payment f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public ExtPaymentsReturnArg0Element()
				{
				}
			}
		}

		public class ExtSaleopenArg2 : List<Accountidentifier1>
		{
			public ExtSaleopenArg2()
			{
			}
		}

		public class ExtSaleupdateArg2 : OptionalValue<Extv2BoomApiClient.ExtSaleupdateArg2.ExtSaleupdateArg2Value>
		{
			public ExtSaleupdateArg2()
			{
			}

			public ExtSaleupdateArg2(Extv2BoomApiClient.ExtSaleupdateArg2.ExtSaleupdateArg2Value value) : base(value)
			{
			}

			public class ExtSaleupdateArg2Value : List<Accountidentifier1>
			{
				public ExtSaleupdateArg2Value()
				{
				}
			}
		}

		public class ExtSetroyaltyArg0 : List<Extv2BoomApiClient.ExtSetroyaltyArg0.ExtSetroyaltyArg0Element>
		{
			public ExtSetroyaltyArg0()
			{
			}

			public class ExtSetroyaltyArg0Element
			{
				[CandidTag(0U)]
				public Accountidentifier1 F0 { get; set; }

				[CandidTag(1U)]
				public ulong F1 { get; set; }

				public ExtSetroyaltyArg0Element(Accountidentifier1 f0, ulong f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public ExtSetroyaltyArg0Element()
				{
				}
			}
		}

		public class ExtensionsReturnArg0 : List<Extension>
		{
			public ExtensionsReturnArg0()
			{
			}
		}

		public class FailedSalesReturnArg0 : List<Extv2BoomApiClient.FailedSalesReturnArg0.FailedSalesReturnArg0Element>
		{
			public FailedSalesReturnArg0()
			{
			}

			public class FailedSalesReturnArg0Element
			{
				[CandidTag(0U)]
				public Accountidentifier1 F0 { get; set; }

				[CandidTag(1U)]
				public Subaccount1 F1 { get; set; }

				public FailedSalesReturnArg0Element(Accountidentifier1 f0, Subaccount1 f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public FailedSalesReturnArg0Element()
				{
				}
			}
		}

		public class GetMetadataReturnArg0 : List<Extv2BoomApiClient.GetMetadataReturnArg0.GetMetadataReturnArg0Element>
		{
			public GetMetadataReturnArg0()
			{
			}

			public class GetMetadataReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Models.MetadataLegacy F1 { get; set; }

				public GetMetadataReturnArg0Element(TokenIndex f0, Models.MetadataLegacy f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetMetadataReturnArg0Element()
				{
				}
			}
		}

		public class GetRegistryReturnArg0 : List<Extv2BoomApiClient.GetRegistryReturnArg0.GetRegistryReturnArg0Element>
		{
			public GetRegistryReturnArg0()
			{
			}

			public class GetRegistryReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Accountidentifier1 F1 { get; set; }

				public GetRegistryReturnArg0Element(TokenIndex f0, Accountidentifier1 f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetRegistryReturnArg0Element()
				{
				}
			}
		}

		public class GetTokensReturnArg0 : List<Extv2BoomApiClient.GetTokensReturnArg0.GetTokensReturnArg0Element>
		{
			public GetTokensReturnArg0()
			{
			}

			public class GetTokensReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Models.MetadataLegacy F1 { get; set; }

				public GetTokensReturnArg0Element(TokenIndex f0, Models.MetadataLegacy f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetTokensReturnArg0Element()
				{
				}
			}
		}

		public enum GetUserNftTxArg1
		{
			[CandidName("hold")]
			Hold,
			[CandidName("transfer")]
			Transfer
		}

		public class GetAllAssethandlesReturnArg0 : List<AssetHandle>
		{
			public GetAllAssethandlesReturnArg0()
			{
			}
		}

		public class GetPagedRegistryReturnArg0 : List<Extv2BoomApiClient.GetPagedRegistryReturnArg0.GetPagedRegistryReturnArg0Element>
		{
			public GetPagedRegistryReturnArg0()
			{
			}

			public class GetPagedRegistryReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Accountidentifier1 F1 { get; set; }

				public GetPagedRegistryReturnArg0Element(TokenIndex f0, Accountidentifier1 f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetPagedRegistryReturnArg0Element()
				{
				}
			}
		}

		public class ListingsReturnArg0 : List<Extv2BoomApiClient.ListingsReturnArg0.ListingsReturnArg0Element>
		{
			public ListingsReturnArg0()
			{
			}

			public class ListingsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Models.Listing F1 { get; set; }

				[CandidTag(2U)]
				public Models.MetadataLegacy F2 { get; set; }

				public ListingsReturnArg0Element(TokenIndex f0, Models.Listing f1, Models.MetadataLegacy f2)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
				}

				public ListingsReturnArg0Element()
				{
				}
			}
		}

		public class SettlementsReturnArg0 : List<Extv2BoomApiClient.SettlementsReturnArg0.SettlementsReturnArg0Element>
		{
			public SettlementsReturnArg0()
			{
			}

			public class SettlementsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Accountidentifier1 F1 { get; set; }

				[CandidTag(2U)]
				public ulong F2 { get; set; }

				public SettlementsReturnArg0Element(TokenIndex f0, Accountidentifier1 f1, ulong f2)
				{
					this.F0 = f0;
					this.F1 = f1;
					this.F2 = f2;
				}

				public SettlementsReturnArg0Element()
				{
				}
			}
		}
	}
}