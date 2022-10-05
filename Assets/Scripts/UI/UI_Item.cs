using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using static UnityEditor.Progress;

public class UI_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public IEnumerable<Item> Items { get; private set; }
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] TextMeshProUGUI hoverText;
    [SerializeField] Image itemSprite; 
    [SerializeField] GameObject hoverBox;
    [SerializeField] GameObject dragItemPrefab;

    GameObject dragItem; 
    Sprite oldSprite;
    bool dragging; 

    public void Setup(IEnumerable<Item> items)
    {
        Items = items;
        Debug.Log(Items); 

        if (items.Count() > 1)
            quantity.text = items.Count().ToString();
        else
            quantity.enabled = false;

        itemSprite.sprite = items.First().Data.itemSprite;
        oldSprite = itemSprite.sprite; 
        hoverText.text = $"{items.First().name}\nWeight: {items.Sum(item => item.weight).ToString("0.0")}kg"; 
    }


    public void OnPointerEnter(PointerEventData eventData) => hoverBox.SetActive(true);
    public void OnPointerExit(PointerEventData eventData) => hoverBox.SetActive(false);

    public void OnDrag(PointerEventData eventData) => dragItem.transform.position = Input.mousePosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragItem = Instantiate(dragItemPrefab, transform);
        dragItem.GetComponent<Image>().sprite = itemSprite.sprite;
        dragItem.AddComponent<CanvasGroup>().blocksRaycasts = false; 
        eventData.selectedObject = gameObject;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(dragItem);
        dragItem = null; 
    }
}
