using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Boom;
using UnityEngine;
using UnityEngine.UI;
using Candid;

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
        var actionResult = await ActionUtil.ProcessAction(mintNftActionId);

        if (actionResult.Tag == UResultTag.Err)
        {
            Debug.LogError(actionResult.AsErr().Content);
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Upss!", actionResult.AsErr().content), 3);
            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        var resultAsOk = actionResult.AsOk();
        DisplayActionResponse(resultAsOk);

        BroadcastState.Invoke(new WaitingForResponse(false));

        Debug.Log($"Mint Nft Success");
    }

    private async UniTaskVoid MintIcrc()
    {
        //await UniTask.SwitchToMainThread();

        BroadcastState.Invoke(new WaitingForResponse(true));
        var actionResult = await ActionUtil.ProcessAction(mintIcrcActionId);

        if (actionResult.Tag == UResultTag.Err)
        {
            Debug.LogError(actionResult.AsErr().Content);
            WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData("Upss!", actionResult.AsErr().content), 3);

            BroadcastState.Invoke(new WaitingForResponse(false));
            return;
        }

        DisplayActionResponse(actionResult.AsOk());

        BroadcastState.Invoke(new WaitingForResponse(false));
    }

    private void DisplayActionResponse(ProcessedActionResponse resonse)
    {
        List<string> inventoryElements = new();

        //NFTs
        Dictionary<string, int> collectionsToDisplay = new();

        if (resonse.callerOutcomes == null) return;


        resonse.callerOutcomes.nfts.Iterate(e =>
        {

            if (collectionsToDisplay.TryAdd(e.Canister, 1) == false) collectionsToDisplay[e.Canister] += 1;

        });

        collectionsToDisplay.Iterate(e =>
        {
            if (ConfigUtil.TryGetNftCollectionConfig(e.Key, out var collectionConfig) == false)
            {
                return;
            }

            inventoryElements.Add($"{(collectionConfig != null ? collectionConfig.name : "Name not Found")} x {e.Value}");
        });

        //Tokens
        resonse.callerOutcomes.tokens.Iterate(e =>
        {
            if (ConfigUtil.TryGetTokenConfig(e.Canister, out var tokenConfig) == false)
            {
                return;
            }

            inventoryElements.Add($"{(tokenConfig != null ? tokenConfig.name : "ICRC")} x {e.Quantity}");
        });


        //ENTITIES
        resonse.callerOutcomes.entityEdits.Iterate(e =>
        {
            if (e.Value.fields.Has(k => k.Value is EntityFieldEdit.IncrementNumber) == false) return;

            if (!ConfigUtil.GetConfigFieldAs<string>(CandidApiManager.Instance.WORLD_CANISTER_ID, e.Value.eid, "name", out var configName)) return;
            if (!e.Value.GetEditedFieldAsNumeber("quantity", out double quantity)) return;

            if (e.Value.TryGetConfig(CandidApiManager.Instance.WORLD_CANISTER_ID, out var config)) inventoryElements.Add($"{configName} x {quantity}");
            else inventoryElements.Add($"{e.Value.GetKey()} x {quantity}");
        });

        WindowManager.Instance.OpenWindow<InventoryPopupWindow>(new InventoryPopupWindow.WindowData("Earned Items", inventoryElements), 3);
    }

}
