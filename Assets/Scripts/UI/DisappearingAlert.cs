using System.Collections;
using UnityEngine;

public class DisappearingAlert : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(1.5f);
    private float _elapsedTime;
    private float _duration = 0.5f;

    private void OnEnable()
    {
        StartFade();
    }

    private void OnDisable()
    {
    }

    private void StartFade()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(FadeAlpha());
    }

    private IEnumerator FadeAlpha()
    {
        yield return _waitForSeconds;

        float startAlpha = 1f;
        float endAlpha = 0f;
        _elapsedTime = 0f;
        
        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, _elapsedTime / _duration);
            _canvasGroup.alpha = alpha;
            yield return null;
        }
        
        gameObject.SetActive(false);
        _canvasGroup.alpha = endAlpha;
    }
}