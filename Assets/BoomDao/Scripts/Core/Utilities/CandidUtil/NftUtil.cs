using Boom.Utility;
using Boom.Values;
using Candid;
using Candid.World.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class NftUtil
{
    /// <summary>
    /// Get a collection by a given collection canister id
    /// </summary>
    /// <param name="collectionId"></param>
    /// <returns></returns>
    public static UResult<DataTypes.NftCollection, string> TryGetCollection(string collectionId)
    {
        var result = UserUtil.GetElementOfType<DataTypes.NftCollection>(collectionId);

        if (result.Tag == UResultTag.Err) return new(result.AsErr());


        return new(result.AsOk());
    }

    /// <summary>
    /// Get your NFT count from a collection by a given collection canister id, you can use the predicate as a filter
    /// </summary>
    /// <param name="collectionId"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static UResult<int, string> GetNftCount(string collectionId, Predicate<string> predicate = null)
    {
        var result = TryGetCollection(collectionId);

        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        var tokens = result.AsOk().tokens;
        tokens ??= new();

        if (predicate == null)
        {
            return new(tokens.Count);
        }
        else
        {
            return new(tokens.Count(e =>
            {
                return predicate(e.metadata);
            }));
        }
    }
    /// <summary>
    /// Get your next NFT from a collection by a given collection canister id, you can use the predicate as a filter
    /// </summary>
    /// <param name="collectionId"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static UResult<DataTypes.NftCollection.Nft, string> TryGetNextNft(string collectionId, Predicate<string> predicate = null)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        var tokens = result.AsOk().tokens;
        tokens ??= new();


        if (tokens.Count == 0) return new($"You have no nft on the collection of id: {collectionId}");

        if (predicate == null) return new(tokens[0]);


        if (tokens.TryLocate(e => predicate(e.metadata), out var outValue1) == false)
        {
            return new($"You have no nft on the collection of id: {collectionId}, that matches the predicate");
        }

        return new(outValue1);
    }
    /// <summary>
    /// Get your next NFT you own's Index from a collection by a given collection canister id, you can use the predicate as a filter
    /// </summary>
    /// <param name="collectionId"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static UResult<uint, string> TryGetNextNftIndex(string collectionId, Predicate<string> predicate = null)
    {
        var result = TryGetNextNft(collectionId, predicate);
        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        return new(result.AsOk().index);
    }

    /// <summary>
    /// Get a NFT you own from a collection by a given collection canister id, you can use the predicate as a filter
    /// </summary>
    /// <param name="collectionId"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static UResult<DataTypes.NftCollection.Nft, string> TryGetNft(string collectionId, long index)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        var tokens = result.AsOk().tokens;
        tokens ??= new();

        if (tokens.Count == 0) return new($"You have no nft on the collection of id: {collectionId}");

        if (tokens.TryLocate(e =>
        {
            return e.index == index;
        }, out var dabNftDetails) == false)
        {
            return new($"You have no nft on the collection of id: {collectionId} of index: {index}"); ;
        }

        return new(dabNftDetails);
    }
    public static bool HasNft(string collectionId, long index)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return false;

        var tokens = result.AsOk().tokens;
        tokens ??= new();

        if (tokens.Count == 0) return false;

        return tokens.Has(e =>
        {
            return e.index == index;
        });
    }
    public static bool HasNft<T>(string collectionId, Func<DataTypes.NftCollection.Nft, T, bool>  predicate, params T[] requirements)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return false;

        var tokens = result.AsOk().tokens;
        tokens ??= new();

        var tokensCopy = tokens.CreateDeepCopy();

        if (tokensCopy.Count == 0) return false;

        foreach ( var requirement in requirements) 
        {
            var matchingNftIndex = tokensCopy.Once(e => tokensCopy.Remove(e), e => predicate(e, requirement));

            if (matchingNftIndex == -1) return false;
        }

        return true;
    }
    public static UResult<List<DataTypes.NftCollection.Nft>, string> Filter<T>(string collectionId, Func<DataTypes.NftCollection.Nft, T, bool> predicate, params T[] requirements)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return new($"You were not able to find any local data on collection of id: {collectionId}");

        var tokens = result.AsOk().tokens;
        tokens ??= new();

        var tokensCopy = tokens.CreateDeepCopy();

        if (tokensCopy.Count == 0) return new($"You have no nft on collection of id: {collectionId}");

        List<DataTypes.NftCollection.Nft> nftsToReturn = new();
        foreach (var requirement in requirements)
        {
            var matchingNftIndex = tokensCopy.Once(e =>
            {
                nftsToReturn.Add(e);
                tokensCopy.Remove(e);
            }, e => predicate(e, requirement));

            if (matchingNftIndex == -1) return new($"You are missing an nft of requirement: {requirement}");
        }

        return new(nftsToReturn);
    }

    /// <summary>
    /// Remove a NFT you own from local storage by collection canister id and by a NFT Index
    /// </summary>
    /// <param name="collectionId"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static UResult<Null, string> TryRemoveNftByIndex(string collectionId, long index)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        var collection = result.AsOk();
        var tokens = collection.tokens;
        tokens ??= new();

        if (tokens.Count == 0) return new($"You have no nft on the collection of id: {collectionId}");

        if (tokens.TryLocate(e => e.index == index, out var token))
        {
            bool success = tokens.Remove(token);
            if (success)
            {
                UserUtil.UpdateData<DataTypes.NftCollection>(null);
            }
            return new(new Null());
        }

        return new($"You have no nft on the collection of id: {collectionId} of index: {index}"); ;
    }

    public static UResult<Null, string> TryRemoveNextNft(string collectionId)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        var collection = result.AsOk();
        var tokens = collection.tokens;
        tokens ??= new();

        if (tokens.Count == 0) return new($"You have no nft on the collection of id: {collectionId}");

        tokens.RemoveAt(0);
        UserUtil.UpdateData<DataTypes.NftCollection>(null);
        return new(new Null());
    }

    public async static void TryAddMintedNft(params MintNft[] mintedNfts)
    {
        bool update = false;
        foreach (var mintedNft in mintedNfts)
        {
            if (mintedNft.Index.HasValue == false)
            {
                $"Could not add some nft from collection: {mintedNft.Canister} cuz doesn't haven an index specified".Warning();
                continue;
            }

            var nftIndex = mintedNft.Index.ValueOrDefault;

            var result = TryGetCollection(mintedNft.Canister);

            if (result.IsErr)
            {
                $"Could not add of index: {nftIndex} from collection: {mintedNft.Canister} cuz you don't have the collection initialized".Warning();
                continue;
            }

            var tokenIdentifier = await CandidApiManager.Instance.WorldHub.GetTokenIdentifier(mintedNft.Canister, nftIndex);

            var collection = result.AsOk();

            collection.tokens ??= new();

            if(collection.tokens.Has(e => e.index == nftIndex) == false)
            {
                collection.tokens.Add(
                new DataTypes.NftCollection.Nft(
                    mintedNft.Canister,
                    nftIndex,
                    tokenIdentifier,
                    $"https://{collection.canister}.raw.icp0.io/?&tokenid={tokenIdentifier}&type=thumbnail",
                    mintedNft.Metadata));

                update = true;
            }
        }

        if(update) UserUtil.UpdateData<DataTypes.NftCollection>(null);
    }

    public async static void TryAddMintedNft(params DataTypes.NftCollection.Nft[] mintedNfts)
    {
        bool update = false;
        foreach (var mintedNft in mintedNfts)
        {
            var nftIndex = mintedNft.index;

            var result = TryGetCollection(mintedNft.canister);

            if (result.IsErr)
            {
                $"Could not add of index: {nftIndex} from collection: {mintedNft.canister} cuz you don't have the collection initialized".Warning();
                continue;
            }

            var tokenIdentifier = await CandidApiManager.Instance.WorldHub.GetTokenIdentifier(mintedNft.canister, nftIndex);

            var collection = result.AsOk();

            collection.tokens ??= new();

            if (collection.tokens.Has(e => e.index == nftIndex) == false)
            {
                collection.tokens.Add(mintedNft);

                update = true;
            }
        }

        if (update) UserUtil.UpdateData<DataTypes.NftCollection>(null);
    }
}
