using worldId = System.String;
using userId = System.String;
using groupId = System.String;
using entityId = System.String;
using actionId = System.String;
using List_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.List_1Item>;
using List = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.ListItem>;
using Hash = System.UInt32;
using AssocList_1 = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocList_1Item>;
using AssocList = EdjCase.ICP.Candid.Models.OptionalValue<Candid.UserNode.Models.AssocListItem>;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using EdjCase.ICP.Agent.Responses;
using Candid.UserNode;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.UserNode
{
	public class UserNodeApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public UserNodeApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task AdminCreateUser(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "adminCreateUser", arg);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> CycleBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "cycleBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> GetAllUserActionStates(userId arg0, worldId arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUserActionStates", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> GetAllUserEntities(userId arg0, worldId arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUserEntities", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<userId>> GetAllUserIds()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUserIds", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<userId>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<userId>> GetAllWorldUserIds(worldId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllWorldUserIds", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<userId>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> GetSpecificUserEntities(userId arg0, worldId arg1, List<UserNodeApiClient.GetSpecificUserEntitiesArg2Item> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getSpecificUserEntities", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task GrantEntityPermission(string arg0, Models.EntityPermission arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantEntityPermission", arg);
		}

		public async Task GrantGlobalPermission(string arg0, Models.GlobalPermission arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantGlobalPermission", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> ImportAllPermissionsOfWorld(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllPermissionsOfWorld", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> ImportAllUsersDataOfWorld(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllUsersDataOfWorld", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> ManuallyOverwriteEntities(userId arg0, groupId arg1, List<Models.StableEntity> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "manuallyOverwriteEntities", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> ProcessAction(userId arg0, actionId arg1, OptionalValue<Models.ActionConstraint> arg2, List<Models.ActionOutcomeOption> arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "processAction", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task RemoveEntityPermission(string arg0, Models.EntityPermission arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeEntityPermission", arg);
		}

		public async Task RemoveGlobalPermission(string arg0, Models.GlobalPermission arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeGlobalPermission", arg);
		}

		public async Task SynchronizeEntityPermissions(string arg0, Models.Trie_1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "synchronizeEntityPermissions", arg);
		}

		public async Task SynchronizeGlobalPermissions(Models.Trie arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "synchronizeGlobalPermissions", arg);
		}

		public class GetSpecificUserEntitiesArg2Item
		{
			[CandidTag(0U)]
			public groupId F0 { get; set; }

			[CandidTag(1U)]
			public entityId F1 { get; set; }

			public GetSpecificUserEntitiesArg2Item(groupId f0, entityId f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetSpecificUserEntitiesArg2Item()
			{
			}
		}
	}
}