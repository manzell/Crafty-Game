using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UI_PlayerAction : MonoBehaviour
{
    [SerializeField] PlayerAction action;
    [SerializeField] TextMeshProUGUI actionName; 
    [SerializeField] GameObject progressFrame;
    [SerializeField] Image progressImage;
    [SerializeField] Button button;
    [SerializeField] Player player;

    public void Awake()
    {
        actionName.text = action.name;
        button.onClick.AddListener(() => action.Action(player));

        if (action is ITakeTime timedAction)
            button.onClick.AddListener(() => StartCoroutine(AnimateAction(timedAction.time)));
        else
            button.onClick.AddListener(() => StartCoroutine(AnimateAction(0)));
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
            size.x = progressFrame.GetComponent<RectTransform>().sizeDelta.x * Mathf.Clamp01(progress / time); 
            progressImage.rectTransform.sizeDelta = size;
            yield return new WaitForSeconds((action as ITakeTime).interval);
        }

        if (action.completeSound)
            source.PlayOneShot(action.completeSound);
        action.actionEnd.Invoke(player);

        size.x = 0;
        progressImage.rectTransform.sizeDelta = size;
        button.enabled = true;
    }
} 