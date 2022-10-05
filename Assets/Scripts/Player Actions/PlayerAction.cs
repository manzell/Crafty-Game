using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerAction : ScriptableObject
{
    public new string name; 

    public UnityEvent<Player, IEnumerable<Item>> actionStart = new(), actionEnd = new();
    public AudioClip startSound, intervalSound, completeSound;

    public IEnumerable<Item> Inputs { get; private set; }

    public abstract void Prepare(Player player, IEnumerable<Item> inputs);
    public abstract void Complete(Player player, IEnumerable<Item> inputs);
    public abstract bool Can(Player player, IEnumerable<Item> inputs);

    public void OnEnable()
    {
        actionStart.AddListener((player, item) => Prepare(player, item));
        actionEnd.AddListener((player, item) => Complete(player, item));
    }

    public void Action(Player player)
    {
        if (Can(player, Inputs))
        {
            player.SetCurrentAction(this); 
            actionStart.Invoke(player, Inputs);
        }
    }

    public void SetInput(IEnumerable<Item> items) => Inputs = items;
}

public interface ITakeTime
{
    float time { get; }
    float interval { get; }
}