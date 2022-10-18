using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Item, IMineable, ISmeltable
{
    [field: SerializeField] public Dictionary<Metal, float> MetalConcentrations { get; private set; } = new(); 

    public Ore(ItemData data)
    {
        Data = data;

        foreach (KeyValuePair<Metal, Distribution> kvp in (data as OreData).MetalConcentrations)
            MetalConcentrations.Add(kvp.Key, kvp.Value.Generate());

        Setup(data); 
    }
}
