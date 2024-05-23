using System;
using UnityEngine;

namespace PossibilitiesContent
{
    public abstract class Possibilitie : MonoBehaviour
    {
        [SerializeField] private int _startPrice;
        [SerializeField] private float _priceMultiplier;

        private int _price;

        public event Action<int> PriceChanged;

        public int Price => _price;

        public float PriceMultiplier => _priceMultiplier;

        private void Start()
        {
            _price = _startPrice;
            PriceChanged?.Invoke(_price);
        }

        public virtual void IncreasePrice()
        {
            _price = Mathf.RoundToInt(_price * _priceMultiplier);
            PriceChanged?.Invoke(_price);
            // Debug.Log("Increase" + _price);
        }

        public void SetPrice(int price)
        {
            if (price <= 0)
                return;

            _price = price;
            // Debug.Log("Set" + _price);
            IncreasePrice();
        }

        public void SetCurrentPrice(int price)
        {
            _price = price;
            PriceChanged?.Invoke(_startPrice);
        }

        public void SetStartPrice()
        {
            _price = _startPrice;
            PriceChanged?.Invoke(_price);
        }
    }
}