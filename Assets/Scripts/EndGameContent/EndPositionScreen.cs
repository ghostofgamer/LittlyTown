using PostProcessContent;
using SpawnContent;
using UI.Screens;
using UnityEngine;

namespace EndGameContent
{
    public class EndPositionScreen : AbstractScreen
    {
        [SerializeField] private Spawner _spawner;
        [SerializeField] private GameLevelScreen _gameLevelScreen;
        [SerializeField] private Blur _blur;

        private void OnEnable()
        {
            _spawner.PositionsFilled += OnOpen;
        }

        private void OnDisable()
        {
            _spawner.PositionsFilled -= OnOpen;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _gameLevelScreen.Close();
            _blur.TurnOn();
        }
    }
}