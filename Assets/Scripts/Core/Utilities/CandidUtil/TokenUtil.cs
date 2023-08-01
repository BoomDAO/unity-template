using EdjCase.ICP.Candid.Models;
using System;

public static class TokenUtil
{
    public static double IncrementTokenByDecimal(string canisterId, double decimalAmount)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(canisterId, e => e.baseUnitAmount, 0);
        var decimals = UserUtil.GetPropertyFromType<DataTypes.TokenConfig, byte>(canisterId, e => e.decimals, 0);

        if (decimalAmount < 0)
        {
            return CandidUtil.ConvertToDecimal(currentBaseUnitAmount, decimals);
        }

        var baseUnitAmountToEditBy = CandidUtil.ConvertToBaseUnit(decimalAmount, decimals);

        var newAmount = currentBaseUnitAmount + baseUnitAmountToEditBy;
        if(currentBaseUnitAmount != newAmount) UserUtil.UpdateData(new DataTypes.Token(canisterId, newAmount));

        //RETURN IN DECIMAL
        return CandidUtil.ConvertToDecimal(newAmount, decimals);
    }
    public static double IncrementTokenByBaseUnit(string canisterId, ulong baseUnitAmount)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(canisterId, e => e.baseUnitAmount, 0);

        var newAmount = currentBaseUnitAmount + baseUnitAmount;

        if (currentBaseUnitAmount != newAmount) UserUtil.UpdateData(new DataTypes.Token(canisterId, (ulong)newAmount));

        //RETURN IN BASE UNIT
        return newAmount;
    }

    public static double DecrementTokenByDecimal(string canisterId, double decimalAmount)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(canisterId, e => e.baseUnitAmount, 0);
        var decimals = UserUtil.GetPropertyFromType<DataTypes.TokenConfig, byte>(canisterId, e => e.decimals, 1);

        if (decimalAmount > 0)
        {
            return CandidUtil.ConvertToDecimal(currentBaseUnitAmount, decimals);
        }

        var baseUnitAmountToEditBy = CandidUtil.ConvertToBaseUnit(decimalAmount, decimals);

        var newAmount = currentBaseUnitAmount - baseUnitAmountToEditBy;
        if (newAmount < 0) newAmount = 0;

        if (currentBaseUnitAmount != newAmount) UserUtil.UpdateData(new DataTypes.Token(canisterId, newAmount));

        //RETURN IN DECIMAL
        return CandidUtil.ConvertToDecimal(newAmount, decimals);
    }
    public static double DecrementTokenByBaseUnit(string canisterId, ulong baseUnitAmount)
    {
        var currentBaseUnitAmount = UserUtil.GetPropertyFromType<DataTypes.Token, ulong>(canisterId, e => e.baseUnitAmount, 0);

        var newAmount = currentBaseUnitAmount > baseUnitAmount? currentBaseUnitAmount - baseUnitAmount : 0;

        if (currentBaseUnitAmount != newAmount) UserUtil.UpdateData(new DataTypes.Token(canisterId, (ulong)newAmount));

        //RETURN IN BASE UNIT
        return newAmount;
    }
}