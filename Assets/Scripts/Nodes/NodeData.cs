using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData : ScriptableObject
{
    public new string name;
    [field: SerializeField] public List<ZoneData> zones { get; private set; }
    public List<ZoneData> Zones => zones;
}
