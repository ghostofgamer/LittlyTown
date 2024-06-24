using PossibilitiesContent;
using TMPro;
using UI.Screens.PossibilitiesShopContent;
using UnityEngine;
using Wallets;

namespace UI.Buttons.PossibilitiesFiles
{
    public class BuyPossibilitieButton : AbstractButton
    {
        [SerializeField] private Possibilitie _possibilitie;
        [SerializeField] private PossibilitiesShopScreen _possibilitiesShopScreen;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private CloseButton _closeButton;
        [SerializeField] private OpenButton _openButton;
        [SerializeField] private MovementIcon _movementIcon;
        [SerializeField] private TMP_Text _possibilitiePriceText;

        private int _currentPrice;

        protected override void OnEnable()
        {
            base.OnEnable();
            _possibilitie.PriceChanged += ChangePrice;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _possibilitie.PriceChanged += ChangePrice;
        }

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);

            if (_goldWallet.CurrentValue < _possibilitiesShopScreen.CurrentPrice)
                return;

            _goldWallet.DecreaseValue(_possibilitiesShopScreen.CurrentPrice);
            _openButton.Open();
            _movementIcon.StartMove(_possibilitiesShopScreen.CurrentAmount);

            if (_possibilitie.Price != _possibilitiesShopScreen.CurrentPrice)
                _possibilitie.SetPrice(_possibilitiesShopScreen.CurrentPrice);
            else
                _possibilitie.IncreasePrice();
        }

        private void Show()
        {
            if (_possibilitiePriceText != null)
                _possibilitiePriceText.text = _possibilitie.Price.ToString();
        }

        private void ChangePrice(int price)
        {
            if (_possibilitiePriceText == null)
                return;

            _currentPrice = price;
            Show();
        }

        public void CheckPossibilityPurchasing()
        {
            _possibilitiePriceText.color = _possibilitie.Price > _goldWallet.CurrentValue ? Color.red : Color.white;
        }
    }
}