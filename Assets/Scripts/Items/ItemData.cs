using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;

public abstract class ItemData : SerializedScriptableObject
{
    public new string name;
    [field: SerializeField] public List<PlayerAction> Actions { get; private set; }
    [field: SerializeField] public Sprite itemSprite { get; private set; }
    [field: SerializeField] public Color itemTint { get; private set; }
    [field: SerializeField] public float Weight { get; set; } /* kilograms */

    public virtual Item Clone() => new Item(this);
}
