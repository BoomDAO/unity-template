using Boom.Utility;
using Candid.World.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class CandidUtil
{
    public const byte ICP_DECIMALS = 8;

    public static byte[] HexStringToByteArray(string hexString)
    {
        var bytes = new byte[hexString.Length / 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = System.Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

    public static ulong ConvertToBaseUnit(this double value, byte decimals)//Zero
    {
        var baseUnitCount = decimals == 0 ? 0 : (ulong)Mathf.Pow(10, decimals);


        return (ulong)(baseUnitCount * value);
    }
    public static double ConvertToDecimal(this ulong value, byte decimals)//Zero
    {
        var baseUnitCount = decimals == 0 ? 0 : (ulong)Mathf.Pow(10, decimals);


        return value / (double)baseUnitCount;
    }

    public static bool MathAdd(string a, double b, out double returnVal)
    {
        returnVal = default;

        if (!a.TryParseValue<double>(out var _a))
        {
            "Could not parse a".Error();
            return false;
        }

        returnVal = _a + b;

        return true;
    }
    public static bool MathSub(string a, double b, out double returnVal)
    {
        returnVal = default;

        if (!a.TryParseValue<double>(out var _a))
        {
            "Could not parse a".Error();
            return false;
        }

        returnVal = _a - b;

        return true;
    }

    public static bool MathAdd(string a, ulong b, out ulong returnVal)
    {
        returnVal = default;

        if (!a.TryParseValue<ulong>(out var _a))
        {
            "Could not parse a".Error();
            return false;
        }

        returnVal = _a + b;

        return true;
    }
}
