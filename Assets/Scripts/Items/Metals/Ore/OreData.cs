using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class OreData : ItemData, IMineable
{
    [field: SerializeField] public Dictionary<MetalData, Distribution> MetalConcentrations { get; private set; }

    public override Item Clone() => new Ore(this); 
}
