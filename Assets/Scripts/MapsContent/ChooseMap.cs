using System;
using System.Collections;
using UnityEngine;

namespace MapsContent
{
    public class ChooseMap : MonoBehaviour
    {
        [SerializeField] private Vector3 _startResetPosition;
        [SerializeField] private bool _isWork;

        private float _step = 46;
        private int _currentIndex;
        private float _elapsedTime;
        private float _duration = 1f;
        private Vector3 _target;
        private Vector3 _currentPosition;
        private Vector3 _startPosition;
        private float _currentStep;
        private float _currentZ;
        private Vector3 _mouseDownPosition;
        private Vector3 _mouseStartPosition;
        private Vector3 _mouseCurrentPosition;
        private float _sensitivity = 1f;
        private float _mouseDelta;
        private Vector3 _startScrollPosition;
        private float _value;
        private Vector3 _position;
        private float _dragDistance;
        private Vector3 _currentDragPosition;
        private bool _isSwap;
        private float _speed = 30f;
        private float _factor = 1f;
        private float _targetDelta = 16f;
        private float _targetDistance = 100f;
        private int _maxIndex = 10;
        private int _minIndex = 0;

        public event Action<int> MapChanged;

        private void Update()
        {
            if (transform.position.x < _startResetPosition.x || transform.position.y < _startResetPosition.y)
                transform.position = new Vector3(_startResetPosition.x, _startResetPosition.y, transform.position.z);
        }

        public void SetStartPosition()
        {
            if (!_isSwap)
            {
                _startScrollPosition = transform.position;
                _mouseStartPosition = Input.mousePosition;
                _mouseDownPosition = Input.mousePosition;
                _mouseCurrentPosition = Input.mousePosition;
                _isWork = true;
                _dragDistance = 0;
            }

            _isSwap = true;
        }

        public void MoveMapTouching()
        {
            if (_isWork)
            {
                _mouseCurrentPosition = Input.mousePosition;
                _dragDistance = Vector3.Distance(_mouseStartPosition, _mouseCurrentPosition);
                _mouseDelta = -(_mouseCurrentPosition.x - _mouseDownPosition.x);

                if (Mathf.Abs(_mouseDelta) > _targetDelta)
                {
                    _value = Mathf.Sign(_mouseCurrentPosition.x - _mouseDownPosition.x) * _sensitivity;
                    _value *= -_factor;
                    _mouseDownPosition = _mouseCurrentPosition;
                    _position = new Vector3(0, 0, _value);
                    transform.position += _position * _speed * Time.deltaTime;
                }
            }
        }

        public void CheckDragDistance()
        {
            if (_isWork)
            {
                if (_dragDistance > _targetDistance)
                {
                    if (
                        _currentIndex +
                        (int)Mathf.Sign(-((int)Mathf.Sign(_mouseCurrentPosition.x - _mouseStartPosition.x))) >
                        _maxIndex ||
                        _currentIndex +
                        (int)Mathf.Sign(-((int)Mathf.Sign(_mouseCurrentPosition.x - _mouseStartPosition.x))) <
                        _minIndex)
                    {
                        ResetValue();
                        return;
                    }

                    _isWork = false;
                    ChangeMap(-((int)Mathf.Sign(_mouseCurrentPosition.x - _mouseStartPosition.x)));
                }
                else
                {
                    ResetValue();
                }
            }
        }

        public void ChangeMap(int index)
        {
            _currentIndex += index;

            if (_currentIndex > _maxIndex || _currentIndex < _minIndex)
            {
                _currentIndex = Mathf.Clamp(_currentIndex, _minIndex, _maxIndex);
                return;
            }

            MapChanged?.Invoke(_currentIndex);
            _currentStep = _step * index;
            _currentZ += _currentStep;
            _target = new Vector3(_startPosition.x, _startPosition.y, _currentZ);
            StartCoroutine(MapMove());
        }

        public void SetPosition(int index)
        {
            _currentIndex = index;
            _startPosition = transform.position;
            _currentZ = _startPosition.z;
            _currentStep = _step * index;
            _currentZ += _currentStep;
            _target = new Vector3(_startPosition.x, _startPosition.y, _currentZ);
            transform.position = _target;
        }

        public void ResetMapPosition()
        {
            _currentIndex = 0;
            MapChanged?.Invoke(_currentIndex);
            transform.position = _startResetPosition;
            _currentPosition = _startResetPosition;
            _currentZ = _currentPosition.z;
        }

        private void ResetValue()
        {
            transform.position = _startScrollPosition;
            _isWork = false;
            _isSwap = false;
        }

        private IEnumerator MapMove()
        {
            _elapsedTime = 0;
            _currentPosition = transform.position;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(_currentPosition, _target, _elapsedTime / _duration);
                yield return null;
            }

            transform.position = _target;
            _isSwap = false;
        }
    }
}