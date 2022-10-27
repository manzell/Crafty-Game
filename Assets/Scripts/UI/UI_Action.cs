using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Action : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject progressFrame;
    [SerializeField] Image progressImage, inputIcon;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI actionName;

    PlayerAction action;
    IEnumerable<PlayerAction> actions;

    AudioSource source;
    Player player;
    Zone zone;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        source = GetComponentInParent<AudioSource>();
    }

    public void Setup(IEnumerable<PlayerAction> actions, Zone zone)
    {
        this.actions = actions;
        Setup(actions.FirstOrDefault(action => action.Can(player, zone)) ?? actions.First(), zone); 
    }

    public void Setup(PlayerAction action, Zone zone)
    {
        this.zone = zone;
        this.action = action;
        actionName.text = action.name;

        ResetAction(); 
    }

    public void ResetAction()
    {
        if (action != null && player.currentAction == action)
            action.Complete(player, zone);

        button.onClick.RemoveAllListeners();
        action?.prepareEvent.RemoveListener(PlayStartSound);
        action?.completeEvent.RemoveListener(PlayCompleteSound);
        action?.progressEvent.RemoveListener(PlayProgressSound);

        if(actions != null)
            action = actions.FirstOrDefault(action => action.Can(player, zone));

        if (action != null)
        {
            action.prepareEvent.AddListener(PlayStartSound);
            action.completeEvent.AddListener(PlayCompleteSound);
            action.progressEvent.AddListener(PlayProgressSound);


            button.onClick.AddListener(StartAction);
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"Start {action.name}";
        }

        button.enabled = action != null;
    }

    public void StartAction()
    {
        if (action != null)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"Stop {action.name}";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ResetAction);

            action.Prepare(player, zone);
            actionStartTime = Time.time;

            StartCoroutine(action.Progress(player, zone));
            StartCoroutine(ProgressAnimation(action));
        }
    }

    void PlayStartSound(Player player, Zone zone)
    {
        if (action.startSound)
            source.PlayOneShot(action.startSound);
    }

    void PlayCompleteSound(Player player, Zone zone)
    {
        if (action.completeSound)
            source.PlayOneShot(action.completeSound);
    }

    void PlayProgressSound(Player player, Zone zone)
    {
        if (action.progressSound)
            source.PlayOneShot(action.progressSound);
    }

    float actionStartTime;
    public IEnumerator ProgressAnimation(PlayerAction action)
    {
        while (player.currentAction == action && action.Interval > 0)
        {
            yield return new WaitForSeconds(1f / 30);

            float progress = ((Time.time - actionStartTime) % action.Interval) / action.Interval;
            float frameWidth = progressFrame.GetComponent<RectTransform>().rect.width;
            Vector2 size = progressImage.rectTransform.sizeDelta;

            size.x = frameWidth * progress;
            progressImage.rectTransform.sizeDelta = size;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        zone.Inputs.AddRange(eventData.selectedObject.GetComponent<UI_DraggableItem>().Items);
        inputIcon.sprite = zone.Inputs.Last().Data.itemSprite;

        ResetAction(); 

        button.enabled = action == null ? false : action.Can(player, zone);
    }
}
