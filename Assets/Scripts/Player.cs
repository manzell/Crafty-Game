using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using Sirenix.OdinInspector; 

public class Player : SerializedMonoBehaviour
{
    public UnityEvent<Item> addItemEvent { get; private set; } = new();
    public UnityEvent<Item> removeItemEvent { get; private set; } = new();
    public UnityEvent<PlayerAction> gainLevelEvent { get; private set; } = new();

    public ZoneData currentZone;
    public PlayerAction currentAction { get; private set; }

    [field: SerializeField] public List<Item> inventory { get; private set; }
    [field: SerializeField] public Dictionary<PlayerAction, int> experience { get; private set; }

    public void SetCurrentAction(PlayerAction action) => currentAction = action;

    public void GiveItem(Item item)
    {
        inventory.Add(item);
        addItemEvent.Invoke(item); 
    }

    public void RemoveItem(Item item)
    {
        if (inventory.Remove(item))
            removeItemEvent.Invoke(item); 
    }

    public int GetLevel(PlayerAction action) => experience.ContainsKey(action) ? Mathf.FloorToInt(Mathf.Log(experience[action], 5f)) : 0;

    public void GiveExperience(PlayerAction action, int amount)
    {
        int oldLevel = GetLevel(action); 

        if (!experience.TryAdd(action, amount))
            experience[action] += amount;

        Debug.Log($"Gaines {amount} XP in {action.name}");

        if (GetLevel(action) != oldLevel)
            gainLevelEvent.Invoke(action); 
    }

    private void Awake()
    {
        gainLevelEvent.AddListener(action => Debug.Log($"Player is now Level {GetLevel(action)} in {action}")); 
    }
}
