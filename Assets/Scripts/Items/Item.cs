using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string name => Data?.name;
    [field: SerializeField] public List<PlayerAction> Actions { get; private set; }
    [field: SerializeField] public ItemData Data { get; protected set; }
    [field: SerializeField] public float Weight { get; set; } /* kilograms */

    public void SetWeight(float weight) => this.Weight = weight;
    public virtual void Setup(ItemData data)
    {
        Data = data; 
        Actions = data.Actions; 
        Weight = data.Weight;
    }

    public void Break()
    {
        Debug.Log($"{name} broke! ALSO Fix Player Acquisition Here");
        GameObject.FindObjectOfType<Player>().RemoveItem(this);
    }
}
