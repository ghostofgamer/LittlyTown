using System.Collections;
using UnityEngine;

namespace ItemContent
{
    public class ItemMoving : MonoBehaviour
    {
        [SerializeField] private Item _item;

        private Vector3 _startPosition;
        private Coroutine _coroutineCyclically;
        private float _durationMoveTarget = 1f;
        private float _durationMoveCyclically = 0.3f;
        private float _elapsedTime;
        private float _progress;
        private Vector3 _direction;
        private Vector3 _target;
        private float _distanceFactor = 0.35f;
        private bool _isMoving;

        public void StopMove()
        {
            _isMoving = false;

            if (_coroutineCyclically != null)
            {
                transform.position = _startPosition;
                StopCoroutine(_coroutineCyclically);
            }
        }

        public void MoveCyclically(Vector3 target)
        {
            if (!_item.IsActive)
                return;

            ChangeStartPositionMovement();
            _coroutineCyclically = StartCoroutine(StartMoveCyclically(target));
        }

        public void MoveTarget(Vector3 target)
        {
            ChangeStartPositionMovement();
            StartCoroutine(StartMoveTarget(target));
        }

        private IEnumerator StartMoveCyclically(Vector3 targetItemPosition)
        {
            _isMoving = true;
            
            while (_isMoving)
            {
                _direction = (targetItemPosition - transform.position).normalized;
                _target = transform.position + _direction * _distanceFactor;
                _elapsedTime = 0;

                while (_elapsedTime < _durationMoveTarget)
                {
                    Move(_target,_durationMoveTarget);
                    yield return null;
                }

                transform.position = _startPosition;
                yield return null;
            }
        }

        private IEnumerator StartMoveTarget(Vector3 targetItemPosition)
        {
            _elapsedTime = 0;

            while (_elapsedTime < _durationMoveCyclically)
            {
                Move(targetItemPosition,_durationMoveCyclically);
                yield return null;
            }

            transform.position = targetItemPosition;
            gameObject.SetActive(false);
        }

        private void Move(Vector3 targetPosition,float duration)
        {
            _progress = _elapsedTime / duration;
            transform.position = Vector3.Lerp(_startPosition, targetPosition, _progress);
            _elapsedTime += Time.deltaTime;
        }

        private void ChangeStartPositionMovement()
        {
            _startPosition = transform.position;

            if (_coroutineCyclically != null)
                StopCoroutine(_coroutineCyclically);
        }
    }
}