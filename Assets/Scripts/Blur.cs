using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Blur : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;

    private float _zero = 0f;
    private float _activeBlurValue = 0.76f;
    private float _elapsedTime;
    private float _duration = 0.5f;
    private Coroutine _coroutine;

    public void TurnOn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(_zero, _activeBlurValue));
        _postProcessVolume.enabled = true;
    }

    public void TurnOff()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(_activeBlurValue, _zero));
        _postProcessVolume.enabled = false;
    }

    private IEnumerator ChangeValue(float start, float end)
    {
        _elapsedTime = 0;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            _postProcessVolume.weight = Mathf.Lerp(start, end, _elapsedTime / _duration);
            yield return null;
        }

        _postProcessVolume.weight = end;
    }
}