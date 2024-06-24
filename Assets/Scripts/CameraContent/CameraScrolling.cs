using UnityEngine;

namespace CameraContent
{
    public class CameraScrolling : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private float _minFov = 30f;
        private float _maxFov = 75f;
        private float _minOrthographicSize = 6f;
        private float _maxOrthographicSize = 23f;
        private float _mouseScrollSensitivity = 10f;
        private float _touchScrollSensitivity = 0.25f;
        private float _currentFov;
        private float _currentSize;
        private float _deltaMagnitude;
        
        private void Start()
        {
            enabled = false;
            _currentFov = _camera.fieldOfView;
        }

        private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                if (!_camera.orthographic)
                {
                    _currentFov -= scroll * _mouseScrollSensitivity;
                    _currentFov = Mathf.Clamp(_currentFov, _minFov, _maxFov);
                    _camera.fieldOfView = _currentFov;
                }
                else
                {
                    _currentSize -= scroll * _mouseScrollSensitivity;
                    _currentSize = Mathf.Clamp(_currentSize, _minOrthographicSize, _maxOrthographicSize);
                    _camera.orthographicSize = _currentSize;
                }
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                float prevTouchDeltaMag = Vector2.Distance(touchZeroPrevPos, touchOnePrevPos);
                float touchDeltaMag = Vector2.Distance(touchZero.position, touchOne.position);
                _deltaMagnitude = prevTouchDeltaMag - touchDeltaMag;
                Camera cam = Camera.main;

                if (cam.orthographic)
                {
                    cam.orthographicSize -= _deltaMagnitude * _touchScrollSensitivity;
                    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, _minOrthographicSize, _maxOrthographicSize);
                }
                else
                {
                    cam.fieldOfView -= _deltaMagnitude * _touchScrollSensitivity;
                    cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, _minFov, _maxFov);
                }
            }
            else
            {
                _deltaMagnitude = 0;
            }
            /*if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;
                float distanceTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                float currentDistanceTouch = (touchZero.position - touchOne.position).magnitude;
                float difference = currentDistanceTouch - distanceTouch;
                Zoom(difference * 0.01f);
            }*/

            /*if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
                float prevMagnitude = Vector2.Distance(touch1PrevPos, touch2PrevPos);
                float currentMagnitude = Vector2.Distance(touch1.position, touch2.position);
                float deltaMagnitude = currentMagnitude - prevMagnitude;
                _currentFov -= deltaMagnitude * _touchScrollSensitivity;
                _currentFov = Mathf.Clamp(_currentFov, _minFov, _maxFov);
                _camera.fieldOfView = _currentFov;

                if (!_camera.orthographic)
                {
                    _currentFov -= deltaMagnitude * _touchScrollSensitivity;
                    _currentFov = Mathf.Clamp(_currentFov, _minFov, _maxFov);
                    _camera.fieldOfView = _currentFov;
                }
                else
                {
                    _currentSize -= deltaMagnitude * _touchScrollSensitivity;
                    _currentSize = Mathf.Clamp(_currentSize, _minOrthographicSize, _maxOrthographicSize);
                    _camera.orthographicSize = _currentSize;
                }
            }*/
        }

        /*private void Zoom(float increment)
        {
            if (Camera.main.orthographic)
            {
                // Camera.main.orthographicSize -= deltaMagnitude * _touchScrollSensitivity;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment,
                    _minOrthographicSize,
                    _maxOrthographicSize);
            }
            else
            {
                // Camera.main.fieldOfView -= deltaMagnitude * _touchScrollSensitivity;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment, _minFov, _maxFov);
            }
        }*/
    }
}