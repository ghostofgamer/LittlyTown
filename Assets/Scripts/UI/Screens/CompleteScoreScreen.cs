using System;
using CountersContent;
using Dragger;
using PostProcessContent;
using UI.Buttons.RewardButtons;
using UnityEngine;
using Wallets;

namespace UI.Screens
{
    public class CompleteScoreScreen : AbstractScreen
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private RewardGoldButton _goldButton;
        [SerializeField] private Blur _blur;
        [SerializeField] private CrystalWallet _crystalWallet;
        [SerializeField] private InputItemDragger _inputItemDragger;
        [SerializeField] private CanvasGroup _gameScreenCanvasGroup;

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
            _inputItemDragger.enabled = false;
            _gameScreenCanvasGroup.blocksRaycasts = false;
        }

        public override void Close()
        {
            base.Close();
            _blur.TurnOff();
            _inputItemDragger.enabled = true;
            _gameScreenCanvasGroup.blocksRaycasts = true;
        }
    }
}