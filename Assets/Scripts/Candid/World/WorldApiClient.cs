using worldId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
using TokenIndex = System.UInt32;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Candid.World;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;

namespace Candid.World
{
	public class WorldApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public WorldApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task AddAdmin(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addAdmin", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result> BurnNft(string arg0, TokenIndex arg1, Principal arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "burnNft", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> CreateActionConfig(Models.ActionConfig arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createActionConfig", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> CreateEntityConfig(Models.EntityConfig arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createEntityConfig", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> CycleBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "cycleBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> DeleteActionConfig(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteActionConfig", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> DeleteEntityConfig(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteEntityConfig", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.ActionConfig>> GetActionConfigs()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getActionConfigs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.ActionConfig>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_4> GetAllUserWorldEntities()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAllUserWorldEntities", arg);
			return reply.ToObjects<Models.Result_4>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.EntityConfig>> GetEntityConfigs()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getEntityConfigs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.EntityConfig>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<string> GetOwner()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getOwner", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task GrantEntityPermission(string arg0, string arg1, string arg2, Models.EntityPermission arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantEntityPermission", arg);
		}

		public async Task GrantGlobalPermission(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantGlobalPermission", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> ProcessActionEntities(Models.ActionArg arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "processActionEntities", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async Task RemoveAdmin(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeAdmin", arg);
		}

		public async Task RemoveEntityPermission(string arg0, string arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeEntityPermission", arg);
		}

		public async Task RemoveGlobalPermission(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeGlobalPermission", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> ResetConfig()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "resetConfig", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> UpdateActionConfig(Models.ActionConfig arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateActionConfig", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> UpdateEntityConfig(Models.EntityConfig arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateEntityConfig", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> VerifyTxIcp(ulong arg0, string arg1, string arg2, ulong arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "verifyTxIcp", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> VerifyTxIcrc(UnboundedUInt arg0, string arg1, string arg2, UnboundedUInt arg3, string arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3), CandidTypedValue.FromObject(arg4));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "verifyTxIcrc", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Principal> WhoAmI()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "whoAmI", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Principal>(this.Converter);
		}
	}
}