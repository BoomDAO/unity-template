using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Candid.World;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using System;
using EdjCase.ICP.Candid.Mapping;

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

		public async System.Threading.Tasks.Task<Models.Result_2> CreateAction(Models.Action arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createAction", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> CreateConfig(Models.StableConfig arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createConfig", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> CycleBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "cycleBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> DeleteAction(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteAction", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> DeleteConfig(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteConfig", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.Action>> ExportActions()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "exportActions", arg);
			return reply.ToObjects<List<Models.Action>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.StableConfig>> ExportConfigs()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "exportConfigs", arg);
			return reply.ToObjects<List<Models.StableConfig>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.Action>> GetAllActions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllActions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Action>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.StableConfig>> GetAllConfigs()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllConfigs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.StableConfig>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_6> GetAllUserActionStates(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAllUserActionStates", arg);
			return reply.ToObjects<Models.Result_6>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_5> GetAllUserEntities(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAllUserEntities", arg);
			return reply.ToObjects<Models.Result_5>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<ValueTuple<string, List<WorldApiClient.GetEntityPermissionsOfWorldArg0ItemItem>>>> GetEntityPermissionsOfWorld()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getEntityPermissionsOfWorld", arg);
			return reply.ToObjects<List<ValueTuple<string, List<WorldApiClient.GetEntityPermissionsOfWorldArg0ItemItem>>>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<worldId>> GetGlobalPermissionsOfWorld()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getGlobalPermissionsOfWorld", arg);
			return reply.ToObjects<List<worldId>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<string> GetOwner()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getOwner", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task GrantEntityPermission(Models.EntityPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantEntityPermission", arg);
		}

		public async Task GrantGlobalPermission(Models.GlobalPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantGlobalPermission", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> ImportAllActionsOfWorld(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllActionsOfWorld", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> ImportAllConfigsOfWorld(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllConfigsOfWorld", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> ImportAllPermissionsOfWorld(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllPermissionsOfWorld", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> ImportAllUsersDataOfWorld(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllUsersDataOfWorld", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_4> ProcessAction(Models.ActionArg arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "processAction", arg);
			return reply.ToObjects<Models.Result_4>(this.Converter);
		}

		public async Task RemoveAdmin(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeAdmin", arg);
		}

		public async Task RemoveAllUserNodeRef()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeAllUserNodeRef", arg);
		}

		public async Task RemoveEntityPermission(Models.EntityPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeEntityPermission", arg);
		}

		public async Task RemoveGlobalPermission(Models.GlobalPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeGlobalPermission", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> ResetActions()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "resetActions", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> ResetConfig()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "resetConfig", arg);
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> UpdateAction(Models.Action arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAction", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> UpdateConfig(Models.StableConfig arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateConfig", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> WithdrawIcpFromPaymentHub()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "withdrawIcpFromPaymentHub", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> WithdrawIcrcFromPaymentHub(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "withdrawIcrcFromPaymentHub", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public class GetEntityPermissionsOfWorldArg0ItemItem
		{
			[CandidTag(0U)]
			public string F0 { get; set; }

			[CandidTag(1U)]
			public Models.EntityPermission F1 { get; set; }

			public GetEntityPermissionsOfWorldArg0ItemItem(string f0, Models.EntityPermission f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetEntityPermissionsOfWorldArg0ItemItem()
			{
			}
		}
	}
}