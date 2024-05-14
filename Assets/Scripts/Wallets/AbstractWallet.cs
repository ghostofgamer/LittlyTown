using System;
using UnityEngine;

namespace Wallets
{
    public class AbstractWallet : MonoBehaviour
    {
        [SerializeField] private int _startValue;

        private int _currentValue;

        public event Action ValueChanged;
        
        public int CurrentValue => _currentValue;

        private void Start()
        {
            _currentValue = _startValue;
            ValueChanged?.Invoke();
        }

        public void AddValue(int value)
        {
            if (value <= 0)
                return;

            _currentValue += value;
            ValueChanged?.Invoke();
        }

        public void Buy(int price)
        {
            if (_currentValue < price)
                return;

            _currentValue -= price;
            ValueChanged?.Invoke();
        }
    }
}
