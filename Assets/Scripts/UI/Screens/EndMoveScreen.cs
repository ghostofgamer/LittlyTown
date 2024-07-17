using System;
using System.Collections;
using CountersContent;
using UnityEngine;

namespace UI.Screens
{
    public class EndMoveScreen : AbstractScreen
    {
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private GameLevelScreen _gameLevelScreen;
        [SerializeField] private bool _isStarted;

        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.3f);
        private Coroutine _coroutine;

        public event Action MoveScreenOpened;

        private void OnEnable()
        {
            if (!_isStarted)
                _moveCounter.MoveOver += OnOpen;
        }

        private void OnDisable()
        {
            if (!_isStarted)
                _moveCounter.MoveOver -= OnOpen;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _gameLevelScreen.Close();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartEventSignal());
        }

        private IEnumerator StartEventSignal()
        {
            yield return _waitForSeconds;
            MoveScreenOpened?.Invoke();
        }
    }
}