using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] GameObject npcPref;

    GameObject currentNpc = null;

    [SerializeField] Transform parent;

    public void SpawnNPC(RectTransform rectTransform, Transform parent)
    {
        this.parent = parent;
        if (currentNpc == null)
        {
            currentNpc = Instantiate(npcPref, parent);
        }

        RectTransform rect = currentNpc.GetComponent<RectTransform>();

        rect.localScale = rectTransform.localScale;

        rect.sizeDelta = rectTransform.sizeDelta;

        rect.anchoredPosition = rectTransform.anchoredPosition;
        rect.anchorMin = rectTransform.anchorMin;
        rect.anchorMax = rectTransform.anchorMax;
        rect.pivot = rectTransform.pivot;
    }

    
    public void PlayNPCAnim(List<RectTransform> list)
    {
        StartCoroutine(CommonUIAnimation.MoveThroughTargets(currentNpc.GetComponent<RectTransform>(), list, 0.5f));

    }
}
