using System;
using Candid;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Values;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : Window
{
    public class WindowData
    {
    }

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
        UserUtil.RegisterToLoginDataChange(UpdateWindow, true);

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

        UserUtil.UnregisterToLoginDataChange(UpdateWindow);
    }

    private void UpdateWindow(DataState<SignInData> state)
    {
        bool isLoading = state.IsLoading();

        var getIsLoginResult = UserUtil.GetSignInType();

        loadingTxt.gameObject.SetActive(isLoading);

        loadingTxt.text = isLoading ? "Loading..." : string.Empty;

        if(getIsLoginResult.Tag == UResultTag.Ok)
        {
            if(getIsLoginResult.AsOk() == UserUtil.SigningType.user)
            {
                logInStateTxt.text = "Signed";
                principalTxt.text = $"Principal: <b>\"{state.data.principal}\"</b>\nAccountId: <b>\"{state.data.accountIdentifier}\"</b>";
                pageControl.SetActive(true);
                logInBtn.gameObject.SetActive(false);
                logOutBtn.gameObject.SetActive(true);
                loadingTxt.gameObject.SetActive(false);
            }
            else
            {
                logInStateTxt.text = "Signed in as Anon";
                principalTxt.text = $"Principal: <b>\"{state.data.principal}\"</b>\nAccountId: <b>\"{state.data.accountIdentifier}\"</b>";
                loadingTxt.text = "You must sign your user";
                pageControl.SetActive(false);
                logInBtn.gameObject.SetActive(true);
                logOutBtn.gameObject.SetActive(false);
                loadingTxt.gameObject.SetActive(true);

                if (isLoading) loadingTxt.text = state.LoadingMsg;
                else loadingTxt.text = "You must sign your user";
            }

        }
        else
        {
            logInStateTxt.text = "Logged out";
            principalTxt.text = $"";
            pageControl.SetActive(false);
            logInBtn.gameObject.SetActive(false);
            logOutBtn.gameObject.SetActive(false);

            loadingTxt.gameObject.SetActive(false);
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

        var getIsLoginResult = UserUtil.GetSignInType();

        if(getIsLoginResult.Tag == UResultTag.Err)
        {
            UserUtil.StartLogin(json, false);
            return;
        }

        if (getIsLoginResult.Tag == UResultTag.Ok && getIsLoginResult.AsOk() == UserUtil.SigningType.anon)
        {
            UserUtil.StartLogin(json, false);
            return;
        }

        Debug.Log("You already have an Agent created");
    }
}