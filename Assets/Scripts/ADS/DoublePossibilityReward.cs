using UnityEngine;

namespace ADS
{
    public class DoublePossibilityReward : RewardVideo
    {
        [SerializeField] private BonusesStart _bonusesStart;

        private int _reward = 2;
        private int _price = 0;

        protected override void OnReward()
        {
            _bonusesStart.IncreaseAmountBulldozers(_reward, _price);
            _bonusesStart.IncreaseAmountReplaces(_reward, _price);
        }
    }
}