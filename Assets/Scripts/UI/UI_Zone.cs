using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UI_Zone : MonoBehaviour
{
    [SerializeField] ZoneData zone; 
    [SerializeField] TextMeshProUGUI zoneName;
    [SerializeField] GameObject playerActionPrefab; 

    public void Setup(ZoneData zone)
    {
        this.zone = zone;
        zoneName.text = zone.name;
        Player player = FindObjectOfType<Player>();

        List<PlayerAction> actions = new List<PlayerAction>();

        foreach (Item item in player.inventory)
            foreach (PlayerAction action in item.Actions)
                actions.Add(action);

        foreach (PlayerAction action in zone.gatherActions)
            actions.Add(action); 

        foreach (PlayerAction action in actions.Distinct())
        {
            Instantiate(playerActionPrefab, transform).GetComponent<UI_PlayerAction>().Setup(action);
        }
    }    
}
