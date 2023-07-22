using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Env
{
    public static class CanisterIds
    {
        public const string ICP_LEDGER = "ryjl3-tyaaa-aaaaa-aaaba-cai";
        public const string ICRC_LEDGER = "6bszp-caaaa-aaaap-abgbq-cai";//"mxzaz-hqaaa-aaaar-qaada-cai"; //CkBtc

        public const string PAYMENT_HUB = "5hr3g-hqaaa-aaaap-abbxa-cai";
        public const string STAKING_HUB = "jozll-yaaaa-aaaap-abf5q-cai";

        public const string WORLD_HUB = "j362g-ziaaa-aaaap-abf6a-cai";
        public const string WORLD = "6irst-uiaaa-aaaap-abgaa-cai";
    }
    public static class Nfts
    {
        public const string BOOM_COLLECTION_CANISTER_ID = "6uvic-diaaa-aaaap-abgca-cai";
    }

    public static class Stacking
    {
        public const double ICP_STAKE = 0.01;
        public const double ICRC_STAKE = 0.000001;
    }
}
