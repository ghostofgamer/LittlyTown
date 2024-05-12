using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    public void ActivationFade(CanvasGroup canvasGroup, float start, float end, float elapsedTime, float duration,Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(Fade(canvasGroup, start, end, elapsedTime, duration));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float start, float end, float elapsedTime, float duration)
    {
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}