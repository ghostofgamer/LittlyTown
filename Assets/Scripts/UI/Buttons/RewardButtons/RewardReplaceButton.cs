using PossibilitiesContent;
using UnityEngine;

namespace UI.Buttons.RewardButtons
{
    public class RewardReplaceButton : RewardButton
    {
        [SerializeField] private MovementIcon _movementIcon;
        
        private int _amount = 1;
        
        protected override void ChangeRewardItem()
        {
            _movementIcon.StartMove(_amount);
        }
    }
}
