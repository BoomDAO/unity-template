using Boom.UI;
using Boom.Utility;
using Boom.Values;
using TMPro;
using UnityEngine;
using Boom;
using Newtonsoft.Json;
using Candid;

public class BalanceWindow : Window
{
    string icpData;
    string icrcData;
    string nftData;

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

        UserUtil.AddListenerDataChangeSelf<DataTypes.Token>(UpdateWindow, true);
        UserUtil.AddListenerDataChangeSelf<DataTypes.NftCollection>(UpdateWindow);
    }
    private void OnDestroy()
    {
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.NftCollection>(UpdateWindow);
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.Token>(UpdateWindow);
    }
    private void UpdateWindow(Data<DataTypes.NftCollection> obj)
    {

        if (UserUtil.IsDataValidSelf<DataTypes.Token>())
        {
           var tokensResult =  UserUtil.GetDataSelf<DataTypes.Token>();

            if (tokensResult.IsOk)
            {
                UpdateWindow(tokensResult.AsOk());
            }
        }
    }

    private void UpdateWindow(Data<DataTypes.Token> obj)
    {
        if (!UserUtil.IsUserLoggedIn(out var loginData))
        {
            icpBalanceText.text = $"ICP: {0}\nICRC: {0}\nNFT Count: {0}";

            return;
        }

        icpData = "ICP: Loading...";
        icrcData = "ICRC: Loading...";
        nftData = "NFT Count: Loading...";

        //
 
        if (ConfigUtil.QueryConfigsByTag(CandidApiManager.Instance.WORLD_CANISTER_ID, "token", out var tokenConfigs))
        {
            tokenConfigs.Iterate(e =>
            {
                if (e.GetConfigFieldAs<string>("canister", out var canisterId))
                {
                    var icpTokenAndConfigsResult = TokenUtil.GetTokenDetails(loginData.principal, canisterId);

                    if(Env.CanisterIds.ICP_LEDGER == canisterId)
                    {
                        if (icpTokenAndConfigsResult.Tag == UResultTag.Err)
                        {
                            $"{icpTokenAndConfigsResult.AsErr()}".Warning();
                            icpData = $"ICP: {0}\n";
                        }
                        else
                        {
                            var (token, configs) = icpTokenAndConfigsResult.AsOk();

                            icpData  = $"ICP: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}\n";
                        }
                    }
                    else
                    {
                        if (icpTokenAndConfigsResult.Tag == UResultTag.Err)
                        {
                            $"{icpTokenAndConfigsResult.AsErr()}, {canisterId}".Warning();
                            icrcData = $"ICRC: {0}\n";
                        }
                        else
                        {
                            var (token, configs) = icpTokenAndConfigsResult.AsOk();

                            icrcData = $"{configs.name}: {token.baseUnitAmount.ConvertToDecimal(configs.decimals).NotScientificNotation()}\n";
                        }
                    }
                }
            });

            var nftCountResult = NftUtil.GetNftCount(loginData.principal, CandidApiManager.Instance.WORLD_COLLECTION_CANISTER_ID);

            if (nftCountResult.Tag == Boom.Values.UResultTag.Ok)
            {
                var nftResult = UserUtil.GetDataSelf<DataTypes.NftCollection>();

                nftData = $"NFT Count: {nftCountResult.AsOk()}";
            }

            icpBalanceText.text = $"{icpData}{icrcData}{nftData}";
        }
    }
}