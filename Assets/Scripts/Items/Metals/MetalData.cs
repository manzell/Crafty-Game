using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector; 

[CreateAssetMenu]
public class MetalData : SerializedScriptableObject
{
    public new string name;
    public int meltingPoint;
    public MetalMixture mixture; // This is the ideal mixture for the metal. Most will just themselves with a value of 1. 

}

public class MetalMixture : Dictionary<MetalData, float> { }