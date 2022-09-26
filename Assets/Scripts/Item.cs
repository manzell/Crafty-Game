using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public new string name;
    [SerializeField] List<PlayerAction> actions;

    [SerializeField] public float weight { get; private set; } /* kilograms */ 
    [SerializeField] public float condition { get; private set; }

    public void SetWeight(float weight) => this.weight = weight;
}
