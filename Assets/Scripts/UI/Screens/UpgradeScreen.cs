using UI.Buttons.UpgradeButtons;
using UnityEngine;

namespace UI.Screens
{
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
}