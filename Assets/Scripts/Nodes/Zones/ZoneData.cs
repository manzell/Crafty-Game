using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; 

public class ZoneData : SerializedScriptableObject
{
    public new string name;
    public DropTable dropTable;
    public List<PlayerAction> actions; 

}
