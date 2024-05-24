using System;
using CountersContent;
using UI.Buttons.RewardButtons;
using UnityEngine;
using Wallets;

namespace UI.Screens
{
    public class CompleteScoreScreen : AbstractScreen
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField]private RewardGoldButton _goldButton;
        [SerializeField] private Blur _blur;
        [SerializeField] private CrystalWallet _crystalWallet;

        private int _reward = 15;

        public event Action ScoreCompleted;
        
        private void OnEnable()
        {
            _scoreCounter.LevelChanged += Open;
        }

        private void OnDisable()
        {
            _scoreCounter.LevelChanged -= Open;
        }

        public override void Open()
        {
            ScoreCompleted?.Invoke();
            base.Open();
            _goldButton.DetermineGoldAmount();
            _blur.TurnOn();
            _crystalWallet.IncreaseValue(_reward);
        }

        public override void Close()
        {
            base.Close();
            _blur.TurnOff();
        }
    }
}
