using Dragger;
using PossibilitiesContent;
using PostProcessContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.PossibilitiesShopContent
{
    public class PossibilitiesShopScreen : AbstractScreen
    {
        [SerializeField] private Possibilitie _possibilitie;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _currentAmountText;
        [SerializeField] private InputItemDragger _inputItemDragger;
        [SerializeField] private GameLevelScreen _gameLevelScreen;
        [SerializeField] private Blur _blur;

        private int _basePrice;
        private int _totalPrice;
        private int _maxValue = 5;
        private int _minValue;
        private int _defaultValue = 1;

        public int CurrentPrice { get; private set; }

        public int CurrentAmount { get; private set; } = 1;

        private void OnEnable()
        {
            _possibilitie.PriceChanged += OnChangePrice;
        }

        private void OnDisable()
        {
            _possibilitie.PriceChanged -= OnChangePrice;
        }

        private void Start()
        {
            Show();
            _basePrice = _possibilitie.Price;
            CurrentPrice = _basePrice;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _blur.TurnOn();
            CurrentAmount = _defaultValue;
            _basePrice = _possibilitie.Price;
            CurrentPrice = _basePrice;
            Show();
            _inputItemDragger.enabled = false;
            _gameLevelScreen.Close();
        }

        public override void Close()
        {
            base.Close();
            _blur.TurnOff();
            _inputItemDragger.enabled = true;
        }

        public void IncreaseAmount()
        {
            if (CurrentAmount < _maxValue)
            {
                CurrentAmount++;
                UpdatePrice();
            }
        }

        public void DecreaseAmount()
        {
            if (CurrentAmount > _minValue)
            {
                CurrentAmount--;
                UpdatePrice();
            }
        }

        private void Show()
        {
            _priceText.text = CurrentPrice.ToString();
            _currentAmountText.text = CurrentAmount.ToString();
        }

        private void OnChangePrice(int price)
        {
            CurrentPrice = price;
            Show();
        }

        private void UpdatePrice()
        {
            CurrentPrice = CalculateTotalPrice(CurrentAmount);
            Show();
        }

        private int CalculateTotalPrice(int amount)
        {
            _totalPrice = 0;

            for (int i = 1; i <= amount; i++)
            {
                CurrentPrice = Mathf.RoundToInt(_basePrice * Mathf.Pow(_possibilitie.PriceMultiplier, i - 1));
                _totalPrice += CurrentPrice;
            }

            return _totalPrice;
        }
    }
}