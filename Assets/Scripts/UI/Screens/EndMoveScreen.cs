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
            _moveCounter.MoveOver += Open;
        }

        private void OnDisable()
        {
            _moveCounter.MoveOver -= Open;
        }

        public override void Open()
        {
            base.Open();
            _gameLevelScreen.Close();
        }
    }
}