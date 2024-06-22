using CountersContent;
using SpawnContent;
using UI.Screens;
using UnityEngine;

namespace ADS
{
    public class RewardMove : RewardVideo
    {
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private EndMoveScreen _endMoveScreen;
        [SerializeField] private Spawner _spawner;
        [SerializeField]private GameLevelScreen _gameLevelScreen;

        protected override void OnReward()
        {
            _moveCounter.ReplenishSteps();
            _endMoveScreen.Close();
            _gameLevelScreen.Open();
            _spawner.OnCreateItem();
        }
    }
}