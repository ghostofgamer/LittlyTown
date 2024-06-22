using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Vignette : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private PostProcessProfile _postProcessPrifile;
    
    private UnityEngine.Rendering.PostProcessing.Vignette _vignete;
    private BoolParameter _depthOfFieldEnabled;
    private float _defaultValue =1f;
    private float _minValue =0.5f;
    private float _elapsedTime;
    private float _duration = 0.5f;
    private Coroutine _coroutine;

    private void Start()
    {
        _vignete = _postProcessVolume.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Vignette>();
        _vignete.intensity.value = _defaultValue;
    }
    
    public void TurnOn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(_vignete.intensity.value, _minValue));
        
        // _vignete.intensity.value = _minValue;
    }

    public void TurnOff()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(_vignete.intensity.value, _defaultValue));
        
        // _vignete.intensity.value = _defaultValue;
    }

    private IEnumerator ChangeValue(float start, float end)
    {
        _elapsedTime = 0;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            _vignete.intensity.value = Mathf.Lerp(start, end, _elapsedTime / _duration);
            yield return null;
        }

        _vignete.intensity.value = end;
    }
}
