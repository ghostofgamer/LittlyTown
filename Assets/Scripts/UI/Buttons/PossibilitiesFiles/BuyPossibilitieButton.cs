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
        [SerializeField] private PossibilitieMovement _possibilitieMovement;
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
            if (_goldWallet.CurrentValue < _possibilitiesShopScreen.CurrentPrice)
                return;

            _goldWallet.Buy(_possibilitiesShopScreen.CurrentPrice);
            _closeButton.Close();
            _possibilitieMovement.StartMove(_possibilitiesShopScreen.CurrentAmount);

            if (_possibilitie.Price != _possibilitiesShopScreen.CurrentPrice)
            {
                _possibilitie.SetPrice(_possibilitiesShopScreen.CurrentPrice);
            }
            else
            {
                _possibilitie.IncreasePrice();
            }
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
    }
}