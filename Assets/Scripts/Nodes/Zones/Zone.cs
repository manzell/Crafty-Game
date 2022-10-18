using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [field: SerializeField] public Dictionary<PlayerAction, Item> DropTable { get; private set; }

    public ZoneData ZoneData { get; private set; }
    public List<Item> Inputs {  get; private set; }   
    public IEnumerable<PlayerAction> Actions { get; private set; }

    public void Setup(ZoneData zone)
    {
        ZoneData = zone;
        name = zone.name;
        gameObject.name = $"Zone: [{name}]"; 
        DropTable = zone.dropTable;
        Actions = zone.actions;
        Inputs = new(); 
    }
}