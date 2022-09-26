using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Node : MonoBehaviour
{
    [SerializeField] Node node;
    [SerializeField] TextMeshProUGUI nodeName;
    [SerializeField] GameObject zonePrefab;

    public void Awake()
    {
        nodeName.text = node.name;
    }
}
