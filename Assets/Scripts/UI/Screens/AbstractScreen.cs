using System.Collections;
using Dragger;
using UnityEngine;

namespace UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AbstractScreen : MonoBehaviour
    {
        [SerializeField] private InputItemDragger _itemDragger;
        [SerializeField] private AbstractScreen _gameLevelScreen;

        private CanvasGroup _canvasGroup;
        private float _elapsedtime;
        private float _duration = 1f;
        private Coroutine _coroutine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Open()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _coroutine = StartCoroutine(Fade(0, 1));

            if (_gameLevelScreen != null)
                _gameLevelScreen.Close();

            if (_itemDragger != null)
                _itemDragger.enabled = false;
        }

        public void Close()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            if (_gameLevelScreen != null)
                _gameLevelScreen.Open();

            if (_itemDragger != null)
                _itemDragger.enabled = true;
        }

        private IEnumerator Fade(float start, float end)
        {
            _elapsedtime = 0f;

            while (_elapsedtime < _duration)
            {
                _canvasGroup.alpha = Mathf.Lerp(start, end, _elapsedtime / _duration);
                _elapsedtime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = end;
        }
    }
}