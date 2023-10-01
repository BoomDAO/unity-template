using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Boom;
using UnityEngine;
using UnityEngine.UI;

public class MintTestTokensWindow : Window
{
    [SerializeField] string mintNftActionId;
    [SerializeField] string mintIcrcActionId;

    [SerializeField] Button mintNft;
    [SerializeField] Button mintIcrc;

    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        //NFT
        mintNft.onClick.AddListener(() =>
        {
            MintNft().Forget();
        });
        //ICRC
        mintIcrc.onClick.AddListener(() =>
        {
            MintIcrc().Forget();
        });
    }

    private async UniTaskVoid MintNft()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var actionResult = await ActionUtil.Action.Default(mintNftActionId);

        if (actionResult.Tag == UResultTag.Err)
        {
            Debug.LogError(actionResult.AsErr().Content);
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Upss!", actionResult.AsErr().content), 3);
            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();
        DisplayActionResponse(resultAsOk);

        //UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));
        NftUtil.TryAddMintedNft(resultAsOk.nfts.ToArray());

        BroadcastState.Invoke(new WaitingForResponse(false));

        Debug.Log($"Mint Nft Success");
    }

    private async UniTaskVoid MintIcrc()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var actionResult = await ActionUtil.Action.Default(mintIcrcActionId);

        if (actionResult.Tag == UResultTag.Err)
        {
            Debug.LogError(actionResult.AsErr().Content);
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Upss!", actionResult.AsErr().content), 3);

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();
        DisplayActionResponse(resultAsOk);

        var actionConfigResult = UserUtil.GetElementOfType<DataTypes.Action>(mintIcrcActionId);

        if (actionConfigResult.IsErr)
        {
            Debug.LogError(actionConfigResult.AsErr());
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Upss!", actionResult.AsErr().content), 3);

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        BroadcastState.Invoke(new WaitingForResponse(false));

        Debug.Log($"Mint ICRC Success, Wait for approval, reward {resultAsOk.tokens.Count}:\n\n " + resultAsOk.tokens.Reduce(e => $"Token canister: {e.Canister}\nQuantity: {e.Quantity}\n\n"));
    }

    private void DisplayActionResponse(ProcessedActionResponse resonse)
    {
        List<string> inventoryElements = new();

        //NFTs
        Dictionary<string, int> collectionsToDisplay = new();
        resonse.nfts.Iterate(e =>
        {

            if (collectionsToDisplay.TryAdd(e.Canister, 1) == false) collectionsToDisplay[e.Canister] += 1;

        });

        collectionsToDisplay.Iterate(e =>
        {
            string tokenName = "Some Name";
            var fetchOwnTokenDataResult = UserUtil.GetElementOfType<DataTypes.NftCollection>(e.Key);

            if (fetchOwnTokenDataResult.IsOk)
            {
                tokenName = fetchOwnTokenDataResult.AsOk().name;
            }
            else
            {
                tokenName = "Name not Found";
            }

            inventoryElements.Add($"{tokenName} x {e.Value}");
        });

        //Tokens
        resonse.tokens.Iterate(e =>
        {
            string tokenName = "Some Name";
            var fetchOwnTokenDataResult = UserUtil.GetElementOfType<DataTypes.TokenMetadata>(e.Canister);

            if (fetchOwnTokenDataResult.IsOk)
            {
                tokenName = fetchOwnTokenDataResult.AsOk().name;
            }
            else
            {
                tokenName = "ICRC";
            }

            inventoryElements.Add($"{tokenName} x {e.Quantity}");
        });

        WindowManager.Instance.OpenWindow<InventoryPopupWindow>(new InventoryPopupWindow.WindowData("Earned Items", inventoryElements), 3);
    }
}
