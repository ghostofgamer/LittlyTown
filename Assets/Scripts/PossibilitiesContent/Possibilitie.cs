using System;
using UnityEngine;

namespace PossibilitiesContent
{
    public abstract class Possibilitie : MonoBehaviour
    {
        [SerializeField] private int _startPrice;
        [SerializeField] private float _priceMultiplier;

        public event Action<int> PriceChanged;

        public int Price { get; private set; }

        public float PriceMultiplier => _priceMultiplier;

        private void Start()
        {
            SetStartPrice();
        }

        public void IncreasePrice()
        {
            Price = Mathf.RoundToInt(Price * _priceMultiplier);
            PriceChanged?.Invoke(Price);
        }

        public void SetPrice(int price)
        {
            if (price <= 0)
                return;

            Price = price;
            IncreasePrice();
        }

        public void SetCurrentPrice(int price)
        {
            Price = price;
            PriceChanged?.Invoke(_startPrice);
        }

        public void SetStartPrice()
        {
            Price = _startPrice;
            PriceChanged?.Invoke(Price);
        }
    }
}