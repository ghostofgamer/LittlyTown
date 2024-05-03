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
        private float _elapsedTime = 0f;
        private float _progress;
        
        public void StopCoroutine()
        {
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

            _startPosition = transform.position;

            if (_coroutineCyclically != null)
                StopCoroutine(_coroutineCyclically);

            _coroutineCyclically = StartCoroutine(MoveCyclicallyStart(target));
        }

        public void MoveTarget(Vector3 target)
        {
            _startPosition = transform.position;

            if (_coroutineCyclically != null)
                StopCoroutine(_coroutineCyclically);

            StartCoroutine(MoveTargetStart(target));
        }

        private IEnumerator MoveCyclicallyStart(Vector3 targetItemPosition)
        {
            while (true)
            {
                Vector3 direction = (targetItemPosition - transform.position).normalized;
                Vector3 target = transform.position + direction;
                _elapsedTime = 0;

                while (_elapsedTime < _durationMoveTarget)
                {
                    _progress = _elapsedTime / _durationMoveTarget;
                    transform.position = Vector3.Lerp(_startPosition, target, _progress);
                    _elapsedTime += Time.deltaTime;
                    yield return null;
                }

                transform.position = _startPosition;
                yield return null;
            }
        }

        private IEnumerator MoveTargetStart(Vector3 targetItemPosition)
        {
            _elapsedTime = 0;

            while (_elapsedTime < _durationMoveCyclically)
            {
                _progress = _elapsedTime / _durationMoveCyclically;
                transform.position = Vector3.Lerp(_startPosition, targetItemPosition, _progress);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetItemPosition;
            gameObject.SetActive(false);
        }
    }
}