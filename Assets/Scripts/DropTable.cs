using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System.Linq;

public class DropTable : SerializedScriptableObject
{
    public Dictionary<PlayerAction, List<DropTableEntry>> table;
    [SerializeField] bool alwaysReturnItem; 

    public Item GetDrop(PlayerAction action)
    {
        if (!table.ContainsKey(action)) 
            return null;

        // This assumes that the drop odds are out of 1. 
        float d = Random.Range(0, alwaysReturnItem ? 1f : table[action].Sum(listEntry => listEntry.odds));

        foreach(DropTableEntry entry in table[action])
        {
            d -= entry.odds; 

            if(d <= 0)
                return entry.data.Clone();
        }

        return null; 
    }
}

public struct DropTableEntry
{
    public ItemData data;
    public int quantity;
    public float odds; 
}
