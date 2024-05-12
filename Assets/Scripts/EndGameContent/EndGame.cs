using System.Collections;
using UI.Screens;
using UnityEngine;

namespace EndGameContent
{
    public class EndGame : AbstractScreen
    {
        [SerializeField] private Spawner _spawner;
        // [SerializeField] private CanvasGroup _canvasGroup;

        /*private Coroutine _coroutine;
        private float _elapsedTime;
        private float _duration = 1f;*/

        private void OnEnable()
        {
            _spawner.PositionsFilled += Open;
        }

        private void OnDisable()
        {
            _spawner.PositionsFilled -= Open;
        }

        /*private void Start()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }*/

        /*private void OpenScreen()
        {
            StartCoroutine(OpenEndScreen());
        }

        private IEnumerator OpenEndScreen()
        {
            _elapsedTime = 0f;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            while (_elapsedTime < _duration)
            {
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, _elapsedTime / _duration);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 1;
        }*/
    }
}