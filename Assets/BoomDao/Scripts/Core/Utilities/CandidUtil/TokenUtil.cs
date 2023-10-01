using Boom.Utility;
using Boom.Values;
using System.Linq;
using UnityEngine;

public static class TokenUtil
{
    public static double GetTokenAmountAsDecimal(string canisterId)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(canisterId, e => e.baseUnitAmount, 0);
        var decimals = UserUtil.GetPropertyFromType<DataTypes.TokenMetadata, byte>(canisterId, e => e.decimals, 0);

        //RETURN IN DECIMAL
        return CandidUtil.ConvertToDecimal(currentBaseUnitAmount, decimals);
    }
    public static double GetTokenAmountAsBaseUnit(string canisterId)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(canisterId, e => e.baseUnitAmount, 0);

        return currentBaseUnitAmount;
    }

    public static void IncrementTokenByDecimal(params (string canisterId, double decimalAmount)[] amountToAdd)
    {
        IncrementTokenByBaseUnit(amountToAdd.Map<(string canisterId, double decimalAmount), (string canisterId, ulong baseUnitAmount) > (e =>
        {

            var decimalCount = UserUtil.GetPropertyFromType<DataTypes.TokenMetadata, byte>(e.canisterId, k => k.decimals);
            if (decimalCount.IsErr)
            {
                Debug.LogWarning("Something went wrong");
                return (e.canisterId,0);
            }

            return (e.canisterId, e.decimalAmount.ConvertToBaseUnit(decimalCount.AsOk()));
        }).ToArray());
    }
    public static void IncrementTokenByBaseUnit(params (string canisterId, ulong baseUnitAmount)[] amountToAdd)
    {
        var tokensResult = UserUtil.GetDataOfType<DataTypes.Token>();

        if (tokensResult.IsErr)
        {
            Debug.LogWarning("Token Data is not yet ready");
            return;
        }

        var tokens = tokensResult.AsOk().data.elements;

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

        UserUtil.UpdateData(new DataTypes.Token[0]);
    }
    public static void DecrementTokenByBaseUnit(params (string canisterId, ulong baseUnitAmount)[] amountToRemove)
    {
        var tokensResult = UserUtil.GetDataOfType<DataTypes.Token>();

        if (tokensResult.IsErr)
        {
            Debug.LogWarning("Token Data is not yet ready");
            return;
        }

        var tokens = tokensResult.AsOk().data.elements;

        foreach (var item in amountToRemove)
        {
            if (tokens.TryGetValue(item.canisterId, out var token))
            {
                token.baseUnitAmount -= item.baseUnitAmount;
            }
        }

        UserUtil.UpdateData(new DataTypes.Token[0]);
    }

    /// <summary>
    /// This is for lassy people who wants to fetch a Token along with its Config by its tokenCanisterId
    /// </summary>
    /// <param name="canisterId">Canister Id of the Token</param>
    /// <returns></returns>
    public static UResult<(DataTypes.Token token, DataTypes.TokenMetadata configs), string> GetTokenDetails(string canisterId)
    {
        var tokenResult = UserUtil.GetElementOfType<DataTypes.Token>(canisterId);

        if (tokenResult.IsErr) return new(tokenResult.AsErr());

        var metadataResult = UserUtil.GetElementOfType<DataTypes.TokenMetadata>(canisterId);

        if (metadataResult.IsErr) return new(metadataResult.AsErr());

        return new((tokenResult.AsOk(), metadataResult.AsOk()));
    }
}