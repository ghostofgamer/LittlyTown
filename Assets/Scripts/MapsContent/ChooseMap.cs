using System;
using System.Collections;
using UnityEngine;

namespace MapsContent
{
    public class ChooseMap : MonoBehaviour
    {
        [SerializeField] private Vector3 _startResetPosition;
        [SerializeField] private bool _isWork = false;

        private float _step = 46;
        private int _currentIndex;
        private float _elapsedTime = 0f;
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
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);
        private float _value;
        private Vector3 _position;
        private float _dragDistance;
        private Vector3 _currentDragPosition;
        
        
        public event Action<int> MapChanged;

        public bool IsWork => _isWork;
        private bool _isSwap;

        private void Update()
        {
            if(transform.position.x<_startResetPosition.x||transform.position.y<_startResetPosition.y)
                transform.position = new Vector3(_startResetPosition.x,_startResetPosition.y,transform.position.z);
        }

        public void SetStartPosition()
        {
            /*_startScrollPosition = transform.position;
            _mouseDownPosition = Input.mousePosition;*/

            if (!_isSwap)
            {
                _startScrollPosition = transform.position;
                _mouseStartPosition = Input.mousePosition;
                _mouseDownPosition = Input.mousePosition;
                _mouseCurrentPosition = Input.mousePosition;
                  _isWork  = true;
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
                Debug.Log("value " + _dragDistance);
            
                _mouseDelta = -(_mouseCurrentPosition.x - _mouseDownPosition.x);

                if (Mathf.Abs(_mouseDelta) > 16f)
                {
                    _value = Mathf.Sign(_mouseCurrentPosition.x - _mouseDownPosition.x) * _sensitivity;
                    _value *= -1;
                    _mouseDownPosition = _mouseCurrentPosition;
                    _position = new Vector3(0, 0, _value);
                    transform.position += _position * 30 * Time.deltaTime;
                }
            }
        }

        public void CheckImpulse()
        {
            if (_isWork)
            {
                if (_dragDistance > 100)
                {
                    if (_currentIndex + (int) Mathf.Sign(-((int) Mathf.Sign(_mouseCurrentPosition.x - _mouseStartPosition.x))) > 10 ||
                        _currentIndex + (int) Mathf.Sign(-((int) Mathf.Sign(_mouseCurrentPosition.x - _mouseStartPosition.x))) < 0)
                    {
                        transform.position = _startScrollPosition;
                        _isWork = false;
                        _isSwap = false;
                        return;
                    }
                    
                    _isWork = false;
                    ChangeMap(-((int) Mathf.Sign(_mouseCurrentPosition.x - _mouseStartPosition.x)));
                }
                else
                {
                    transform.position = _startScrollPosition;
                    _isWork = false;
                    _isSwap = false;
                }
                
                
                /*if (Mathf.Abs(_mouseDelta) > 7)
                {
                    if (_currentIndex + (int) Mathf.Sign(_mouseDelta) > 10 ||
                        _currentIndex + (int) Mathf.Sign(_mouseDelta) < 0)
                    {
                        transform.position = _startScrollPosition;
                        _isWork = false;
                        _isSwap = false;
                        return;
                    }
                    
                    _isWork = false;
                    ChangeMap((int) Mathf.Sign(_mouseDelta));
                    return;
                }

                transform.position = _startScrollPosition;
                _isWork = false;
                _isSwap = false;*/
            }
        }

        public void ChangeMap(int index)
        {
            _currentIndex += index;

            if (_currentIndex > 10 || _currentIndex < 0)
            {
                _currentIndex = Mathf.Clamp(_currentIndex, 0, 10);
                return;
            }

            MapChanged?.Invoke(_currentIndex);
            _currentStep = _step * index;
            _currentZ += _currentStep;
            _target = new Vector3(_startPosition.x, _startPosition.y, _currentZ);
            StartCoroutine(MapMove());
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

        /*public void StartWork()
        {
            StartCoroutine(EnableWork());
        }

        private IEnumerator EnableWork()
        {
            yield return _waitForSeconds;
            _isWork = true;
        }

        public void StopWork()
        {
            _isWork = false;
        }*/
    }
}