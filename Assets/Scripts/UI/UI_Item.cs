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
    [SerializeField] List<Item> Items = new();
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] TextMeshProUGUI hoverText;
    [SerializeField] Image itemSprite; 
    [SerializeField] GameObject hoverBox;
    [SerializeField] GameObject dragItemPrefab;

    public void Setup(IEnumerable<Item> items)
    {
        Items = items.ToList();

        if (items.Count() == 0) 
            return;
        else 
            Setup(items.First()); 
    }

    public void Setup(Item item)
    {
        if (item.Data != null)
        {
            itemSprite.sprite = item.Data.itemSprite;
            itemSprite.color = item.Data.itemTint;
        }

        quantity.text = Items.Count() > 1 ? Items.Count().ToString() : string.Empty; 
        hoverText.text = $"{item.name}\nWeight: {item.Weight.ToString("0.0")}kg";
        quantity.gameObject.SetActive(Items.Count() > 1);
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
