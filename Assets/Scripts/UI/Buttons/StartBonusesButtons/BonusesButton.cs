using UnityEngine;
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

        protected override void OnClick()
        {
            if (_crystalWallet.CurrentValue < _price)
                return;

            _crystalWallet.DecreaseValue(_price);
            ActivateChoose();
            SelectBonus();
        }

        protected abstract void SelectBonus();

        private void ActivateChoose()
        {
            _notSelectedObject.SetActive(false);
            _selectedObject.SetActive(true); 
        }

        public void DeactivateChoose()
        {
            _notSelectedObject.SetActive(true);
            _selectedObject.SetActive(false);
        }
    }
}
