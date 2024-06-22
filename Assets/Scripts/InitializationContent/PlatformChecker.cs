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
        private int _perspectiveValueMobile = 49;
        private int _perspectiveValuePc = 30;
        private int _orthographicValueMobile = 9;
        private int _orthographicValuePc = 6;

        private void Awake()
        {
            _currentIndex = Application.isMobilePlatform ? _mobileIndex : _pcIndex;

            if (_currentIndex != _mobileIndex)
                _cameraMovement.Init(_perspectiveValueMobile, _orthographicValueMobile);
            else
                _cameraMovement.Init(_perspectiveValuePc, _orthographicValuePc);
        }
    }
}