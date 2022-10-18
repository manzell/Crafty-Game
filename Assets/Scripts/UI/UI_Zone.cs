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
    Zone zone;
    UI_Action uiDefaultAction;

    [SerializeField] TextMeshProUGUI zoneName;
    [SerializeField] GameObject actionPrefab;

    private void Awake()
    {;
        zone = GetComponent<Zone>();
        uiDefaultAction = GetComponent<UI_Action>();
    }

    public void Setup(ZoneData zoneData)
    {
        zone.Setup(zoneData);
        zoneName.text = zoneData.name;

        uiDefaultAction.Setup(zone.DropTable.Keys?.Concat(zone.Actions), zone);

        foreach (UI_Action uiAction in GetComponentsInChildren<UI_Action>().Where(uiAction => uiAction != uiDefaultAction))
            Destroy(uiAction.gameObject); 

        foreach(PlayerAction action in zoneData.actions)
            Instantiate(actionPrefab, transform).GetComponent<UI_Action>().Setup(action, zone);
    }

}
