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
        private int _saveValue = 1;
        private int _openValue;
        private int _value;

        private void OnEnable()
        {
            _initializator.IndexChanged += OnChangeActivationButton;
        }

        private void OnDisable()
        {
            _initializator.IndexChanged -= OnChangeActivationButton;
        }

        private void Start()
        {
            _save.SetData(OpenMap + _startValue, _saveValue);
        }

        public override void OnOpen()
        {
            OnChangeActivationButton();
            base.OnOpen();
            _itemKeeper.SwitchOff();
            _cameraMovement.ZoomOut();
            _inputChooseMap.enabled = true;
            _chooseMap.enabled = true;
            _inputChooseMap.StartWork();
            _mapsInfo.OnActivatedInfo(_initializator.Index);
            _mapInformation.SetActive(true);
            _mapInformations[_initializator.Index].SetActive(true);
            _mapActivator.ActivateAllMaps();
        }

        public override void Close()
        {
            _inputChooseMap.StopWork();
            _chooseMap.enabled = false;
            base.Close();
            _mapInformation.SetActive(false);
            _mapActivator.ChangeActivityMaps();
        }

        public void OnChangeActivationButton()
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