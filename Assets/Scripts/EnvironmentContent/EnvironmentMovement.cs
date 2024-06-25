using System.Collections;
using InitializationContent;
using UnityEngine;

namespace EnvironmentContent
{
    public class EnvironmentMovement : MonoBehaviour
    {
        [SerializeField] private GameObject[] _environments;
        [SerializeField] private Initializator _initializator;

        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float _elapsedTime;
        private float _duration = 1f;
        private float _targetDistanceValue = 500f;
        private Coroutine _coroutine;
        private bool _isActive;

        private void Start()
        {
            _startPosition = _environments[_initializator.Index].transform.position;
            _targetPosition = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z + _targetDistanceValue);
        }

        public void GoAway()
        {
            _targetPosition = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z + _targetDistanceValue);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isActive = false;
            _coroutine = StartCoroutine(Move(_environments[_initializator.Index].transform.position, _targetPosition));
        }

        public void ReturnPosition()
        {
            _environments[_initializator.Index].transform.position = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z - _targetDistanceValue);
            _environments[_initializator.Index].SetActive(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isActive = true;
            _coroutine = StartCoroutine(Move(_environments[_initializator.Index].transform.position, _startPosition));
        }

        private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition)
        {
            _elapsedTime = 0f;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                _environments[_initializator.Index].transform.position = Vector3.Lerp(startPosition, targetPosition, _elapsedTime / _duration);
                yield return null;
            }

            _environments[_initializator.Index].transform.position = targetPosition;
            _environments[_initializator.Index].SetActive(_isActive);
        }
    }
}