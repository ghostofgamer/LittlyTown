using UnityEngine;

namespace UI.Buttons.UpgradeButtons
{
    public class PackageLittleTownButton : UpgradeButton
    {
        [SerializeField] private PackageLittleTown _packageLittleTown;
        
        protected override void OnClick()
        {
            BuyUpgrade();
        }

        protected override void BuyUpgrade()
        {
            if (!TryBuyUpgrade())
                return;
            
            base.BuyUpgrade();
            _packageLittleTown.PurchasedPackage();
        }
    }
}
