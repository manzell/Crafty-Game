using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Item, IMineable, ISmeltable
{
    [field: SerializeField] public MetalMixture MetalConcentrations { get; private set; } = new(); 

    public Ore(ItemData data): base(data)
    {
        foreach (KeyValuePair<MetalData, Distribution> kvp in (data as OreData).MetalConcentrations)
            MetalConcentrations.Add(kvp.Key, kvp.Value.Generate());
    }
}
