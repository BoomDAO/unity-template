using Candid;
using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Values;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Boom.Utility;
using Boom;
using System.Linq;

public class LoginWindow : Window
{
    public class WindowData
    {
    }

    public Button logInBtn;
    public Button logOutBtn;
    public TextMeshProUGUI logInStateTxt;
    public TextMeshProUGUI loadingTxt;
    public TextMeshProUGUI principalTxt;
    public GameObject pageControl;
    public GameObject roomsButton;

    Window balanceWindow;

    readonly List<Type> typesToLoad = new();

    bool? initialized;

    public override bool RequireUnlockCursor()
    {
        return true;
    }
    public override void Setup(object data)
    {
        UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(UpdateWindow, true);
        BroadcastState.Register<CanLogin>(UpdateWindow, true);
        UserUtil.AddListenerDataChangeSelf<DataTypes.Entity>(UpdateWindow);
        UserUtil.AddListenerDataChangeSelf<DataTypes.ActionState>(UpdateWindow);
        UserUtil.AddListenerDataChangeSelf<DataTypes.NftCollection>(UpdateWindow);

        logInBtn.onClick.AddListener(LogIn);
        logOutBtn.onClick.AddListener(LogoutUser);

        loadingTxt.text = "Loading...";
        principalTxt.text = "";
        pageControl.SetActive(false);

        typesToLoad.Add(typeof(DataTypes.Entity));
        typesToLoad.Add(typeof(DataTypes.ActionState));
        typesToLoad.Add(typeof(DataTypes.NftCollection));

        if (CandidApiManager.Instance.BoomDaoGameType == CandidApiManager.GameType.SinglePlayer) roomsButton.SetActive(false);
    }


    private void OnDestroy()
    {
        LoginManager.Instance.CancelLogin();

        logInBtn.onClick.RemoveListener(LogIn);
        logOutBtn.onClick.RemoveListener(LogoutUser);

        UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(UpdateWindow);
        BroadcastState.Unregister<CanLogin>(UpdateWindow);
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.Entity>(UpdateWindow);
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.ActionState>(UpdateWindow);
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.NftCollection>(UpdateWindow);
    }

    private void UpdateWindow(CanLogin state)
    {
        logInBtn.interactable = state.value;
    }

    private void UpdateWindow(Data<DataTypes.Entity> state)
    {
        if (!UserUtil.IsFetchingData<DataTypes.Entity>() && typesToLoad.Count > 0)
        {
            if (typesToLoad.Contains(typeof(DataTypes.Entity)))
            {
                typesToLoad.Remove(typeof(DataTypes.Entity));
            }
        }

        var loginDataStateResult = UserUtil.GetLogInData();
        if (loginDataStateResult.IsOk) UpdateWindow(loginDataStateResult.AsOk());
    }
    private void UpdateWindow(Data<DataTypes.ActionState> state)
    {
        if (!UserUtil.IsFetchingData<DataTypes.ActionState>() && typesToLoad.Count > 0)
        {
            if (typesToLoad.Contains(typeof(DataTypes.ActionState)))
            {
                typesToLoad.Remove(typeof(DataTypes.ActionState));
            }
        }

        var loginDataStateResult = UserUtil.GetLogInData();
        if (loginDataStateResult.IsOk) UpdateWindow(loginDataStateResult.AsOk());
    }
    private void UpdateWindow(Data<DataTypes.NftCollection> state)
    {
        if (UserUtil.IsDataValidSelf<DataTypes.NftCollection>() && typesToLoad.Count > 0)
        {
            if (typesToLoad.Contains(typeof(DataTypes.NftCollection)))
            {
                typesToLoad.Remove(typeof(DataTypes.NftCollection));
            }
        }

        var loginDataStateResult = UserUtil.GetLogInData();
        if (loginDataStateResult.IsOk) UpdateWindow(loginDataStateResult.AsOk());
    }

    private void UpdateWindow(MainDataTypes.LoginData state)
    {
        var loginDataResult = UserUtil.GetLogInData();

        //bool isValid = UserUtil.IsDataValid<DataTypes.LoginData>();
        //if (!isValid) return;

        bool isLoading = UserUtil.IsLoginIn();

        if (loginDataResult.IsOk && !isLoading)
        {
            var loginData = loginDataResult.AsOk();
            var getIsLoginResult = UserUtil.GetLoginType();

            if(getIsLoginResult.AsOk() == UserUtil.LoginType.User)
            {
                var isUserDataLoaded =
                    UserUtil.IsDataValidSelf<DataTypes.Entity>() &&
                    UserUtil.IsDataValidSelf<DataTypes.ActionState>() &&
                    UserUtil.IsDataValidSelf<DataTypes.NftCollection>();

                logInBtn.gameObject.SetActive(false);

                if (isUserDataLoaded || (initialized.HasValue && initialized.Value))
                {
                    initialized = true;

                    logInStateTxt.text = "Logged In";
                    principalTxt.text = $"Principal: <b>\"{loginData.principal}\"</b>\nAccountId: <b>\"{loginData.accountIdentifier}\"</b>";
                    pageControl.SetActive(true);
                    logOutBtn.gameObject.SetActive(true);
                    loadingTxt.text = "";

                    if(balanceWindow == null) balanceWindow = WindowManager.Instance.OpenWindow<BalanceWindow>(null, 1000);

                }
                else
                {
                    loadingTxt.text = $"{typesToLoad.Reduce(e=> $"Loading {e.Name} Data...","\n")}";
                }
            }
            else//Logged In As Anon
            {
                if (initialized.HasValue && initialized.Value)
                {
                    typesToLoad.Add(typeof(DataTypes.Entity));
                    typesToLoad.Add(typeof(DataTypes.ActionState));
                    typesToLoad.Add(typeof(DataTypes.NftCollection));
                }
                initialized = false;


                logInStateTxt.text = "";//"Logged in as Anon";
                principalTxt.text = ""; //$"Principal: <b>\"{state.data.principal}\"</b>\nAccountId: <b>\"{state.data.accountIdentifier}\"</b>";
                pageControl.SetActive(false);
                logInBtn.gameObject.SetActive(true);
                logOutBtn.gameObject.SetActive(false);
                loadingTxt.text = "Please login";

                if (balanceWindow) balanceWindow.Close();

            }
        }
        else
        {
            if (isLoading) loadingTxt.text = "Loading...";
            else loadingTxt.text = "";
            logInStateTxt.text = "";
            principalTxt.text = $"";
            pageControl.SetActive(false);
            logInBtn.gameObject.SetActive(false);
            logOutBtn.gameObject.SetActive(false);
        }
    }

    //

    private void LogoutUser()
    {
        Broadcast.Invoke<UserLogout>();
    }

    //Login
    public void LogIn()
    {
        Broadcast.Invoke<UserLoginRequest>();
    }
}