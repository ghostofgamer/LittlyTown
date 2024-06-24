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
        private float _perspectiveZoomDownValue = 56;
        private float _standardOrthographicSize = 6f;
        private float _orthographicSizeZoomIn = 13f;
        private float _orthographicSizeZoomOut = 10f;
        private float _elapsedTime;
        private float _duration = 3f;
        private float _currentFOVValue;
        private float _currentSizeValue;
        private Coroutine _coroutine;
        private float _cameraPositionGameSceneY = 24f;
        private float _cameraPositionFirstSceneY = 22.1f;
        private float _cameraPositionCollectionSceneY = 22.5f;
        private float _cameraPositionChooseSceneSceneY = 27.1f;
        
        public float StandardOrthographicSize => _standardOrthographicSize;

        public void Init(int fovPerspective, int sizeOrthographic, int perspectiveZoomDownValue,
            int orthographicSizeZoomOut, int perspectiveZoomUpValue, int orthographicSizeZoomIn)
        {
            _standardPerspectiveFOV = fovPerspective;
            _camera.fieldOfView = _standardPerspectiveFOV;
            _standardOrthographicSize = sizeOrthographic;
            _camera.orthographicSize = _standardOrthographicSize;
            _perspectiveZoomDownValue = perspectiveZoomDownValue;
            _orthographicSizeZoomOut = orthographicSizeZoomOut;
            _perspectiveZoomUpValue = perspectiveZoomUpValue;
            _orthographicSizeZoomIn = orthographicSizeZoomIn;
        }

        public void ZoomGameScene()
        {
            StartCoroutine(50, 10,_cameraPositionGameSceneY);
        }
        
        public void ZoomIn()
        {
            StartCoroutine(_perspectiveZoomUpValue, _orthographicSizeZoomIn,_cameraPositionCollectionSceneY);
        }

        public void ZoomOut()
        {
            StartCoroutine(_perspectiveZoomDownValue, _orthographicSizeZoomOut,_cameraPositionChooseSceneSceneY);
        }

        public void ResetZoom()
        {
            StartCoroutine(_standardPerspectiveFOV, _standardOrthographicSize,_cameraPositionFirstSceneY);
        }

        private void StartCoroutine(float perspectiveFovValue, float orthographicSizeValue,float positionY)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(!_camera.orthographic
                ? StartChangeZoom(perspectiveFovValue,positionY)
                : StartChangeZoom(orthographicSizeValue,positionY));
        }

        private IEnumerator StartChangeZoom(float targetValue,float positionY)
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

                Vector3 startPosition = _camera.transform.position;
                Vector3 targetPosition = new Vector3(startPosition.x,positionY,startPosition.z);
                _camera.transform.position = Vector3.Lerp(startPosition,targetPosition,_elapsedTime / _duration);
                
                yield return null;
            }

            _openButton.enabled = true;
        }
    }
}