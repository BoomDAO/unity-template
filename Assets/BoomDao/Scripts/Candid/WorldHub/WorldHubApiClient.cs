using TokenIndex = System.UInt32;
using TokenIdentifier = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Candid.WorldHub;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;

namespace Candid.WorldHub
{
	public class WorldHubApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public WorldHubApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
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

		public async System.Threading.Tasks.Task<Models.Result> AdminCreateUser(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "admin_create_user", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task AdminDeleteUser(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "admin_delete_user", arg);
		}

		public async System.Threading.Tasks.Task<bool> CheckUsernameAvailability(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "checkUsernameAvailability", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> CreateNewUser(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createNewUser", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> CycleBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "cycleBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<AccountIdentifier> GetAccountIdentifier(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAccountIdentifier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<AccountIdentifier>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<string>> GetAllAdmins()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllAdmins", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<string>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<string>> GetAllNodeIds()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllNodeIds", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<string>>(this.Converter);
		}

		public async System.Threading.Tasks.Task<TokenIdentifier> GetTokenIdentifier(string arg0, TokenIndex arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTokenIdentifier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<TokenIdentifier>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> GetUserNodeCanisterId(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserNodeCanisterId", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
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

		public async System.Threading.Tasks.Task<Models.Result> SetUsername(string arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setUsername", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<UnboundedUInt> TotalUsers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "totalUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async System.Threading.Tasks.Task<string> Whoami()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "whoami", arg);
			return reply.ToObjects<string>(this.Converter);
		}
	}
}