using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public new string name => Data?.name;
    [field: SerializeField] public List<PlayerAction> Actions { get; private set; }
    [field: SerializeField] public ItemData Data { get; protected set; }
    [field: SerializeField] public float weight { get; private set; } /* kilograms */

    public void SetWeight(float weight) => this.weight = weight;
    public virtual void Setup(ItemData data)
    {
        Data = data; 
        Actions = data.Actions; 
    }
}
