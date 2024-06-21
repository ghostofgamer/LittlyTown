using System.Collections;
using UI.Buttons;
using UnityEngine;

namespace CameraContent
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private OpenButton _openButton;

        private float _standardPerspectiveFOV = 30f;
        private float _perspectiveZoomUpValue = 70f;
        private float _perspectiveZoomDownValue = 50;
        private float _standardOrthographicSize = 6f;
        private float _orthographicSizeZoomIn = 13f;
        private float _orthographicSizeZoomOut = 10f;
        private float _elapsedTime;
        private float _duration = 1.5f;
        private float _currentFOVValue;
        private float _currentSizeValue;
        private Coroutine _coroutine;

        public void Init(int fovPerspective, int sizeOrthographic)
        { 
            _standardPerspectiveFOV = fovPerspective;
            _camera.fieldOfView = _standardPerspectiveFOV;
            _standardOrthographicSize = sizeOrthographic;
            _camera.orthographicSize = _standardOrthographicSize;
        }

        public void ZoomIn()
        {
            StartCoroutine(_perspectiveZoomUpValue,_orthographicSizeZoomIn);
        }

        public void ZoomOut()
        {
            StartCoroutine(_perspectiveZoomDownValue,_orthographicSizeZoomOut);
        }

        public void ResetZoom()
        {
            StartCoroutine(_standardPerspectiveFOV,_standardOrthographicSize);
        }

        private void StartCoroutine(float perspectiveFovValue,float orthographicSizeValue)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(!_camera.orthographic ? StartChangeZoom(perspectiveFovValue) : StartChangeZoom(orthographicSizeValue));
        }
        
        private IEnumerator StartChangeZoom(float targetValue)
        {
            _openButton.enabled = false;
            _elapsedTime = 0;
            _currentFOVValue = _camera.fieldOfView;
            _currentSizeValue = _camera.orthographicSize;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;

                if (!_camera.orthographic)
                    _camera.fieldOfView = Mathf.Lerp(_currentFOVValue, targetValue, _elapsedTime / _duration);
                else
                    _camera.orthographicSize = Mathf.Lerp(_currentSizeValue, targetValue, _elapsedTime / _duration);

                yield return null;
            }

            _openButton.enabled = true;
        }
    }
}