using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommonUIAnimation : MonoBehaviour
{
    public static IEnumerator BreathingAnimation(RectTransform rectTransform, Vector2 startScale, Vector2 endScale, float duration = 0.5f)
    {
        while (true)
        {
            if(!rectTransform.gameObject.activeSelf) yield break;
            // Scale up
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                Vector2 scale = Vector2.Lerp(startScale, endScale, t);
                rectTransform.localScale = new Vector3(scale.x, scale.y, 1f);
                yield return null;
            }

            // Scale down
            timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                Vector2 scale = Vector2.Lerp(endScale, startScale, t);
                rectTransform.localScale = new Vector3(scale.x, scale.y, 1f);
                yield return null;
            }
        }
    }

    public static IEnumerator ScaleTo(RectTransform rectTransform, Vector2 startScale, Vector2 endScale, float duration = 0.5f)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            Vector2 scale = Vector2.Lerp(startScale, endScale, t);
            rectTransform.localScale = new Vector3(scale.x, scale.y, 1f);
            yield return null;
        }
        rectTransform.localScale = new Vector3(endScale.x, endScale.y, 1f);
    }

    public static IEnumerator OpenPopUp(RectTransform rectTransform, Vector2 startScale, Vector2 endScale, float duration = 0.5f, Action action = null)
    {
        rectTransform.gameObject.SetActive(true);
        yield return ScaleTo(rectTransform, startScale, endScale, duration);
        action?.Invoke();
    }

    public static IEnumerator ClosePopUp(RectTransform rectTransform, Vector2 startScale, Vector2 endScale, float duration = 0.5f, Action action = null)
    {
        yield return ScaleTo(rectTransform, startScale, endScale, duration);
        action?.Invoke();
    }

    public static IEnumerator OpenThenClosePopUp(RectTransform rectTransform, Vector2 startScale, Vector2 endScale, float duration = 0.5f, Action action = null)
    {
        yield return OpenPopUp(rectTransform, startScale, endScale, duration);
        yield return new WaitForSeconds(2);
        yield return ClosePopUp(rectTransform, endScale, startScale, duration);
        action?.Invoke();
    }

    public static IEnumerator MoveThroughTargets(RectTransform mover, List<RectTransform> targets, float moveDuration = 0.5f)
    {
        if (targets == null || targets.Count == 0 || mover == null)
            yield break;

        Transform moverParent = mover.parent;

        foreach (RectTransform target in targets)
        {
            Vector3 worldPos = target.position;
            Vector3 localPos = moverParent.InverseTransformPoint(worldPos);
            Vector3 start = mover.localPosition;
            float timer = 0f;

            while (timer < moveDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / moveDuration);
                mover.localPosition = Vector3.Lerp(start, localPos, t);
                yield return null;
            }

            mover.localPosition = localPos;
        }
    }
}
