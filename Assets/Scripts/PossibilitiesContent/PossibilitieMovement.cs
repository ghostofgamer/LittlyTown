using System;
using System.Collections;
using UnityEngine;

namespace PossibilitiesContent
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PossibilitieMovement : MonoBehaviour
    {
        [SerializeField] private RectTransform _targetPossibilitie;

        private float _elapsedTime;
        private float _duration = 0.5f;
        private float _durationJump = 0.165f;
        private float _jumpHeight = 65f;
        private Vector3 _targetPosition;
        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _jumpPosition;
        private Coroutine _coroutine;
        private CanvasGroup _canvasGroup;
        
        public event Action<int> MovementCompleted;

        private void Start()
        {
            _targetPosition = _targetPossibilitie.localPosition;
            // Debug.Log("PossibilitytargetPosition " + _targetPosition);
            // _jumpPosition = _startPosition + Vector3.up * _jumpHeight;
            _jumpPosition = _startPosition + new Vector3(1,1,0) * _jumpHeight;
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public void StartMove(int value)
        {
            if (value < 0)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ChangeAnimation(value));
        }

        private IEnumerator ChangeAnimation(int value)
        {
            _elapsedTime = 0f;
            _canvasGroup.alpha = 1;
            _startPosition = Vector3.zero;
            
            while (_elapsedTime < _durationJump)
            {
                _elapsedTime += Time.deltaTime;
                gameObject.transform.localPosition =
                    Vector3.Lerp(_startPosition, _jumpPosition, _elapsedTime / _durationJump);
                yield return null;
            }

            _startPosition = gameObject.transform.localPosition;
            _elapsedTime = 0f;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                gameObject.transform.localPosition =
                    Vector3.Lerp(_startPosition, _targetPosition, _elapsedTime / _duration);
                yield return null;
            }

            _canvasGroup.alpha = 0;
            MovementCompleted?.Invoke(value);
        }
    }
}