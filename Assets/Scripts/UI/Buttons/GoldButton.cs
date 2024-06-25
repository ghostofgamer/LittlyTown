using System.Collections;
using UnityEngine;

namespace UI.Buttons
{
    public class GoldButton : AbstractButton
    {
        [SerializeField] private CanvasGroup _goldInfo;

        private float _start = 0;
        private float _end = 1;
        private float _elapsedTime;
        private float _duration = 0.5f;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1.5f);

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ActivationFade());
        }

        private IEnumerator ActivationFade()
        {
            _elapsedTime = 0f;

            while (_elapsedTime < _duration)
            {
                Fade(_start, _end);
                yield return null;
            }

            _goldInfo.alpha = _end;
            yield return _waitForSeconds;
            _elapsedTime = 0f;

            while (_elapsedTime < _duration)
            {
                Fade(_end, _start);
                yield return null;
            }

            _goldInfo.alpha = _start;
        }

        private void Fade(float start, float end)
        {
            _goldInfo.alpha = Mathf.Lerp(start, end, _elapsedTime / _duration);
            _elapsedTime += Time.deltaTime;
        }
    }
}