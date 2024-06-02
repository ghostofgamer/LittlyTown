using UnityEngine;

namespace UI.Buttons.StartBonusesButtons
{
    public class BonusBulldozersButton : BonusesButton
    {
        protected override void SelectBonus()
        {
            BonusesStart.IncreaseAmountBulldozers(Reward, Price);
        }
    }
}