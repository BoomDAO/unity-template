using Candid;
using Candid.extv2_boom;
using Candid.extv2_boom.Models;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MintBurnNftWindow : Window
{
    [SerializeField] string burnNftActionId;
    [SerializeField] string mintNftActionId;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] TMP_Text mintingPriceText;
    [SerializeField] Button burnButton;
    [SerializeField] Button mintButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button closeButton;
    [SerializeField, ShowOnly] bool burningNft;
    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        UserUtil.RegisterToDataChange<DataTypes.BoomDaoNftCollection>(UpdateWindow, true);

        burnButton.onClick.AddListener(Burn);
        mintButton.onClick.AddListener(Mint);

        burnNftActionId = "burn_nft_tiket";
        mintNftActionId = "mint_nft_tiket";
    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.BoomDaoNftCollection>(UpdateWindow);
        burnButton.onClick.RemoveListener(Burn);
        mintButton.onClick.RemoveListener(Mint);
    }


    private void UpdateWindow(DataState<Data<DataTypes.BoomDaoNftCollection>> state)
    {
        var isUserSigned = UserUtil.IsUserSignedIn();

        if (isUserSigned.Tag == UResultTag.Err)
        {
            Debug.LogError(isUserSigned.AsErr());
            return;
        }

        if (isUserSigned.AsOk() == false)
        {
            loadingText.text = "You must log in...";
            burnButton.gameObject.SetActive(false);

            return;
        }

        //Minting

        var getDataResult = UserUtil.GetDataElementOfType<DataTypes.ActionConfig>(mintNftActionId);

        if (getDataResult.Tag == UResultTag.Err)
        {
            Debug.LogError(getDataResult.AsErr());
            return;
        }


        var actionConfig = getDataResult.AsOk();

        if (!actionConfig.ActionPlugin.HasValue)
        {
            Debug.LogError($"ActionId: {mintNftActionId} doesnt have an Action Plugin value");
            return;
        }

        var actionPlugin = actionConfig.ActionPlugin.ValueOrDefault;

        if (actionPlugin.Tag != ActionPluginTag.VerifyTransferIcp)
        {
            Debug.LogError($"ActionId: {mintNftActionId} Action Plugin is not of type: {ActionPluginTag.VerifyTransferIcp}");
            return;
        }

        var asVerifyTxIcpConfig = actionPlugin.AsVerifyTransferIcp();

        var mintingCost = asVerifyTxIcpConfig.Amt;

        mintingPriceText.text = $"Mint cost: {mintingCost}";

        //Burning
        if (!state.IsReady())
        {
            loadingText.text = "Checking for NFTs left...";
            burnButton.gameObject.SetActive(false);

            return;
        }

        var nftCountResult = NftUtil.GetNftCount(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        var nftCount = nftCountResult.AsOk();
        bool hasRequiredNfts = nftCount > 0;

        Debug.Log($"Your NFT Count for collection of id:\"{Env.Nfts.BOOM_COLLECTION_CANISTER_ID}\"  is: {nftCount}");

        if (hasRequiredNfts)
        {
            loadingText.text = $"You have {nftCount} NFTs left!";
            burnButton.gameObject.SetActive(true);
        }
        else
        {
            loadingText.text = "You have no NFTs left.";
            burnButton.gameObject.SetActive(false);

            Debug.Log("Update Wheel Window");
        }
    }

    private async void Burn()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));

        var getIsLoginResult = UserUtil.GetSignInType();

        if (getIsLoginResult.Tag == UResultTag.Err)
        {
            Debug.LogError(getIsLoginResult.AsErr());
            return;
        }

        bool isLoggedIn = getIsLoginResult.AsOk() == UserUtil.SigningType.user;

        if (isLoggedIn == false)
        {
            Debug.Log("> > > You must log in");

            return;
        }

        if (burningNft) return;

        burningNft = true;
        var result = await TxUtil.Action.BurnNft(burnNftActionId, Env.Nfts.BOOM_COLLECTION_CANISTER_ID);
        burningNft = false;

        UserUtil.RequestData<DataTypes.Item>();
        UserUtil.RequestData<DataTypes.BoomDaoNftCollection>();

        if (result.Tag == UResultTag.Err) Debug.LogError(result.AsErr());
        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }

    private async void Mint()
    {
        BroadcastState.Invoke(new DisableButtonInteraction(true));

        var actionResult = await TxUtil.Action.TransferAndVerifyIcp(mintNftActionId);

        if (actionResult.Tag == UResultTag.Err)
        {
            Debug.LogError("Mint Failure, msg: " + actionResult.AsErr());
            return;
        }

        var actionResultAsOk = actionResult.AsOk();

        Debug.Log($"Minting NFT Success, NFTs minted count: {actionResultAsOk.F2.Count}");

        UserUtil.RequestData<DataTypes.BoomDaoNftCollection>();

        BroadcastState.Invoke(new DisableButtonInteraction(false));
    }
}