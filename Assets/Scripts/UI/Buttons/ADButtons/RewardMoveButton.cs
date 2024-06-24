using ADS;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons.ADButtons
{
    public class RewardMoveButton : AbstractButton
    {
        [SerializeField] private RewardMove _rewardMove;

        protected override void OnClick()
        {
            Button.enabled = false;
            _rewardMove.Show();
        }
    }
}