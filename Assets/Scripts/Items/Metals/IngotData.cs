using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IngotData : ItemData
{
    public override Item Clone() => new Ingot(this);
}
