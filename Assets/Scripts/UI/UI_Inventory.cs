using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] GameObject ItemPrefab; 
    Player player;

    List<UI_Item> uiItems = new(); 

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        player.addItemEvent.AddListener(SetupUIItem);
        player.removeItemEvent.AddListener(OnRemoveItem);

        foreach (Item item in player.Inventory)
            SetupUIItem(item); 

        //foreach (string itemName in player.Inventory.Select(item => item.name).Distinct())
        //    CreateUIItem(itemName);
    }

    void SetupUIItem(Item item)
    {
        UI_Item itemUIStack = uiItems.Where(uiItem => uiItem.Items.First().name == item.name).FirstOrDefault(); 

        if(itemUIStack == null)
        {
            UI_Item newUIItem = Instantiate(ItemPrefab, transform).GetComponent<UI_Item>();
            newUIItem.Setup(item); 
            uiItems.Add(newUIItem);
        }
    }

    void UpdateUIItem(Item item)
    {
        UI_Item uiStack = uiItems.Where(_item => _item.name == item.name).FirstOrDefault();

        if (uiStack != null)
            uiStack.Setup(item); 
    }

    void OnRemoveItem(Item item)
    {
        throw new System.NotImplementedException(); 
    }
}
