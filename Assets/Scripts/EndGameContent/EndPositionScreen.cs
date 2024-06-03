using UI.Screens;
using UnityEngine;

namespace EndGameContent
{
    public class EndPositionScreen : AbstractScreen
    {
        [SerializeField] private Spawner _spawner;
        [SerializeField]private GameLevelScreen _gameLevelScreen;
        [SerializeField]private Blur _blur;

        private void OnEnable()
        {
            _spawner.PositionsFilled += Open;
        }

        private void OnDisable()
        {
            _spawner.PositionsFilled -= Open;
        }

        public override void Open()
        {
            base.Open();
            _gameLevelScreen.Close();
            _blur.TurnOn();
        }
    }
}