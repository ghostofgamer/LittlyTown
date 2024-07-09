using System;
using System.Collections;
using UnityEngine;

namespace Wallets
{
    public class AbstractWallet : MonoBehaviour
    {
        [SerializeField] private int _startValue;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        private int _currentValue;
        private Coroutine _coroutine;
        private float _elapsedTime;
        private float _duration = 1f;
        private float _targetValue;
        private float _firstValue;
        private bool _isLoadValue;
        private bool _isProfit = true;

        public event Action ValueChanged;

        public event Action ValueChangedCompleted;

        public int CurrentValue => _currentValue;

        protected virtual void Start()
        {
            if (_isLoadValue)
                return;

            _currentValue = _startValue;
            ValueChanged?.Invoke();
        }

        public virtual void IncreaseValue(int value)
        {
            if (value <= 0 || !_isProfit)
                return;

            _currentValue += value;
            ValueChanged?.Invoke();
        }

        public void SmoothlyIncreaseValue(int value)
        {
            if (value <= 0)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(SmoothlyChangeValue(value));
        }

        public virtual void DecreaseValue(int price)
        {
            if (_currentValue < price)
                return;

            _audioSource.PlayOneShot(_audioSource.clip);
            _currentValue -= price;
            ValueChanged?.Invoke();
        }

        public void SetValue(int value)
        {
            _isLoadValue = true;
            _currentValue = value;
            ValueChanged?.Invoke();
        }

        public void SetInitialValue()
        {
            _currentValue = _startValue;
            ValueChanged?.Invoke();
        }

        public void DisableProfit()
        {
            _isProfit = false;
        }

        public void EnableProfit()
        {
            _isProfit = true;
        }

        private IEnumerator SmoothlyChangeValue(int value)
        {
            _elapsedTime = 0f;
            _targetValue = _currentValue + value;
            _firstValue = _currentValue;
            _audioSource.PlayOneShot(_audioClip);

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                _currentValue = (int)Mathf.Lerp(_firstValue, _targetValue, _elapsedTime / _duration);
                ValueChanged?.Invoke();
                yield return null;
            }

            ValueChangedCompleted?.Invoke();
        }
    }
}