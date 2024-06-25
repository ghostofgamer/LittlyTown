using Enums;
using TMPro;
using UI.Screens;
using UnityEngine;
using Wallets;

namespace UI.Buttons.UpgradeButtons
{
    public abstract class UpgradeButton : AbstractButton
    {
        [SerializeField] private CrystalWallet _crystalWallet;
        [SerializeField] private int _price;
        [SerializeField] private Upgrades _upgradeName;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField]private UpgradeScreen _upgradeScreen;
        
        protected Upgrades UpgradeName => _upgradeName;

        protected virtual void BuyUpgrade()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _crystalWallet.DecreaseValue(_price);
            _upgradeScreen.CheckPossibilityPurchasing();
        }

        protected bool TryBuyUpgrade()
        {
            return _crystalWallet.CurrentValue >= _price;
        }

        public void CheckAvailability()
        {
            _priceText.color = _price > _crystalWallet.CurrentValue ? Color.red : Color.white;
        }
    }
}