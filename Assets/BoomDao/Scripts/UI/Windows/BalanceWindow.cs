using Boom.UI;
using Boom.Utility;
using Boom.Values;
using TMPro;
using UnityEngine;
using Boom;
public class BalanceWindow : Window
{
    [SerializeField] TextMeshProUGUI icpBalanceText;

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
        icpBalanceText.text = $"ICP: Loading...\nICRC: Loading...\nNFT Count: Loading...";

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
        if (UserUtil.IsDataValid<DataTypes.Token>())
        {
           var tokensResult =  UserUtil.GetDataOfType<DataTypes.Token>();

            if (tokensResult.IsOk)
            {
                UpdateWindow(tokensResult.AsOk());
            }
        }
    }

    private void UpdateWindow(DataState<Data<DataTypes.Token>> obj)
    {
        if (!UserUtil.IsUserLoggedIn())
        {
            icpBalanceText.text = $"ICP: {0}\nICRC: {0}\nNFT Count: {0}";

            return;
        }

        if (obj.IsLoading() && UserUtil.IsDataValid<DataTypes.NftCollection>() == false)
        {
            icpBalanceText.text = $"ICP: Loading...\nICRC: Loading...\nNFT Count: Loading...";
            return;
        }
        else
        {
            icpBalanceText.text = "";
        }

        //
 
        if (EntityUtil.QueryConfigsByTag("token", out var tokenConfigs))
        {
            tokenConfigs.Iterate(e =>
            {
                if (e.GetConfigFieldAs<string>("canister", out var canisterId))
                {
                    var icpTokenAndConfigsResult = TokenUtil.GetTokenDetails(canisterId);

                    if(Env.CanisterIds.ICP_LEDGER == canisterId)
                    {
                        if (icpTokenAndConfigsResult.Tag == UResultTag.Err)
                        {
                            $"{icpTokenAndConfigsResult.AsErr()}".Warning();
                            icpBalanceText.text += $"ICP: {0}\n";
                        }
                        else
                        {
                            var (token, configs) = icpTokenAndConfigsResult.AsOk();

                            icpBalanceText.text += $"ICP: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}\n";
                        }
                    }
                    else
                    {
                        if (icpTokenAndConfigsResult.Tag == UResultTag.Err)
                        {
                            $"{icpTokenAndConfigsResult.AsErr()}".Warning();
                            icpBalanceText.text += $"ICRC: {0}\n";
                        }
                        else
                        {
                            var (token, configs) = icpTokenAndConfigsResult.AsOk();

                            icpBalanceText.text += $"{configs.name}: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}\n";
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"config of tag \"token\" doesn't have field \"canister\"");
                } 
            });

            var nftCountResult = NftUtil.GetNftCount(Env.Nfts.BOOM_COLLECTION_CANISTER_ID);

            if (nftCountResult.Tag == Boom.Values.UResultTag.Err)
            {
                Debug.LogWarning(nftCountResult.AsErr());

                icpBalanceText.text += $"NFT Count: Loading...";

                return;
            }

            var nftCount = nftCountResult.AsOk();

            icpBalanceText.text += $"NFT Count: {nftCount}";
        }
    }
}