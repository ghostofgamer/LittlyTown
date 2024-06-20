using System;
using System.Collections;
using Enums;
using SaveAndLoad;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DayNight : MonoBehaviour
{
    [SerializeField] private SettingsModes _settingsModes;
    [SerializeField] private Light _light;
    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;
    [SerializeField] private Color _dayGrass;
    [SerializeField] private Color _nightGrass;
    [SerializeField] private Color _dayTrail;
    [SerializeField] private Color _nightTrail;
    [SerializeField] private Color _nightBackGround;
    [SerializeField] private Color _dayBackGround;
    [SerializeField] private Color _dayWater;
    [SerializeField] private Color _nightWater;
    
    
    
    [SerializeField] private Gradient _grassGradient;
    [SerializeField] private Gradient _waterGradient;
    
    
    [SerializeField] private Save _save;
    [SerializeField] private Load _load;
    [SerializeField] private ParticleSystem _fireflies;
    [SerializeField] private Material _grassMaterial;
    [SerializeField] private Material _trailMaterial;
    [SerializeField] private Material _backgroundMaterial;
    [SerializeField] private Material _waterMaterial;
    [SerializeField] private PostProcessVolume _postProcessVolume;
    
    private float _currentGrassGradientValue;
    private float _targetGrassGradientValue;

    private float _duration = 1;
    private float _elapsedTime;
    private float _dayValue = -30f;
    private float _nightValue = -180f;
    private float _xValue = 50;
    private float _currentY;
    private float _dayIntensity = 1.3f;
    private float _nightIntensity = 0.35f;
    private float _currentIntensity;
    private Color _currentColor;
    private Coroutine _coroutine;
    private int _day = 1;
    private int _night = 0;
    private int _currentValue;
    private Bloom _bloom;
    private Coroutine _gradientCoroutine;
    
    public event Action TimeDayChanged;

    public bool IsNight { get; private set; }

    private void Start()
    {
        _bloom = _postProcessVolume.profile.GetSetting<Bloom>();
        LoadValue();
    }

    public void ChangeDayTime()
    {
        IsNight = !IsNight;
        _fireflies.gameObject.SetActive(IsNight);
        TimeDayChanged?.Invoke();
        SaveValue();

        _bloom.active = IsNight;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        if(_gradientCoroutine!=null)
            StopCoroutine(_gradientCoroutine);
        
        _gradientCoroutine =  StartCoroutine(ChangeGrassColorCoroutine());
        
        _coroutine = StartCoroutine(IsNight
            ? StartChange(_currentY, _nightValue, _currentIntensity, _nightIntensity, _currentColor, _nightColor,
                _grassMaterial.color, _nightGrass, _trailMaterial.color, _nightTrail, _backgroundMaterial.color,
                _nightBackGround,_waterMaterial.color,_nightWater)
            : StartChange(_currentY, _dayValue, _currentIntensity, _dayIntensity, _currentColor, _dayColor,
                _grassMaterial.color, _dayGrass, _trailMaterial.color, _dayTrail, _backgroundMaterial.color,
                _dayBackGround,_waterMaterial.color,_dayWater));
    }

    private IEnumerator StartChange(float start, float target, float startIntensity, float targetIntensity,
        Color startColor, Color targetColor, Color startGrassColor, Color targetGrassColor, Color startTrailColor,
        Color targetTrailColor, Color startBackgroundColor, Color targetBackgroundColor, Color startWatercolor,
        Color targetWatercolor)
    {
        _elapsedTime = 0;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float angle = Mathf.Lerp(start, target, _elapsedTime / _duration);
            _currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, _elapsedTime / _duration);
            _currentColor = Color.Lerp(startColor, targetColor, _elapsedTime / _duration);
            
            
            /*
            _grassMaterial.color = Color.Lerp(startGrassColor, targetGrassColor, _elapsedTime / _duration);
            _grassMaterial.color = _grassGradient.Evaluate(_elapsedTime / _duration);
            */
            
            
            
            _trailMaterial.color = Color.Lerp(startTrailColor, targetTrailColor, _elapsedTime / _duration);
            _backgroundMaterial.color =
                Color.Lerp(startBackgroundColor, targetBackgroundColor, _elapsedTime / _duration);
            // _waterMaterial.color = Color.Lerp(startWatercolor, targetWatercolor, _elapsedTime / _duration);
            // _waterMaterial.SetColor("_WaterColor", Color.Lerp(startWatercolor, targetWatercolor, _elapsedTime / _duration));
            _light.color = _currentColor;
            _light.intensity = _currentIntensity;
            _currentY = angle;
            _light.transform.rotation = Quaternion.Euler(_xValue, angle, 0);
            yield return null;
        }
    }
    
    IEnumerator ChangeGrassColorCoroutine()
    {
        _targetGrassGradientValue = IsNight ? 1f : 0f;
        _elapsedTime = 0f;
        float startGrassGradientValue = _currentGrassGradientValue;
        
        while (_elapsedTime < 5)
        {
            _elapsedTime += Time.deltaTime;
            _currentGrassGradientValue = Mathf.Lerp(startGrassGradientValue, _targetGrassGradientValue, _elapsedTime/_duration);
            _grassMaterial.color = _grassGradient.Evaluate(_currentGrassGradientValue);
            // _waterMaterial.SetColor("Water Color", _waterGradient.Evaluate(_currentGrassGradientValue));
            _waterMaterial.SetColor("_WaterColor", _waterGradient.Evaluate(_currentGrassGradientValue));
            yield return null;
        }

        _currentGrassGradientValue = _targetGrassGradientValue;
        _grassMaterial.color = _grassGradient.Evaluate(_currentGrassGradientValue);
        // _waterMaterial.SetColor("Water Color", _waterGradient.Evaluate(_currentGrassGradientValue));
        _waterMaterial.SetColor("_WaterColor", _waterGradient.Evaluate(_currentGrassGradientValue));
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
        _grassMaterial.color = value == _day ? _dayGrass : _nightGrass;
        _trailMaterial.color = value == _day ? _dayTrail : _nightTrail;
        _backgroundMaterial.color = value == _day ? _dayBackGround : _nightBackGround;
        // _waterMaterial.color = value == _day ? _dayWater : _nightWater;
        _currentGrassGradientValue = IsNight ? 1 : 0;
        _waterMaterial.SetColor("_WaterColor", (value == _day) ? _dayWater : _nightWater);
        // _waterMaterial.SetColor("Water Color", (value == _day) ? _dayWater : _nightWater);
        _bloom.active = value != _day;
        TimeDayChanged?.Invoke();
    }
}