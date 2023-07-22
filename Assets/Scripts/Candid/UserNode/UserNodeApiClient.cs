using worldId = System.String;
using userId = System.String;
using quantity = System.Double;
using groupId = System.String;
using entityId = System.String;
using duration = EdjCase.ICP.Candid.Models.UnboundedUInt;
using attribute = System.String;
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
using System.Collections.Generic;
using Candid.UserNode;
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

		public async System.Threading.Tasks.Task<List<userId>> GetAllUserIds()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUserIds", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<userId>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_3> GetAllUserWorldActions(userId arg0, worldId arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUserWorldActions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_3>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> GetAllUserWorldEntities(userId arg0, worldId arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUserWorldEntities", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<userId>> GetAllWorldUserIds(worldId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllWorldUserIds", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<userId>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> GetSpecificUserWorldEntities(userId arg0, worldId arg1, List<UserNodeApiClient.GetSpecificUserWorldEntitiesArg2Item> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getSpecificUserWorldEntities", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async Task GrantEntityPermission(string arg0, string arg1, string arg2, string arg3, Models.EntityPermission arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3), CandidTypedValue.FromObject(arg4));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantEntityPermission", arg);
		}

		public async Task GrantGlobalPermission(worldId arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantGlobalPermission", arg);
		}

		public async System.Threading.Tasks.Task<Models.Result> ImportAllPermissionsOfWorld(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllPermissionsOfWorld", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> ImportAllUsersDataOfWorld(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllUsersDataOfWorld", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_2> ManuallyOverwriteEntities(userId arg0, groupId arg1, List<Models.Entity> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "manuallyOverwriteEntities", arg);
			return reply.ToObjects<Models.Result_2>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result_1> ProcessAction(userId arg0, actionId arg1, Models.ActionConfig arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "processAction", arg);
			return reply.ToObjects<Models.Result_1>(this.Converter);
		}

		public async Task RemoveEntityPermission(string arg0, string arg1, string arg2, string arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeEntityPermission", arg);
		}

		public async Task RemoveGlobalPermission(worldId arg0, string arg1)
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

		public class GetSpecificUserWorldEntitiesArg2Item
		{
			[CandidTag(0U)]
			public groupId F0 { get; set; }

			[CandidTag(1U)]
			public entityId F1 { get; set; }

			public GetSpecificUserWorldEntitiesArg2Item(groupId f0, entityId f1)
			{
				this.F0 = f0;
				this.F1 = f1;
			}

			public GetSpecificUserWorldEntitiesArg2Item()
			{
			}
		}
	}
}