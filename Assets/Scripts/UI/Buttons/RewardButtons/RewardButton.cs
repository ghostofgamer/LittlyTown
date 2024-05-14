using UI.Screens;
using UnityEngine;

namespace UI.Buttons.RewardButtons
{
    public abstract class RewardButton : AbstractButton
    {
        [SerializeField] private CompleteScoreScreen _completeScoreScreen;
    
        protected override void OnClick()
        {
            _completeScoreScreen.Close();
            ChangeRewardItem();
        }

        protected abstract void ChangeRewardItem();
    }
}
