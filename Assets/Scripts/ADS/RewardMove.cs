using CountersContent;
using MapsContent;
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
        [SerializeField] private GameLevelScreen _gameLevelScreen;
        [SerializeField] private bool _isStartCreateMap;
        [SerializeField] private StartMap _startMap;
        
        protected override void OnReward()
        {
            _moveCounter.ReplenishSteps();
            _endMoveScreen.Close();
            _gameLevelScreen.OnOpen();

            if (!_isStartCreateMap)
                _spawner.OnCreateItem();
            else
                _startMap.StartCreate();
        }
    }
}