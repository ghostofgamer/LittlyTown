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
        }

        public override void IncreaseValue(int value)
        {
            base.IncreaseValue(value);
            _save.SetData(Crystal, CurrentValue);
        }

        public override void DecreaseValue(int price)
        {
            base.DecreaseValue(price);
            _save.SetData(Crystal, CurrentValue);
        }
    }
}