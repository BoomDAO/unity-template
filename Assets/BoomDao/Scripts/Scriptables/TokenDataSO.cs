using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TokenDataSO", menuName = "TokenDataSO")]
public class TokenDataSO : ScriptableObject
{
    [field: SerializeField] public string CanisterId { get; private set; }
}
