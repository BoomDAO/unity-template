using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectionDataSO", menuName = "Scriptable Objects/CollectionDataSO")]
public class CollectionDataSO : ScriptableObject
{
    [field: SerializeField] public string CollectionName { get; private set; }
    [field: SerializeField] public string CanisterId { get; private set; }
}
