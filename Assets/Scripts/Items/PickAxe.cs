using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxe : Item, IDurableItem, IMiningTool
{
    [field: SerializeField] public float Durability { get; private set; }
    [field: SerializeField] public float MaxDurability { get; private set; }
    [field: SerializeField] public float DamageChanceReduction { get; private set; }
    [field: SerializeField] public float MiningEfficiency { get; private set; }

    public override void Setup(ItemData data)
    {
        Durability = (data as PickAxeData).maxDurability;
        MaxDurability = (data as PickAxeData).maxDurability;
        DamageChanceReduction = (data as PickAxeData).DamageChanceReduction;
        MiningEfficiency = (data as PickAxeData).MiningEfficiency;

        base.Setup(data);
    }

    public void AdjustDurability(float f)
    {
        Durability += f;

        if (f < 0)
            Debug.Log($"{name} lost durability! ({Mathf.RoundToInt(Durability + f)}/{Mathf.RoundToInt(MaxDurability)})");

        if (Durability <= 0)
            Break(); 
    }
}
