using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using static Unity.VisualScripting.Member;

public class UI_Zone : MonoBehaviour
{
    [SerializeField] UI_Action uiDefaultAction;
    [SerializeField] Zone zone; 
    [SerializeField] TextMeshProUGUI zoneName;
    [SerializeField] GameObject actionPrefab;

    public void Setup(ZoneData zoneData)
    {
        zone.Setup(zoneData);
        zoneName.text = zoneData.name;

        uiDefaultAction.Setup(zone.DropTable.Table.Keys, zone);

        foreach (UI_Action uiAction in GetComponentsInChildren<UI_Action>().Where(uiAction => uiAction != uiDefaultAction))
            Destroy(uiAction.gameObject); 

        foreach(PlayerAction action in zoneData.actions)
            Instantiate(actionPrefab, transform).GetComponent<UI_Action>().Setup(action, zone);
    }
}
