using System;
using System.Collections;
using UnityEngine;

namespace Wallets
{
    public class AbstractWallet : MonoBehaviour
    {
        [SerializeField] private int _startValue;
        [SerializeField] private AudioSource _audioSource;

        private int _currentValue;
        private Coroutine _coroutine;
        private float _elapsedTime;
        private float _duration = 1f;
        private bool _isLoadValue;
        private bool _isProfit = true;
        
        public event Action ValueChanged;

        public int CurrentValue => _currentValue;

        protected virtual void Start()
        {
            if (_isLoadValue)
                return;

            // Debug.Log("GoldStart");
            _currentValue = _startValue;
            ValueChanged?.Invoke();
        }

        public virtual void IncreaseValue(int value)
        {
            if (value <= 0||!_isProfit)
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

        private IEnumerator SmoothlyChangeValue(int value)
        {
            _elapsedTime = 0f;

            float targetValue = _currentValue + value;
            float startValue = _currentValue;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                _currentValue = (int) Mathf.Lerp(startValue, targetValue, _elapsedTime / _duration);
                ValueChanged?.Invoke();
                yield return null;
            }
        }

        public void SetValue(int value)
        {
            _isLoadValue = true;
            // Debug.Log("GoldSEt");
            _currentValue = value;
            // Debug.Log("GetGold " + _currentValue);
            ValueChanged?.Invoke();
        }

        public void SetInitialValue()
        {
            _currentValue = _startValue;
            // Debug.Log("GoldSEt " + _currentValue);
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
    }
}