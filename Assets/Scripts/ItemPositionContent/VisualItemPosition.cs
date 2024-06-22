using System.Collections;
using UnityEngine;

namespace ItemPositionContent
{
    public class VisualItemPosition : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _visualPosition;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;

        private float _fadeDuration = 0.06f;
        private Coroutine _coroutine;
        private float _elapsedTime;
        private Color _color;

        public void ActivateVisual()
        {
            _visualItemsDeactivator.OnDeactivationVisual();

            if (_visualPosition.color.a < 1f)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(Fade(_visualPosition.color.a, 1f));
            }
        }

        public void DeactivateVisual()
        {
            if (_visualPosition.color.a > 0f)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(Fade(_visualPosition.color.a, 0f));
            }
        }

        private IEnumerator Fade(float start, float end)
        {
            _elapsedTime = 0f;
            _color = _visualPosition.color;

            while (_elapsedTime < _fadeDuration)
            {
                _elapsedTime += Time.deltaTime;
                _color.a = Mathf.Lerp(start, end, _elapsedTime / _fadeDuration);
                _visualPosition.color = _color;
                yield return null;
            }

            _color.a = end;
            _visualPosition.color = _color;
        }
    }
}