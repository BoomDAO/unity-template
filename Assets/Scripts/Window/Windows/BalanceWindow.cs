using Candid;
using Candid.IcpLedger.Models;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
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
        BroadcastState.Register<DataState<TokensData>>(UpdateWindow, true);
        icpBalanceText.text = "Balance = 0";
        reloadButton.onClick.AddListener(ReloadBalanceHandler);
    }

    private void ReloadBalanceHandler()
    {
        UserUtil.UpdateBalanceReq_Icp();
        UserUtil.UpdateBalanceReq_Rc();
    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<DataState<TokensData>>(UpdateWindow);
    }

    private void UpdateWindow(DataState<TokensData> obj)
    {
        var icpBalanceResult = UserUtil.GetToken(Env.CanisterIds.ICP_LEDGER);
        var icrcBalanceResult = UserUtil.GetToken(Env.CanisterIds.ICRC_LEDGER);

        icpBalanceText.text = $"ICP : {0}";
        icrcBalances.text = $"ICRC : {0}";

        if (icpBalanceResult.Tag == ItsJackAnton.Values.UResultTag.Ok)
        {
            icpBalanceText.text = $"ICP : {icpBalanceResult.AsOk().Amount}";
        }
        else
        {
            Debug.Log(icpBalanceResult.AsErr());
        }

        if (icrcBalanceResult.Tag == ItsJackAnton.Values.UResultTag.Ok)
        {
            icrcBalances.text = $"ICRC : {icrcBalanceResult.AsOk().Amount}";
        }
        else
        {
            Debug.Log(icrcBalanceResult.AsErr());
        }
    }
}