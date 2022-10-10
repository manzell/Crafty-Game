using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; 

public class ZoneData : SerializedScriptableObject
{
    [SerializeField] NodeData node;
    [field: SerializeField] public List<PlayerAction> gatherActions; 
}
