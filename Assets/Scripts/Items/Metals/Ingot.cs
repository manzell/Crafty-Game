using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ingot : Item, ISmeltable
{
    public MetalData metalType;
    public MetalMixture MetalConcentrations { get; private set; } = new();

    public void Setup(MetalMixture mix)
    {
        MetalConcentrations = mix;
        Weight = MetalConcentrations.Sum(kvp => kvp.Value);
        metalType = GameObject.FindObjectOfType<Metals>().GetAlloy(MetalConcentrations);
        name = metalType + " Ingot"; 
    }

    public Ingot(IngotData data)
    {
        name = $"{data.name} Ingot"; 
        Weight = data.Weight;
    }
}