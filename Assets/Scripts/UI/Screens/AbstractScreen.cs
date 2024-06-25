using System.Collections;
using Dragger;
using UnityEngine;

namespace UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AbstractScreen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private float _elapsedTime;
        private float _duration = 1f;
        private Coroutine _coroutine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void OnOpen()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _coroutine = StartCoroutine(Fade(0, 1));
        }

        public virtual void Close()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private IEnumerator Fade(float start, float end)
        {
            _elapsedTime = 0f;

            while (_elapsedTime < _duration)
            {
                _canvasGroup.alpha = Mathf.Lerp(start, end, _elapsedTime / _duration);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = end;
        }
    }
}