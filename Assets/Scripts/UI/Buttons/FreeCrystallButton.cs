using UI.Buttons;
using UnityEngine;
using Wallets;

public class FreeCrystallButton : AbstractButton
{
    [SerializeField] private CrystalWallet _crystallWallet;

    private int _value = 100;
    
    protected override void OnClick()
    {
        _crystallWallet.IncreaseValue(_value);
    }
}
