using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public abstract class PlayerAction : ScriptableObject
{
    public new string name; 

    public UnityEvent<Player> actionStart = new(), actionEnd = new();
    public AudioClip startSound, intervalSound, completeSound; 

    protected abstract void Prepare(Player player);
    protected abstract void Complete(Player player);
    protected abstract bool Can(Player player);

    public void OnEnable()
    {
        actionStart.AddListener(player => Prepare(player));
        actionEnd.AddListener(player => Complete(player));
    }

    public void Action(Player player)
    {
        if (Can(player))
        {
            player.SetCurrentAction(this); 
            actionStart.Invoke(player);
        }
    }
}

public interface ITakeTime
{
    float time { get; }
    float interval { get; }
}