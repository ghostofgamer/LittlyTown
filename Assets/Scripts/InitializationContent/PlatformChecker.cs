using CameraContent;
using UnityEngine;

namespace InitializationContent
{
    public class PlatformChecker : MonoBehaviour
    {
        [SerializeField] private CameraMovement _cameraMovement;

        private int _pcIndex = 0;
        private int _mobileIndex = 1;
        private int _currentIndex;
        private int _perspectiveValueMobile = 59;
        private int _perspectiveZoomDownValueMobile = 56;
        private int _perspectiveZoomUpValueMobile = 70;
        private int _perspectiveValuePc = 30;
        private int _perspectiveZoomDownValuePc = 46;
        private int _perspectiveZoomUpValuePc = 60;
        private int _orthographicValueMobile = 12;
        private int _orthographicSizeZoomInMobile = 15;
        private int _orthographicSizeZoomOutMobile = 12;
        private int _orthographicValuePc = 6;
        private int _orthographicSizeZoomOutPc = 8;
        private int _orthographicSizeZoomInPc = 11;

        private void Awake()
        {
            _currentIndex = Application.isMobilePlatform ? _mobileIndex : _pcIndex;

            if (_currentIndex == _mobileIndex)
                _cameraMovement.Init(_perspectiveValueMobile, _orthographicValueMobile,
                    _perspectiveZoomDownValueMobile, _orthographicSizeZoomOutMobile, _perspectiveZoomUpValueMobile,
                    _orthographicSizeZoomInMobile);
            else
                _cameraMovement.Init(_perspectiveValuePc, _orthographicValuePc, _perspectiveZoomDownValuePc,
                    _orthographicSizeZoomOutPc, _perspectiveZoomUpValuePc, _orthographicSizeZoomInPc);
        }
    }
}