using Candid;
using Candid.IcpLedger.Models;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalanceWindow : Window
{
    [SerializeField] Button reloadButton;
    [SerializeField] TMP_Text icpBalanceText;
    [SerializeField] TMP_Text icrcBalances;

    public class WindowData
    {
        //DATA
    }
    public override bool RequireUnlockCursor()
    {
        return false;//or true
    }

    public override void Setup(object data)
    {
        icpBalanceText.text = $"ICP : {0}";
        icrcBalances.text = $"ICRC : {0}";
        UserUtil.RegisterToDataChange<DataTypes.Token>(UpdateWindow, true);

        reloadButton.onClick.AddListener(ReloadBalanceHandler);
    }

    private void ReloadBalanceHandler()
    {
        UserUtil.RequestData<DataTypes.Token>();
    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.Token>(UpdateWindow);
    }

    private void UpdateWindow(DataState<Data<DataTypes.Token>> obj)
    {
        var icpBalanceResult = UserUtil.GetDataElementOfType<DataTypes.Token>(Env.CanisterIds.ICP_LEDGER);
        var icrcBalanceResult = UserUtil.GetDataElementOfType<DataTypes.Token>(Env.CanisterIds.ICRC_LEDGER);


        if (icpBalanceResult.Tag == ItsJackAnton.Values.UResultTag.Ok)
        {
            icpBalanceText.text = $"ICP : {icpBalanceResult.AsOk().Amount.NotScientificNotation()}";
        }
        else
        {
            var result = UserUtil.IsAnonSignedIn();

            if(result.Tag == ItsJackAnton.Values.UResultTag.Ok)
            {
                if(result.AsOk()) icpBalanceText.text = $"ICP : {0}";
            }

            Debug.LogWarning(icpBalanceResult.AsErr());
        }

        if (icrcBalanceResult.Tag == ItsJackAnton.Values.UResultTag.Ok)
        {
            icrcBalances.text = $"ICRC : {icrcBalanceResult.AsOk().Amount.NotScientificNotation()}";
        }
        else
        {
            var result = UserUtil.IsAnonSignedIn();

            if (result.Tag == ItsJackAnton.Values.UResultTag.Ok)
            {
                if (result.AsOk()) icrcBalances.text = $"ICRC : {0}";
            }

            Debug.LogWarning(icrcBalanceResult.AsErr());
        }
    }
}