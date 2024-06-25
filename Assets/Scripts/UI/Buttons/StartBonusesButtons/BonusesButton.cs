using UnityEngine;
using UpgradesContent;
using Wallets;

namespace UI.Buttons.StartBonusesButtons
{
    public abstract class BonusesButton : AbstractButton
    {
        [SerializeField] private GameObject _notSelectedObject;
        [SerializeField] private GameObject _selectedObject;
        [SerializeField] private BonusesStart _bonusesStart;
        [SerializeField] private int _reward;
        [SerializeField] private int _price;
        [SerializeField] private CrystalWallet _crystalWallet;

        protected int Reward => _reward;

        protected BonusesStart BonusesStart => _bonusesStart;

        protected int Price => _price;

        public void ActivateChoose()
        {
            _crystalWallet.DecreaseValue(_price);
            _notSelectedObject.SetActive(false);
            _selectedObject.SetActive(true);
        }

        public void DeactivateChoose()
        {
            Button.enabled = true;
            _notSelectedObject.SetActive(true);
            _selectedObject.SetActive(false);
        }

        protected abstract void SelectBonus();

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);

            if (_crystalWallet.CurrentValue < _price)
                return;

            Button.enabled = false;
            SelectBonus();
        }
    }
}