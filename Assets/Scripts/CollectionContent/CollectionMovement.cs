using System;
using System.Collections;
using UnityEngine;

namespace CollectionContent
{
    public class CollectionMovement : MonoBehaviour
    {
        private int _currentIndex;
        private float _stepX = -13.63f;
        private float _stepZ = 13.63f;
        private int _index;
        private float _elapsedTime = 0f;
        private float _duration = 0.5f;
        private Vector3 _target;
        private Vector3 _currentPosition;
        private Vector3 _startPosition;
        private float _currentStepX;
        private float _currentStepZ;
        private float _currentZ;
        private float _currentX;
        private float _startX;
        private float _startZ;
        private float _endX;
        private float _endZ;
        private int _maxIndex;
        private int _minIndex = 0;
        private Coroutine _coroutine;

        public event Action<int> PositionScrolled;

        private void Start()
        {
            Initialization();
        }

        public void ChangeCurrentIndexItem(int index)
        {
            _currentIndex += index;

            if (_currentIndex < 0)
            {
                RewindCollection(_maxIndex, _endZ, _endX);
                return;
            }

            if (_currentIndex > _maxIndex)
            {
                RewindCollection(_minIndex, _startZ, _startX);
                return;
            }

            PositionScrolled?.Invoke(_currentIndex);
            _currentStepX = _stepX * index;
            _currentStepZ = _stepZ * index;
            _currentZ += _currentStepZ;
            _currentX += _currentStepX;
            _target = new Vector3(_currentX, _startPosition.y, _currentZ);
            StartCollectionMove();
        }

        private void RewindCollection(int index, float currentZ, float currentX)
        {
            _currentIndex = index;
            PositionScrolled?.Invoke(_currentIndex);
            _currentZ = currentZ;
            _currentX = currentX;
            _target = new Vector3(_currentX, _startPosition.y, _currentZ);
            StartCollectionMove();
        }

        private void StartCollectionMove()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(CollectionMove());
        }

        private IEnumerator CollectionMove()
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
        }

        private void Initialization()
        {
            _maxIndex = gameObject.transform.childCount - 1;
            _startPosition = transform.position;
            _currentZ = _startPosition.z;
            _currentX = _startPosition.x;
            _startX = _startPosition.x;
            _startZ = _startPosition.z;
            _endX = _startX + _stepX * _maxIndex;
            _endZ = _startZ + _stepZ * _maxIndex;
            PositionScrolled?.Invoke(_currentIndex);
        }
    }
}