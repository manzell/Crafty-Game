using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PickAxe : Item, IDurableItem, IMiningTool
{
    [SerializeField] float durability, maxDurability;
    public float Durability => durability;
    public float MaxDurability => maxDurability; 

    public void AdjustDurability(float f)
    {
        if (f < 0)
            Debug.Log($"{name} lost durability! ({Mathf.RoundToInt(durability)}/({Mathf.RoundToInt(maxDurability)})"); 

        durability += f;
    }

    public float DamageChanceReduction => 0f;
}
