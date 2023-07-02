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
    public const long BASE_UNIT_ICP = 100_000_000;
    public const long BASE_UNIT_CKBTC = 100_000_000;

    public static byte[] HexStringToByteArray(string hexString)
    {
        var bytes = new byte[hexString.Length / 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

    public static ulong TokenizeToCkBtc(this double value)
    {
        return (ulong)(100_000_000 * value);
    }

    public static ulong ConvertToBaseUnit(this double value, ulong baseZeroCount)//Zero
    {
        return (ulong)(baseZeroCount * value);
    }
    public static double ConvertToDecimal(this ulong value, ulong baseZeroCount)//Zero
    {
        return value / (double)baseZeroCount;
    }

}
