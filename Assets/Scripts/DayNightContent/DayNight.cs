using System;
using System.Collections;
using Enums;
using SaveAndLoad;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DayNightContent
{
    public class DayNight : MonoBehaviour
    {
        private const string WaterColor = "_WaterColor";

        [SerializeField] private SettingsModes _settingsModes;
        [SerializeField] private Light _light;
        [SerializeField] private Gradient _grassGradient;
        [SerializeField] private Gradient _waterGradient;
        [SerializeField] private Gradient _backGroundGradient;
        [SerializeField] private Gradient _trailGradient;
        [SerializeField] private Gradient _lightGradient;
        [SerializeField] private Save _save;
        [SerializeField] private Load _load;
        [SerializeField] private ParticleSystem _fireflies;
        [SerializeField] private Material _grassMaterial;
        [SerializeField] private Material _trailMaterial;
        [SerializeField] private Material _backgroundMaterial;
        [SerializeField] private Material _waterMaterial;
        [SerializeField] private PostProcessVolume _postProcessVolume;

        private float _currentGradientValue;
        private float _targetGradientValue;
        private float _duration = 1;
        private float _elapsedTime;
        private float _dayValue = -30f;
        private float _nightValue = -180f;
        private float _xValue = 50;
        private float _currentY;
        private float _dayIntensity = 1.16f;
        private float _nightIntensity = 0.35f;
        private float _currentIntensity;
        private Coroutine _coroutine;
        private int _day = 1;
        private int _night = 0;
        private int _currentValue;
        private Bloom _bloom;
        private float _nightGradientValue = 1f;
        private float _dayGradientValue = 0f;
        private float _startGrassGradientValue;


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
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(
                IsNight
                    ? StartChange(_currentY, _nightValue, _currentIntensity, _nightIntensity)
                    : StartChange(_currentY, _dayValue, _currentIntensity, _dayIntensity));
        }

        private IEnumerator StartChange(float start, float target, float startIntensity, float targetIntensity)
        {
            _elapsedTime = 0;
            _targetGradientValue = IsNight ? _nightGradientValue : _dayGradientValue;
            _startGrassGradientValue = _currentGradientValue;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                float angle = Mathf.Lerp(start, target, _elapsedTime / _duration);
                _currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, _elapsedTime / _duration);
                _light.intensity = _currentIntensity;
                _currentY = angle;
                _light.transform.rotation = Quaternion.Euler(_xValue, angle, 0);
                _currentGradientValue = Mathf.Lerp(_startGrassGradientValue, _targetGradientValue, _elapsedTime / _duration);
                SetColor();
                yield return null;
            }

            _currentGradientValue = _targetGradientValue;
            SetColor();
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
            _light.intensity = value == _day ? _dayIntensity : _nightIntensity;
            _light.transform.rotation = Quaternion.Euler(_xValue, value == _day ? _dayValue : _nightValue, 0);
            _currentY = value == _day ? _dayValue : _nightValue;
            _currentIntensity = _light.intensity;
            _currentGradientValue = IsNight ? _nightGradientValue : _dayGradientValue;
            SetColor();
            _bloom.active = value != _day;
            TimeDayChanged?.Invoke();
        }

        private void SetColor()
        {
            _light.color = _lightGradient.Evaluate(_currentGradientValue);
            _grassMaterial.color = _grassGradient.Evaluate(_currentGradientValue);
            _waterMaterial.SetColor(WaterColor, _waterGradient.Evaluate(_currentGradientValue));
            _trailMaterial.color = _trailGradient.Evaluate(_currentGradientValue);
            _backgroundMaterial.color = _backGroundGradient.Evaluate(_currentGradientValue);
        }
    }
}