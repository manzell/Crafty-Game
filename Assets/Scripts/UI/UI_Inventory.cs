using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] GameObject ItemPrefab; 
    Player player;

    Dictionary<string, UI_Item> uiItems = new(); 

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        player.addItemEvent.AddListener(OnGiveItem);
        player.removeItemEvent.AddListener(OnRemoveItem);

        foreach (string itemName in player.inventory.Select(item => item.name).Distinct())
            CreateUIItem(itemName); 
    }

    void CreateUIItem(string itemName)
    {
        UI_Item uiItem = Instantiate(ItemPrefab, transform).GetComponent<UI_Item>();
        uiItem.Setup(player.inventory.Where(item => item.name == itemName));
        uiItems.Add(itemName, uiItem);
    }

    void UpdateUIItem(string itemName)
    {
        if (uiItems.ContainsKey(itemName))
            uiItems[itemName].Setup(player.inventory.Where(i => i.name == itemName)); 
    }

    void OnGiveItem(Item item)
    {
        if (uiItems.ContainsKey(item.name))
            UpdateUIItem(item.name); 
        else
            CreateUIItem(item.name);
    }

    void OnRemoveItem(Item item)
    {
        if (uiItems.ContainsKey(item.name))
            UpdateUIItem(item.name);
    }
}
