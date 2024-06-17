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
        
        public void ActivateVisual()
        {
            _visualItemsDeactivator.OnDeactivationVisual();
            // _visualPosition.SetActive(true);
            if (_visualPosition.color.a < 1f)
            {
                if(_coroutine!=null)
                    StopCoroutine(_coroutine);
                
                StartCoroutine(FadeRoutine(_visualPosition.color.a, 1f));
            }
        }
        
        public void DeactivateVisual()
        {
            // _visualPosition.SetActive(false);
            if (_visualPosition.color.a > 0f)
            {
                if(_coroutine!=null)
                    StopCoroutine(_coroutine);
                
                StartCoroutine(FadeRoutine(_visualPosition.color.a, 0f));
            }
        }
        
        public void FadeIn()
        {
            StartCoroutine(FadeRoutine(0f, 1f));
        }

        public void FadeOut()
        {
            StartCoroutine(FadeRoutine(1f, 0f));
        }

        private IEnumerator FadeRoutine(float start, float end)
        {
            float time = 0f;
            Color color = _visualPosition.color;

            while (time < _fadeDuration)
            {
                time += Time.deltaTime;
                color.a = Mathf.Lerp(start, end, time / _fadeDuration);
                _visualPosition.color = color;
                yield return null;
            }

            // Убедитесь, что значение цвета точно равно целевому значению в конце
            color.a = end;
            _visualPosition.color = color;
        }
    }
}
