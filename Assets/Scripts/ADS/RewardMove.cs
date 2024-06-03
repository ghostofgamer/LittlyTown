using UI.Screens;
using UnityEngine;

namespace ADS
{
    public class RewardMove : RewardVideo
    {
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private EndMoveScreen _endMoveScreen;
        [SerializeField] private Spawner _spawner;

        protected override void OnReward()
        {
            _moveCounter.ReplenishSteps();
            _endMoveScreen.Close();
            _spawner.OnCreateItem();
        }
    }
}