using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Linq;
using static UnityEditor.Progress;

public class UI_PlayerAction : MonoBehaviour, IDropHandler
{
    [SerializeField] PlayerAction action;
    [SerializeField] TextMeshProUGUI actionName; 
    [SerializeField] GameObject progressFrame;
    [SerializeField] Image progressImage, inputIcon;
    [SerializeField] Button button;
    [field: SerializeField] public IEnumerable<Item> Inputs { get; private set; } 


    public void Setup(PlayerAction action)
    {
        this.action = action;
        actionName.text = action.name;
        button.onClick.AddListener(() => action.Action(FindObjectOfType<Player>())); 

        if (action is ITakeTime timedAction)
            button.onClick.AddListener(() => StartCoroutine(AnimateAction(timedAction.time)));
        else
            button.onClick.AddListener(() => StartCoroutine(AnimateAction(0)));
    }

    public void ClearItem()
    {
        Inputs = null;
        inputIcon.sprite = null; 
    }

    public IEnumerator AnimateAction(float time) 
    {
        float progress = 0;
        Vector2 size = progressImage.rectTransform.sizeDelta;
        button.enabled = false;

        AudioSource source = GetComponent<AudioSource>(); 

        if(action.startSound)
            source.PlayOneShot(action.startSound);

        while (progress < time)
        {
            progress += (action as ITakeTime).interval;
            if (action.intervalSound != null)
                source.PlayOneShot(action.intervalSound);

            float progressPct = Mathf.Clamp01(progress / time);
            float frameWidth = progressFrame.GetComponent<RectTransform>().rect.width;

            size.x = frameWidth * progressPct; 
            progressImage.rectTransform.sizeDelta = size;

            yield return new WaitForSeconds((action as ITakeTime).interval);
        }

        if (action.completeSound)
            source.PlayOneShot(action.completeSound);
        action.actionEnd.Invoke(FindObjectOfType<Player>(), Inputs);

        size.x = 0;
        progressImage.rectTransform.sizeDelta = size;
        button.enabled = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Inputs = eventData.selectedObject.GetComponent<UI_Item>().Items;
        inputIcon.sprite = Inputs.First().Data.itemSprite;
    }
} 