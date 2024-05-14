using System;
using System.Collections;
using UnityEngine;

namespace Wallets
{
    public class AbstractWallet : MonoBehaviour
    {
        [SerializeField] private int _startValue;

        private int _currentValue;
        private Coroutine _coroutine;
        private float _elapsedTime;
        private float _duration = 1f;
        
        public event Action ValueChanged;
        
        public int CurrentValue => _currentValue;

        private void Start()
        {
            _currentValue = _startValue;
            ValueChanged?.Invoke();
        }

        public void IncreaseValue(int value)
        {
            if (value <= 0)
                return;

            _currentValue += value;
            ValueChanged?.Invoke();
        }

        public void SmoothlyIncreaseValue(int value)
        {
            if (value <= 0)
                return;
            
            if(_coroutine!=null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(SmoothlyChangeValue(value));
        }

        public void DecreaseValue(int price)
        {
            if (_currentValue < price)
                return;

            _currentValue -= price;
            ValueChanged?.Invoke();
        }

        private IEnumerator SmoothlyChangeValue(int value)
        {
            _elapsedTime = 0f;

            float targetValue = _currentValue + value;
            float startValue = _currentValue;
            
            while (_elapsedTime<_duration)
            {
                _elapsedTime += Time.deltaTime;
                 _currentValue = (int)Mathf.Lerp(startValue, targetValue, _elapsedTime / _duration);
                ValueChanged?.Invoke();
                yield return null;
            }
        }
    }
}
