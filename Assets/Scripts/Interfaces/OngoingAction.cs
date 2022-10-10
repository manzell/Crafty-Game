using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public abstract class OngoingAction : PlayerAction
{
    public UnityEvent<Player> progressEvent { get; }
    public AudioClip progressSound;
    public float interval { get; protected set; }

    public abstract IEnumerator Progress(Player player);
}