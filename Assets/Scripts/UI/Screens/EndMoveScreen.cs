using CountersContent;
using UnityEngine;

namespace UI.Screens
{
    public class EndMoveScreen : AbstractScreen
    {
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private GameLevelScreen _gameLevelScreen;

        private void OnEnable()
        {
            _moveCounter.MoveOver += OnOpen;
        }

        private void OnDisable()
        {
            _moveCounter.MoveOver -= OnOpen;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _gameLevelScreen.Close();
        }
    }
}