using UI.Buttons.StartBonusesButtons;
using UnityEngine;
using Wallets;

namespace ADS
{
    public class DoublePossibilityReward : RewardVideo
    {
        [SerializeField] private BonusesStart _bonusesStart;
        [SerializeField] private BonusesButton _bonusesButton;
        
        private int _reward = 2;
        private int _price = 0;

        protected override void OnReward()
        {
            _bonusesButton.ActivateChoose();
            _bonusesStart.IncreaseAmountBulldozers(_reward, _price);
            _bonusesStart.IncreaseAmountReplaces(_reward, _price);
        }
    }
}