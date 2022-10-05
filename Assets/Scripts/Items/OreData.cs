using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreData : ItemData, IMineable, ISmeltable
{
    public enum MetalType { Iron, Gold, Platinum, Silver }

    [SerializeField] MetalType metalType;
    [field: SerializeField] public Dictionary<MetalType, float> MetalConcentrations { get; private set; }

}

public interface ISmeltable
{
    public Dictionary<OreData.MetalType, float> MetalConcentrations { get; } 
}
