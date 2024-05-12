using TMPro;
using UnityEngine;

namespace Wallets
{
    public class AbstractWallet : MonoBehaviour
    {
        [SerializeField] private int _startValue;
        [SerializeField] private TMP_Text _valueText;
        
        private int _currentValue;

        public int CurrentValue => _currentValue;

        private void Start()
        {
            _currentValue = _startValue;
            Show();
        }

        public void AddValue(int value)
        {
            if (value <= 0)
                return;

            _currentValue += value;
            Show();
        }

        public void Buy(int price)
        {
            if (_currentValue < price)
                return;

            _currentValue -= price;
            Show();
        }

        private void Show()
        {
            _valueText.text = _currentValue.ToString();
        }
    }
}
