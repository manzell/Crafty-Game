using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingot : Item, ISmeltable
{
    public Metal metalType;
    public Dictionary<Metal, float> MetalConcentrations { get; private set; } = new(); 
}
