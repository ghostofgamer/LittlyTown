using ADS;
using UI.Buttons.StartBonusesButtons;
using UnityEngine;

namespace UI.Buttons.ADButtons
{
    public class RewardDoublePossibilityButton : BonusesButton
    {
        [SerializeField] private DoublePossibilityReward _doublePossibilityReward;
        
        protected override void SelectBonus()
        {
            Button.enabled = false;
            _doublePossibilityReward.Show();
        }
    }
}