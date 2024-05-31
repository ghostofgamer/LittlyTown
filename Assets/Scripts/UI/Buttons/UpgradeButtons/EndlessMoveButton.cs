using System;
using SaveAndLoad;
using UnityEngine;

namespace UI.Buttons.UpgradeButtons
{
    public class EndlessMoveButton : UpgradeButton
    {
        [SerializeField] private GameObject _notPurchased;
        [SerializeField] private GameObject _purchased;
        [SerializeField] private Load _load;
        [SerializeField] private Save _save;

        private int _purchasedIndex = 1;
        private int _defaultIndex = 0;
        private int _currentIndex;

        public event Action EndlessPurchased;
        
        private void Start()
        {
            _currentIndex = _load.Get(UpgradeName.ToString(), _defaultIndex);

            if (_currentIndex >= _purchasedIndex)
                ClosePurchased();
        }

        protected override void OnClick()
        {
            BuyUpgrade();
        }

        protected override void BuyUpgrade()
        {
            if (!TryBuyUpgrade())
                return;
            
            base.BuyUpgrade();
            _save.SetData(UpgradeName.ToString(), _purchasedIndex);
            ClosePurchased();
        }

        private void ClosePurchased()
        {
            EndlessPurchased?.Invoke();
            _notPurchased.SetActive(false);
            _purchased.SetActive(true);
            Button.enabled = false;
        }
    }
}