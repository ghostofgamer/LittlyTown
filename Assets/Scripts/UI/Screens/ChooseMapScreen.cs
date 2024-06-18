using System;
using Dragger;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChooseMapScreen : AbstractScreen
{
    private const string LastActiveMap = "LastActiveMap";
    private const string ActiveMap = "ActiveMap";
    private const string OpenMap = "OpenMap";

    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private Load _load;
    [SerializeField] private Save _save;
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private GameObject _mapInformation;
    [SerializeField] private GameObject[] _mapInformations;
    [SerializeField] private Initializator _initializator;
    [SerializeField] private MapActivator _mapActivator;
    [SerializeField] private MapsInfo _mapsInfo;
    [SerializeField] private GameObject _openMapContent;
    [SerializeField] private GameObject _closeMapContent;

    private int _startValue = 0;
    private int _currentValue;

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
        // CheckActivation();
        // _inputItemDragger.enabled = false;
        base.Open();
        _itemDragger.SwitchOff();
        _cameraMovement.ZoomOut();
        _chooseMap.enabled = true;
        _chooseMap.StartWork();
        _mapsInfo.ActivatedInfo(_initializator.Index);
        _mapInformation.SetActive(true);
        _mapInformations[_initializator.Index].SetActive(true);
        _mapActivator.ActivateAllMaps();
    }

    public override void Close()
    {
        base.Close();
        _cameraMovement.ResetZoom();
        // _chooseMap.StopWork();
        _chooseMap.enabled = false;
        _mapInformation.SetActive(false);
        _mapActivator.ChangeActivityMaps();
    }

    /*private void CheckActivation()
    {
        _currentValue = _load.Get(LastActiveMap, _startValue);

        if (_currentValue > _startValue)
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
    }*/

    public void ChangeActivationButton()
    {
        int openValue = _load.Get(OpenMap + _initializator.Index, _startValue);

        if (openValue == _startValue)
        {
            _openMapContent.SetActive(false);
            _closeMapContent.SetActive(true);
            return;
        }

        _openMapContent.SetActive(true);
        _closeMapContent.SetActive(false);

        // Debug.Log( "Активация ");
        int value = _load.Get(ActiveMap + _initializator.Index, _startValue);

        if (value > _startValue)
        {
            // Debug.Log("true " + value);
            _startButton.SetActive(false);
            _continueButton.SetActive(true);
            _restartButton.SetActive(true);
        }
        else
        {
            // Debug.Log("false " + value);
            _startButton.SetActive(true);
            _continueButton.SetActive(false);
            _restartButton.SetActive(false);
        }
    }
}