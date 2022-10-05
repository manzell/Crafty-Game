using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Node : MonoBehaviour
{
    [SerializeField] NodeData NodeData;
    [SerializeField] TextMeshProUGUI nodeName;
    [SerializeField] GameObject zonePrefab, zonesArea;

    private void Awake()
    {
        if (NodeData)
            Setup(NodeData); 
    }

    public void Setup(NodeData node)
    {
        this.NodeData = node; 
        nodeName.text = node.name;

        foreach(ZoneData zone in node.Zones)
            Instantiate(zonePrefab, zonesArea.transform).GetComponent<UI_Zone>().Setup(zone);
    }
}
