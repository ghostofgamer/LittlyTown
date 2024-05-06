using System.Collections;
using UnityEngine;

namespace UI.Buttons
{
    public class RotationButton : AbstractButton
    {
        [SerializeField] private float _angle;
        [SerializeField] private Transform _environment;

        private float _duration = 1f;
        private float _elapsedTime = 0f;
        private Coroutine _coroutine;
        private Vector3 _target;
        private Quaternion _startPosition;

        protected override void OnClick()
        {
            float currentYRotation = _environment.rotation.eulerAngles.y;
            _target = new Vector3(_environment.rotation.x, currentYRotation + _angle, _environment.rotation.z);
            // _target = new Vector3(_environment.rotation.x, _environment.rotation.y + _angle, _environment.rotation.z);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Turn());
        }

        private IEnumerator Turn()
        {
            _elapsedTime = 0;
            _startPosition = _environment.rotation;

            while (_elapsedTime < _duration)
            {
                _environment.rotation =
                    Quaternion.Slerp(_startPosition, Quaternion.Euler(_target), _elapsedTime / _duration);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            _environment.rotation = Quaternion.Euler(_target);
        }
    }
}