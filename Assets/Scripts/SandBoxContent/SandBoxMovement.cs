using System.Collections;
using UnityEngine;

namespace SandBoxContent
{
    public class SandBoxMovement : MonoBehaviour
    {
        [SerializeField] private GameObject _environment;
        [SerializeField] private Vector3 _startPosition;

        private Vector3 _targetPosition;
        private Coroutine _coroutine;
        private bool _isActive;
        private float _timeElapsed;
        private float _durationTime = 1f;
        private float _step = 500f;

        public void GoOffScreen()
        {
            _targetPosition = new Vector3(
                _environment.transform.position.x,
                _environment.transform.position.y,
                _environment.transform.position.z + _step);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isActive = false;
            _coroutine = StartCoroutine(Move(_environment.transform.position, _targetPosition));
        }

        public void ReturnPosition()
        {
            _environment.transform.position = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z - _step);
            _environment.SetActive(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isActive = true;
            _coroutine = StartCoroutine(Move(_environment.transform.position, _startPosition));
        }

        private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition)
        {
            _timeElapsed = 0f;

            while (_timeElapsed < _durationTime)
            {
                _timeElapsed += Time.deltaTime;
                _environment.transform.position =
                    Vector3.Lerp(startPosition, targetPosition, _timeElapsed / _durationTime);
                yield return null;
            }

            _environment.transform.position = targetPosition;
            _environment.SetActive(_isActive);
        }
    }
}