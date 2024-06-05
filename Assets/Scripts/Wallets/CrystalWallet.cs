using SaveAndLoad;
using UnityEngine;

namespace Wallets
{
    public class CrystalWallet : AbstractWallet
    {
        private const string Crystal = "Crystal";

        [SerializeField] private Save _save;
        [SerializeField] private Load _load;

        private int _defaultValue = 100;

        private void Awake()
        {
            SetValue(_load.Get(Crystal, _defaultValue));
            int number = _load.Get(Crystal, _defaultValue); 
            // Debug.Log("Load " + number);
        }

        public override void IncreaseValue(int value)
        {
            base.IncreaseValue(value);
            _save.SetData(Crystal, CurrentValue);
            // Debug.Log("Increase " + CurrentValue);
        }

        public override void DecreaseValue(int price)
        {
            base.DecreaseValue(price);
            _save.SetData(Crystal, CurrentValue);
        }
    }
}