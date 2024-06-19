using System;
using System.Collections;
using Enums;
using SaveAndLoad;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] private SettingsModes _settingsModes;
    [SerializeField] private Light _light;
    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;
    [SerializeField] private Color _dayGrass;
    [SerializeField] private Color _nightGrass;
    [SerializeField] private Save _save;
    [SerializeField] private Load _load;
    [SerializeField] private ParticleSystem _fireflies;
    [SerializeField] private Material _grassMaterial;
    
    private float _duration = 1;
    private float _elapsedTime;
    private float _dayValue = -30f;
    private float _nightValue = -180f;
    private float _xValue = 50;
    private float _currentY;
    private float _dayIntensity = 1.3f;
    private float _nightIntensity = 0.3f;
    private float _currentIntensity;
    private Color _currentColor;
    private Coroutine _coroutine;
    private int _day = 1;
    private int _night = 0;
    private int _currentValue;

    public event Action TimeDayChanged;

    public bool IsNight { get; private set; }

    private void Start()
    {
        LoadValue();
    }

    public void ChangeDayTime()
    {
        IsNight = !IsNight;
        _fireflies.gameObject.SetActive(IsNight);
        TimeDayChanged?.Invoke();
        SaveValue();
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(IsNight
            ? StartChange(_currentY, _nightValue, _currentIntensity, _nightIntensity, _currentColor, _nightColor,_grassMaterial.color,_nightGrass)
            : StartChange(_currentY, _dayValue, _currentIntensity, _dayIntensity, _currentColor, _dayColor,_grassMaterial.color,_dayGrass));
    }

    private IEnumerator StartChange(float start, float target, float startIntensity, float targetIntensity,
        Color startColor, Color targetColor,Color startGrassColor,Color targetGrass)
    {
        _elapsedTime = 0;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float angle = Mathf.Lerp(start, target, _elapsedTime / _duration);
            _currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, _elapsedTime / _duration);
            _currentColor = Color.Lerp(startColor, targetColor, _elapsedTime / _duration);
            _grassMaterial.color = Color.Lerp(startGrassColor, targetGrass, _elapsedTime / _duration);
            _light.color = _currentColor;
            _light.intensity = _currentIntensity;
            _currentY = angle;
            _light.transform.rotation = Quaternion.Euler(_xValue, angle, 0);
            yield return null;
        }
    }

    private void SaveValue()
    {
        _save.SetData(_settingsModes.ToString(), IsNight ? _night : _day);
    }

    private void LoadValue()
    {
        _currentValue = _load.Get(_settingsModes.ToString(), _day);
        IsNight = _currentValue == _night;
        _fireflies.gameObject.SetActive(IsNight);
        ChangeValue(_currentValue);
    }

    private void ChangeValue(int value)
    {
        _light.color = value == _day ? _dayColor : _nightColor;
        _light.intensity = value == _day ? _dayIntensity : _nightIntensity;
        _light.transform.rotation = Quaternion.Euler(_xValue, value == _day ? _dayValue : _nightValue, 0);
        _currentY = value == _day ? _dayValue : _nightValue;
        _currentIntensity = _light.intensity;
        _currentColor = _light.color;
        TimeDayChanged?.Invoke();
    }
}