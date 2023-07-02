using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using System;

public static class NftUtil
{
    public static UResult<DataTypes.BoomDaoNftCollection, string> TryGetCollection(string collectionId)
    {
        var result = UserUtil.GetDataElementOfType<DataTypes.BoomDaoNftCollection>(collectionId);

        if (result.Tag == UResultTag.Err) return new(result.AsErr());


        return new(result.AsOk());
    }

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
    public static UResult<DataTypes.BoomDaoNftCollection.DabNftDetails, string> TryGetNextNft(string collectionId, Predicate<string> predicate = null)
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
    public static UResult<long, string> TryGetNextNftIndex(string collectionId, Predicate<string> predicate = null)
    {
        var result = TryGetNextNft(collectionId, predicate);
        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        return new(result.AsOk().index);
    }
    public static UResult<DataTypes.BoomDaoNftCollection.DabNftDetails, string> TryGetNft(string collectionId, long index)
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
    public static UResult<Null, string> TryRemoveNftByIndex(string collectionId, long index)
    {
        var result = TryGetCollection(collectionId);
        if (result.Tag == UResultTag.Err) return new(result.AsErr());

        var tokens = result.AsOk().tokens;
        tokens ??= new();

        if (tokens.Count == 0) return new($"You have no nft on the collection of id: {collectionId}");

        if (tokens.TryLocate(e => e.index == index, out var token))
        {
            bool success = tokens.Remove(token);
            if (success)
            {
                UserUtil.UpdateData(new DataTypes.BoomDaoNftCollection[0]);
            }
            return new(new Null());
        }

        return new($"You have no nft on the collection of id: {collectionId} of index: {index}"); ;
    }
}
