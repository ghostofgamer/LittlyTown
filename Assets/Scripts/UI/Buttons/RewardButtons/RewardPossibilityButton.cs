using PossibilitiesContent;
using UnityEngine;

namespace UI.Buttons.RewardButtons
{
    public class RewardPossibilityButton : RewardButton
    {
        [SerializeField] private PossibilitieMovement _possibilitieMovement;
        
        private int _amount = 1;
        
        protected override void ChangeRewardItem()
        {
            _possibilitieMovement.StartMove(_amount);
        }
    }
}
