using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ore : Item, IMineable
{
    enum MetalType { Iron, Gold, Platinum, SIlver }

    [SerializeField] new float weight;
    [SerializeField] MetalType metalType;
    [SerializeField] float purity;

    public float OrePurity => purity; 
}
