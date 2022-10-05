using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector; 

public class ItemData : SerializedScriptableObject
{
    public new string name;
    [field: SerializeField] public List<PlayerAction> Actions { get; private set; }
    [field: SerializeField] public Sprite itemSprite { get; private set; }
    [field: SerializeField] public float weight { get; } /* kilograms */ 
}
