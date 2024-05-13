using PossibilitiesContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.PossibilitiesShopContent
{
    public class PossibilitiesShopScreen : AbstractScreen
    {
        [SerializeField] private Possibilitie _possibilitie;
        [SerializeField] private TMP_Text _priceText;

        private int _currentPrice;
        private int _basePrice;
        private int _currentAmount = 1;
        
        public int CurrentPrice => _currentPrice;

        private void OnEnable()
        {
            _possibilitie.PriceChanged += ChangePrice;
        }

        private void OnDisable()
        {
            _possibilitie.PriceChanged -= ChangePrice;
        }

        private void Start()
        {
            Show();
            _basePrice = _possibilitie.Price;
            _currentPrice = _basePrice;
        }

        public override void Open()
        {
            base.Open();
            _basePrice = _possibilitie.Price;
            _currentPrice = _basePrice;
            Show();
        }

        private void Show()
        {
            _priceText.text = _possibilitie.Price.ToString();
        }

        private void ChangePrice(int price)
        {
            _currentPrice = price;
            Show();
        }

        public void IncreaseAmount()
        {
            _currentAmount++;
            UpdatePrice();
        }

        public void DecreaseAmount()
        {
            if (_currentAmount > 1)
            {
                _currentAmount--;
                UpdatePrice();
            }
        }

        private void UpdatePrice()
        {
            _currentPrice = CalculateTotalPrice(_currentAmount);
            _priceText.text = "Цена: " + _currentPrice + " монет";
        }

        private int CalculateTotalPrice(int amount)
        {
            int totalPrice = 0;

            for (int i = 1; i <= amount; i++)
            {
                _currentPrice = Mathf.RoundToInt(_basePrice * Mathf.Pow(_possibilitie.PriceMultiplier, i - 1));
                totalPrice += _currentPrice;
            }

            return totalPrice;
        }
    }
}