using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using Sirenix.OdinInspector;
using System.Linq;

public class Player : SerializedMonoBehaviour
{
    public UnityEvent<Item> addItemEvent { get; private set; } = new();
    public UnityEvent<Item> removeItemEvent { get; private set; } = new();
    public UnityEvent<PlayerAction> gainLevelEvent { get; private set; } = new();

    public ZoneData currentZone;
    public PlayerAction currentAction { get; private set; }

    [field: SerializeField] public List<Item> Inventory { get; private set; }
    [field: SerializeField] public List<ItemData> StartingInventory { get; private set; }
    [field: SerializeField] public Dictionary<PlayerAction, int> experience { get; private set; }

    private void Awake()
    {
        gainLevelEvent.AddListener(action => Debug.Log($"Player is now Level {GetLevel(action)} in {action}"));

        foreach(ItemData itemData in StartingInventory)
            GiveItem(itemData.Clone());
    }

    public void SetCurrentAction(PlayerAction action) => currentAction = action;

    public void GiveItem(Item item)
    {
        Inventory.Add(item);
        addItemEvent.Invoke(item); 
    }

    public void GiveItems(List<Item> items)
    {
        foreach (Item item in items) 
            GiveItem(item);
    }   

    public void RemoveItem(Item item)
    {
        if (Inventory.Remove(item))
            removeItemEvent.Invoke(item); 
    }

    public int GetLevel(PlayerAction action) => experience.ContainsKey(action) ? Mathf.FloorToInt(Mathf.Log(experience[action], 5f)) : 0;

    public void GiveExperience(PlayerAction action, int amount)
    {
        Debug.Log($"Gained {amount} XP in {action.name}");

        int oldLevel = GetLevel(action); 

        if (!experience.TryAdd(action, amount))
            experience[action] += amount;

        if (GetLevel(action) != oldLevel)
            gainLevelEvent.Invoke(action); 
    }
}