using System;
using System.Collections;
using UnityEngine;

namespace PossibilitiesContent
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MovementIcon : MonoBehaviour
    {
        [SerializeField] private RectTransform _targetPossibility;

        private float _elapsedTime;
        private float _duration = 0.5f;
        private float _durationJump = 0.165f;
        private float _jumpHeight = 65f;
        private Vector3 _targetPosition;
        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _jumpPosition;
        private Coroutine _coroutine;
        private CanvasGroup _canvasGroup;
        private float _factor = 1f;

        public event Action<int> MovementCompleted;

        private void Start()
        {
            _targetPosition = _targetPossibility.localPosition;
            _jumpPosition = _startPosition + new Vector3(_factor, _factor, 0) * _jumpHeight;
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public void StartMove(int value)
        {
            if (value < 0)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Move(value));
        }

        private IEnumerator Move(int value)
        {
            _elapsedTime = 0f;
            _canvasGroup.alpha = _factor;
            _startPosition = Vector3.zero;
            yield return MoveToPosition(_jumpPosition, _durationJump);
            _startPosition = gameObject.transform.localPosition;
            yield return MoveToPosition(_targetPosition, _duration);
            _canvasGroup.alpha = 0;
            MovementCompleted?.Invoke(value);

            IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
            {
                _elapsedTime = 0f;

                while (_elapsedTime < duration)
                {
                    _elapsedTime += Time.deltaTime;
                    gameObject.transform.localPosition =
                        Vector3.Lerp(_startPosition, targetPosition, _elapsedTime / duration);
                    yield return null;
                }
            }
        }
    }
}