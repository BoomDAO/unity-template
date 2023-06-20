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
        BroadcastState.Register<DataState<IcpData>>(UpdateWindow);
        BroadcastState.Register<DataState<IcrcData>>(UpdateWindow, true);
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
        BroadcastState.Unregister<DataState<IcpData>>(UpdateWindow);
        BroadcastState.Unregister<DataState<IcrcData>>(UpdateWindow);
    }
    private void UpdateWindow(DataState<IcrcData> obj)
    {
        BroadcastState.TryRead<DataState<IcpData>>(out var icDataState);
        UpdateWindow(icDataState);
    }

    private void UpdateWindow(DataState<IcpData> obj)
    {
        if (CandidApiManager.IsUserLoggedIn == false)
        {
            icpBalanceText.text = "ICP = 0";
            return;
        }

        if(obj.IsReady() == false)
        {
            icpBalanceText.text = "ICP = 0";
            return;
        }

        icpBalanceText.text = $"ICP : {obj.data.amt/(float) 100_000_000}";

        if(BroadcastState.TryRead<DataState<IcrcData>>(out var IcrcDataDataState))
        {
            if (IcrcDataDataState.IsReady()) icrcBalances.text = $"{IcrcDataDataState.data.name}: {IcrcDataDataState.data.amt / Mathf.Pow(10, (float)IcrcDataDataState.data.decimalCount)}";
            else if (IcrcDataDataState.IsLoading()) icrcBalances.text = "ICRC: Loading...";
            else icrcBalances.text = "ICRC: None...";
        }
        else icrcBalances.text = "ICRC: None...";
    }
}