using CountersContent;
using UI.Buttons.RewardButtons;
using UnityEngine;

namespace UI.Screens
{
    public class CompleteScoreScreen : AbstractScreen
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField]private RewardGoldButton _goldButton;
        [SerializeField] private Blur _blur;

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
            base.Open();
            _goldButton.DetermineGoldAmount();
            _blur.TurnOn();
        }

        public override void Close()
        {
            base.Close();
            _blur.TurnOff();
        }
    }
}
