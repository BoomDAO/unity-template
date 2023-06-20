using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
[Serializable]
public class Wrapper<T>
{
    [Preserve] public T value;
}

[Preserve]
[Serializable]
public class Gacha
{
    [Preserve] public string Id;
    public List<GachaRoll> rolls;
}

[Preserve] [Serializable]
public class GachaRoll
{
    [Preserve] public List<GachaItem> variables;
}

[Preserve] [Serializable]
public class GachaItem
{
    [Preserve] public string itemId;
    [Preserve] public double quantity;
    [Preserve] public float weight;
}

[Preserve]
[Serializable]
public class InGameNftData
{
    public string collection;
    public string itemId;
    public List<long> indexes;

    public InGameNftData(string collection, string itemId, List<long> indexes)
    {
        this.collection = collection;
        this.itemId = itemId;
        this.indexes = indexes;
    }

    public int Count { get => indexes.Count; }
}