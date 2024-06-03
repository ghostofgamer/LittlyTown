using ADS;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons.ADButtons
{
    public class RewardMoveButton : AbstractButton
    {
        [SerializeField] private RewardMove _rewardMove;
        /*[SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private EndMoveScreen _endMoveScreen;
        [SerializeField]private Spawner _spawner;*/

        protected override void OnClick()
        {
            Button.enabled = false;
            _rewardMove.Show();
            /*_moveCounter.ReplenishSteps();
            _endMoveScreen.Close();
            _spawner.OnCreateItem();*/
        }
    }
}