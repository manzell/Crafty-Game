using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Item, IMineable, ISmeltable
{
    [SerializeField] OreData.MetalType metalType;
    [field: SerializeField] public Dictionary<OreData.MetalType, float> MetalConcentrations { get; private set; } = new(); 

    public override void Setup(ItemData data)
    {
        Data = data; 
        OreData oreData = data as OreData;

        foreach(KeyValuePair<OreData.MetalType, float> kvp in oreData.MetalConcentrations)
            MetalConcentrations.Add(kvp.Key, Random.Range(0, kvp.Value));
    }
}
