using InitializationContent;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using Wallets;

namespace UI.Buttons
{
    public class BuyMapButton : AbstractButton
    {
        private const string OpenMap = "OpenMap";

        [SerializeField] private GameObject _alertPrefab;
        [SerializeField] private Save _save;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private ChooseMapScreen _chooseMapScreen;
        [SerializeField] private CrystalWallet _crystalWallet;
        [SerializeField] private Transform _container;

        private int _levelBuyValue = 1;
        private int _price = 300;

        protected override void OnClick()
        {
            if (_price <= _crystalWallet.CurrentValue)
            {
                _crystalWallet.DecreaseValue(_price);
                _save.SetData(OpenMap + _initializator.Index, _levelBuyValue);
                _chooseMapScreen.ChangeActivationButton();
            }
            else
            {
                Instantiate(_alertPrefab, _container);
            }
        }
    }
}