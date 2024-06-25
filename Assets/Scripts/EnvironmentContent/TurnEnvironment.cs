using System.Collections;
using InitializationContent;
using MergeContent;
using UnityEngine;

namespace EnvironmentContent
{
    public class TurnEnvironment : MonoBehaviour
    {
        [SerializeField] private GameObject[] _environments;
        [SerializeField] private GameObject _container;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private LookMerger _lookMerger;

        private GameObject _currentEnvironment;
        private Vector3 _target;
        private Quaternion _startRotation;
        private Coroutine _coroutineRotate;
        private float _elapsedTime;
        private float _durationReturn = 0.15f;
        private bool _isEndGameRotate;
        private float _speed = 10;
        private float _angle;
        private int _step = 90;
        private bool _isRotating;

        private void Update()
        {
            if (_isEndGameRotate)
                _environments[_initializator.Index].transform.Rotate(0, _speed * Time.deltaTime, 0);

            if (_isRotating)
                UpdateRotation();
        }

        public void ChangeRotation(int index)
        {
            _lookMerger.StopMoveMatch();
            _angle += _step * index;
            _target = new Vector3(_environments[_initializator.Index].transform.rotation.x, _angle,
                _environments[_initializator.Index].transform.rotation.z);
            _isRotating = true;
        }

        public void SetEnvironment(GameObject environment)
        {
            _currentEnvironment = environment;
        }

        public void StartRotate()
        {
            _isEndGameRotate = true;
        }

        public void StopRotate()
        {
            _isEndGameRotate = false;

            if (_coroutineRotate != null)
                StopCoroutine(_coroutineRotate);

            _coroutineRotate = StartCoroutine(ReturnRotate());
        }

        private void UpdateRotation()
        {
            _currentEnvironment.transform.rotation = Quaternion.Lerp(_currentEnvironment.transform.rotation,
                Quaternion.Euler(_target), _speed * Time.deltaTime);

            if (_currentEnvironment.transform.rotation == Quaternion.Euler(_target))
            {
                _currentEnvironment.transform.rotation = Quaternion.Euler(_target);
                _isRotating = false;
            }
        }

        private IEnumerator ReturnRotate()
        {
            _angle = 0f;
            _elapsedTime = 0;
            _startRotation = _environments[_initializator.Index].transform.rotation;

            while (_elapsedTime < _durationReturn)
            {
                _environments[_initializator.Index].transform.rotation =
                    Quaternion.Slerp(_startRotation, Quaternion.identity, _elapsedTime / _durationReturn);
                _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            _environments[_initializator.Index].transform.rotation = Quaternion.identity;
            _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
        }
    }
}