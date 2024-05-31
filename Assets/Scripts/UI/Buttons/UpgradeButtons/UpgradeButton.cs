using UnityEngine;
using Wallets;

namespace UI.Buttons.UpgradeButtons
{
    public abstract class UpgradeButton : AbstractButton
    {
        [SerializeField] private CrystalWallet _crystalWallet;
        [SerializeField] private int _price;
        [SerializeField] private Upgrades _upgradeName;

        protected Upgrades UpgradeName => _upgradeName;

        protected virtual void BuyUpgrade()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _crystalWallet.DecreaseValue(_price);
        }

        protected bool TryBuyUpgrade()
        {
            return _crystalWallet.CurrentValue >= _price;
        }
    }
}