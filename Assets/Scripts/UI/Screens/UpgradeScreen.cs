using System.Collections;
using System.Collections.Generic;
using UI.Buttons.UpgradeButtons;
using UI.Screens;
using UnityEngine;

public class UpgradeScreen : AbstractScreen
{
    [SerializeField] private UpgradeButton[] _upgradeButtons;

    public override void Open()
    {
        base.Open();
        CheckPossibilityPurchasing();
    }

    public void CheckPossibilityPurchasing()
    {
        foreach (var button in _upgradeButtons)
            button.CheckAvailability();
    }
}