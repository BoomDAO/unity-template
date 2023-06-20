using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DabNftUtil
{
    public static string MainGameCollectionID
    {
        get
        {
            return Env.Nfts.BOOM_COLLECTION;
        }
    }
    public static bool TryGetNft(string collectionId, out IEnumerable<DabNftDetails> outValue)
    {
        BroadcastState.TryRead<DataState<DabNftsData>>(out var dabNftsState);

        var plethoraNfts = dabNftsState.data.plethoraNftCollections;

        plethoraNfts ??= new();

        if (plethoraNfts.TryLocate(e => e.canisterId == collectionId, out var returnValue))
        {
            outValue = returnValue.tokens;
            return true;
        }
        else
        {
            var nonPlethoraNfts = dabNftsState.data.nonPlethoraNftCollections;

            nonPlethoraNfts ??= new();

            if (nonPlethoraNfts.TryLocate(e => e.canisterId == collectionId, out returnValue))
            {
                outValue = returnValue.tokens;
                return true;
            }
        }

        outValue = default;
        return false;
    }

    public static bool TryGetCollection(string collectionId, out DabNftCollection collection)
    {
        BroadcastState.TryRead<DataState<DabNftsData>>(out var dabNftsState);

        var plethoraNfts = dabNftsState.data.plethoraNftCollections;

        plethoraNfts ??= new();

        collection = default;

        if (plethoraNfts.TryLocate(e => e.canisterId == collectionId, out var returnValue))
        {
            collection = returnValue;
            return true;
        }
        return false;
    }
    public static int GetNftCount(string collectionId, string usage = "")
    {
        if (TryGetCollection(collectionId, out var returnValue))
        {
            if(string.IsNullOrEmpty(usage))
            {
                return returnValue.tokens.Count;
            }
            else
            {
                return returnValue.tokens.Count(e =>
                {
                    if (e.metadata.AccessJsonValue("usage").As<string>(out var outValue)) return outValue == usage;
                    return false;
                });
            }
        }
        return 0;
    }

    public static bool TryGetNextNftIndex(string collectionId, out long index, string usage = "")
    {
        index = -1;

        if (TryGetCollection(collectionId, out var outValue0) == false)
        {
            return false;
        }
        if (outValue0.tokens == null || outValue0.tokens.Count == 0) return false;

        if (string.IsNullOrEmpty(usage))
        {
            index = outValue0.tokens[0].index;
            return true;
        }


        if (outValue0.tokens.TryLocate(e =>
        {
            if (e.metadata.AccessJsonValue("usage").As<string>(out var outValue)) return outValue == usage;

            return false;
        }, out var outValue1) == false)
        {
            return false;
        }

        index = outValue1.index;
        return true;
    }

    public static bool TryGetNft(string collectionId, long index, out DabNftDetails dabNftDetails)
    {
        dabNftDetails = default;

        if (TryGetCollection(collectionId, out var outValue) == false)
        {
            return false;
        }

        if (outValue.tokens == null || outValue.tokens.Count == 0) return false;

        if (outValue.tokens.TryLocate(e =>
        {
            return e.index == index;
        }, out dabNftDetails) == false)
        {
            return false;
        }

        return true;
    }

    public static bool TryRemoveNftByIndex(string collectionId, long index)
    {
        if (TryGetCollection(collectionId, out var collection))
        {
            if (collection.tokens == null || collection.tokens.Count == 0) return false;

            if (collection.tokens.TryLocate(e => e.index == index, out var token))
            {
                bool success = collection.tokens.Remove(token);
                if (success)
                {
                    BroadcastState.ForceInvoke<DataState<DabNftsData>>(e => e);
                }
                return success;
            }
        }

        return false;
    }
    public static bool AddNft(string collectionId, long index)
    {
        BroadcastState.ForceInvoke<DataState<DabNftsData>>(e =>
        {
            e.data.plethoraNftCollections ??= new();
            if(e.data.plethoraNftCollections.TryLocate(collection => collection.canisterId == collectionId, out var collection))
            {

            }
            else
            {
                e.data.plethoraNftCollections.Add(new DabNftCollection()
                {
                    tokens = new List<DabNftDetails>()
                    {

                    }
                });
            }
            return e;
        });


        return false;
    }
}
