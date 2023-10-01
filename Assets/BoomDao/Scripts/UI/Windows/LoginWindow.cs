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

    readonly List<Type> typesToLoad = new();

    bool? initialized;

    public override bool RequireUnlockCursor()
    {
        return true;
    }
    public override void Setup(object data)
    {
        UserUtil.RegisterToLoginDataChange(UpdateWindow, true);
        BroadcastState.Register<CanLogin>(UpdateWindow, true);
        UserUtil.RegisterToDataChange<DataTypes.Entity>(UpdateWindow);
        UserUtil.RegisterToDataChange<DataTypes.ActionState>(UpdateWindow);
        UserUtil.RegisterToDataChange<DataTypes.NftCollection>(UpdateWindow);

        logInBtn.onClick.AddListener(LogIn);
        logOutBtn.onClick.AddListener(LogoutUser);

        loadingTxt.text = "Loading...";
        principalTxt.text = "";

        typesToLoad.Add(typeof(DataTypes.Entity));
        typesToLoad.Add(typeof(DataTypes.ActionState));
        typesToLoad.Add(typeof(DataTypes.NftCollection));
    }


    private void OnDestroy()
    {
        LoginManager.Instance.CancelLogin();

        logInBtn.onClick.RemoveListener(LogIn);
        logOutBtn.onClick.RemoveListener(LogoutUser);

        UserUtil.UnregisterToLoginDataChange(UpdateWindow);
        BroadcastState.Unregister<CanLogin>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.Entity>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.ActionState>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.NftCollection>(UpdateWindow);
    }

    private void UpdateWindow(CanLogin state)
    {
        logInBtn.interactable = state.value;
    }

    private void UpdateWindow(DataState<Data<DataTypes.Entity>> state)
    {
        if (state.IsReady() && typesToLoad.Count > 0)
        {
            if (typesToLoad.Contains(typeof(DataTypes.Entity)))
            {
                typesToLoad.Remove(typeof(DataTypes.Entity));
            }
        }

        var loginDataStateResult = UserUtil.GetLogInDataState();
        if (loginDataStateResult.IsOk) UpdateWindow(loginDataStateResult.AsOk());
    }
    private void UpdateWindow(DataState<Data<DataTypes.ActionState>> state)
    {
        if (state.IsReady() && typesToLoad.Count > 0)
        {
            if (typesToLoad.Contains(typeof(DataTypes.ActionState)))
            {
                typesToLoad.Remove(typeof(DataTypes.ActionState));
            }
        }

        var loginDataStateResult = UserUtil.GetLogInDataState();
        if (loginDataStateResult.IsOk) UpdateWindow(loginDataStateResult.AsOk());
    }
    private void UpdateWindow(DataState<Data<DataTypes.NftCollection>> state)
    {
        if (UserUtil.IsDataValid<DataTypes.NftCollection>(Env.Nfts.BOOM_COLLECTION_CANISTER_ID) && typesToLoad.Count > 0)
        {
            if (typesToLoad.Contains(typeof(DataTypes.NftCollection)))
            {
                typesToLoad.Remove(typeof(DataTypes.NftCollection));
            }
        }

        var loginDataStateResult = UserUtil.GetLogInDataState();
        if (loginDataStateResult.IsOk) UpdateWindow(loginDataStateResult.AsOk());
    }

    private void UpdateWindow(DataState<LoginData> state)
    {
        bool isLoading = state.IsLoading();
        var getIsLoginResult = UserUtil.GetLogInType();

        if (getIsLoginResult.Tag == UResultTag.Ok)
        {
            if(getIsLoginResult.AsOk() == UserUtil.LoginType.User)
            {
                var isUserDataLoaded =
                    UserUtil.IsDataValid<DataTypes.Entity>() &&
                    UserUtil.IsDataValid<DataTypes.ActionState>() &&
                    UserUtil.IsDataValid<DataTypes.NftCollection>(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

                logInBtn.gameObject.SetActive(false);

                if (isUserDataLoaded || (initialized.HasValue && initialized.Value))
                {
                    initialized = true;

                    logInStateTxt.text = "Logged In";
                    principalTxt.text = $"Principal: <b>\"{state.data.principal}\"</b>\nAccountId: <b>\"{state.data.accountIdentifier}\"</b>";
                    pageControl.SetActive(true);
                    logOutBtn.gameObject.SetActive(true);
                    loadingTxt.text = "";
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
            }
        }
        else
        {
            if (isLoading) loadingTxt.text = state.LoadingMsg;
            else loadingTxt.text = "Loading...";
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
        UserUtil.StartLogin("Logging In...");
    }
}