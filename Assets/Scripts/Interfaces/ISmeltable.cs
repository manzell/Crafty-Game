using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISmeltable
{
    public MetalMixture MetalConcentrations { get; }
    public float Weight { get; set; }
}