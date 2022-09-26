using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; 

[CreateAssetMenu]
public class Zone : SerializedScriptableObject
{
    [SerializeField] Node node;
    [SerializeField] Dictionary<Item, int> availableItems;
    public Dictionary<Item, int> AvailableItems => availableItems; 
}
