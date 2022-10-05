using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; 

public class ZoneData : SerializedScriptableObject
{
    [SerializeField] NodeData node;
    [field: SerializeField] public Dictionary<ItemData, int> availableItems { get; private set; }
}
