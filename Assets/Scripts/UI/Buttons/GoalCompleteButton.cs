using GoalContent;
using UnityEngine;
using Wallets;

namespace UI.Buttons
{
    public class GoalCompleteButton : AbstractButton
    {
        [SerializeField] private CrystalWallet _crystalWallet;
        [SerializeField] private GoalsCounter _goalsCounter;
        [SerializeField] private GameObject _progressInfo;
        [SerializeField] private GameObject _completeInfo;
        [SerializeField] private Goal _goal;

        private int _reward = 30;
        private int _value = 1;

        protected override void OnClick()
        {
            CompleteGoal();
            _crystalWallet.IncreaseValue(_reward);
        }

        public void CompleteGoal()
        {
            _goal.SetCompleteValue(_value);
            _progressInfo.SetActive(false);
            _completeInfo.SetActive(true);
            _goalsCounter.CompleteGoal();
        }
    }
}