using Dragger;
using UnityEngine;

namespace CameraContent
{
    public class CameraScrolling : MonoBehaviour
    {
        private const string MouseScrollWheel = "Mouse ScrollWheel";

        [SerializeField] private Camera _camera;
        [SerializeField] private InputItemDragger _inputItemDragger;
        [SerializeField] private ItemThrower _itemThrower;
        [SerializeField] private ItemDragger _itemDragger;

        private float _minFov = 15f;
        private float _maxFov = 75f;
        private float _minOrthographicSize = 1f;
        private float _maxOrthographicSize = 23f;
        private float _mouseScrollSensitivity = 10f;
        private float _currentFov;
        private float _currentSize;
        private float _deltaMagnitude;
        private float _baseSizeOrFOV;
        private float _baseDistance;
        private float _currentDistance;
        private float _rate;
        private float _fov;
        private float _size;
        private float _scroll;
        private int _maxTouches = 2;
        private int _minTouches = 1;

        private void Start()
        {
            enabled = false;
            _currentFov = _camera.fieldOfView;
        }

        private void Update()
        {
            _scroll = Input.GetAxis(MouseScrollWheel);

            if (_scroll != 0)
            {
                if (!_camera.orthographic)
                {
                    _currentFov -= _scroll * _mouseScrollSensitivity;
                    _currentFov = Mathf.Clamp(_currentFov, _minFov, _maxFov);
                    _camera.fieldOfView = _currentFov;
                }
                else
                {
                    _currentSize -= _scroll * _mouseScrollSensitivity;
                    _currentSize = Mathf.Clamp(_currentSize, _minOrthographicSize, _maxOrthographicSize);
                    _camera.orthographicSize = _currentSize;
                }
            }

            if (Input.touches.Length == _maxTouches)
            {
                _itemThrower.ReturnPosition();
                _itemDragger.DisableSelected();
                _inputItemDragger.enabled = false;

                switch (Input.touches[1].phase)
                {
                    case TouchPhase.Began:
                        _baseSizeOrFOV = _camera.orthographic ? _camera.orthographicSize : _camera.fieldOfView;
                        _baseDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                        break;

                    case TouchPhase.Moved:
                        _currentDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                        _rate = _baseDistance / _currentDistance;

                        if (_camera.orthographic)
                        {
                            _size = _baseSizeOrFOV * _rate;
                            _camera.orthographicSize = Mathf.Clamp(_size, _minOrthographicSize, _maxOrthographicSize);
                        }
                        else
                        {
                            _fov = _baseSizeOrFOV * _rate;
                            _camera.fieldOfView = Mathf.Clamp(_fov, _minFov, _maxFov);
                        }

                        break;
                }
            }

            if (Input.touches.Length < _minTouches)
            {
                _inputItemDragger.enabled = true;
            }
        }
    }
}