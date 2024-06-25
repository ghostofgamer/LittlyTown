using SaveAndLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradesContent
{
    public class PackageLittleTown : MonoBehaviour
    {
        private const string LittleTown = "LittleTown";

        [SerializeField] private Button _packageButton;
        [SerializeField] private Load _load;
        [SerializeField] private Save _save;

        private int _currentIndex;
        private int _defaultIndex = 0;
        private int _purchaseIndex = 1;

        public int Amount { get; private set; } = 3;

        public bool IsActive { get; private set; }

        private void Start()
        {
            _currentIndex = _load.Get(LittleTown, _defaultIndex);

            if (_currentIndex >= _purchaseIndex)
                PurchasedPackage();
        }

        public void PurchasedPackage()
        {
            _currentIndex++;
            IsActive = true;
            _packageButton.interactable = false;
            _save.SetData(LittleTown, _currentIndex);
        }

        public void Activated()
        {
            _currentIndex = 0;
            IsActive = false;
            _packageButton.interactable = true;
            _save.SetData(LittleTown, _currentIndex);
        }
    }
}