using System;
using Candid;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : Window
{
    public class WindowData
    {
    }
    [SerializeField] DataState dataState;
    public Button logInBtn;
    public Button logOutBtn;
    public TMP_Text logInStateTxt;
    public TMP_Text loadingTxt;
    public TMP_Text principalTxt;
    public GameObject pageControl;
    string enqueueJob;

    Window inventoryWindow;
    Window balanceWindow;
    public override bool RequireUnlockCursor()
    {
        return true;
    }
    public override void Setup(object data)
    {
        BroadcastState.Register<DataState<UserNodeData>>(UpdateWindow, true);

        logInBtn.onClick.AddListener(LogIn);
        logOutBtn.onClick.AddListener(LogoutUser);

        loadingTxt.text = "";
        principalTxt.text = "";
    }

    private void OnDestroy()
    {
        LoginManager.Instance.CancelLogin();

        logInBtn.onClick.RemoveListener(LogIn);
        logOutBtn.onClick.RemoveListener(LogoutUser);

        BroadcastState.Unregister<DataState<UserNodeData>>(UpdateWindow);
    }

    private void UpdateWindow(DataState<UserNodeData> state)
    {
        bool isLoading = state.IsLoading();
        dataState = state.State;

        logInBtn.gameObject.SetActive(!CandidApiManager.IsUserLoggedIn);
        logOutBtn.gameObject.SetActive(CandidApiManager.IsUserLoggedIn);
        loadingTxt.gameObject.SetActive(isLoading);

        loadingTxt.text = isLoading ? "Loading..." : string.Empty;
        logInStateTxt.text = CandidApiManager.IsUserLoggedIn ? "Logged In" : "Logged out";

        if (state.IsReady() && CandidApiManager.IsUserLoggedIn)
        {
            principalTxt.text = $"Signed in as <b>\"{CandidApiManager.UserPrincipal}</b>";
            pageControl.SetActive(true);
        }
        else
        {
            principalTxt.text = $"";
            pageControl.SetActive(false);
        }

    }

    //

    public void CancelWalletIntegration()
    {
        Close();
    }

    private void LogoutUser()
    {
        PlayerPrefs.SetString("authTokenId", string.Empty);
        Broadcast.Invoke<UserLogout>();
        if (inventoryWindow) Destroy(inventoryWindow);
    }

    //Login
    public void LogIn()
    {
        //Debug.Log("Try Log In");

        PlayerPrefs.SetString("walletType", "II");

        BroadcastState.ForceInvoke<DataState<UserNodeData>>((e) =>
        {
            e.SetAsLoading();
            return e;
        });
        BroadcastState.ForceInvoke<DataState<DabNftsData>>(e =>
        {
            e.SetAsLoading();
            return e;
        });

        enqueueJob = EnqueueJobManager.Instance.EnqueueJob(() =>
        {
            WindowGod.Instance.OpenWindow<BalanceWindow>(null);
            WindowGod.Instance.OpenWindow<InventoryWindow>(null);
        });

#if UNITY_WEBGL && !UNITY_EDITOR
            LoginManager.Instance.StartLoginFlowWebGl(OnLoginCompleted);
            return;
#endif

        LoginManager.Instance.StartLoginFlow(OnLoginCompleted);
    }

    void OnLoginCompleted(string json)
    {
        EnqueueJobManager.Instance.ExecuteJob(enqueueJob);
        //Debug.Log("Try Create Agent");

        if (!CandidApiManager.IsUserLoggedIn)
        {
            //Debug.Log("Create Agent");

            CandidApiManager.Instance.CreateAgentUsingIdentityJson(json, false).Forget();
        }
        else
        {
            Debug.Log("You already have an Agent created");

        }
    }
}