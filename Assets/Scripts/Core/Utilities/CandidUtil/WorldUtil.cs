using Boom.Values;
using Candid.World;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

public static class WorldUtil
{
    public async static UniTask<UResult<Dictionary<string, Return>, string>> ProcessWorldCall<Return>(System.Func<WorldApiClient, UniTask<Return>> task, params string[] worldIds)
    {
        try
        {
            var agentResult = UserUtil.GetAgent();
            if (agentResult.IsErr) throw new(agentResult.AsErr());

            Dictionary<string, Return> responses = new();

            foreach (var wid in worldIds)
            {
                WorldApiClient worldApiClient = new(agentResult.AsOk(), Principal.FromText(wid));

                var response = await task(worldApiClient);

                responses.TryAdd(wid, response);

            }

            return new(responses);
        }
        catch (System.Exception e)
        {
            return new(e.Message);
        }
    }

}