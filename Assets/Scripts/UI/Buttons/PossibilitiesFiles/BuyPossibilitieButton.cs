using PossibilitiesContent;
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

        protected override void OnClick()
        {
            if (_goldWallet.CurrentValue < _possibilitiesShopScreen.CurrentPrice)
                return;

            _goldWallet.Buy(_possibilitiesShopScreen.CurrentPrice);

            if (_possibilitie.Price != _possibilitiesShopScreen.CurrentPrice)
            {
                _possibilitie.SetPrice(_possibilitiesShopScreen.CurrentPrice);
            }
            else
            {
                _possibilitie.IncreasePrice();
            }
        }
    }
}