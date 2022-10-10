using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerAction : ScriptableObject
{
    public new string name;

    public UnityEvent<Player> prepareEvent { get; } = new();
    public UnityEvent<Player> completeEvent { get; } = new(); 
    public AudioClip startSound, completeSound;

    [field: SerializeField] public List<Item> Inputs { get; private set; } = new();

    public abstract void Prepare(Player player);
    public abstract void Complete(Player player);
    public abstract bool Can(Player player);

    public void Action(Player player)
    {
        if (Can(player))
        {
            player.SetCurrentAction(this);
            Prepare(player);
            prepareEvent.Invoke(player); 
        }
    }

    public void Reset(Player player)
    {
        player.SetCurrentAction(null); 
        Inputs.Clear();
    }

    public void SetInputs(IEnumerable<Item> items) => Inputs = items.ToList();
}