using System.Collections;
using CameraContent;
using InitializationContent;
using SaveAndLoad;
using UI.Buttons;
using UnityEngine;

namespace UI.Screens
{
    public class FirstScreen : AbstractScreen
    {
        private const string LastActiveMap = "LastActiveMap";
        private const string ActiveMap = "ActiveMap";
        private const string Map = "Map";

        [SerializeField] private Load _load;
        [SerializeField] private ContinueButton _continueButton;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private CameraMovement _cameraMovement;

        private int _startValue = 0;
        private int _currentValue;
        private int _currentValueActiveMap;
        private int _lastValue;
        private int _currentMapValue;
        private bool _value;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1.65f); 
        
        private void Start()
        {
            _lastValue = _load.Get(LastActiveMap, _startValue);
            _currentMapValue = _load.Get(Map, _startValue);
            _currentValue = _load.Get(LastActiveMap, _startValue);
            _currentValueActiveMap = _load.Get(ActiveMap + _currentMapValue, _startValue);
            _value = _currentValue > 0 && _currentValueActiveMap > 0;
            _continueButton.gameObject.SetActive(_value);

            if (_lastValue > 0)
                StartCoroutine(SmoothOpen());
        }

        public override void Open()
        {
            base.Open();
            _currentValue = _load.Get(LastActiveMap, _startValue);
            _currentValueActiveMap = _load.Get(ActiveMap + _initializator.Index, _startValue);
            _value = _currentValue > 0 && _currentValueActiveMap > 0;
            _continueButton.gameObject.SetActive(_value);
            _cameraMovement.ResetZoom();
        }

        private IEnumerator SmoothOpen()
        {
            yield return _waitForSeconds;
            Open();
        }

        private void InitValue()
        {
            _currentValue = _load.Get(LastActiveMap, _startValue);
            _currentValueActiveMap = _load.Get(ActiveMap + _initializator.Index, _startValue);
            _value = _currentValue > 0 && _currentValueActiveMap > 0;
            _continueButton.gameObject.SetActive(_value); 
        }
    }
}