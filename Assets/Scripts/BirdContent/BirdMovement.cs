using System;
using UnityEngine;

namespace BirdContent
{
    public class BirdMovement : MonoBehaviour
    {
        [SerializeField] private BirdSpawner _birdSpawner;

        private Transform[] _points;
        private int _currentPoint;
        private Transform _targetPosition;
        private bool _isFly;
        private float _speed = 3f;
        private float _rotateSpeed = 3f;
        private Vector3 _targetDirection;
        private Quaternion _targetRotation;

        private void Update()
        {
            if (!_isFly)
                return;

            transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, Time.deltaTime * _speed);
            _targetDirection = _targetPosition.position - transform.position;
            _targetDirection.Normalize();
            _targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_targetDirection), _rotateSpeed * Time.deltaTime);
            transform.rotation = _targetRotation;

            if (transform.position == _targetPosition.position)
                NextPoint();
        }

        public void Init(Transform path)
        {
            _points = new Transform[path.childCount];

            for (int i = 0; i < _points.Length; i++)
                _points[i] = path.GetChild(i);

            _targetPosition = _points[0];
            _currentPoint = 0;
            _isFly = true;
        }

        private void NextPoint()
        {
            _currentPoint++;

            if (_currentPoint >= _points.Length)
            {
                _isFly = false;
                _birdSpawner.FinishBird();
                gameObject.SetActive(false);
            }
            else
            {
                _targetPosition = _points[_currentPoint];
            }
        }
    }
}