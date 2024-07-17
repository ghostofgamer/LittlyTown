using CountersContent;
using UnityEngine;

namespace ADS
{
    public class AdMove : RewardVideo
    {
        [SerializeField] private MoveCounter _moveCounter;

        protected override void OnReward()
        {
            _moveCounter.ReplenishSteps();
        }

        public void Rew()
        {
            _moveCounter.ReplenishSteps();
        }
    }
}
