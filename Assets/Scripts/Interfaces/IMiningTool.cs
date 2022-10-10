using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMiningTool : IDurableItem
{
    float DamageChanceReduction { get; }
    float MiningEfficiency { get; }
}
