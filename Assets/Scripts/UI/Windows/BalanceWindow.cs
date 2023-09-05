using Boom.UI;
using Boom.Utility;
using Boom.Values;
using TMPro;
using UnityEngine;

public class BalanceWindow : Window
{
    [SerializeField] TextMeshProUGUI icpBalanceText;
    [SerializeField] TextMeshProUGUI icrcBalances;
    [SerializeField] TextMeshProUGUI nftCountTxt;

    public class WindowData
    {
        //DATA
    }
    public override bool RequireUnlockCursor()
    {
        return false;//or true
    }

    public override void Setup(object data)
    {
        icpBalanceText.text = $"ICP:  Loading...";
        icrcBalances.text = $"ICRC: Loading...";
        nftCountTxt.text = $"NFT Count: {0}";

        UserUtil.RegisterToDataChange<DataTypes.Token>(UpdateWindow);
        UserUtil.RegisterToDataChange<DataTypes.NftCollection>(UpdateWindow);
    }
    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.NftCollection>(UpdateWindow);
        UserUtil.UnregisterToDataChange<DataTypes.Token>(UpdateWindow);
    }
    private void UpdateWindow(DataState<Data<DataTypes.NftCollection>> obj)
    {
        if (!UserUtil.IsUserLoggedIn())
        {
            nftCountTxt.text = $"NFT Count: 0";
            return;
        }


        var nftCountResult = NftUtil.GetNftCount(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

        if(nftCountResult.Tag == Boom.Values.UResultTag.Err)
        {
            Debug.LogWarning(nftCountResult.AsErr());
            nftCountTxt.text = $"NFT Count: Loading...";
            return;
        }

        var nftCount = nftCountResult.AsOk();

        nftCountTxt.text = $"NFT Count: {nftCount}";
    }

    private void UpdateWindow(DataState<Data<DataTypes.Token>> obj)
    {
        if (!UserUtil.IsUserLoggedIn())
        {
            icpBalanceText.text = $"ICP: {0}";
            icrcBalances.text = $"ICRC: {0}";

            return;
        }

        if (obj.IsLoading())
        {
            icpBalanceText.text = $"ICP: Loading...";
            icrcBalances.text = $"ICRC: Loading...";
            return;
        }

        //

        var icpTokenAndConfigsResult = UserUtil.GetTokenAndConfigs(Env.CanisterIds.ICP_LEDGER);

        if (icpTokenAndConfigsResult.Tag == UResultTag.Err)
        {
            $"{icpTokenAndConfigsResult.AsErr()}".Warning();
            icpBalanceText.text = $"ICP: {0}";
        }
        else
        {
            var (token, configs) = icpTokenAndConfigsResult.AsOk();

            icpBalanceText.text = $"ICP: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}";
        }

        //

        var icrcTokenAndConfigsResult = UserUtil.GetTokenAndConfigs(Env.CanisterIds.ICRC_LEDGER);

        if (icrcTokenAndConfigsResult.Tag == UResultTag.Err)
        {
            $"{icrcTokenAndConfigsResult.AsErr()}".Warning();
            icrcBalances.text = $"ICRC: {0}";
        }
        else
        {
            var (token, configs) = icrcTokenAndConfigsResult.AsOk();

            icrcBalances.text = $"{configs.name}: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}";
        }
    }
}