using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class UI_PlayerAction : MonoBehaviour, IDropHandler
{
    [SerializeField] PlayerAction action;
    [SerializeField] TextMeshProUGUI actionName; 
    [SerializeField] GameObject progressFrame;
    [SerializeField] Image progressImage, inputIcon;
    [SerializeField] Button button;

    float time;
    Player player;

    public void Setup(PlayerAction action)
    {
        this.action = action;
        actionName.text = action.name;
        ResetAction(); 
    }

    public void ClearItem() => inputIcon.sprite = null; 

    public void StartAction()
    {
        player = FindObjectOfType<Player>();
        action.Prepare(player);

        time = Time.time;

        if (action is OngoingAction ongoingAction)
        {
            AudioSource source = GetComponent<AudioSource>();

            StartCoroutine(ongoingAction.Progress(player));
            StartCoroutine(ProgressAnimation(ongoingAction));

            ongoingAction.prepareEvent.AddListener(player => { if (ongoingAction.startSound) source.PlayOneShot(ongoingAction.startSound); });
            ongoingAction.progressEvent.AddListener(player => { if (ongoingAction.progressSound) source.PlayOneShot(ongoingAction.progressSound); });
            ongoingAction.completeEvent.AddListener(player => { if (ongoingAction.completeSound) source.PlayOneShot(ongoingAction.completeSound); });
        }
        else
        {
            action.Complete(player);
        }

        button.GetComponentInChildren<TextMeshProUGUI>().text = $"Stop {action.name}";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ResetAction);
    }

    public void ResetAction()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StartAction);
        button.GetComponentInChildren<TextMeshProUGUI>().text = $"Start {action.name}";
    }

    public IEnumerator ProgressAnimation(OngoingAction action) 
    {
        while(player.currentAction == action)
        {
            float progress = ((Time.time - time) % action.interval) / action.interval;
            float frameWidth = progressFrame.GetComponent<RectTransform>().rect.width;
            Vector2 size = progressImage.rectTransform.sizeDelta;
            
            size.x = frameWidth * progress;
            progressImage.rectTransform.sizeDelta = size;

            yield return new WaitForSeconds(1f/30);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        action.SetInputs(eventData.selectedObject.GetComponent<UI_DraggableItem>().Items);        
        inputIcon.sprite = action.Inputs.First().Data.itemSprite;

        button.enabled = action.Can(player);
    }
} 