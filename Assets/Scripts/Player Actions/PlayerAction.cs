using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections;

public abstract class PlayerAction : SerializedScriptableObject
{
    public new string name;
    [field:SerializeField] public float Interval { get; protected set; }

    public UnityEvent<Player, Zone> prepareEvent { get; } = new();
    public UnityEvent<Player, Zone> completeEvent { get; } = new();
    public UnityEvent<Player, Zone> progressEvent { get; } = new();
    public AudioClip progressSound, startSound, completeSound;

    public abstract void Prepare(Player player, Zone zone);
    public abstract void Complete(Player player, Zone zone);
    public abstract bool Can(Player player, Zone zone);
    public abstract IEnumerator Progress(Player player, Zone zone);
}