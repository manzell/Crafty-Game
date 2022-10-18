using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UI_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public IEnumerable<Item> Items { get; private set; }
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] TextMeshProUGUI hoverText;
    [SerializeField] Image itemSprite; 
    [SerializeField] GameObject hoverBox;
    [SerializeField] GameObject dragItemPrefab;

    public void Setup(IEnumerable<Item> items)
    {
        Items = items;

        if (items.Count() == 0) return; 

        itemSprite.sprite = items.First().Data.itemSprite;
        itemSprite.color = items.First().Data.itemTint;
        hoverText.text = $"{items.First().name}\nWeight: {items.Sum(item => item.Weight).ToString("0.0")}kg";
        quantity.text = items.Count().ToString();
        
        quantity.gameObject.SetActive(items.Count() > 1);
    }

    public void OnPointerEnter(PointerEventData eventData) => hoverBox.SetActive(true);
    public void OnPointerExit(PointerEventData eventData) => hoverBox.SetActive(false);
    public void OnDrag(PointerEventData eventData) => eventData.selectedObject.transform.position = Input.mousePosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.selectedObject = Instantiate(dragItemPrefab, transform);
        eventData.selectedObject.AddComponent<UI_DraggableItem>().Items = Items;
        eventData.selectedObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
        eventData.selectedObject.GetComponent<CanvasGroup>().alpha = 0.75f;
        eventData.selectedObject.GetComponent<Image>().sprite = itemSprite.sprite;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(eventData.selectedObject);
        eventData.selectedObject = null; 
    }
}
