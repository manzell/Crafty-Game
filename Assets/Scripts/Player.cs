using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using Sirenix.OdinInspector; 

public class Player : SerializedMonoBehaviour
{
    public UnityEvent<PlayerAction> gainLevel = new(); 

    [SerializeField] List<Item> inventory;
    [SerializeField] public Dictionary<PlayerAction, int> experience;

    public List<Item> Inventory => inventory; 
    public Zone currentZone;
    public PlayerAction currentAction { get; private set; }

    public void SetCurrentAction(PlayerAction action) => currentAction = action;

    public void GiveItem(Item item) => inventory.Add(item);

    public int GetLevel(PlayerAction action) => experience.ContainsKey(action) ? Mathf.FloorToInt(Mathf.Log(experience[action], 5f)) : 0;

    public void GiveExperience(PlayerAction action, int amount)
    {
        int oldLevel = GetLevel(action); 

        if (!experience.TryAdd(action, amount))
            experience[action] += amount;

        Debug.Log($"Gaines {amount} XP in {action.name}");

        if (GetLevel(action) != oldLevel)
            gainLevel.Invoke(action); 
    }

    private void Awake()
    {
        gainLevel.AddListener(action => Debug.Log($"Player is now Level {GetLevel(action)} in {action}")); 
    }
}
