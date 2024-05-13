using CountersContent;
using UnityEngine;

namespace UI.Screens
{
    public class EndScoreScreen : AbstractScreen
    {
        [SerializeField] private ScoreCounter _scoreCounter;

        private void OnEnable()
        {
            _scoreCounter.LevelChanged += Open;
        }

        private void OnDisable()
        {
            _scoreCounter.LevelChanged -= Open;
        }
    }
}
