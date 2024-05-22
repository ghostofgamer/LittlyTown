using System;
using System.Collections;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;

    private float _duration = 1;
    private float _elapsedTime;
    private float _dayValue = -30f;
    private float _nightValue = -180f;
    private float _xValue = 50;
    private float _currentY;
    private float _dayIntensity = 1;
    private float _nightIntensity = 0.6f;
    private float _currentIntensity;
    private Color _currentColor;
    private Coroutine _coroutine;

    public event Action TimeDayChanged;
    public bool IsNight { get; private set; }

    private void Start()
    {
        _currentY = _dayValue;
        _currentIntensity = _light.intensity;
        _currentColor = _light.color;
    }

    public void ChangeDayTime()
    {
        IsNight = !IsNight;
        TimeDayChanged?.Invoke();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(IsNight
            ? StartChange(_currentY, _nightValue, _currentIntensity, _nightIntensity,_currentColor,_nightColor)
            : StartChange(_currentY, _dayValue, _currentIntensity, _dayIntensity,_currentColor,_dayColor));
    }

    private IEnumerator StartChange(float start, float target, float startIntensity, float targetIntensity,Color startColor,Color targetColor)
    {
        _elapsedTime = 0;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float angle = Mathf.Lerp(start, target, _elapsedTime / _duration);
            _currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, _elapsedTime / _duration);
            _currentColor = Color.Lerp(startColor, targetColor, _elapsedTime / _duration);
            _light.color = _currentColor;
            _light.intensity = _currentIntensity;
            _currentY = angle;
            _light.transform.rotation = Quaternion.Euler(_xValue, angle, 0);
            yield return null;
        }
    }
}