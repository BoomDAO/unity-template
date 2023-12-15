using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Candid.World;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using WorldId = System.String;
using EntityId = System.String;

namespace Candid.World
{
	public class WorldApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public WorldApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task AddAdmin(WorldApiClient.AddAdminArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addAdmin", arg);
		}

		public async Task AddTrustedOrigins(WorldApiClient.AddTrustedOriginsArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addTrustedOrigins", arg);
		}

		public async Task<Models.Result4> CreateAction(Models.Action arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createAction", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<Models.Result4> CreateConfig(Models.StableConfig arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createConfig", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<Models.Result4> CreateEntity(Models.EntitySchema arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createEntity", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<UnboundedUInt> CycleBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "cycleBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<Models.Result4> DeleteAction(WorldApiClient.DeleteActionArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteAction", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task DeleteActionLockState(Models.ActionLockStateArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteActionLockState", arg);
		}

		public async Task DeleteAllActionLockStates()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteAllActionLockStates", arg);
		}

		public async Task<Models.Result2> DeleteAllActions()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteAllActions", arg);
			return reply.ToObjects<Models.Result2>(this.Converter);
		}

		public async Task<Models.Result2> DeleteAllConfigs()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteAllConfigs", arg);
			return reply.ToObjects<Models.Result2>(this.Converter);
		}

		public async Task DeleteCache()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAsync(this.CanisterId, "deleteCache", arg);
		}

		public async Task<Models.Result4> DeleteConfig(WorldApiClient.DeleteConfigArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteConfig", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<Models.Result4> DeleteEntity(WorldApiClient.DeleteEntityArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteEntity", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<Models.Action> EditAction(WorldApiClient.EditActionArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "editAction", arg);
			return reply.ToObjects<Models.Action>(this.Converter);
		}

		public async Task<Models.StableConfig> EditConfig(WorldApiClient.EditConfigArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "editConfig", arg);
			return reply.ToObjects<Models.StableConfig>(this.Converter);
		}

		public async Task<Models.EntitySchema> EditEntity(WorldApiClient.EditEntityArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "editEntity", arg);
			return reply.ToObjects<Models.EntitySchema>(this.Converter);
		}

		public async Task<List<Models.Action>> ExportActions()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "exportActions", arg);
			return reply.ToObjects<List<Models.Action>>(this.Converter);
		}

		public async Task<List<Models.StableConfig>> ExportConfigs()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "exportConfigs", arg);
			return reply.ToObjects<List<Models.StableConfig>>(this.Converter);
		}

		public async Task<bool> GetActionLockState(Models.ActionLockStateArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getActionLockState", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<List<Models.Action>> GetAllActions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllActions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Action>>(this.Converter);
		}

		public async Task<List<Models.StableConfig>> GetAllConfigs()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllConfigs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.StableConfig>>(this.Converter);
		}

		public async Task<Models.Result6> GetAllUserActionStates(WorldApiClient.GetAllUserActionStatesArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAllUserActionStates", arg);
			return reply.ToObjects<Models.Result6>(this.Converter);
		}

		public async Task<Models.Result5> GetAllUserEntities(WorldApiClient.GetAllUserEntitiesArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAllUserEntities", arg);
			return reply.ToObjects<Models.Result5>(this.Converter);
		}

		public async Task<Dictionary<string, Dictionary<string, Models.EntityPermission>>> GetEntityPermissionsOfWorld()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getEntityPermissionsOfWorld", arg);
			return reply.ToObjects<Dictionary<string, Dictionary<string, Models.EntityPermission>>>(this.Converter);
		}

		public async Task<WorldApiClient.GetGlobalPermissionsOfWorldReturnArg0> GetGlobalPermissionsOfWorld()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getGlobalPermissionsOfWorld", arg);
			return reply.ToObjects<WorldApiClient.GetGlobalPermissionsOfWorldReturnArg0>(this.Converter);
		}

		public async Task<string> GetOwner()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getOwner", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<UnboundedUInt> GetProcessActionCount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getProcessActionCount", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<List<string>> GetTrustedOrigins()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "get_trusted_origins", arg);
			return reply.ToObjects<List<string>>(this.Converter);
		}

		public async Task GrantEntityPermission(Models.EntityPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantEntityPermission", arg);
		}

		public async Task GrantGlobalPermission(Models.GlobalPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "grantGlobalPermission", arg);
		}

		public async Task<Models.Result4> ImportAllActionsOfWorld(WorldApiClient.ImportAllActionsOfWorldArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllActionsOfWorld", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<Models.Result4> ImportAllConfigsOfWorld(WorldApiClient.ImportAllConfigsOfWorldArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllConfigsOfWorld", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<Models.Result4> ImportAllPermissionsOfWorld(WorldApiClient.ImportAllPermissionsOfWorldArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllPermissionsOfWorld", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task<Models.Result4> ImportAllUsersDataOfWorld(WorldApiClient.ImportAllUsersDataOfWorldArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllUsersDataOfWorld", arg);
			return reply.ToObjects<Models.Result4>(this.Converter);
		}

		public async Task LogsClear()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "logsClear", arg);
		}

		public async Task<List<string>> LogsGet()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "logsGet", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<string>>(this.Converter);
		}

		public async Task<UnboundedUInt> LogsGetCount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "logsGetCount", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<Models.Result3> ProcessAction(Models.ActionArg arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "processAction", arg);
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task RemoveAdmin(WorldApiClient.RemoveAdminArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeAdmin", arg);
		}

		public async Task RemoveAllUserNodeRef()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeAllUserNodeRef", arg);
		}

		public async Task RemoveEntityPermission(Models.EntityPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeEntityPermission", arg);
		}

		public async Task RemoveGlobalPermission(Models.GlobalPermission arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeGlobalPermission", arg);
		}

		public async Task RemoveTrustedOrigins(WorldApiClient.RemoveTrustedOriginsArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeTrustedOrigins", arg);
		}

		public async Task<Models.Result2> ResetActionsAndConfigsToHardcodedTemplate()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "resetActionsAndConfigsToHardcodedTemplate", arg);
			return reply.ToObjects<Models.Result2>(this.Converter);
		}

		public async Task UpdateOwnership(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "updateOwnership", arg);
		}

		public async Task<bool> ValidateEntityConstraints(List<Models.StableEntity> arg0, List<Models.EntityConstraint> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "validateEntityConstraints", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Models.Result1> WithdrawIcpFromWorld(WorldApiClient.WithdrawIcpFromWorldArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "withdrawIcpFromWorld", arg);
			return reply.ToObjects<Models.Result1>(this.Converter);
		}

		public async Task<Models.Result> WithdrawIcrcFromWorld(WorldApiClient.WithdrawIcrcFromWorldArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "withdrawIcrcFromWorld", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<Models.CanisterWsCloseResult> WsClose(Models.CanisterWsCloseArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ws_close", arg);
			return reply.ToObjects<Models.CanisterWsCloseResult>(this.Converter);
		}

		public async Task<Models.CanisterWsGetMessagesResult> WsGetMessages(Models.CanisterWsGetMessagesArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ws_get_messages", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.CanisterWsGetMessagesResult>(this.Converter);
		}

		public async Task<Models.CanisterWsMessageResult> WsMessage(Models.CanisterWsMessageArguments arg0, OptionalValue<Models.WSSentArg> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ws_message", arg);
			return reply.ToObjects<Models.CanisterWsMessageResult>(this.Converter);
		}

		public async Task<Models.CanisterWsOpenResult> WsOpen(Models.CanisterWsOpenArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ws_open", arg);
			return reply.ToObjects<Models.CanisterWsOpenResult>(this.Converter);
		}

		public class AddAdminArg0
		{
			[CandidName("principal")]
			public string Principal { get; set; }

			public AddAdminArg0(string principal)
			{
				this.Principal = principal;
			}

			public AddAdminArg0()
			{
			}
		}

		public class AddTrustedOriginsArg0
		{
			[CandidName("originUrl")]
			public string OriginUrl { get; set; }

			public AddTrustedOriginsArg0(string originUrl)
			{
				this.OriginUrl = originUrl;
			}

			public AddTrustedOriginsArg0()
			{
			}
		}

		public class DeleteActionArg0
		{
			[CandidName("aid")]
			public string Aid { get; set; }

			public DeleteActionArg0(string aid)
			{
				this.Aid = aid;
			}

			public DeleteActionArg0()
			{
			}
		}

		public class DeleteConfigArg0
		{
			[CandidName("cid")]
			public string Cid { get; set; }

			public DeleteConfigArg0(string cid)
			{
				this.Cid = cid;
			}

			public DeleteConfigArg0()
			{
			}
		}

		public class DeleteEntityArg0
		{
			[CandidName("eid")]
			public string Eid { get; set; }

			[CandidName("uid")]
			public string Uid { get; set; }

			public DeleteEntityArg0(string eid, string uid)
			{
				this.Eid = eid;
				this.Uid = uid;
			}

			public DeleteEntityArg0()
			{
			}
		}

		public class EditActionArg0
		{
			[CandidName("aid")]
			public string Aid { get; set; }

			public EditActionArg0(string aid)
			{
				this.Aid = aid;
			}

			public EditActionArg0()
			{
			}
		}

		public class EditConfigArg0
		{
			[CandidName("cid")]
			public string Cid { get; set; }

			public EditConfigArg0(string cid)
			{
				this.Cid = cid;
			}

			public EditConfigArg0()
			{
			}
		}

		public class EditEntityArg0
		{
			[CandidName("entityId")]
			public string EntityId { get; set; }

			[CandidName("userId")]
			public string UserId { get; set; }

			public EditEntityArg0(string entityId, string userId)
			{
				this.EntityId = entityId;
				this.UserId = userId;
			}

			public EditEntityArg0()
			{
			}
		}

		public class GetAllUserActionStatesArg0
		{
			[CandidName("uid")]
			public string Uid { get; set; }

			public GetAllUserActionStatesArg0(string uid)
			{
				this.Uid = uid;
			}

			public GetAllUserActionStatesArg0()
			{
			}
		}

		public class GetAllUserEntitiesArg0
		{
			[CandidName("page")]
			public OptionalValue<UnboundedUInt> Page { get; set; }

			[CandidName("uid")]
			public string Uid { get; set; }

			public GetAllUserEntitiesArg0(OptionalValue<UnboundedUInt> page, string uid)
			{
				this.Page = page;
				this.Uid = uid;
			}

			public GetAllUserEntitiesArg0()
			{
			}
		}

		public class GetGlobalPermissionsOfWorldReturnArg0 : List<WorldId>
		{
			public GetGlobalPermissionsOfWorldReturnArg0()
			{
			}
		}

		public class ImportAllActionsOfWorldArg0
		{
			[CandidName("ofWorldId")]
			public string OfWorldId { get; set; }

			public ImportAllActionsOfWorldArg0(string ofWorldId)
			{
				this.OfWorldId = ofWorldId;
			}

			public ImportAllActionsOfWorldArg0()
			{
			}
		}

		public class ImportAllConfigsOfWorldArg0
		{
			[CandidName("ofWorldId")]
			public string OfWorldId { get; set; }

			public ImportAllConfigsOfWorldArg0(string ofWorldId)
			{
				this.OfWorldId = ofWorldId;
			}

			public ImportAllConfigsOfWorldArg0()
			{
			}
		}

		public class ImportAllPermissionsOfWorldArg0
		{
			[CandidName("ofWorldId")]
			public string OfWorldId { get; set; }

			public ImportAllPermissionsOfWorldArg0(string ofWorldId)
			{
				this.OfWorldId = ofWorldId;
			}

			public ImportAllPermissionsOfWorldArg0()
			{
			}
		}

		public class ImportAllUsersDataOfWorldArg0
		{
			[CandidName("ofWorldId")]
			public string OfWorldId { get; set; }

			public ImportAllUsersDataOfWorldArg0(string ofWorldId)
			{
				this.OfWorldId = ofWorldId;
			}

			public ImportAllUsersDataOfWorldArg0()
			{
			}
		}

		public class RemoveAdminArg0
		{
			[CandidName("principal")]
			public string Principal { get; set; }

			public RemoveAdminArg0(string principal)
			{
				this.Principal = principal;
			}

			public RemoveAdminArg0()
			{
			}
		}

		public class RemoveTrustedOriginsArg0
		{
			[CandidName("originUrl")]
			public string OriginUrl { get; set; }

			public RemoveTrustedOriginsArg0(string originUrl)
			{
				this.OriginUrl = originUrl;
			}

			public RemoveTrustedOriginsArg0()
			{
			}
		}

		public class WithdrawIcpFromWorldArg0
		{
			[CandidName("toPrincipal")]
			public string ToPrincipal { get; set; }

			public WithdrawIcpFromWorldArg0(string toPrincipal)
			{
				this.ToPrincipal = toPrincipal;
			}

			public WithdrawIcpFromWorldArg0()
			{
			}
		}

		public class WithdrawIcrcFromWorldArg0
		{
			[CandidName("toPrincipal")]
			public string ToPrincipal { get; set; }

			[CandidName("tokenCanisterId")]
			public string TokenCanisterId { get; set; }

			public WithdrawIcrcFromWorldArg0(string toPrincipal, string tokenCanisterId)
			{
				this.ToPrincipal = toPrincipal;
				this.TokenCanisterId = tokenCanisterId;
			}

			public WithdrawIcrcFromWorldArg0()
			{
			}
		}
	}
}