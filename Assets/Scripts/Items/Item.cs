using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name => Data?.name;
    [field: SerializeField] public List<PlayerAction> Actions { get; private set; }
    [field: SerializeField] public ItemData Data { get; protected set; }
    [field: SerializeField] public float Weight { get; set; } /* kilograms */


    public Item(ItemData data)
    {
        Data = data; 
        Actions = data.Actions; 
        Weight = data.Weight;
    }

    public void SetWeight(float weight) => this.Weight = weight;

    public void Break()
    {
        Debug.Log($"{name} broke! ALSO Fix Player Acquisition Here. Also we're not removing this from the inputs of the current action");
        GameObject.FindObjectOfType<Player>().RemoveItem(this);
    }
}
