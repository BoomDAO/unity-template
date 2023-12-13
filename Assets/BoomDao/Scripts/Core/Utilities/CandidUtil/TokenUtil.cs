using Boom.Utility;
using Boom.Values;
using System.Linq;
using UnityEngine;
using Wasmtime;
using static Env;

public static class TokenUtil
{
    public static double GetTokenAmountAsDecimal(string uid, string canisterId)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(uid, canisterId, e => e.baseUnitAmount, 0);

        if (ConfigUtil.TryGetTokenConfig(canisterId, out var tokenConfig) == false)
        {
            Debug.LogError("Failure to find config of tokenId: " + canisterId);
            return 0;
        }

        //RETURN IN DECIMAL
        return CandidUtil.ConvertToDecimal(currentBaseUnitAmount, tokenConfig.decimals);
    }
    public static double GetTokenAmountAsBaseUnit(string uid, string canisterId)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(uid, canisterId, e => e.baseUnitAmount, 0);

        return currentBaseUnitAmount;
    }

    public static void IncrementTokenByDecimal(string uid, params (string canisterId, double decimalAmount)[] amountToAdd)
    {
        try
        {
            IncrementTokenByBaseUnit(uid, amountToAdd.Map<(string canisterId, double decimalAmount), (string canisterId, ulong baseUnitAmount)>(e =>
            {
                if (ConfigUtil.TryGetTokenConfig(e.canisterId, out var tokenConfig) == false)
                {
                    throw new System.Exception($"Isse finding token config of canisterId: {e.canisterId}");
                }

                return (e.canisterId, e.decimalAmount.ConvertToBaseUnit(tokenConfig.decimals));
            }).ToArray());
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public static void IncrementTokenByBaseUnit(string uid, params (string canisterId, ulong baseUnitAmount)[] amountToAdd)
    {
        var tokensResult = UserUtil.GetData<DataTypes.Token>(uid);

        if (tokensResult.IsErr)
        {
            Debug.LogWarning("Token Data is not yet ready");
            return;
        }

        var tokens = tokensResult.AsOk().elements;

        foreach (var item in amountToAdd)
        {
            if(tokens.TryGetValue(item.canisterId, out var token))
            {
                token.baseUnitAmount += item.baseUnitAmount;
            }
            else
            {
                tokens.Add(item.canisterId, new DataTypes.Token(item.canisterId, item.baseUnitAmount));
            }
        }

        UserUtil.UpdateData(uid, new DataTypes.Token[0]);
    }
    public static void DecrementTokenByBaseUnit(string uid, params (string canisterId, ulong baseUnitAmount)[] amountToRemove)
    {
        var tokensResult = UserUtil.GetData<DataTypes.Token>(uid);

        if (tokensResult.IsErr)
        {
            Debug.LogWarning("Token Data is not yet ready");
            return;
        }

        var tokens = tokensResult.AsOk().elements;

        foreach (var item in amountToRemove)
        {
            if (tokens.TryGetValue(item.canisterId, out var token))
            {
                token.baseUnitAmount -= item.baseUnitAmount;
            }
        }

        UserUtil.UpdateData(uid, new DataTypes.Token[0]);
    }

    /// <summary>
    /// This is for lassy people who wants to fetch a Token along with its Config by its tokenCanisterId
    /// </summary>
    /// <param name="canisterId">Canister Id of the Token</param>
    /// <returns></returns>
    public static UResult<(DataTypes.Token token, MainDataTypes.AllTokenConfigs.TokenConfig configs), string> GetTokenDetails(string uid, string canisterId)
    {
        var tokenResult = UserUtil.GetElementOfType<DataTypes.Token>(uid, canisterId);

        if (tokenResult.IsErr) return new(tokenResult.AsErr());

        if (ConfigUtil.TryGetTokenConfig(canisterId, out var tokenConfig) == false)
        {
            return new($"Issue finding token configs of canisterId: {canisterId}");
        }

        return new((tokenResult.AsOk(), tokenConfig));
    }
}