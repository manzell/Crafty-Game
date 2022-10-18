using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISmeltable
{
    public Dictionary<Metal, float> MetalConcentrations { get; }
    public float Weight { get; set; }
}