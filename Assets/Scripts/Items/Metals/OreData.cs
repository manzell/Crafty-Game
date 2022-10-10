using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreData : ItemData, IMineable, ISmeltable
{
    [SerializeField] Metal metalType;
    [field: SerializeField] public Dictionary<Metal, float> MetalConcentrations { get; private set; }

}

public interface ISmeltable
{
    public Dictionary<Metal, float> MetalConcentrations { get; }
    public float Weight { get; set; }
}
