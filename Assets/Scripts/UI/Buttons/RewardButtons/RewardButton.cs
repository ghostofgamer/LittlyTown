using UI.Screens;
using UnityEngine;

namespace UI.Buttons.RewardButtons
{
    public abstract class RewardButton : AbstractButton
    {
        [SerializeField] private CompleteScoreScreen _completeScoreScreen;
    
        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _completeScoreScreen.Close();
            ChangeRewardItem();
        }

        protected abstract void ChangeRewardItem();
    }
}
