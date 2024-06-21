using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Blur : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private DepthOfField _depthOfField;
    private BoolParameter _depthOfFieldEnabled;
    
    private float _minValue = 0.3f;
    private float _activeBlurValue = 0.76f;
    private float _elapsedTime;
    private float _duration = 0.5f;
    private Coroutine _coroutine;

    private void Start()
    {
        _depthOfField = _postProcessVolume.profile.GetSetting<DepthOfField>();
    }

    public void TurnOn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(_minValue, _activeBlurValue));
        // _postProcessVolume.enabled = true;
        _depthOfField.active = true;
    }

    public void TurnOff()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(_activeBlurValue, _minValue));
        // _postProcessVolume.enabled = false;
        _depthOfField.active = false;
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