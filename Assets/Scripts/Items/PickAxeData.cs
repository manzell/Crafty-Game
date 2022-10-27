using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeData : ItemData
{
    public float maxDurability;
    public float DamageChanceReduction;
    public float MiningEfficiency;

    public override Item Clone() => new PickAxe(this);
}
