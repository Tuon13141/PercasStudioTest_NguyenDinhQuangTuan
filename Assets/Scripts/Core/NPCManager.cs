using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] GameObject npcPref;

    GameObject currentNpc = null;

    Transform parent;

    public void SpawnNPC(RectTransform rectTransform, Transform parent)
    {
        this.parent = parent;
        if (currentNpc == null)
        {
            currentNpc = Instantiate(npcPref, parent);
        }

        currentNpc.SetActive(true);

        RectTransform rect = currentNpc.GetComponent<RectTransform>();

        rect.localScale = rectTransform.localScale;

        rect.sizeDelta = rectTransform.sizeDelta;

        currentNpc.transform.localPosition = rectTransform.localPosition;
    }

    
    public void PlayNPCAnim(List<RectTransform> list)
    {
        StartCoroutine(CommonUIAnimation.MoveThroughTargets(currentNpc.GetComponent<RectTransform>(), list, 0.1f));

    }

    public void StopAnim()
    {
        if (currentNpc != null)
            currentNpc.SetActive(false);
        StopAllCoroutines();
    }
}
