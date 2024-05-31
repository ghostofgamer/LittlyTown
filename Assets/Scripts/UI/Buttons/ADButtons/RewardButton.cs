using ADS;
using UnityEngine;

namespace UI.Buttons.ADButtons
{
    public class RewardButton : AbstractButton
    {
        [SerializeField] private RewardVideo _reward;

        protected override void OnClick()
        {
            Button.enabled = false;
            _reward.Show();
        }
    }
}