using System.Collections;
using UnityEngine;

namespace PossibilitiesContent
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GoldMovement : MonoBehaviour
    {
        [SerializeField] private RectTransform _targetWallet;

        private float _elapsedTime;
        private float _duration = 0.5f;
        private Vector3 _targetPosition;
        private Vector3 _startPosition = Vector3.zero;
        private Coroutine _coroutine;
        private CanvasGroup _canvasGroup;
        private float _fullAlpha = 1f;

        private void Start()
        {
            _targetPosition = _targetWallet.localPosition;
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public void StartMove()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ChangeAnimation());
        }

        private IEnumerator ChangeAnimation()
        {
            _elapsedTime = 0f;
            _canvasGroup.alpha = _fullAlpha;
            _startPosition = Vector3.zero;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                gameObject.transform.localPosition =
                    Vector3.Lerp(_startPosition, _targetPosition, _elapsedTime / _duration);
                yield return null;
            }

            _canvasGroup.alpha = 0;
        }
    }
}