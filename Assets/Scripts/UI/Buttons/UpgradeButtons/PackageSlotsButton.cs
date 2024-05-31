using SaveAndLoad;
using UnityEngine;

namespace UI.Buttons.UpgradeButtons
{
    public class PackageSlotsButton : UpgradeButton
    {
        [SerializeField] private StorageButton[] _storages;
        [SerializeField] private GameObject _notPurchased;
        [SerializeField] private GameObject _purchased;
        [SerializeField] private Load _load;
        [SerializeField] private Save _save;

        private int _currentIndex;
        private int _defaultIndex = 0;
        private int _indexAllStorageButton = 2;

        private void Start()
        {
            _currentIndex = _load.Get(UpgradeName.ToString(), _defaultIndex);

            if (_currentIndex <= _defaultIndex)
                return;

            OpenStorage();
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
            _currentIndex++;
            _save.SetData(UpgradeName.ToString(), _currentIndex);
            OpenStorage();
            ClosePurchased();
        }

        private void ClosePurchased()
        {
            if (_currentIndex < _indexAllStorageButton)
                return;

            _notPurchased.SetActive(false);
            _purchased.SetActive(true);
            Button.enabled = false;
        }

        private void OpenStorage()
        {
            for (int i = 0; i < _currentIndex; i++)
                _storages[i].OpenStorage();
        }
    }
}