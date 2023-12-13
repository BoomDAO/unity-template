using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Candid.WorldHub;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using System;
using WorldId = System.String;
using UserId = System.String;
using EntityId = System.String;
using TokenIndex = System.UInt32;
using TokenIdentifier = System.String;
using AccountIdentifier = System.String;

namespace Candid.WorldHub
{
	public class WorldHubApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public WorldHubApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task AddAdmin(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "addAdmin", arg);
		}

		public async Task<Models.Result> AdminCreateUser(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "admin_create_user", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task AdminDeleteUser(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "admin_delete_user", arg);
		}

		public async Task<bool> CheckUsernameAvailability(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "checkUsernameAvailability", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Models.Result> CreateNewUser(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createNewUser", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<UnboundedUInt> CycleBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "cycleBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<AccountIdentifier> GetAccountIdentifier(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAccountIdentifier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<AccountIdentifier>(this.Converter);
		}

		public async Task<List<string>> GetAllAdmins()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllAdmins", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<string>>(this.Converter);
		}

		public async Task<List<string>> GetAllNodeIds()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllNodeIds", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<string>>(this.Converter);
		}

		public async Task<List<string>> GetAllUserIds()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUserIds", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<string>>(this.Converter);
		}

		public async Task<Models.StableEntity> GetEntity(UserId arg0, EntityId arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getEntity", arg);
			return reply.ToObjects<Models.StableEntity>(this.Converter);
		}

		public async Task<Dictionary<string, Dictionary<string, Models.EntityPermission>>> GetEntityPermissionsOfWorld()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getEntityPermissionsOfWorld", arg);
			return reply.ToObjects<Dictionary<string, Dictionary<string, Models.EntityPermission>>>(this.Converter);
		}

		public async Task<WorldHubApiClient.GetGlobalPermissionsOfWorldReturnArg0> GetGlobalPermissionsOfWorld()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getGlobalPermissionsOfWorld", arg);
			return reply.ToObjects<WorldHubApiClient.GetGlobalPermissionsOfWorldReturnArg0>(this.Converter);
		}

		public async Task<TokenIdentifier> GetTokenIdentifier(string arg0, TokenIndex arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTokenIdentifier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<TokenIdentifier>(this.Converter);
		}

		public async Task<Models.Result> GetUserNodeCanisterId(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserNodeCanisterId", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<string> GetUserNodeWasmVersion()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserNodeWasmVersion", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
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

		public async Task<Models.Result> ImportAllPermissionsOfWorld(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllPermissionsOfWorld", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<Models.Result> ImportAllUsersDataOfWorld(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "importAllUsersDataOfWorld", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task RemoveAdmin(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "removeAdmin", arg);
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

		public async Task<Models.Result> SetUsername(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setUsername", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<UnboundedUInt> TotalUsers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "totalUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<Models.Result> UpdateEntity(WorldHubApiClient.UpdateEntityArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateEntity", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<UnboundedInt> UpdateUserNodeWasmModule(WorldHubApiClient.UpdateUserNodeWasmModuleArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateUserNodeWasmModule", arg);
			return reply.ToObjects<UnboundedInt>(this.Converter);
		}

		public async Task UpgradeUsernodes(UnboundedInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "upgrade_usernodes", arg);
		}

		public async Task<WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0> ValidateUpgradeUsernodes(UnboundedInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "validate_upgrade_usernodes", arg);
			return reply.ToObjects<WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0>(this.Converter);
		}

		public class GetGlobalPermissionsOfWorldReturnArg0 : List<WorldId>
		{
			public GetGlobalPermissionsOfWorldReturnArg0()
			{
			}
		}

		public class UpdateEntityArg0
		{
			[CandidName("entity")]
			public Models.StableEntity Entity { get; set; }

			[CandidName("uid")]
			public UserId Uid { get; set; }

			public UpdateEntityArg0(Models.StableEntity entity, UserId uid)
			{
				this.Entity = entity;
				this.Uid = uid;
			}

			public UpdateEntityArg0()
			{
			}
		}

		public class UpdateUserNodeWasmModuleArg0
		{
			[CandidName("version")]
			public string Version { get; set; }

			[CandidName("wasm")]
			public List<byte> Wasm { get; set; }

			public UpdateUserNodeWasmModuleArg0(string version, List<byte> wasm)
			{
				this.Version = version;
				this.Wasm = wasm;
			}

			public UpdateUserNodeWasmModuleArg0()
			{
			}
		}

		[Variant]
		public class ValidateUpgradeUsernodesReturnArg0
		{
			[VariantTagProperty]
			public WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0Tag Tag { get; set; }

			[VariantValueProperty]
			public object? Value { get; set; }

			public ValidateUpgradeUsernodesReturnArg0(WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0Tag tag, object? value)
			{
				this.Tag = tag;
				this.Value = value;
			}

			protected ValidateUpgradeUsernodesReturnArg0()
			{
			}

			public static WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0 Err(string info)
			{
				return new WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0(WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0Tag.Err, info);
			}

			public static WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0 Ok(string info)
			{
				return new WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0(WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0Tag.Ok, info);
			}

			public string AsErr()
			{
				this.ValidateTag(WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0Tag.Err);
				return (string)this.Value!;
			}

			public string AsOk()
			{
				this.ValidateTag(WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0Tag.Ok);
				return (string)this.Value!;
			}

			private void ValidateTag(WorldHubApiClient.ValidateUpgradeUsernodesReturnArg0Tag tag)
			{
				if (!this.Tag.Equals(tag))
				{
					throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
				}
			}
		}

		public enum ValidateUpgradeUsernodesReturnArg0Tag
		{
			Err,
			Ok
		}
	}
}