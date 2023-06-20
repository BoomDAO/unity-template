using Candid;
using Candid.extv2_boom;
using Candid.extv2_boom.Models;
using EdjCase.ICP.Candid.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BurnNftWindow : Window
{

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
        //if (CandidApiManager.IsUserLoggedIn == false)
        //{
        //    Debug.Log("> > > You must log in");

        //    return;
        //}

        //Debug.Log("> > > TryBurnNftAndSpin");
        //if (burningNft) return;

        //var count = DabNftUtil.GetNftCount(DabNftUtil.MainGameCollectionID, Env.Nfts.NFT_OF_USAGE_TO_BURN);

        //if (count > 0)
        //{
        //    if (DabNftUtil.TryGetNextNftIndex(DabNftUtil.MainGameCollectionID, out long index, Env.Nfts.NFT_OF_USAGE_TO_BURN))
        //    {
        //        Debug.Log("Try to burn nft of index: " + index);

        //        burningNft = true;
        //        burnButton.interactable = false;
        //        closeButton.interactable = false;
        //        shopButton.interactable = false;

        //        if (DabNftUtil.TryRemoveNftByIndex(DabNftUtil.MainGameCollectionID, index) == false)
        //        {
        //            Debug.LogWarning("> > > Burn: was not able to remove burned nft on client");
        //        }
        //        var response = await CandidApiManager.Instance.WorldApiClient.BurnNft(DabNftUtil.MainGameCollectionID, (uint)index, CandidApiManager.UserAccountIdentity);
        //        burningNft = false;
        //        burnButton.interactable = true;
        //        closeButton.interactable = true;
        //        shopButton.interactable = true;

        //        if (response.Tag == Candid.game.Models.Result_2Tag.Ok)
        //        {
        //            Debug.Log("> > > Burn Success");
        //            //DabNftUtil.TryGetNft(DabNftUtil.MainGameCollectionID, index, out var dabNftDetails);
        //            var okValue = response.AsOk();
        //            var gameTx = okValue.F0;
        //            var nfts = okValue.F1;
        //            var ItemsToAdd = ((gameTx.Items.ValueOrDefault ?? new()).Add ?? new()).ValueOrDefault ?? new();
        //            Debug.Log($"> > > Burn Success, items to add: {JsonConvert.SerializeObject(ItemsToAdd)}");

        //            BroadcastState.ForceInvoke<DataState<UserNodeData>>(gameUserDataState =>
        //            {
        //                if (gameTx != null)
        //                {
        //                    ItemsToAdd.Iterate(addedItem =>
        //                    {
        //                        Debug.Log($"Add item off id {addedItem.Id}, quantity: {addedItem.Quantity}");

        //                        gameUserDataState.data.items = gameUserDataState.data.items ?? new();
        //                        if (gameUserDataState.data.items.TryAdd(addedItem.Id, new(addedItem.Id, addedItem.Quantity)) == false)
        //                        {
        //                            gameUserDataState.data.items[addedItem.Id].quantity += addedItem.Quantity;
        //                        }
        //                    });
        //                }

        //                return gameUserDataState;
        //            });
        //        }
        //        else
        //        {
        //            Debug.LogError("Failed to retrieve display items on the AbundanceWheel :" + response.AsErr());
        //        }
        //    }
        //}
    }

    private async void Mint()
    {
        Extv2BoomApiClient collectionInterface = new(CandidApiManager.Instance.Agent, Principal.FromText(Env.Nfts.BOOM_COLLECTION));

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