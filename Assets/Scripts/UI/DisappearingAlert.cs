using System.Collections;
using UnityEngine;

namespace UI
{
    public class DisappearingAlert : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1.5f);
        private float _elapsedTime;
        private float _duration = 0.5f;
        private  float _startAlpha ;
        private float _endAlpha;
        private float _alpha;

        private void OnEnable()
        {
            StartFade();
        }

        private void StartFade()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(FadeAlpha());
        }

        private IEnumerator FadeAlpha()
        {
            yield return _waitForSeconds;
            _startAlpha = 1f;
            _endAlpha = 0f;
            _elapsedTime = 0f;
        
            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                _alpha = Mathf.Lerp(_startAlpha, _endAlpha, _elapsedTime / _duration);
                _canvasGroup.alpha = _alpha;
                yield return null;
            }
        
            gameObject.SetActive(false);
            _canvasGroup.alpha = _endAlpha;
        }
    }
}