using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu]
public class DropTable : SerializedScriptableObject
{
    [field: SerializeField] public Dictionary<PlayerAction, List<DropTableEntry>> Table { get; private set; } = new(); 
    [SerializeField] bool alwaysReturnItem;
    
    public Item GetDrop(PlayerAction action)
    {
        if (!Table.ContainsKey(action)) 
            return null;

        float d = Random.Range(0, alwaysReturnItem ? 1f : Table[action].Sum(listEntry => listEntry.odds));

        foreach(DropTableEntry entry in Table[action])
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
