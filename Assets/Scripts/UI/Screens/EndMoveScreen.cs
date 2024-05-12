using UnityEngine;

namespace UI.Screens
{
    public class EndMoveScreen : AbstractScreen
    {
        [SerializeField] private MoveCounter _moveCounter;

        private void OnEnable()
        {
            _moveCounter.MoveOver += Open;
        }

        private void OnDisable()
        {
            _moveCounter.MoveOver -= Open;
        }
    }
}