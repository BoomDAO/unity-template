using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using Candid.IcrcLedger;
using Candid.World.Models;
using Cysharp.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();
        DisplayActionResponse(resultAsOk);

        //UserUtil.RequestData<DataTypes.NftCollection>(new NftCollectionToFetch(Env.Nfts.BOOM_COLLECTION_CANISTER_ID, "Test Nft Collection", false));
        NftUtil.TryAddMintedNft(resultAsOk.F2.ToArray());
        UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());

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

        var actionConfigResult = UserUtil.GetElementOfType<DataTypes.ActionConfig>(mintIcrcActionId);

        if (actionConfigResult.IsErr)
        {
            Debug.LogError(actionConfigResult.AsErr());
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Upss!", actionResult.AsErr().content), 3);

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var actionConfig = actionConfigResult.AsOk();

        string canisterId = "";
        actionConfig.ActionResult.Outcomes.Once(e => e.PossibleOutcomes.Once(k =>
        {
            if (k.Option.Tag == ActionOutcomeOption.OptionInfoTag.MintToken)
            {
                canisterId = k.Option.AsMintToken().Canister;
            }
        }));

        UserUtil.RequestData<DataTypes.Token>(canisterId);
        UserUtil.UpdateData<DataTypes.Action>(resultAsOk.F0.ConvertToDataType());

        BroadcastState.Invoke(new WaitingForResponse(false));

        Debug.Log($"Mint ICRC Success, Wait for approval, reward {resultAsOk.F3.Count}:\n\n " + resultAsOk.F3.Reduce(e => $"Token canister: {e.Canister}\nQuantity: {e.Quantity}\n\n"));
    }

    private void DisplayActionResponse(ActionResponse resonse)
    {
        List<string> inventoryElements = new();

        //NFTs
        Dictionary<string, int> collectionsToDisplay = new();
        resonse.F2.Iterate(e =>
        {

            if (collectionsToDisplay.TryAdd(e.Canister, 1) == false) collectionsToDisplay[e.Canister] += 1;

        });

        collectionsToDisplay.Iterate(e =>
        {
            string tokenName = "Some Name";
            var fetchOwnTokenDataResult = UserUtil.GetElementOfType<DataTypes.NftCollection>(e.Key);

            if (fetchOwnTokenDataResult.IsOk)
            {
                tokenName = fetchOwnTokenDataResult.AsOk().collectionName;
            }
            else
            {
                tokenName = "Name not Found";
            }

            inventoryElements.Add($"{tokenName} x {e.Value}");
        });

        //Tokens
        resonse.F3.Iterate(e =>
        {
            string tokenName = "Some Name";
            var fetchOwnTokenDataResult = UserUtil.GetElementOfType<DataTypes.TokenConfig>(e.Canister);

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
