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
using UnityEngine;
using UnityEngine.UI;

public class BurnNftWindow : Window
{
    [SerializeField] string burnNftActionId;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] Button burnButton;
    [SerializeField] Button mintButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button closeButton;
    [SerializeField] bool burningNft;
    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        BroadcastState.Register<DataState<DabNftsData>>(UpdateWindow, true);
        burnButton.onClick.AddListener(Burn);
        mintButton.onClick.AddListener(Mint);
    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<DataState<DabNftsData>>(UpdateWindow);
        burnButton.onClick.RemoveListener(Burn);
        mintButton.onClick.RemoveListener(Mint);
    }
    private void UpdateWindow(DataState<DabNftsData> obj)
    {
        bool isLoggedIn = CandidApiManager.IsUserLoggedIn;

        if (isLoggedIn == false)
        {
            loadingText.text = "Checking for spins left...";
            burnButton.gameObject.SetActive(false);

            return;
        }

        BroadcastState.TryRead<DataState<DabNftsData>>(out var dabNftsState);

        if (!dabNftsState.IsReady())
        {
            loadingText.text = "Checking for spins left...";
            burnButton.gameObject.SetActive(false);

            return;
        }

        int nftCount = DabNftUtil.GetNftCount(DabNftUtil.MainGameCollectionID, Env.Nfts.NFT_OF_USAGE_TO_BURN);

        bool hasRequiredNfts = nftCount > 0;

        Debug.Log($"Your NFT Count for collection of name:\"Plethora\" of usage: \"{Env.Nfts.NFT_OF_USAGE_TO_BURN}\" is: {nftCount}");

        if (hasRequiredNfts)
        {
            loadingText.text = $"You have {nftCount} spins left!";
            burnButton.gameObject.SetActive(true);
        }
        else
        {
            loadingText.text = "You have no spin left.";
            burnButton.gameObject.SetActive(false);

            Debug.Log("Update Wheel Window");
        }
    }

    private async void Burn()
    {
        if (CandidApiManager.IsUserLoggedIn == false)
        {
            Debug.Log("> > > You must log in");

            return;
        }

        Debug.Log("> > > TryBurnNftAndSpin");
        if (burningNft) return;

        BroadcastState.TryRead<DataState<WorldConfigsData>>(out var worldConfigsData);
        BroadcastState.TryRead<DataState<DabNftsData>>(out var dabNftsData);

        if (worldConfigsData.IsReady() == false)
        {
            Debug.LogError("OffersConfig is not ready");
            burningNft = false;
            return;
        }
        if (dabNftsData.IsReady() == false)
        {
            Debug.LogError("DabNftsData is not ready");
            burningNft = false;
            return;
        }
        if(!DabNftUtil.TryGetNextNftIndex(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, out uint nftIndex))
        {
            Debug.LogError($"Could not find next nft to burn cuz u might not have any of the selected collection {Env.Nfts.BOOM_COLLECTION_CANISTER_ID}");

            return;
        }

        if (UserUtil.TryGetActionConfigData(burnNftActionId, out var config) == false)
        {
            Debug.LogError($"id {burnNftActionId} doesn't exist in configs");
            burningNft = false;
            return;
        }

        if (!config.Tag.HasValue)
        {
            burningNft = false;
            return;
        }
        if (config.Tag.ValueOrDefault != "BurnNft")
        {
            Debug.LogError($"id {burnNftActionId} is not of tag BurnNft");
            burningNft = false;
            return;
        }

        var actionPlugin = config.ActionPlugin.ValueOrDefault;

        if (actionPlugin != null)
        {
            Debug.Log($"Action Type TAG: {actionPlugin.Tag}");
            if (actionPlugin.Tag != ActionPluginTag.BurnNft)
            {
                Debug.LogError($"id {burnNftActionId} is not of type BurnNft");
                burningNft = false;
                return;
            }
        }

        var actionResult = await TxUtil.ProcessActionEntities(new ActionArgValueTypes.BurnNftArg(burnNftActionId, nftIndex));

        if(actionResult.Tag == UResultTag.Ok)
        {
            Debug.Log("Burn Success");
        }
        else
        {
            Debug.Log("Burn Failure");
        }
    }

    private async void Mint()
    {
        Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(Env.Nfts.BOOM_COLLECTION_CANISTER_ID));

        var tokensCountResult = await collectionInterface.GetTotalTokens();

        Debug.Log($"Try to mint nft of index " + tokensCountResult);

        var mintingResult = await collectionInterface.ExtMint(new()
        {
            new Extv2BoomApiClient.ExtMintArg0Item(CandidApiManager.UserAccountIdentity,
            new Candid.extv2_boom.Models.Metadata(Candid.extv2_boom.Models.MetadataTag.Nonfungible, new  Metadata.NonfungibleInfo()
            {
                Name ="",
                Asset = $"nftAsset:jh775-jaaaa-aaaal-qbuda-cai{tokensCountResult}",
                Thumbnail = $"nftAsset:jh775-jaaaa-aaaal-qbuda-cai{tokensCountResult}",
                Metadata = new OptionalValue<MetadataContainer>(new MetadataContainer(MetadataContainerTag.Json, "{"+"\"usage\": \"pastry-variable-offer\""+"}"))
            }))
        });


        Debug.Log("You have minted nft of token " + mintingResult);
    }
}