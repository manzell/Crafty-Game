using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Node : ScriptableObject
{
    public new string name;
    [SerializeField] List<Zone> zones;
}
