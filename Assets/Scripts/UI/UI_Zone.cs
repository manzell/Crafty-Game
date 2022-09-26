using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class UI_Zone : MonoBehaviour
{
    [SerializeField] Zone zone; 
    [SerializeField] TextMeshProUGUI zoneName;
    [SerializeField] GameObject playerActionPrefab; 

    public void Awake()
    {
        zoneName.text = zone.name;
    }    
}
