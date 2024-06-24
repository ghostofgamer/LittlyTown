using UnityEngine;
using Wallets;

namespace UI.Buttons
{
    public class FreeCrystallButton : AbstractButton
    {
        [SerializeField] private CrystalWallet _crystallWallet;

        private int _value = 100;
    
        protected override void OnClick()
        {
            _crystallWallet.IncreaseValue(_value);
        }
    }
}
