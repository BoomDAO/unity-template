using Candid;
using Candid.IcpLedger.Models;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Candid.CandidApiManager;

public static class CandidUtil
{
    public static async Task UpdatePosdata(IAgent agent, string canisterId, string updatecallName, CandidTypedValue[] args)
    {
        if (agent == null)
        {
            Debug.Log("Must log in first");
            return;
        }

        Principal canisterIdPrincipal = Principal.FromText(canisterId);

        CandidArg arg = CandidArg.FromCandid(args);
        CandidArg reply = await agent.CallAndWaitAsync(canisterIdPrincipal, updatecallName, arg);
    }
    public static async Task<T> QueryData<T>(IAgent agent, string canisterId, string updatecallName, CandidTypedValue[] args)
    {
        if (agent == null)
        {
            Debug.Log("Must log in first");
            return default;
        }

        Principal canisterIdPrincipal = Principal.FromText(canisterId);

        CandidArg arg = CandidArg.FromCandid(args);
        QueryResponse response = await agent.QueryAsync(canisterIdPrincipal, updatecallName, arg);
        CandidArg reply = response.ThrowOrGetReply();

        var json = reply.ToString().Trim('(',')').Trim('\'');
        Debug.Log($"QUERY: {json}");

        T val = JsonConvert.DeserializeObject<T>(json);
        if (val != null) return val;

        return reply.ToObjects<T>(null);
    }

    public static byte[] HexStringToByteArray(string hexString)
    {
        var bytes = new byte[hexString.Length / 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

    public static ulong TokenizeToIcp(this double value)
    {
        return (ulong)(100_000_000 * value);
    }
    public static double DestokenizeFromIcp(this ulong value)
    {
        return value/ (double)100_000_000;
    }
    public static ulong TokenizeToCkBtc(this double value)
    {
        return (ulong)(100_000_000 * value);
    }

    public static ulong Tokenize(this double value, ulong baseZeroCount)//Zero
    {
        return (ulong)(baseZeroCount * value);
    }
    public static double Destokenize(this ulong value, ulong baseZeroCount)//Zero
    {
        return value / (double)baseZeroCount;
    }
}
