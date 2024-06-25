using CameraContent;
using InitializationContent;
using Keeper;
using MapsContent;
using SaveAndLoad;
using UnityEngine;

namespace UI.Screens
{
    public class ChooseMapScreen : AbstractScreen
    {
        private const string LastActiveMap = "LastActiveMap";
        private const string ActiveMap = "ActiveMap";
        private const string OpenMap = "OpenMap";
    
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private GameObject _startButton;
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private GameObject _restartButton;
        [SerializeField] private Load _load;
        [SerializeField] private Save _save;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private ChooseMap _chooseMap;
        [SerializeField] private InputChooseMap _inputChooseMap;
        [SerializeField] private GameObject _mapInformation;
        [SerializeField] private GameObject[] _mapInformations;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private MapActivator _mapActivator;
        [SerializeField] private MapsInfo _mapsInfo;
        [SerializeField] private GameObject _openMapContent;
        [SerializeField] private GameObject _closeMapContent;

        private int _startValue = 0;
        private int _currentValue;
        private int _openValue;
        private int _value;

        private void OnEnable()
        {
            _initializator.IndexChanged += ChangeActivationButton;
        }

        private void OnDisable()
        {
            _initializator.IndexChanged -= ChangeActivationButton;
        }

        private void Start()
        {
            _save.SetData(OpenMap + _startValue, 1);
        }

        public override void Open()
        {
            ChangeActivationButton();
            base.Open();
            _itemKeeper.SwitchOff();
            _cameraMovement.ZoomOut();
            // _cameraMovement.ZoomIn();
            _inputChooseMap.enabled = true;
            _chooseMap.enabled = true;
            // _chooseMap.StartWork();
            _inputChooseMap.StartWork();
            _mapsInfo.OnActivatedInfo(_initializator.Index);
            _mapInformation.SetActive(true);
            _mapInformations[_initializator.Index].SetActive(true);
            _mapActivator.ActivateAllMaps();
        }

        public override void Close()
        {
            // _chooseMap.StopWork();
            // _inputChooseMap.enabled = false;
            _inputChooseMap.StopWork();
            _chooseMap.enabled = false;
            base.Close();
            // _cameraMovement.ResetZoom();
            _mapInformation.SetActive(false);
            _mapActivator.ChangeActivityMaps();
        }

        public void ChangeActivationButton()
        {
            _openValue = _load.Get(OpenMap + _initializator.Index, _startValue);

            if (_openValue == _startValue)
            {
                _openMapContent.SetActive(false);
                _closeMapContent.SetActive(true);
                return;
            }

            _openMapContent.SetActive(true);
            _closeMapContent.SetActive(false);
        
            _value = _load.Get(ActiveMap + _initializator.Index, _startValue);

            if (_value > _startValue)
            {
                _startButton.SetActive(false);
                _continueButton.SetActive(true);
                _restartButton.SetActive(true);
            }
            else
            {
                _startButton.SetActive(true);
                _continueButton.SetActive(false);
                _restartButton.SetActive(false);
            }
        }
    }
}