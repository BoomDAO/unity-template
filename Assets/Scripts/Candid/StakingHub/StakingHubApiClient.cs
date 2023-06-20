using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using Candid.StakingHub;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Responses;
using System.Threading.Tasks;

namespace Candid.StakingHub
{
	public class StakingHubApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public EdjCase.ICP.Candid.CandidConverter? Converter { get; }

		public StakingHubApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async System.Threading.Tasks.Task<Models.Result> DissolveExt(string arg0, uint arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "dissolveExt", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> DissolveIcp()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "dissolveIcp", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Result> DissolveIcrc(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "dissolveIcrc", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async System.Threading.Tasks.Task<List<Models.Stake>> GetUserStakes(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserStakes", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Stake>>(this.Converter);
		}

		public async Task KillCron()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "killCron", arg);
		}

		public async System.Threading.Tasks.Task<Models.Response> UpdateExtStakes(uint arg0, string arg1, string arg2, string arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateExtStakes", arg);
			return reply.ToObjects<Models.Response>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Response> UpdateIcpStakes(ulong arg0, string arg1, string arg2, ulong arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateIcpStakes", arg);
			return reply.ToObjects<Models.Response>(this.Converter);
		}

		public async System.Threading.Tasks.Task<Models.Response> UpdateIcrcStakes(UnboundedUInt arg0, string arg1, string arg2, UnboundedUInt arg3, string arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0), CandidTypedValue.FromObject(arg1), CandidTypedValue.FromObject(arg2), CandidTypedValue.FromObject(arg3), CandidTypedValue.FromObject(arg4));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateIcrcStakes", arg);
			return reply.ToObjects<Models.Response>(this.Converter);
		}
	}
}