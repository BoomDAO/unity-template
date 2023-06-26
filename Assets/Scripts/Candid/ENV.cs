using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Env
{
    public static class CanisterIds
    {
        public const string ICP_LEDGER = "ryjl3-tyaaa-aaaaa-aaaba-cai";
        public const string ICRC_LEDGER = "mxzaz-hqaaa-aaaar-qaada-cai"; //CkBtc

        public const string PAYMENT_HUB = "5hr3g-hqaaa-aaaap-abbxa-cai";
        public const string STAKING_HUB = "4p3dm-lyaaa-aaaal-qb4da-cai";

        public const string WORLD_HUB = "c4mme-3qaaa-aaaag-abiia-cai";
        public const string WORLD = "747qi-paaaa-aaaal-qb3wa-cai";
    }
    public static class Nfts
    {
        public const string BOOM_COLLECTION_CANISTER_ID = "b5kkq-6iaaa-aaaal-qb6ga-cai";
        public const string NFT_OF_USAGE_TO_BURN = "pastry-variable-offer";
    }

    public static class Stacking
    {
        public const double ICP_STAKE = 0.01;
        public const double ICRC_STAKE = 0.000001;
    }
}
