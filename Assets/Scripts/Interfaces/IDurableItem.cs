using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDurableItem
{
    [SerializeField] public float Durability { get; }
    [SerializeField] public float MaxDurability { get; }
    public void AdjustDurability(float f);
}
